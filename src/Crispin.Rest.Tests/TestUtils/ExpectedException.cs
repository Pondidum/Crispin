using System;

namespace Crispin.Rest.Tests.TestUtils
{
	public class ExpectedException : Exception
	{
		public ExpectedException() : base("this should be thrown")
		{
		}
	}
}
