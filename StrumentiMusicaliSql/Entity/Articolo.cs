using PropertyChanged;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel;
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
        public Articolo()
        {
            Strumento.PropertyChanged += Strumento_PropertyChanged;
            Libro.PropertyChanged += Strumento_PropertyChanged;
        }


        private void Strumento_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTitolo = Titolo;

        }

        [CustomUIViewAttribute(Ordine = 6, Category = "Online")]
        public bool BoxProposte { get; set; }

        [CustomUIViewAttribute(Ordine = 7,Titolo = "Carica in E-Commerce",Category ="Online")]
        public bool CaricainECommerce { get; set; } = true;
        [CustomUIViewAttribute(Ordine = 8, Titolo = "Carica in Mercatino", Category = "Online")]
        public bool CaricaInMercatino { get; set; } = true;

        [CustomUIViewAttribute(Width = 350, Ordine = 1, Combo = TipoDatiCollegati.Categorie,Titolo ="Categoria")]
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

        [CustomUIViewAttribute(Width = 80, Ordine = 39, Category = "Prezzo",Money =true)]
        [Required]
        public decimal Prezzo { get; set; } = 0;

        [CustomUIViewAttribute(Width = 80, Ordine = 41, Category = "Prezzo", Money = true)]
        [Required]
        public bool NonImponibile { get; set; } = false;

        [CustomUIViewAttribute(Width = 60, Ordine = 44, Category = "Prezzo", Money = true)]
        public bool PrezzoARichiesta { get; set; } = false;

        [CustomUIViewAttribute(Width = 60, Ordine = 45, Category = "Prezzo", Money = true)]
        public decimal PrezzoBarrato { get; set; } = 0;

        [CustomUIViewAttribute(Width = 80, Ordine = 40,Category ="Prezzo", Money = true)]
        public decimal PrezzoAcquisto { get; set; } = 0;

        [MaxLength(2000)]
        [CustomUIViewAttribute(Width = 500, Ordine = 111, MultiLine = 4,Titolo ="Testo annuncio", Category = "Online")]
        public string Testo { get; set; }

        [CustomUIViewAttribute(Width = 500, Ordine = 3,Titolo ="Titolo annuncio")]
        [MaxLength(100), Required]
        public string Titolo {
            get;
            set;
        }

        [CustomUIViewAttribute(Ordine = 5, Category = "Online")]
        public bool UsaAnnuncioTurbo { get; set; }

        [CustomUIViewAttribute(Ordine = 20, Category = "Note")]
        [MaxLength(100)]
        [AlsoNotifyFor("UpdateTitolo")]
        public string Note1 {
            get;
            set;
        }

        [CustomUIViewAttribute(Ordine = 21, Category = "Note")]
        [MaxLength(100)]
        public string Note2 { get; set; } = "";

        [CustomUIViewAttribute(Ordine = 22, Category = "Note")]
        [MaxLength(100)]
        public string Note3 { get; set; } = "";
        

        [CustomUIViewAttribute(Ordine = 30)]
        [AlsoNotifyFor("UpdateTitolo")]
        public virtual Libro Libro { get; set; } = new Libro();

        [MaxLength(50)]
        [CustomHideUIAttribute]
        public string TagImport { get; set; }


        [NotMapped]
        [CustomHideUIAttribute]
        public bool? ShowLibro { get; set; } = null;

        /*Indica se è un libro*/
        public bool IsLibro()
        {
            var dato= (!string.IsNullOrEmpty(Libro.Autore)
                    || !string.IsNullOrEmpty(Libro.Edizione)
                    || !string.IsNullOrEmpty(Libro.Settore));
            if (dato)
            {
                System.Diagnostics.Debug.Print("Prova");
            }
            return dato;
        }

        [NotMapped]
        [CustomUIViewAttribute(Width = 80, Ordine = 50, Enable = false, Category = "Magazzino",Titolo ="Quantità in negozio")]
        public int QtaNegozio { get; set; } = 0;
        [NotMapped]
        [AlsoNotifyFor("Titolo")]
        [CustomHideUIAttribute]
        public string UpdateTitolo {
            set {

            }
            get {
                Titolo = GetTitoloDesc();
                return Titolo;
            }
        }
        private string GetTitoloDesc()
        {
            if (this.ID == 0)
            {


                string titolo = "";
                if (this.ShowLibro.GetValueOrDefault(false))
                {
                    titolo = (Libro.Autore + " " +
                                Libro.TitoloDelLibro + " " +
                                Libro.Genere + " " +
                                Libro.Edizione + " " +
                                Libro.Edizione2 + " " +
                                Libro.Ordine).Trim().Replace("    ", " ").Replace("  ", " ");
                }
                else
                {
                    titolo = (this.Strumento.Marca + " "  + Strumento.Nome + " " + Strumento.Modello + " " + this.Strumento.Colore).Trim().Replace("    ", " ").Replace("  ", " ");

                }
                return titolo;
            }
            return Titolo;

        }
        [NotMapped]
        [CustomHideUIAttribute]
        public bool? ShowStrumento { get; set; } = null;

        [AlsoNotifyFor("UpdateTitolo")]
        [CustomUIViewAttribute(Ordine = 5,ShowGroupName =false)]
        public virtual StrumentoAcc Strumento { get; set; } = new StrumentoAcc();

    }
    public class Libro : INotifyPropertyChanged
    {
        [MaxLength(100)]
        public string TitoloDelLibro { get; set; }
        [MaxLength(20)]
        [CustomUIViewAttribute( Combo = TipoDatiCollegati.LibroAutore, ComboLibera = true)]
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

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StrumentoAcc : INotifyPropertyChanged
    {
        [CustomUIViewAttribute(Width = 150, Ordine = 20,Combo =TipoDatiCollegati.Marca,ComboLibera =true)]
        [MaxLength(100)]
        public string Marca { get; set; } = "";

        [CustomUIViewAttribute(Width = 150, Ordine = 30, Combo = TipoDatiCollegati.Modello, ComboLibera = true)]
        [MaxLength(100)]
        public string Modello { get; set; } = "";

        [CustomUIViewAttribute(Width = 150, Ordine = 40)]
        [MaxLength(100)]
        public string CodiceOrdine { get; set; } = "";

        [CustomUIViewAttribute(Ordine = 50, Combo = TipoDatiCollegati.Rivenditore, ComboLibera = true)]
        [MaxLength(100)]
        public string Rivenditore { get; set; } = "";

        [CustomUIViewAttribute(Ordine = 60, Combo = TipoDatiCollegati.Colore, ComboLibera = true)]
        [MaxLength(50)]
        public string Colore { get; set; } = "";


        [CustomUIViewAttribute(Ordine = 70,Titolo ="Nome strumento", Combo = TipoDatiCollegati.NomeStrumento, ComboLibera = true)]
        [MaxLength(50)]
        public string Nome { get; set; } = "";

        public event PropertyChangedEventHandler PropertyChanged;

    }
}