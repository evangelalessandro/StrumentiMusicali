using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Scontrino
{
    public class ScontrinoTestata : BaseEntity
    {
        [StringLength(50,ErrorMessage ="Occorre impostare nome postazione")]
        
        public string NomePostazione { get; set; } = "";

        public enStatoElaborato StatoElaborazione { get; set; } = enStatoElaborato.DaElaborare;


        public DateTime DataErrore { get; set; } = new DateTime(1900, 1, 1);
        public DateTime DataConfermaSuccesso { get; set; } = new DateTime(1900, 1, 1);

        [StringLength(200,ErrorMessage = "Occorre specificare il nome file",MinimumLength =10)]
        public string NomeFile { get; set; } = "";
        
        [CustomHideUI()]
        public int Deposito { get; set; }

        public decimal Totale { get; set; }
    }

    public enum enStatoElaborato
    {
        DaElaborare,
        Elaborato,
        InErrore,
        AnnullatoUtente
    }
}