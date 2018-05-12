using System;
using System.Collections.Generic;
using Crispin.Events;
using Crispin.Infrastructure;
using Crispin.Views;

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
		public IEnumerable<string> Tags => _tags;

		private readonly HashSet<string> _tags;

		private Toggle()
		{
			_tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			Register<ToggleCreated>(Apply);
			Register<TagAdded>(Apply);
			Register<TagRemoved>(Apply);
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
		
		public ToggleView ToView() => new ToggleView
		{
			ID = ID,
			Name = Name,
			Description = Description,
			Tags = new HashSet<string>(_tags)
		};


		//handlers which apply the results of the domainy things
		private void Apply(ToggleCreated e)
		{
			ID = e.ID;
			Name = e.Name;
			Description = e.Description;
		}

		private void Apply(TagAdded e) => _tags.Add(e.Name);
		private void Apply(TagRemoved e) => _tags.Remove(e.Name);
	}
}
