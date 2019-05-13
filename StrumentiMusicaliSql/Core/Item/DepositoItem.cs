using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core.Item
{
    public class DepositoItem : BaseItem<Deposito>
	{
		public DepositoItem()
		{

		}
		public DepositoItem(Deposito deposito)
		{
			NomeDeposito = deposito.NomeDeposito;
			Entity = deposito;
			ID = deposito.ID;
		}
		public string NomeDeposito { get; set; }
	}
}
