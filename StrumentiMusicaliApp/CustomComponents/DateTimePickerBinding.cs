﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.CustomComponents
{

	internal class DateTimePickerBinding : System.Windows.Forms.Binding
	{
		public DateTimePickerBinding(object dataSource, string dataMember, DataSourceUpdateMode dataSourceUpdateMode)
			: base("Value", dataSource, dataMember, true, dataSourceUpdateMode)
		{
			this.Format += new ConvertEventHandler(DateTimePickerBinding_Format);
			this.Parse += new ConvertEventHandler(DateTimePickerBinding_Parse);
		}

		void DateTimePickerBinding_Parse(object sender, ConvertEventArgs e)
		{
			DateTimePicker dateTimePicker = (Control as DateTimePicker);
			if (dateTimePicker != null)
			{
				if (dateTimePicker.Checked == false)
				{
					dateTimePicker.ShowCheckBox = true;
					dateTimePicker.Checked = false;
					e.Value = new DateTime?();
				}
				else
				{
					DateTime val = Convert.ToDateTime(e.Value);
					e.Value = new DateTime?(val);
				}
			}
		}

		void DateTimePickerBinding_Format(object sender, ConvertEventArgs e)
		{
			DateTimePicker dateTimePicker = (this.Control as DateTimePicker);
			if (dateTimePicker != null)
			{
				if (e.Value == null)
				{
					dateTimePicker.ShowCheckBox = true;
					dateTimePicker.Checked = false;
				}
				else
				{
					dateTimePicker.ShowCheckBox = true;
					dateTimePicker.Checked = true;
				}
			}
		}
	}
}
