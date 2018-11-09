using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public enum enCondizioneArticolo
	{
		Nuovo = 1,
		ExDemo = 3,
		UsatoGarantito = 5
	}

	public class Articolo : BaseEntity
	{
		public bool BoxProposte { get; set; }

		public bool CaricainEcommerce { get; set; } = true;

		public bool CaricainMercatino { get; set; } = true;

		[Required]
		public int CategoriaID { get; set; }

		[CustomHideUIAttribute]
		public virtual Categoria Categoria { get; set; }

		[MaxLength(100)]
		public string CodiceAbarre { get; set; }

		public enCondizioneArticolo Condizione { get; set; } = enCondizioneArticolo.Nuovo;

 
		public bool ImmaginiDaCaricare { get; set; } = true;

		[MaxLength(100)]
		public string Marca { get; set; }

		public bool Pinned { get; set; }

		[Required]
		public decimal Prezzo { get; set; } = 0;

		public bool PrezzoARichiesta { get; set; } = false;

		public decimal PrezzoBarrato { get; set; } = 0;

		[MaxLength(2000)]
		public string Testo { get; set; }

		[MaxLength(100), Required]
		public string Titolo { get; set; }

		public bool UsaAnnuncioTurbo { get; set; }

		[MaxLength(100)]
		public string Note1 { get; set; }
		[MaxLength(100)]
		public string Note2 { get; set; }
		[MaxLength(100)]
		public string Note3 { get; set; }
		[MaxLength(100)]
		public string Rivenditore { get; set; }

		public virtual Libro Libro { get; set; } = new Libro();
		[MaxLength(50)]
		public string TagImport { get; set; }
		[MaxLength(50)]
		public string Colore { get; set; }
	}
	public class Libro
	{
		public string Autore { get; set; }
		public string Edizione { get; set; }
		public string Edizione2 { get; set; }
		public string Genere { get; set; }
		public string Ordine { get; set; }
		public string Settore { get; set; }
	}
}