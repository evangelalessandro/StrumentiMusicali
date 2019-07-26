using PropertyChanged;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
    [AddINotifyPropertyChangedInterface]
    public class Pagamento : BaseEntity
    {
        public Pagamento()
        {

        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        [CustomUIViewAttribute(Width = 250, Ordine = 1)]
        [Required]
        [MaxLength(100)]
        public string Cognome { get; set; }

        [CustomUIViewAttribute(Width = 250, Ordine = 5)]
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [CustomUIViewAttribute(Width = 250, Ordine = 10)]
        [MaxLength(120)]
        public string Indirizzo { get; set; }


        [CustomUIViewAttribute(Width = 250, Ordine = 15)]
        [MaxLength(60)]
        public string Telefono { get; set; }

        [CustomUIViewAttribute(Width = 250,
            Ordine = 20, Titolo = "Numero carta d'identità")]
        [MaxLength(60)]
        public string CartaIdentita { get; set; }

        [CustomUIViewAttribute(Width = 250, Ordine = 125, Titolo = "Note", MultiLine = 4)]
        [MaxLength(500)]
        public string Note { get; set; }


        [CustomUIViewAttribute(Width = 500, Ordine = 50, Titolo = "Articolo")]
        [MaxLength(100)]
        public string ArticoloAcq { get; set; }

        [NotMapped]
        [CustomHideUIAttribute]
        public Boolean ShowNumeroRate {
            get {
                if (ID == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        [NotMapped]
        [CustomUIViewAttribute(Width = 100, Ordine = 55)]
        public Int32 NumeroRate { get; set; } = 3;

        //public event PropertyChangedEventHandler PropertyChanged;

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 60)]
        public decimal ImportoTotale { get; set; }


        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 65, Titolo = "Data Inizio")]
        public DateTime DataInizio { get; set; } = DateTime.Now;

        [NotMapped]
        [CustomHideUIAttribute]
        public Boolean ShowImportoRata {
            get {
                if (ID == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        [NotMapped]
        [CustomHideUIAttribute]
        public Boolean ShowDataRata { get { return ShowImportoRata; } }

        [NotMapped]
        [CustomHideUIAttribute]
        public Boolean ShowImportoResiduo { get { return ShowImportoRata; } }

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 70)]
        public decimal ImportoRata { get; set; }
        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 75, Titolo = "Data pagamento rata")]
        public DateTime DataRata { get; set; } = DateTime.Now;

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 80)]
        public decimal ImportoResiduo { get; set; }

        [Required]
        [CustomHideUIAttribute]
        /*raggruppamento dei pagamenti*/
        public Guid IDPagamentoMaster { get; set; }




    }

    public class PagamentoDocumenti : BaseEntity
    {

        [Required]
        public string FileName { get; set; }
        [Required]
        public string PathFile { get; set; }

        [CustomHideUIAttribute]
        [Required]
        public int Ordine { get; set; } = 0;

        [CustomHideUIAttribute]
        [Required]
        public Guid IDPagamentoMaster { get; set; }


    }
}