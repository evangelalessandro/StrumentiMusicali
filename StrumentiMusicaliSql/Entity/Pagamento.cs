using PropertyChanged;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using StrumentiMusicali.Library.Repo;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace StrumentiMusicali.Library.Entity
{
    [AddINotifyPropertyChangedInterface]
    public class Pagamento : BaseEntity
    {
        public Pagamento()
        {
            if ((this as INotifyPropertyChanged) != null)
                (this as INotifyPropertyChanged).PropertyChanged += Pagamento_PropertyChanged;
        }

        private void Pagamento_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var prova = UpdateImportoTotale;
        }

        [CustomUIViewAttribute(Width = 250, Ordine = 1)]
        [Required]
        public string Cognome { get; set; }

        [CustomUIViewAttribute(Width = 250, Ordine = 2)]
        [Required]
        public string Nome { get; set; }

        [Required]
        [CustomUIViewAttribute(Width = 450, Ordine = 3, Combo = TipoDatiCollegati.Articoli, Titolo = "Articolo")]
        [AlsoNotifyFor("UpdateImportoTotale")]
        public int ArticoloID { get; set; }

        [CustomHideUIAttribute]
        [AlsoNotifyFor("UpdateImportoTotale")]
        public virtual Articolo Articolo { get; set; }

        [NotMapped]
        [AlsoNotifyFor("ImportoTotale")]
        [CustomHideUIAttribute]
        public decimal UpdateImportoTotale {
            set {

            }
            get {
                if (ImportoTotale==0)
                ImportoTotale = GetImportoArticolo();
                return ImportoTotale;
            }
        }
        private decimal GetImportoArticolo()
        {
            if (this.ID == 0)
            {

                using (var uof = new UnitOfWork())
                {
                    var art = uof.ArticoliRepository.Find(a => a.ID == ArticoloID).Select(a => a.Prezzo).DefaultIfEmpty(0).FirstOrDefault();

                    return art;

                }

            }
            return ImportoTotale;

        }
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
        [CustomUIViewAttribute(Width = 100, Ordine = 4)]
        public Int32 NumeroRate { get; set; } = 3;

        //public event PropertyChangedEventHandler PropertyChanged;

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 5)]
        public decimal ImportoTotale { get; set; }


        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 6, Titolo = "Data Inizio")]
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
        [CustomUIViewAttribute(Width = 100, Ordine = 9)]
        public decimal ImportoRata { get; set; }
        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 10, Titolo = "Data pagamento rata")]
        public DateTime DataRata { get; set; } = DateTime.Now;

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 11)]
        public decimal ImportoResiduo { get; set; }


    }
}