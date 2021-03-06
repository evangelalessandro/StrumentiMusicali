﻿using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.CustomComponents;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Item.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            return default;
        }

        public static T GetCurrentItemSelected<T>(this GridView dataGrid)
        {
            if (dataGrid.FocusedRowHandle >= 0)
            {
                var current = dataGrid.GetRow(dataGrid.FocusedRowHandle);
                return (T)current;
            }
            return default;
        }
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {

            List<T> list = new List<T>();

            T objP = new T();
            var listPro = objP.GetType().GetProperties().Where(a => table.Columns.Contains(a.Name)).ToList();

            var listProInfo = new List<PropertyInfo>();
            foreach (var item in listPro)
            {
                PropertyInfo propertyInfo = objP.GetType().GetProperty(item.Name);

                listProInfo.Add(propertyInfo);
            }

            foreach (var row in table.AsEnumerable())
            {

                T obj = new T();
                foreach (var prop in listPro)
                {
                    try
                    {
                        PropertyInfo propertyInfo = listProInfo.Where(a => a.Name == prop.Name).First();

                        if (row[prop.Name] != null)
                        {
                            if (row[prop.Name] != DBNull.Value)
                            {
                                propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                            }
                            else
                            {
                                
                                propertyInfo.SetValue(obj, null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }

                list.Add(obj);
            }

            return list;

        }

        public static string GetTextSplitted(string name)
        {
            var titolo = string.Join(" ", Regex.Split(name, @"(?<!^)(?=[A-Z])"));

            if (name.CompareTo(name.ToUpperInvariant()) == 0)
            {
                return name;
            }

            return titolo;
        }


        public static void AddButtonSaveAndClose(RibbonMenuPanel pnl, ICloseSave control, bool addSave = true)
        {
            var rib3 = pnl.Add("Chiudi", StrumentiMusicali.Core.Properties.ImageIcons.Close_48);
            rib3.Tag = MenuTab.TagClose;

            rib3.Click += (a, e) =>
            {
                control.RaiseClose();
            };
            if (addSave)
            {


                var rib1 = pnl.Add("Salva", StrumentiMusicali.Core.Properties.ImageIcons.Save);


                rib1.Click += (a, e) =>
                {
                    control.RaiseSave();
                };
            }
        }

        public static void SelezionaRiga(this GridView dataGrid, int idItem)
        {
            var colVisible = -1;
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visible)
                {
                    colVisible = i;
                    break;
                }
            }
            if (colVisible == -1)
                return;
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                if (((BaseItemID)dataGrid.GetRow(i)).ID == idItem)
                {
                    dataGrid.FocusedRowHandle = i;
                    //dataGrid.SelectCell(i, dataGrid.Columns[colVisible]);
                    break;
                }
            }
        }

        public static async Task SelezionaRiga(this DataGridView dataGrid, int idItem)
        {
            var colVisible = -1;
            for (int i = 0; i < dataGrid.ColumnCount; i++)
            {
                if (dataGrid.Columns[i].Visible)
                {
                    colVisible = i;
                    break;
                }
            }
            if (colVisible == -1)
                return;
            for (int i = 0; i < dataGrid.RowCount; i++)
            {
                if (((BaseItemID)dataGrid.Rows[i].DataBoundItem).ID == idItem)
                {
                    dataGrid.Rows[i].Selected = true;
                    dataGrid.CurrentCell = dataGrid.Rows[i].Cells[colVisible];
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

        public static void SetDataBind(Control parentControl, CustomUIViewAttribute attribute, object businessObject)
        {
            FixDateNull(businessObject);
            parentControl.InvokeIfRequired((b) =>
            {
                var listControlWithTag = FindControlByType<Control>(parentControl).Where(a => a.Tag != null && a.Tag.ToString().Length > 0);

                foreach (var item in UtilityProp.GetProperties(businessObject))
                {
                    var listByTag = listControlWithTag.Where(a => a.Tag.ToString() == item.Name);

                    foreach (var cnt in listByTag)
                    {
                        if (cnt.DataBindings.Count > 0)
                            cnt.DataBindings.RemoveAt(0);
                        if (cnt is TextBox)
                        {
                            cnt.DataBindings.Add("Text", businessObject, item.Name);
                            if (attribute != null && attribute.MultiLine > 0)
                            {
                                (cnt as TextBox).Multiline = true;
                            }
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
                        else if (cnt is ListBox)
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
                            InitDate(cnt as DateEdit, attribute);
                            cnt.DataBindings.Add("DateTime", businessObject, item.Name, true);
                        }
                        else if (cnt is LookUpEdit)
                        {
                            cnt.DataBindings.Add("EditValue", businessObject, item.Name);
                        }
                        else
                        {
                            throw new Exception("Bind non gestito");
                        }
                    }
                }
            });
        }

        private static void InitDate(DateEdit de, CustomUIViewAttribute attribute)
        {
            de.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;

            de.Properties.NullDate = DateTime.MinValue;

            de.Properties.NullText = "<Vuoto>";
            if (attribute != null)
            {
                if (attribute.DateTimeView)
                {
                    string sDateTimeFormat = "dd.MM.yyyy HH:mm:ss";
                    SetDateFormat(de, sDateTimeFormat);
                }

                if (attribute.TimeView)
                {
                    string sDateTimeFormat = "HH:mm:ss";
                    SetDateFormat(de, sDateTimeFormat);
                }

                if (attribute.DateView)
                {
                    string sDateTimeFormat = "dd.MM.yyyy";
                    SetDateFormat(de, sDateTimeFormat);
                }
            }
        }

        private static void SetDateFormat(DateEdit de, string DatasDateTimeFormat)
        {
            de.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            de.Properties.DisplayFormat.FormatString = DatasDateTimeFormat;

            de.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            de.Properties.EditFormat.FormatString = DatasDateTimeFormat;
            de.Properties.EditMask = DatasDateTimeFormat;
        }

        public static void FixDateNull(object entity)
        {
            var list = UtilityProp.GetProperties(entity);
            foreach (var item in list.Where(a => a.PropertyType.ToString().Contains("DateTime")))
            {
                var val = item.GetValue(entity);
                if (val == null)
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
     
        public static List<T> GetSelectedRows<T>(this GridView dataGrid)
        {
            
            List<T> list = new List<T>();
            foreach (int i in dataGrid.GetSelectedRows())
            {
                list.Add( (T)dataGrid.GetRow(i));


            }
            return list;
        }

        internal static void InitGridDev(GridView dgvRighe,bool multiSelect=false)
        {
            dgvRighe.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            dgvRighe.OptionsBehavior.AllowIncrementalSearch = true;
            dgvRighe.OptionsBehavior.Editable = false;
            dgvRighe.OptionsView.ShowAutoFilterRow = true;
            dgvRighe.OptionsSelection.MultiSelect = multiSelect;
            dgvRighe.OptionsView.RowAutoHeight = true;
            dgvRighe.OptionsView.ColumnAutoWidth = true;
            dgvRighe.OptionsView.BestFitMode = GridBestFitMode.Fast;
            dgvRighe.OptionsView.ShowFooter = true;
            dgvRighe.OptionsView.ShowGroupPanel = false;
        }
    }
}