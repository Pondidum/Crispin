﻿namespace Crispin.Events
{
	public class EnabledOnAnyCondition
	{
		public EditorID Editor { get; }

		public EnabledOnAnyCondition(EditorID editor)
		{
			Editor = editor;
		}

		public override string ToString() => $"Changed to require any condition to be Enabled to be active";
	}
}
