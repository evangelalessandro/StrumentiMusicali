using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public class Articolo : BaseEntity
	{
		[Key]
		[MaxLength(50), Required]
		public string ID { get; set; }
		[Required]
		public int Categoria { get; set; }
		public enCondizioneArticolo Condizione { get; set; } = enCondizioneArticolo.Nuovo;
		[MaxLength(100)]
		public string Marca { get; set; }
		[MaxLength(100), Required]
		public string Titolo { get; set; }
		[MaxLength(2000), Required]
		public string Testo { get; set; }
		[Required]
		public decimal Prezzo { get; set; } = 0;

		public decimal PrezzoBarrato { get; set; } = 0;
		public bool PrezzoARichiesta { get; set; } = false;
		public string UrlSchedaProdotto { get; set; }
		public string UrlSchedaProdottoTurbo { get; set; }
		public bool BoxProposte { get; set; }
		public bool UsaAnnuncioTurbo { get; set; }
		public bool Pinned { get; set; }

		[MaxLength(100)]
		public string CodiceAbarre { get; set; }

		public bool CaricainEcommerce { get; set; } = true;

		public bool CaricainMercatino { get; set; } = true;


		public bool ImmaginiDaCaricare { get; set; } = true;
	}
	public enum enCondizioneArticolo
	{
		Nuovo = 1,
		ExDemo = 3,
		UsatoGarantito = 5
	}
}
