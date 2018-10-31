using DevExpress.XtraEditors;
using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.App.CustomComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.Utility
{
	public static class UtilityView
	{
		public static T GetCurrentItemSelected<T>(this DataGridView dataGrid)
		{
			if (dataGrid.SelectedRows.Count > 0)
			{
				return (T)dataGrid.SelectedRows[0].DataBoundItem;
			}
			return default(T);
		}

		public static async Task SelezionaRiga(this DataGridView dataGrid, string idItem)
		{
			for (int i = 0; i < dataGrid.RowCount; i++)
			{
				if (((BaseItemID)(dataGrid.Rows[i].DataBoundItem)).ID == idItem)
				{
					dataGrid.Rows[i].Selected = true;
					dataGrid.CurrentCell = dataGrid.Rows[i].Cells[1];
					break;
				}
			}
		}

		public delegate void InvokeIfRequiredDelegate<T>(T obj)
				where T : ISynchronizeInvoke;

		public static void InvokeIfRequired<T>(this T obj, InvokeIfRequiredDelegate<T> action)
			where T : ISynchronizeInvoke
		{
			if (obj.InvokeRequired)
			{
				obj.Invoke(action, new object[] { obj });
			}
			else
			{
				action(obj);
			}
		}

		public static void SetDataBind(Control parentControl, object businessObject)
		{
			FixDateNull(businessObject);
			parentControl.InvokeIfRequired((b) =>
			{
				var listControlWithTag = FindControlByType<Control>(parentControl).Where(a => a.Tag != null && a.Tag.ToString().Length > 0);

				foreach (var item in GetProperties(businessObject))
				{
					var listByTag = listControlWithTag.Where(a => a.Tag.ToString() == item.Name);

					foreach (var cnt in listByTag)
					{
						if (cnt.DataBindings.Count > 0)
							cnt.DataBindings.RemoveAt(0);
						if (cnt is TextBox)
						{
							cnt.DataBindings.Add("Text", businessObject, item.Name);
						}
						else if (cnt is NumericUpDown)
						{
							cnt.DataBindings.Add("Value", businessObject, item.Name);
						}
						else if (cnt is CheckBox)
						{
							cnt.DataBindings.Add("Checked", businessObject, item.Name);
						}
						else if (cnt is System.Windows.Forms.ComboBox)
						{
							cnt.DataBindings.Add("SelectedValue", businessObject, item.Name);
						}
						else if (cnt is DateTimePicker)
						{
							cnt.DataBindings.Add(new DateTimePickerBinding(businessObject, item.Name
								, DataSourceUpdateMode.OnPropertyChanged));

							//(cnt as DateTimePicker).DataBindings.Add("Value", businessObject, item.Name);
						}
						else if (cnt is DateEdit)
						{
							InitDate(cnt as DateEdit);
							cnt.DataBindings.Add("DateTime", businessObject, item.Name, true);
						}
						else if (cnt is LookUpEdit)
						{
							cnt.DataBindings.Add("EditValue", businessObject, item.Name);
						}
					}
				}
			});
		}

		private static void InitDate(DateEdit de)
		{
			de.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;

			de.Properties.NullDate = DateTime.MinValue;

			de.Properties.NullText = "<Vuoto>";
			//string DatasDateTimeFormat = "dd.MM.yyyy HH:mm:ss";

			//de.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			//de.Properties.DisplayFormat.FormatString = DatasDateTimeFormat;

			//de.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			//de.Properties.EditFormat.FormatString = DatasDateTimeFormat;
			//de.Properties.EditMask = DatasDateTimeFormat;
		}

		public static void FixDateNull(object entity)
		{
			var list = GetProperties(entity);
			foreach (var item in list.Where(a => a.PropertyType.ToString().Contains("DateTime")))
			{
				var val = item.GetValue(entity);
				if ((val == null))
				{
					item.SetValue(entity, DateTime.MinValue);
				}
			}
		}

		public static List<T> FindControlByType<T>(Control mainControl, bool getAllChild = true) where T : Control
		{
			List<T> lt = new List<T>();
			for (int i = 0; i < mainControl.Controls.Count; i++)
			{
				if (mainControl.Controls[i] is T) lt.Add((T)mainControl.Controls[i]);
				if (getAllChild) lt.AddRange(FindControlByType<T>(mainControl.Controls[i], getAllChild));
			}
			return lt;
		}

		public static IEnumerable<PropertyInfo> GetProperties(Object obj)
		{
			Type t = obj.GetType();

			return t.GetProperties()
				.Where(p => (p.Name != "EntityKey" && p.Name != "EntityState"))
				.Select(p => p).ToList();
		}
	}
}