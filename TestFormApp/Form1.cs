using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraVerticalGrid.Rows;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TestFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            vGrid.CellValueChanged += VGrid_CellValueChanged;
            this.vGrid.Rows.Clear();

            using (var uof = new UnitOfWork())
            {
                var _art = uof.ArticoliRepository.Find(a => 1 == 1).First();
                //var _art = new Articolo();
                _art.ShowLibro =
                    _art.IsLibro();
                _art.ShowStrumento = !_art.ShowLibro;

                //vGrid.DataSource = _art;
                ////art.Titolo
                //vGrid.Update();
                BindProp(_art, "");
            }
        }

        public void BindProp(object objToBind, string prefixText)
        {
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

        private List<CopyProp> _listProp = new List<CopyProp>();

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

        private class CopyProp
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

            foreach (var item in _listProp)
            {
                if (item.EditRow.Properties.Value != item.GetValue())
                {
                    item.EditRow.Properties.Value = item.GetValue();
                }
            }
        }
    }
}