using System;
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

		private void DateTimePickerBinding_Parse(object sender, ConvertEventArgs e)
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

		private void DateTimePickerBinding_Format(object sender, ConvertEventArgs e)
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
					dateTimePicker.ShowCheckBox = false;
					dateTimePicker.Checked = true;
				}
			}
		}
	}
}