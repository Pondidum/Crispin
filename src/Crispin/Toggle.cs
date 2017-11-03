using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Projections;

namespace Crispin
{
	public class Toggle : AggregateRoot
	{
		public static Toggle CreateNew(EditorID creator, string name, string description = "")
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name), "Toggles must have a non-whitespace name.");

			var toggle = new Toggle();
			toggle.ApplyEvent(new ToggleCreated(creator, ToggleID.CreateNew(), name.Trim(), description));

			return toggle;
		}

		public static Toggle LoadFrom(IEnumerable<object> events)
		{
			var toggle = new Toggle();
			((IEvented)toggle).LoadFromEvents(events);

			return toggle;
		}


		public string Name { get; private set; }
		public string Description { get; private set; }
		public DateTime? LastToggled { get; private set; }
		public IEnumerable<string> Tags => _tags;

		private readonly HashSet<string> _tags;
		private readonly ToggleState _state;

		private Toggle()
		{
			_tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			_state = new ToggleState();

			Register<ToggleCreated>(Apply);

			Register<ToggleSwitchedOnForAnonymous>(Apply);
			Register<ToggleSwitchedOffForAnonymous>(Apply);
			Register<ToggleSwitchedOnForUser>(Apply);
			Register<ToggleSwitchedOffForUser>(Apply);
			Register<ToggleSwitchedOnForGroup>(Apply);
			Register<ToggleSwitchedOffForGroup>(Apply);

			Register<TagAdded>(Apply);
			Register<TagRemoved>(Apply);
		}

		//public methods which do domainy things
		public bool IsActive(IGroupMembership membership, UserID userID)
			=> _state.IsActive(membership, userID);

		public void ChangeState(EditorID editor, UserID user, States? newState)
		{
			if (newState.HasValue == false)
				ApplyEvent(new ToggleUnsetForUser(editor, user));
			else if (newState.Value == States.On)
				ApplyEvent(new ToggleSwitchedOnForUser(editor, user));
			else
				ApplyEvent(new ToggleSwitchedOffForUser(editor, user));
		}

		public void ChangeState(EditorID editor, GroupID group, States? newState)
		{
			if (newState.HasValue == false)
				ApplyEvent(new ToggleUnsetForGroup(editor, group));
			else if (newState.Value == States.On)
				ApplyEvent(new ToggleSwitchedOnForGroup(editor, group));
			else
				ApplyEvent(new ToggleSwitchedOffForGroup(editor, group));
		}

		public void ChangeDefaultState(EditorID editor, States newState) => ApplyEvent(newState == States.On
			? new ToggleSwitchedOnForAnonymous(editor) as Event
			: new ToggleSwitchedOffForAnonymous(editor) as Event);

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

		//handlers which apply the results of the domainy things
		private void Apply(ToggleCreated e)
		{
			ID = e.ID;
			Name = e.Name;
			Description = e.Description;
		}

		private void Apply(ToggleSwitchedOffForAnonymous e)
		{
			LastToggled = e.TimeStamp;
			_state.HandleSwitching(newState: States.Off);
		}

		private void Apply(ToggleSwitchedOnForAnonymous e)
		{
			LastToggled = e.TimeStamp;
			_state.HandleSwitching(newState: States.On);
		}

		private void Apply(ToggleSwitchedOffForUser e)
		{
			LastToggled = e.TimeStamp;
			_state.HandleSwitching(e.User, newState: States.Off);
		}

		private void Apply(ToggleSwitchedOnForUser e)
		{
			LastToggled = e.TimeStamp;
			_state.HandleSwitching(e.User, newState: States.On);
		}

		private void Apply(ToggleSwitchedOffForGroup e)
		{
			LastToggled = e.TimeStamp;
			_state.HandleSwitching(e.Group, newState: States.Off);
		}

		private void Apply(ToggleSwitchedOnForGroup e)
		{
			LastToggled = e.TimeStamp;
			_state.HandleSwitching(e.Group, newState: States.On);
		}

		private void Apply(TagAdded e) => _tags.Add(e.Name);
		private void Apply(TagRemoved e) => _tags.Remove(e.Name);
	}
}
