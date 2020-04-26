using NLog;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.App.View
{
    public partial class PagamentiListView :
        BaseGridViewGeneric<PagamentoItem, ControllerPagamenti, Pagamento>
    {
         

        public PagamentiListView(ControllerPagamenti controller)
            : base(controller)
        {

            onEditItemShowView += ((a, b) =>
            { b.Cancel = false; });

        }



        public override void FormatGrid()
        {

        }


    }
}