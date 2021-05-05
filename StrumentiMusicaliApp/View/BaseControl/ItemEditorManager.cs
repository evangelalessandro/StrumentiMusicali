using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid.Rows;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Core.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Enums;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace StrumentiMusicali.App.View.BaseControl
{
    public partial class ItemEditorManager
    {
        private DevExpress.XtraVerticalGrid.VGridControl _vGrid;
        private GridControl _gcControl;
        private GridView _dgvRighe;
        public ItemEditorManager(DevExpress.XtraVerticalGrid.VGridControl vGrid)
        {
            _vGrid = vGrid;
        }
        public ItemEditorManager(GridControl gcControl, GridView dgvRighe)
        {
            _dgvRighe = dgvRighe;
            _gcControl = gcControl;
        }
        /// <summary>
        /// aggiorna tutte le proprietà
        /// </summary>
        private void RefreshAllProp()
        {
            foreach (var item in _listProp)
            {
                if (item.EditRow.Properties.Value != item.GetValue())
                {
                    item.EditRow.Properties.Value = item.GetValue();
                }
            }
        }
        private RepositoryItem AddCombo(CustomUIViewAttribute widthAttr, string titolo)
        {
            RepositoryItemLookUpEdit newControl = new RepositoryItemLookUpEdit();
            newControl.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            using (var uof = new UnitOfWork())
            {


                switch (widthAttr.Combo)
                {
                    case TipoDatiCollegati.Nessuno:
                        break;
                    case TipoDatiCollegati.TipiSoggetto:
                        {
                            RepositoryItemCheckedComboBoxEdit repo = new RepositoryItemCheckedComboBoxEdit();

                            var list = Enum.GetNames(typeof(enTipiSoggetto)).ToList().Select(a => new { ID = a, Descrizione = a }).ToList();
                            repo.ValueMember = "ID";
                            repo.DisplayMember = "Descrizione";
                            repo.DataSource = (list);
                            //newControl.PopulateColumns();
                            repo.AllowMultiSelect = true;
                            repo.SeparatorChar = Convert.ToChar(",");
                            repo.SelectAllItemVisible = true;
                            repo.SelectAllItemCaption = "(Tutti)";
                            repo.ShowButtons = true;
                            repo.IncrementalSearch = true;
                            return repo;
                        }
                        break;
                    case TipoDatiCollegati.Clienti:
                        {
                            var list = uof.ClientiRepository.Select(a => new { a.ID, Descrizione = a.RagioneSociale.Length > 0 ? a.RagioneSociale : a.Cognome + " " + a.Nome })
                                .Distinct().OrderBy(a => a.Descrizione).ToList();
                            newControl.ValueMember = "ID";
                            newControl.DisplayMember = "Descrizione";

                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
                        break;
                    case TipoDatiCollegati.Fornitori:
                        {
                            var list = uof.FornitoriRepository
                                .Select(a => new { a.ID, Descrizione = a.RagioneSociale.Length > 0 ? a.RagioneSociale : a.Cognome + " " + a.Nome }).Distinct().OrderBy(a => a.Descrizione).ToList();
                            newControl.ValueMember = "ID";
                            newControl.DisplayMember = "Descrizione";

                            newControl.DataSource = (list);
                            newControl.PopulateColumns();
                        }
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
                    case TipoDatiCollegati.TipiPagamentiDocumenti:
                        break;
                    case TipoDatiCollegati.RiordinoPeriodi:
                        {
                            var list = uof.RiordinoPeriodiRepository.Find(a => true).ToList()
                                .Select(a => new { a.ID, a.Descrizione, Periodi= a.PeriodoSottoScortaInizio.ToString(@"dd\MM") + " " + a.PeriodoSottoScortaFine.ToString(@"dd\MM") }).ToList()
                                .OrderBy(a => a.Descrizione).ToList();
                            newControl.ValueMember = "ID";
                            newControl.DisplayMember= "Descrizione";
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
        private RepositoryItem ImpostaEditor(object objToBind, System.Reflection.PropertyInfo item,
            EditorRow row, CustomUIViewAttribute widthAttr, string titolo)
        {
            if (widthAttr != null && widthAttr.Combo != TipoDatiCollegati.Nessuno)
            {
                var repoItem = AddCombo(widthAttr, titolo);
                return repoItem;
            }
            else if (item.PropertyType.FullName.Contains("String"))
            {

                var maxLength = (MaxLengthAttribute)item.GetCustomAttributes(typeof(MaxLengthAttribute), true).FirstOrDefault();

                if (widthAttr != null && widthAttr.MultiLine > 0)
                {

                    var editor = new RepositoryItemMemoEdit();
                    editor.AutoHeight = true;
                    //row.Height = row.Height * widthAttr.MultiLine;
                    if (maxLength != null)
                        editor.MaxLength = maxLength.Length;

                    return editor;
                }
                else
                {

                    var editor = new RepositoryItemTextEdit();
                    if (maxLength != null)
                        editor.MaxLength = maxLength.Length;
                    if (_vGrid != null)
                    {
                        _vGrid.Appearance.SelectedCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        _vGrid.Appearance.FocusedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    }
                    return editor;
                }

            }
            else if (item.PropertyType.FullName.Contains("Boolean"))
            {
                var check = new RepositoryItemCheckEdit();
                check.ValueChecked = true;
                check.ValueUnchecked = false;
                return check;
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
                if (row != null)
                    row.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;



                return num;
            }
            else if (item.PropertyType.FullName.Contains("Int32"))
            {
                var num = new RepositoryItemTextEdit();
                num.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                num.Mask.UseMaskAsDisplayFormat = true;

                if (widthAttr != null && widthAttr.Percentuale)
                {
                    num.Mask.EditMask = "P0";
                }
                else
                {
                    num.Mask.EditMask = "D";
                }
                num.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                num.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;

                return num;
            }
            else if (item.PropertyType.FullName.Contains("DateTime"))
            {
                var num = new RepositoryItemTextEdit();
                num.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                num.Mask.UseMaskAsDisplayFormat = true;
                if (widthAttr != null)
                    if (widthAttr.MaskDate != "")
                    {
                        num.Mask.EditMask = widthAttr.MaskDate;
                    }
                    else if (widthAttr.DateTimeView)
                    {
                        num.Mask.EditMask = "HH:mm:ss dd/MM/yyyy";
                    }
                    else if (widthAttr.DateView)
                    {
                        num.Mask.EditMask = "dd/MM/yyyy";

                    }


                return num;
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
                return null;
            }
        }
        private string AggingiRigaECategoria(string prefixText, EditorRow row, CustomUIViewAttribute widthAttr)
        {
            if (_vGrid == null)
                return "";
            if (prefixText == "")
            {
                if (widthAttr != null && !string.IsNullOrEmpty(widthAttr.Category))
                {
                    prefixText = widthAttr.Category;
                }

            }
            if (prefixText.Length > 0)
            {
                var categoryContained = this._vGrid.Rows.Where(a => a is CategoryRow
                && a.Properties.Caption == prefixText).FirstOrDefault();

                if (categoryContained == null)
                {
                    var category = new CategoryRow();
                    category.Properties.Caption = prefixText;
                    this._vGrid.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
                                    category});
                    category.Appearance.BackColor = Color.FromArgb(98, 147, 188);
                    category.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                    this._vGrid.Appearance.Category.Font = new Font(category.Appearance.Font.FontFamily, category.Appearance.Font.Size, FontStyle.Bold);
                    categoryContained = category;

                }
                categoryContained.ChildRows.Add(row);
            }
            else
            {
                this._vGrid.Rows.Add(row);
            }

            return prefixText;
        }
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
            if (editorRow != null)
                editorRow.Tag = prop;

            var hideAttr = (CustomHideUIAttribute)item.GetCustomAttributes(typeof(CustomHideUIAttribute), true).FirstOrDefault();

            if ((attribute != null && attribute.Enable == false
                )
                ||
                hideAttr != null
                )
            {
                if (editorRow != null)
                {
                    editorRow.Enabled = false;
                }
                if (_dgvRighe != null)
                {
                    _dgvRighe.Columns[item.Name].OptionsColumn.AllowEdit = false;
                }
            }

        }
        List<CopyProp> _listProp = new List<CopyProp>();
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
            int indexProp = 0;
            EvaluateEnableField evaluate = new EvaluateEnableField();
            foreach (var item in listProp)
            {
                indexProp++;
                var hideAttr = item.CustomAttributes.Where(a => a.AttributeType == typeof(CustomHideUIAttribute)).FirstOrDefault();


                GridColumn col = null;
                if (_dgvRighe != null)
                {
                    col = _dgvRighe.Columns[item.Name];
                }
                if (hideAttr != null || !evaluate.VisibleItem(objToBind, item.Name))
                {
                    if (col != null)
                    {
                        col.Visible = false;
                    }
                    continue;

                }


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

                var widthAttr = (CustomUIViewAttribute)item.GetCustomAttributes(typeof(CustomUIViewAttribute), true).FirstOrDefault();
                string titolo = GetTitle(item, widthAttr);


                EditorRow row = null;

                if (this._vGrid != null)
                {
                    row = new EditorRow();
                    row.Properties.FieldName = item.Name;
                    row.Properties.Value = item.GetValue(objToBind);
                    row.Properties.Caption = titolo;
                }

                if (col != null)
                {
                    col.Caption = titolo;
                    col.VisibleIndex = indexProp;
                }

                var itemRepo = ImpostaEditor(objToBind, item, row, widthAttr, titolo);

                if (itemRepo != null)
                {
                    if (row != null)
                        row.Properties.RowEdit = itemRepo;
                    if (_gcControl != null)
                    {
                        _gcControl.RepositoryItems.Add(itemRepo);

                        var colToEdit = _dgvRighe.Columns[item.Name];
                        colToEdit.ColumnEdit = itemRepo;
                        if (widthAttr != null && widthAttr.Percentuale)
                        {
                            colToEdit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            colToEdit.DisplayFormat.FormatString = "{0:n0} %";
                        }
                    }
                }
                //if (row.Properties.RowEdit != null)
                if (itemRepo != null)
                {
                    AggingiRigaECategoria(prefixText, row, widthAttr);
                    BindObj(item, row, objToBind, widthAttr);
                }
            }
        }

        private static string GetTitle(System.Reflection.PropertyInfo item, CustomUIViewAttribute widthAttr)
        {
            string titolo = UtilityView.GetTextSplitted(item.Name);

            if (widthAttr != null)
            {
                if (widthAttr.Titolo.Length > 0)
                {
                    titolo = widthAttr.Titolo;
                }
            }


            return titolo;
        }
    }
}
