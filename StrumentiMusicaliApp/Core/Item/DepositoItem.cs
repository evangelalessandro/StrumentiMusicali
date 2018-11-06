using StrumentiMusicali.App.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Item
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
