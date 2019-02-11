using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Localization;


namespace StrumentiMusicali.App
{
	
	public class ItalianGridLocalizer : GridLocalizer
	{
		public override string Language { get { return "Italian"; } }
		public override string GetLocalizedString(GridStringId id)
		{
			string ret = "";
			switch (id)
			{
				// ...  
				case GridStringId.GridGroupPanelText: return "Trascina un'intestazione di colonna in quest'area per raggrupparla";
				case GridStringId.MenuColumnClearSorting: return "Reset ordinamento";
				case GridStringId.MenuGroupPanelHide: return "Nascondi pannello raggruppamento";
				case GridStringId.MenuColumnRemoveColumn: return "Rimuovi colonna";
				case GridStringId.MenuColumnFilterEditor: return "Filtra e modifica";
				case GridStringId.MenuColumnFindFilterShow: return "Mostra ricerca";
				case GridStringId.MenuColumnAutoFilterRowShow: return "Mostra auto filtro";
				case GridStringId.MenuColumnAutoFilterRowHide: return "Nascondi auto filtro";
				case GridStringId.MenuColumnSortAscending: return "Ordinamento crescente";
				case GridStringId.MenuColumnSortDescending: return "Ordinamento decrescente";
				case GridStringId.MenuColumnGroup: return "Raggruppa per campo";
				case GridStringId.MenuColumnUnGroup: return "Togli raggruppamento per campo";
				case GridStringId.MenuColumnClearAllSorting: return "Resetta ordinamenti";
				case GridStringId.MenuGroupPanelShow: return "Visualizza box gruppamenti";

				case GridStringId.MenuColumnColumnCustomization: return "Scegli colonne";
				case GridStringId.MenuColumnBestFit: return "Ottimizza larghezza colonna";
				case GridStringId.MenuColumnFilter: return "Visualizza box gruppamenti";
				case GridStringId.MenuColumnClearFilter: return "Cancella filtri";
				case GridStringId.MenuColumnBestFitAllColumns: return "Ottimizza larghezza (tutte colonne)";
				// ...  
				default:
					ret = base.GetLocalizedString(id);
					break;
			}
			return ret;
		}
	}

	//public class GermanEditorsLocalizer : Localizer
	//{
	//	public override string Language { get { return "Deutsch"; } }
	//	public override string GetLocalizedString(StringId id)
	//	{
	//		switch (id)
	//		{
	//			// ... 
	//			case StringId.NavigatorTextStringFormat: return "Zeile {0} von {1}";
	//			case StringId.PictureEditMenuCut: return "Ausschneiden";
	//			case StringId.PictureEditMenuCopy: return "Kopieren";
	//			case StringId.PictureEditMenuPaste: return "Einfugen";
	//			case StringId.PictureEditMenuDelete: return "Loschen";
	//			case StringId.PictureEditMenuLoad: return "Laden";
	//			case StringId.PictureEditMenuSave: return "Speichern";
	//				// ... 
	//		}
	//		return "";
	//	}
	//}
}

