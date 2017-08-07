using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Crispin.Tests
{
	public class AcceptanceTests
	{
		[Fact]
		public void When_creating_feature_toggle_without_a_description()
		{
			var toggle = Toggle.CreateNew(name: "first-toggle");

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldNotBe(Guid.Empty),
				() => toggle.Name.ShouldBe("first-toggle"),
				() => toggle.Description.ShouldBe(string.Empty)
			);
		}

		[Fact]
		public void When_create_a_feature_toggle_with_a_description()
		{
			var toggle = Toggle.CreateNew(name: "first-toggle", description: "my cool description");

			toggle.ShouldSatisfyAllConditions(
				() => toggle.ID.ShouldNotBe(Guid.Empty),
				() => toggle.Name.ShouldBe("first-toggle"),
				() => toggle.Description.ShouldBe("my cool description")
			);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("         ")]
		[InlineData("		")]
		public void When_creating_a_toggle_with_no_name(string name)
		{
			Should
				.Throw<ArgumentNullException>(() => Toggle.CreateNew(name: name))
				.Message.ShouldContain("name");
		}

		[Theory]
		[InlineData("      one")]
		[InlineData("two      ")]
		[InlineData("  three  ")]
		public void When_creating_a_toggle_and_the_name_has_leading_or_trailing_whitespace(string name)
		{
			Toggle
				.CreateNew(name: name)
				.Name
				.ShouldBe(name.Trim());
		}
	}

	public class AggregateRoot
	{
		private readonly Dictionary<Type, Action<object>> _handlers;

		protected AggregateRoot()
		{
			_handlers = new Dictionary<Type, Action<object>>();
		}

		protected void Register<TEvent>(Action<TEvent> handler) => _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
		protected void ApplyEvent<TEvent>(TEvent @event) => _handlers[@event.GetType()](@event);
	}

	public class Toggle : AggregateRoot
	{
		public static Toggle CreateNew(string name, string description = "")
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Toggles must have a non-whitespace name.");

			var toggle = new Toggle();
			toggle.ApplyEvent(new ToggleCreated(Guid.NewGuid(), name.Trim(), description));

			return toggle;
		}


		public Guid ID { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		private Toggle()
		{
			Register<ToggleCreated>(Apply);
		}

		//public methods which do domainy things


		//handlers which apply the results of the domainy things
		private void Apply(ToggleCreated e)
		{
			ID = e.ID;
			Name = e.Name;
			Description = e.Description;
		}
	}

	public class ToggleCreated
	{
		public Guid ID { get; }
		public string Name { get; }
		public string Description { get; }

		public ToggleCreated(Guid id, string name, string description)
		{
			ID = id;
			Name = name;
			Description = description;
		}
	}
}
