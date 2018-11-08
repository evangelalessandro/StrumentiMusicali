﻿using System;

namespace StrumentiMusicali.App.View.Attributes
{
	public class UIAmbienteAttribute : Attribute
	{
		public UIAmbienteAttribute(bool viewInForm)
		{
			OnlyViewInForm = viewInForm;
		}
		public bool Exclusive { get; set; } = true;
		public bool OnlyViewInForm { get; set; }

	}
}