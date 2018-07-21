using System;
using System.Collections.Generic;
using System.Linq;
using Crispin.Conditions;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Views;

namespace Crispin
{
	public class Toggle : AggregateRoot
	{
		public static Toggle CreateNew(EditorID creator, string name, string description = "", ToggleID toggleID = null)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name), "Toggles must have a non-whitespace name.");

			var toggle = new Toggle();
			toggle.ApplyEvent(new ToggleCreated(
				creator,
				toggleID ?? ToggleID.CreateNew(),
				name.Trim(),
				description));

			return toggle;
		}

		public static Toggle LoadFrom(IEnumerable<Event> events)
		{
			var toggle = new Toggle();
			((IEvented)toggle).LoadFromEvents(events);

			return toggle;
		}


		public string Name { get; private set; }
		public string Description { get; private set; }
		public IEnumerable<string> Tags => _tags;
		public ConditionModes ConditionMode { get; private set; }
		public IEnumerable<Condition> Conditions => _conditions.All;

		private readonly HashSet<string> _tags;
		private readonly ConditionCollection _conditions;
		private ConditionID _currentConditionID;
		private readonly ConditionBuilder _conditionBuilder;

		private Toggle()
		{
			_tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_conditions = new ConditionCollection();
			_conditionBuilder = new ConditionBuilder();

			_currentConditionID = ConditionID.Empty;

			Register<ToggleCreated>(Apply);
			Register<TagAdded>(Apply);
			Register<TagRemoved>(Apply);

			Register<EnabledOnAllConditions>(Apply);
			Register<EnabledOnAnyCondition>(Apply);
			Register<ConditionAdded>(Apply);
			Register<ConditionRemoved>(Apply);
		}

		//public methods which do domainy things
		public void AddTag(EditorID editor, string tag)
		{
			if (_tags.Contains(tag))
				return;

			ApplyEvent(new TagAdded(editor, tag));
		}

		public void RemoveTag(EditorID editor, string tag)
		{
			if (_tags.Contains(tag) == false)
				return;

			ApplyEvent(new TagRemoved(editor, tag));
		}

		public void EnableOnAnyCondition(EditorID editor)
		{
			if (ConditionMode == ConditionModes.Any)
				return;

			ApplyEvent(new EnabledOnAnyCondition(editor));
		}

		public void EnableOnAllConditions(EditorID editor)
		{
			if (ConditionMode == ConditionModes.All)
				return;

			ApplyEvent(new EnabledOnAllConditions(editor));
		}

		public ConditionID AddCondition(EditorID editor, Dictionary<string, object> conditionProperties, ConditionID parentConditionID = null)
		{
			var messages = _conditionBuilder.CanCreateFrom(conditionProperties).ToArray();

			if (messages.Any())
				throw new ConditionException(messages.First());

			ValidateParentCondition(parentConditionID);

			var conditionID = NextConditionID();

			ApplyEvent(new ConditionAdded(editor, conditionID, parentConditionID, conditionProperties));

			return conditionID;
		}

		public void RemoveCondition(EditorID editor, ConditionID conditionID)
		{
			if (_conditions.HasCondition(conditionID) == false)
				throw new ConditionNotFoundException(conditionID);

			ApplyEvent(new ConditionRemoved(editor, conditionID));
		}

		public Condition Condition(ConditionID added) => _conditions.FindCondition(added);

		public ToggleView ToView() => new ToggleView
		{
			ID = ID,
			Name = Name,
			Description = Description,
			Tags = new HashSet<string>(_tags)
		};

		private void ValidateParentCondition(ConditionID parentConditionID)
		{
			if (parentConditionID == null)
				return;

			var parent = _conditions.FindCondition(parentConditionID);

			if (parent == null)
				throw new ConditionNotFoundException(parentConditionID);

			if (parent is IParentCondition == false)
				throw new ConditionException($"{parent.GetType().Name} does not support children.");
		}

		private ConditionID NextConditionID() => _currentConditionID = _currentConditionID.Next();


		//handlers which apply the results of the domainy things
		private void Apply(ToggleCreated e)
		{
			ID = e.AggregateID;
			Name = e.Name;
			Description = e.Description;
			ConditionMode = ConditionModes.All;
		}

		private void Apply(TagAdded e) => _tags.Add(e.Name);
		private void Apply(TagRemoved e) => _tags.Remove(e.Name);

		private void Apply(EnabledOnAllConditions e) => ConditionMode = ConditionModes.All;
		private void Apply(EnabledOnAnyCondition e) => ConditionMode = ConditionModes.Any;

		private void Apply(ConditionAdded e)
		{
			var condition = _conditionBuilder.CreateCondition(e.ConditionID, e.Properties);

			_currentConditionID = e.ConditionID;
			_conditions.Add(condition, e.ParentConditionID);
		}

		private void Apply(ConditionRemoved e)
		{
			if (e.ConditionID.IsNewerThan(_currentConditionID))
				_currentConditionID = e.ConditionID;

			_conditions.Remove(e.ConditionID);
		}
	}
}
