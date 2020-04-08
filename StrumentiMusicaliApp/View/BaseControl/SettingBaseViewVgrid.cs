using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace StrumentiMusicali.App.View.BaseControl
{
    public class SettingBaseViewVgrid : BaseDataControl, IMenu, ICloseSave
    {

        public SettingBaseViewVgrid()
           : base()
        {
            InitializeComponent();
            vGrid.CellValueChanged += VGrid_CellValueChanged;
        }
        public void BindProp(object objToBind, string prefixText)
        {
            if (objToBind is INotifyPropertyChanged)
            {
                (objToBind as INotifyPropertyChanged).PropertyChanged += (a, e) =>
                {
                    RefreshAllProp();
                };
            }
            var listProp = UtilityProp.GetProperties(objToBind).OrderBy(a =>
            {
                var sel = (CustomUIViewAttribute)a.GetCustomAttributes(typeof(CustomUIViewAttribute), true).FirstOrDefault();
                if (sel == null || sel.Ordine == 0)
                {
                    return 100;
                }
                return sel.Ordine;
            });
            foreach (var item in listProp)
            {
                var hideAttr = item.CustomAttributes.Where(a => a.AttributeType == typeof(CustomHideUIAttribute)).FirstOrDefault();
                if (hideAttr != null)
                    continue;

                var customHide = listProp.FirstOrDefault(a => a.Name == "Show" + item.Name);
                if (customHide != null)
                {
                    var toShow = customHide.GetValue(objToBind);
                    bool toShowVal = false;

                    if (Boolean.TryParse(toShow.ToString(), out toShowVal))
                    {
                        if (!toShowVal)
                        {
                            /*nascondo l'elemento*/
                            continue;
                        }
                    }
                }

                var row = new EditorRow();

                row.Properties.FieldName = item.Name;
                row.Properties.Value = item.GetValue(objToBind);


                var widthAttr = (CustomUIViewAttribute)item.GetCustomAttributes(typeof(CustomUIViewAttribute), true).FirstOrDefault();

                string titolo = UtilityView.GetTextSplitted(item.Name);

                if (widthAttr != null)
                {
                    if (widthAttr.Titolo.Length > 0)
                    {
                        titolo = widthAttr.Titolo;
                    }
                }


                row.Properties.Caption = titolo;

                ImpostaEditor(objToBind, item, row, widthAttr, titolo);

                if (row.Properties.RowEdit != null)
                {
                    AggingiRigaECategoria(prefixText, row, widthAttr);
                    BindObj(item, row, objToBind, widthAttr);
                }
            }
            ComplexBestFit();
        }
        public void ComplexBestFit()
        {
            int maxRowHeaderWidth = -1;
            int maxRecordWidth = -1;

            vGrid.BestFit();
            if (vGrid.RowHeaderWidth > maxRowHeaderWidth)
                maxRowHeaderWidth = vGrid.RowHeaderWidth;
            int recordWidth = CalcBestRecordWidth(vGrid);
            if (recordWidth > maxRecordWidth)
                maxRecordWidth = recordWidth;



            vGrid.RowHeaderWidth = maxRowHeaderWidth;
            //vGrid.RecordWidth = maxRecordWidth;

        }

        public int CalcBestRecordWidth(VGridControl vertGrid)
        {
            int minRecordWidth = 10;
            int recordCount = vertGrid.RecordCount;
            Graphics measureGraphics = vertGrid.CreateGraphics();
            foreach (BaseRow row in vertGrid.Rows)
                if (row.Visible)
                {
                    Font rowFont = row.Appearance.Font;
                    for (int currCell = 0; currCell < recordCount; currCell++)
                    {
                        string text = vertGrid.GetCellDisplayText(row, currCell);
                        int desiredRecordWidth = (int)measureGraphics.MeasureString(text, rowFont).Width;
                        if (desiredRecordWidth > minRecordWidth)
                            minRecordWidth = desiredRecordWidth;
                    }
                }
            return minRecordWidth;
        }
        private string AggingiRigaECategoria(string prefixText, EditorRow row, CustomUIViewAttribute widthAttr)
        {
            if (prefixText == "")
            {
                if (widthAttr != null && !string.IsNullOrEmpty(widthAttr.Category))
                {
                    prefixText = widthAttr.Category;
                }

            }
            if (prefixText.Length > 0)
            {
                var categoryContained = this.vGrid.Rows.Where(a => a is CategoryRow
                && a.Properties.Caption == prefixText).FirstOrDefault();

                if (categoryContained == null)
                {
                    var category = new CategoryRow();
                    category.Properties.Caption = prefixText;
                    this.vGrid.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
                                    category});
                    category.Appearance.BackColor = Color.FromArgb(98, 147, 188);
                    category.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                    this.vGrid.Appearance.Category.Font = new Font(category.Appearance.Font.FontFamily, category.Appearance.Font.Size, FontStyle.Bold);
                    categoryContained = category;

                }
                categoryContained.ChildRows.Add(row);
            }
            else
            {
                this.vGrid.Rows.Add(row);
            }

            return prefixText;
        }

        private void ImpostaEditor(object objToBind, System.Reflection.PropertyInfo item,
            EditorRow row, CustomUIViewAttribute widthAttr, string titolo)
        {
            if (widthAttr != null && widthAttr.Combo != TipoDatiCollegati.Nessuno)
            {
                row.Properties.RowEdit = AddCombo(widthAttr, titolo);
            }
            else if (item.PropertyType.FullName.Contains("String"))
            {

                var maxLength = (MaxLengthAttribute)item.GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault();

                if (widthAttr != null && widthAttr.MultiLine > 0)
                {

                    var editor = new RepositoryItemMemoEdit();
                    row.Properties.RowEdit = editor;
                    editor.AutoHeight = true;
                    //row.Height = row.Height * widthAttr.MultiLine;
                    if (maxLength != null)
                        editor.MaxLength = maxLength.Length;

                }
                else
                {

                    var editor = new RepositoryItemTextEdit();
                    row.Properties.RowEdit = editor;
                    if (maxLength != null)
                        editor.MaxLength = maxLength.Length;
                    //editor.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    //editor.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    vGrid.Appearance.SelectedCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    vGrid.Appearance.FocusedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                }

            }
            else if (item.PropertyType.FullName.Contains("Boolean"))
            {
                var check = new RepositoryItemCheckEdit();
                check.ValueChecked = true;
                check.ValueUnchecked = false;
                row.Properties.RowEdit = check;

            }
            else if (item.PropertyType.FullName.Contains("Decimal"))
            {
                var num = new RepositoryItemTextEdit();
                num.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                num.Mask.UseMaskAsDisplayFormat = true;
                num.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                num.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                num.Enter += (object sender, EventArgs e) =>
                {
                    var be = (BaseEdit)sender;
                    be.SelectAll();
                };
                if (widthAttr != null && widthAttr.Money)
                {
                    num.Mask.EditMask = "C2";
                }
                else
                {
                    num.Mask.EditMask = "D";
                }
                row.Properties.RowEdit = num;
                row.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            }
            else if (item.PropertyType.FullName.Contains("Int32"))
            {
                var num = new RepositoryItemTextEdit();
                num.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                num.Mask.UseMaskAsDisplayFormat = true;
                num.Mask.EditMask = "D";
                row.Properties.RowEdit = num;
                num.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                num.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

            }
            else if (item.PropertyType.FullName.Contains("DateTime"))
            {
                var num = new RepositoryItemTextEdit();
                num.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                num.Mask.UseMaskAsDisplayFormat = true;
                if (widthAttr.DateTimeView)
                {
                    num.Mask.EditMask = "HH:mm:ss dd/MM/yyyy";
                }
                else if (widthAttr.DateView)
                {
                    num.Mask.EditMask = "dd/MM/yyyy";
                }


                row.Properties.RowEdit = num;

            }
            else
            {

                if (item != null && (!item.PropertyType.Name.StartsWith("System.")
                    ))
                {
                    string gruppo = "[" + titolo + "]  ";
                    if (widthAttr != null && widthAttr.ShowGroupName == false)
                    {
                        gruppo = "";
                    }
                    BindProp(item.GetValue(objToBind), gruppo);
                }
                else
                {

                }
            }
        }

        private RepositoryItemLookUpEdit AddCombo(CustomUIViewAttribute widthAttr, string titolo)
        {
            RepositoryItemLookUpEdit newControl = new RepositoryItemLookUpEdit();
            newControl.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            using (var uof = new UnitOfWork())
            {


                switch (widthAttr.Combo)
                {
                    case TipoDatiCollegati.Nessuno:
                        break;
                    case TipoDatiCollegati.Clienti:
                        break;
                    case TipoDatiCollegati.Categorie:
                        {
                            var list = uof.CategorieRepository.Find(a => true).Select(a => new { a.ID, Descrizione = a.Reparto + " - " + a.Nome + " - " + a.Codice }).OrderBy(a => a.Descrizione).ToList();
                            newControl.ValueMember = "ID";
                            newControl.DisplayMember = "Descrizione";
                            newControl.DataSource = (list);
                            newControl.PopulateColumns();

                        }
                        break;
                    case TipoDatiCollegati.Marca:
                        {
                            var list = uof.ArticoliRepository.Find(a => true).Select(a => a.Strumento.Marca).Distinct().OrderBy(a => a).ToList();

                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.NomeStrumento:
                        {
                            var list = uof.ArticoliRepository.Find(a => true).Select(a => a.Strumento.Nome).Distinct().OrderBy(a => a).ToList();

                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.LibroAutore:
                        {
                            var list = uof.ArticoliRepository.Find(a => true).Select(a => a.Libro.Autore).Distinct().OrderBy(a => a).ToList();

                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.Rivenditore:
                        {
                            var list = uof.ArticoliRepository.Find(a => true).Select(a => a.Strumento.Rivenditore.Trim()).Distinct().OrderBy(a => a).ToList();

                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.Colore:
                        {
                            var list = uof.ArticoliRepository.Find(a => true).Select(a => a.Strumento.Colore.Trim()).Distinct().OrderBy(a => a).ToList();

                            newControl.DataSource = (list);
                            //newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.Modello:
                        {
                            var list = uof.ArticoliRepository.Find(a => true).Select(a => a.Strumento.Modello).Distinct().OrderBy(a => a).ToList();

                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.Articoli:
                        {
                            var list = uof.ArticoliRepository.Find(a => true).Select(a => new { a.ID, a.Titolo, a.Prezzo }).ToList()
                                .Select(a => new { a.ID, Descrizione = a.Titolo, Prezzo = a.Prezzo }).OrderBy(a => a.Descrizione).ToList();
                            newControl.ValueMember = "ID";
                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.Condizione:
                        {
                            var list = Enum.GetNames(typeof(enCondizioneArticolo)).Select(a =>
                            new
                            {
                                ID = (enCondizioneArticolo)Enum.Parse(typeof(enCondizioneArticolo), a),
                                Descrizione = UtilityView.GetTextSplitted(a)
                            }
                            ).OrderBy(a => a.Descrizione).ToList();
                            newControl.ValueMember = "ID";
                            newControl.DisplayMember = "Descrizione";
                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    default:
                        break;
                }

                if (widthAttr.ComboLibera)
                {
                    newControl.AcceptEditorTextAsNewValue = DevExpress.Utils.DefaultBoolean.True;
                }
            }

            return newControl;
        }
        List<CopyProp> _listProp = new List<CopyProp>();
        private void BindObj(System.Reflection.PropertyInfo item, EditorRow editorRow,
            object objToBind, CustomUIViewAttribute attribute)
        {
            var prop = new CopyProp()
            {
                ObjectItem = objToBind,
                Prop = item,
                EditRow = editorRow
            };
            _listProp.Add(prop);
            editorRow.Tag = prop;

            var hideAttr = (CustomHideUIAttribute)item.GetCustomAttributes(typeof(CustomHideUIAttribute), true).FirstOrDefault();

            if ((attribute != null && attribute.Enable == false
                )
                ||
                hideAttr != null
                )
            {
                editorRow.Enabled = false;
            }

        }
        class CopyProp
        {
            public System.Reflection.PropertyInfo Prop { get; set; }
            public object ObjectItem { get; set; }

            public EditorRow EditRow { get; set; }

            public void SetValue(object val)
            {
                Prop.SetValue(ObjectItem, val);
            }
            public Object GetValue()
            {
                return Prop.GetValue(ObjectItem);
            }
        }
        private void VGrid_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            var tag = (CopyProp)e.Row.Tag;

            tag.SetValue(e.Value);

            //RefreshAllProp();
        }
        /// <summary>
        /// aggiorna tutte le proprietà
        /// </summary>
        public void RefreshAllProp()
        {
            foreach (var item in _listProp)
            {
                if (item.EditRow.Properties.Value != item.GetValue())
                {
                    item.EditRow.Properties.Value = item.GetValue();
                }
            }
        }

        private void InitializeComponent()
        {
            this.vGrid = new DevExpress.XtraVerticalGrid.VGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.vGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // vGrid
            // 
            this.vGrid.Appearance.BandBorder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.BandBorder.Options.UseFont = true;
            this.vGrid.Appearance.BandBorder.Options.UseTextOptions = true;
            this.vGrid.Appearance.BandBorder.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.Category.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.Category.Options.UseFont = true;
            this.vGrid.Appearance.Category.Options.UseTextOptions = true;
            this.vGrid.Appearance.Category.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.CategoryExpandButton.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.CategoryExpandButton.Options.UseFont = true;
            this.vGrid.Appearance.CategoryExpandButton.Options.UseTextOptions = true;
            this.vGrid.Appearance.CategoryExpandButton.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.DisabledRecordValue.BackColor = System.Drawing.Color.Gainsboro;
            this.vGrid.Appearance.DisabledRecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.DisabledRecordValue.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseBackColor = true;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseFont = true;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseForeColor = true;
            this.vGrid.Appearance.DisabledRecordValue.Options.UseTextOptions = true;
            this.vGrid.Appearance.DisabledRecordValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.vGrid.Appearance.DisabledRecordValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.DisabledRow.BackColor = System.Drawing.SystemColors.Control;
            this.vGrid.Appearance.DisabledRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.DisabledRow.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.DisabledRow.Options.UseFont = true;
            this.vGrid.Appearance.DisabledRow.Options.UseForeColor = true;
            this.vGrid.Appearance.DisabledRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.DisabledRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.DisabledRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.Empty.Options.UseFont = true;
            this.vGrid.Appearance.Empty.Options.UseTextOptions = true;
            this.vGrid.Appearance.Empty.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.ExpandButton.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ExpandButton.Options.UseFont = true;
            this.vGrid.Appearance.ExpandButton.Options.UseTextOptions = true;
            this.vGrid.Appearance.ExpandButton.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.FocusedCell.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.FocusedCell.Options.UseFont = true;
            this.vGrid.Appearance.FocusedCell.Options.UseTextOptions = true;
            this.vGrid.Appearance.FocusedCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.FocusedRecord.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.FocusedRecord.Options.UseFont = true;
            this.vGrid.Appearance.FocusedRecord.Options.UseTextOptions = true;
            this.vGrid.Appearance.FocusedRecord.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.FocusedRow.Options.UseFont = true;
            this.vGrid.Appearance.FocusedRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.FocusedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.FocusedRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.HideSelectionRow.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.vGrid.Appearance.HideSelectionRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.HideSelectionRow.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.vGrid.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.vGrid.Appearance.HideSelectionRow.Options.UseFont = true;
            this.vGrid.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.vGrid.Appearance.HideSelectionRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.HideSelectionRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.HideSelectionRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.HorzLine.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.HorzLine.Options.UseFont = true;
            this.vGrid.Appearance.HorzLine.Options.UseTextOptions = true;
            this.vGrid.Appearance.HorzLine.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.ModifiedRecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ModifiedRecordValue.Options.UseFont = true;
            this.vGrid.Appearance.ModifiedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ModifiedRow.Options.UseFont = true;
            this.vGrid.Appearance.PressedRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.PressedRow.Options.UseFont = true;
            this.vGrid.Appearance.PressedRow.Options.UseTextOptions = true;
            this.vGrid.Appearance.PressedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.PressedRow.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.ReadOnlyRecordValue.BackColor = System.Drawing.Color.Gainsboro;
            this.vGrid.Appearance.ReadOnlyRecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ReadOnlyRecordValue.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.ReadOnlyRecordValue.Options.UseBackColor = true;
            this.vGrid.Appearance.ReadOnlyRecordValue.Options.UseFont = true;
            this.vGrid.Appearance.ReadOnlyRecordValue.Options.UseForeColor = true;
            this.vGrid.Appearance.ReadOnlyRow.BackColor = System.Drawing.SystemColors.Control;
            this.vGrid.Appearance.ReadOnlyRow.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.ReadOnlyRow.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.ReadOnlyRow.Options.UseBackColor = true;
            this.vGrid.Appearance.ReadOnlyRow.Options.UseFont = true;
            this.vGrid.Appearance.ReadOnlyRow.Options.UseForeColor = true;
            this.vGrid.Appearance.RecordValue.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.RecordValue.Options.UseFont = true;
            this.vGrid.Appearance.RecordValue.Options.UseTextOptions = true;
            this.vGrid.Appearance.RecordValue.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.vGrid.Appearance.RecordValue.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.RowHeaderPanel.BackColor = System.Drawing.SystemColors.Control;
            this.vGrid.Appearance.RowHeaderPanel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.RowHeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseBackColor = true;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseFont = true;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseForeColor = true;
            this.vGrid.Appearance.RowHeaderPanel.Options.UseTextOptions = true;
            this.vGrid.Appearance.RowHeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.vGrid.Appearance.RowHeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Appearance.VertLine.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vGrid.Appearance.VertLine.Options.UseFont = true;
            this.vGrid.Appearance.VertLine.Options.UseTextOptions = true;
            this.vGrid.Appearance.VertLine.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
            this.vGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGrid.LayoutStyle = DevExpress.XtraVerticalGrid.LayoutViewStyle.SingleRecordView;
            this.vGrid.Location = new System.Drawing.Point(0, 0);
            this.vGrid.Name = "vGrid";
            this.vGrid.OptionsBehavior.UseEnterAsTab = true;
            this.vGrid.OptionsFilter.AllowFilter = false;
            this.vGrid.OptionsView.AutoScaleBands = true;
            this.vGrid.OptionsView.FixRowHeaderPanelWidth = true;
            this.vGrid.OptionsView.ShowButtons = false;
            this.vGrid.RecordWidth = 167;
            this.vGrid.RowHeaderWidth = 33;
            this.vGrid.ShowButtonMode = DevExpress.XtraVerticalGrid.ShowButtonModeEnum.ShowOnlyInEditor;
            this.vGrid.Size = new System.Drawing.Size(895, 560);
            this.vGrid.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 560);
            this.Controls.Add(this.vGrid);
            this.Name = "SettingBaseViewVgrid";
            this.Text = "SettingBaseViewVgrid";
            ((System.ComponentModel.ISupportInitialize)(this.vGrid)).EndInit();
            this.ResumeLayout(false);

        }


        private DevExpress.XtraVerticalGrid.VGridControl vGrid;
        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~SettingBaseViewVgrid()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        public MenuTab GetMenu()
        {
            if (_menuTab == null)
            {
                _menuTab = new MenuTab();

                AggiungiComandi();
            }
            return _menuTab;
        }

        private MenuTab _menuTab = null;


        private void AggiungiComandi()
        {
            var tabArticoli = _menuTab.Add("Principale");
            var pnl = tabArticoli.Add("Comandi");
            UtilityView.AddButtonSaveAndClose(pnl, this);
        }
        public event EventHandler<EventArgs> OnSave;
        public event EventHandler<EventArgs> OnClose;

        public void RaiseSave()
        {
            if (OnSave != null)
            {
                OnSave(this, new EventArgs());
            }
        }

        public void RaiseClose()
        {
            if (OnClose != null)
            {
                OnClose(this, new EventArgs());
            }
        }
    }
}
