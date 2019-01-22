using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public class DatiIntestazioneStampaFattura
	{
		[Key]
		[CustomHideUIAttribute]
		public int ID { get; set; } = 1;
		public string NegozioRagSoc { get; set; } = "";
		public string NegozioIndirizzo { get; set; } = "";
		public string NegozioTelefonoFax { get; set; } = "";
		public string NegozioPIVA { get; set; } = "";
		public string NegozioCF { get; set; } = "";
		public string NegozioEmail { get; set; } = "";
		public string NegozioEmailPEC { get; set; } = "";
		public string NomeBanca { get; set; } = "";
		public string IBAN { get; set; } = "";
	}
}
