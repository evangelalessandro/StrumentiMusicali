using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StrumentiMusicali.App.Core.Controllers.FatturaElett
{
	internal class FatturaHeader
	{
		public enTipoDocumento TipoDocumento { get; set; }
		public DateTime Data { get; set; }
		public string Numero { get; set; }
		public decimal? ImportoTotaleDocumento { get; set; }
		public List<FatturaRighe> Righe { get; set; } = new List<FatturaRighe>();
		public enTipoPagamento ModalitaPagamento { get; set; } = enTipoPagamento.Nessuno;
	}

	internal enum enTipoDocumento
	{
		[Description("fattura")]
		TD01,

		[Description("acconto/anticipo su fattura")]
		TD02,

		[Description("acconto/anticipo su parcella")]
		TD03,

		[Description("nota di credito")]
		TD04,

		[Description("nota di debito")]
		TD05,

		[Description("parcella")]
		TD06,

		[Description("autofattura")]
		TD20
	}

	internal enum enTipoPagamento
	{
		Nessuno,
		Contanti,
		Bonifico,
		Contrassegno,
	}

	internal class FatturaRighe
	{
		public string Descrizione { get; set; }
		public decimal? QTA { get; set; }
		public decimal PrezzoUnitario { get; set; }
		public decimal PrezzoTotale { get; set; }

		/// <summary>
		/// valore compreso tra 0 e iva (22 in caso di iva al 22%
		/// </summary>
		public int AliquotaIVA { get; set; } = 0;
	}
}