using PropertyChanged;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    [Table("ListiniPrezzi_VenditaNomi")]
    [AddINotifyPropertyChangedInterface]
    public class ListinoPrezziVenditaNome : BaseEntity
    {
        public ListinoPrezziVenditaNome()
        {
        }


        [CustomUIView(Ordine = 10, Titolo = "Nome listino")]
        [StringLength(30, ErrorMessage = "Il nome deve essere da 1 a 30 caratteri", MinimumLength = 1)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Occorre specificare un nome")]
        [Index(IsClustered = false, IsUnique = true, Order = 1)]
        public string Nome { get; set; }

        [CustomUIView(Ordine = 11, Titolo = "Data inizio validità", DateTimeView = true)]
        public DateTime DataInizioValidita { get; set; } = DateTime.Now;


        [CustomUIView(Ordine = 12, Titolo = "Data fine validità", DateTimeView = true)]
        public DateTime DataFineValidita { get; set; } = DateTime.Now.AddDays(30);

        [CustomUIView(Width = 100, Titolo = "Sconto 1 %", Percentuale = true)]
        [AlsoNotifyFor("PercentualeVariazione")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Valore non valido, deve essere positivo e tra 0 e 100")]
        public int Sconto1 { get; set; } = 0;

        [CustomUIView(Width = 100, Titolo = "Sconto 2 %", Percentuale = true)]
        [AlsoNotifyFor("PercentualeVariazione")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Valore non valido, deve essere positivo e tra 0 e 100")]
        public int Sconto2 { get; set; } = 0;


        [CustomUIView(Width = 100, Titolo = "Sconto 3 %", Percentuale = true)]
        [AlsoNotifyFor("PercentualeVariazione")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Valore non valido, deve essere positivo e tra 0 e 100")]
        public int Sconto3 { get; set; } = 0;

        [CustomUIView(Width = 100, Titolo = "Maggiorazione %", Percentuale = true)]
        [AlsoNotifyFor("PercentualeVariazione")]
        [Range(minimum: 0, maximum: 100, ErrorMessage = "Valore non valido, deve essere positivo e tra 0 e 100")]
        public int Maggiorazione { get; set; } = 0;


        [CustomUIView(Width = 100, Titolo = "Percentuale variazione finale %", Enable = false, Percentuale = true)]
        public int PercentualeVariazione
        {
            get
            {
                var valFinale = 100;
                if (Sconto1>0)
                {
                    valFinale = valFinale * (100 - Sconto1) / 100;
                }
                if (Sconto2 > 0)
                {
                    valFinale = valFinale * (100 - Sconto2) / 100;
                }
                if (Sconto3 > 0)
                {
                    valFinale = valFinale * (100 - Sconto3) / 100;
                }
                if (Maggiorazione > 0)
                {
                    valFinale = valFinale * (100 + Maggiorazione) / 100;
                }
                if (valFinale == 100)
                {
                    return 0;
                }
                else if (valFinale < 100)
                    return -(100 - valFinale);
                else
                {
                    return valFinale - 100 ;
                }
            }
            set {; }
        }
    }
}