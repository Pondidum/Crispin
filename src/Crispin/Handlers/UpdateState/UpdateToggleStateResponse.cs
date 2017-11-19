using Crispin.Projections;
using Crispin.Views;

namespace Crispin.Handlers.UpdateState
{
	public class UpdateToggleStateResponse
	{
		public ToggleID ToggleID { get; set; }
		public StateView State { get; set; }
	}
}
