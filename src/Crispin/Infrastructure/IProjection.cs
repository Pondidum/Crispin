namespace Crispin.Infrastructure
{
	public interface IProjection
	{
		void Consume(Event @event);

		object ToMemento();
		void FromMemento(object memento);
	}
}
