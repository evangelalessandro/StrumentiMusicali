using PropertyChanged;
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

        NonSpecificato = -100
    }
    [AddINotifyPropertyChangedInterface]
    public class Articolo : BaseEntity
    {
        [CustomUIViewAttribute(Ordine = 6)]
        public bool BoxProposte { get; set; }

        [CustomUIViewAttribute(Ordine = 7)]
        public bool CaricainECommerce { get; set; } = true;
        [CustomUIViewAttribute(Ordine = 8)]
        public bool CaricaInMercatino { get; set; } = true;

        [CustomUIViewAttribute(Width = 350, Ordine = 1, Combo = TipoDatiCollegati.Categorie)]
        [Required]
        public int CategoriaID { get; set; }

        [CustomHideUIAttribute]
        public virtual Categoria Categoria { get; set; }

        [CustomUIViewAttribute(Ordine = 9, Width = 250)]
        [MaxLength(100)]
        public string CodiceABarre { get; set; }

        [CustomUIViewAttribute(Ordine = 4, Combo = TipoDatiCollegati.Condizione)]
        public enCondizioneArticolo Condizione { get; set; } = enCondizioneArticolo.Nuovo;

        [CustomHideUIAttribute]
        public bool ImmaginiDaCaricare { get; set; } = true;



        [CustomHideUIAttribute]
        public bool Pinned { get; set; }

        [CustomUIViewAttribute(Width = 80, Ordine = 39)]
        [Required]
        public decimal Prezzo { get; set; } = 0;

        [CustomUIViewAttribute(Width = 80, Ordine = 41)]
        [Required]
        public bool NonImponibile { get; set; } = false;

        [CustomUIViewAttribute(Width = 60, Ordine = 44)]
        public bool PrezzoARichiesta { get; set; } = false;

        [CustomUIViewAttribute(Width = 60, Ordine = 45)]
        public decimal PrezzoBarrato { get; set; } = 0;

        [CustomUIViewAttribute(Width = 80, Ordine = 40)]
        public decimal PrezzoAcquisto { get; set; } = 0;

        [MaxLength(2000)]
        [CustomUIViewAttribute(Width = 500, Ordine = 111, MultiLine = 4)]
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

        
        [NotMapped]
        [CustomHideUIAttribute]
        public bool? ShowLibro { get; set; } = null;


        [NotMapped]
        [CustomUIViewAttribute(Width = 80, Ordine = 50, Enable = false)]
        public int QtaNegozio { get; set; } = 0;

        public void UpdateTitolo()
        {
            if (this.ID == 0)
            {
                if (this.ShowLibro.GetValueOrDefault(false))
                {
                    Titolo = (Libro.Autore + " " + 
                                Libro.TitoloDelLibro + " " + 
                                Libro.Genere + " " + 
                                Libro.Edizione + " " + 
                                Libro.Edizione2 + " " + 
                                Libro.Ordine).Trim().Replace("    ", " ").Replace("  ", " ");
                }
                else
                {
                    Titolo = (this.Strumento.Marca + " " + this.Strumento.Colore + " " + Strumento.Modello).Trim().Replace("    ", " ").Replace("  ", " ");

                }
            }
        }
        [NotMapped]
        [CustomHideUIAttribute]
        public bool? ShowStrumento { get; set; } = null;

        [CustomUIViewAttribute(Ordine = 5)]
        public virtual StrumentoAcc Strumento { get; set; } = new StrumentoAcc();

    }
    public class Libro
    {
        [MaxLength(100)]
        public string TitoloDelLibro { get; set; }
        [MaxLength(20)]
        public string Autore { get; set; }
        [MaxLength(20)]
        public string Edizione { get; set; }
        [MaxLength(20)]
        public string Edizione2 { get; set; }
        [MaxLength(20)]
        public string Genere { get; set; }
        [MaxLength(20)]
        public string Ordine { get; set; }
        public string Settore { get; set; }

    }

    public class StrumentoAcc
    {
        [CustomUIViewAttribute(Width = 150, Ordine = 2)]
        [MaxLength(100)]
        public string Marca { get; set; } = "";

        [CustomUIViewAttribute(Width = 150, Ordine = 3)]
        [MaxLength(100)]
        public string Modello { get; set; } = "";

        [CustomUIViewAttribute(Ordine = 4)]
        [MaxLength(50)]
        public string Colore { get; set; } = "";

    }
}