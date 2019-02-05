using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public enum enCondizioneArticolo
	{
		Nuovo = 1,
		ExDemo = 3,
		UsatoGarantito = 5,

		NonSpecificato=-100
	}

	public class Articolo : BaseEntity
	{
		[CustomUIViewAttribute(Ordine = 6)]
		public bool BoxProposte { get; set; }

		[CustomUIViewAttribute(Ordine = 7)]
		public bool CaricainECommerce { get; set; } = true;
		[CustomUIViewAttribute(Ordine = 8)]
		public bool CaricaInMercatino { get; set; } = true;

		[CustomUIViewAttribute(Width = 350, Ordine = 1,Combo =TipoDatiCollegati.Categorie )]
		[Required]
		public int CategoriaID { get; set; }

		[CustomHideUIAttribute]
		public virtual Categoria Categoria { get; set; }

		[CustomUIViewAttribute(Ordine = 9,Width =250)]
		[MaxLength(100)]
		public string CodiceABarre { get; set; }

		[CustomUIViewAttribute(Ordine = 4,Combo = TipoDatiCollegati.Condizione)]
		public enCondizioneArticolo Condizione { get; set; } = enCondizioneArticolo.Nuovo;

		[CustomHideUIAttribute]
		public bool ImmaginiDaCaricare { get; set; } = true;

		[CustomUIViewAttribute(Width = 150, Ordine = 2)]
		[MaxLength(100)]
		public string Marca { get; set; }
		[CustomHideUIAttribute]
		public bool Pinned { get; set; }

		[CustomUIViewAttribute(Width = 80, Ordine = 40)]
		[Required]
		public decimal Prezzo { get; set; } = 0;
		[CustomUIViewAttribute(Width = 60, Ordine = 41)]
		public bool PrezzoARichiesta { get; set; } = false;

		[CustomUIViewAttribute(Width = 60, Ordine = 42)]
		public decimal PrezzoBarrato { get; set; } = 0;

		[CustomUIViewAttribute(Width = 80, Ordine = 43)]
		public decimal PrezzoAcquisto { get; set; } = 0;

		[MaxLength(2000)]
		[CustomUIViewAttribute(Width = 500, Ordine = 111,MultiLine =4)]
		public string Testo { get; set; }

		[CustomUIViewAttribute(Width = 500, Ordine = 3)]
		[MaxLength(100), Required]
		public string Titolo { get; set; }

		[CustomUIViewAttribute(Ordine = 5)]
		public bool UsaAnnuncioTurbo { get; set; }

		[CustomUIViewAttribute(Ordine = 20)]
		[MaxLength(100)]
		public string Note1 { get; set; }
		[CustomUIViewAttribute(Ordine = 21)]

		[MaxLength(100)]
		public string Note2 { get; set; }

		[CustomUIViewAttribute(Ordine = 22)]
		[MaxLength(100)]
		public string Note3 { get; set; }

		[CustomUIViewAttribute(Ordine = 15)]
		[MaxLength(100)]
		public string Rivenditore { get; set; }

		[CustomUIViewAttribute(Ordine = 30)]
		public virtual Libro Libro { get; set; } = new Libro();

		[MaxLength(50)]
		[CustomHideUIAttribute]
		public string TagImport { get; set; }

		[CustomUIViewAttribute(Ordine = 45)]
		[MaxLength(50)]
		public string Colore { get; set; }

		[NotMapped]
		[CustomHideUIAttribute]
		public bool? ShowLibro { get; set; } = null;
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