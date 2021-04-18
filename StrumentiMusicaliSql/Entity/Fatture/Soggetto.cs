using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
    [Table("Soggetti")]
    public class Soggetto : BaseEntity
    {
        [MaxLength(100)]
        [CustomUIViewAttribute(Ordine = 1, Combo =TipoDatiCollegati.TipiSoggetto)]
        public string TipiSoggetto { get; set; }
        [CustomUIViewAttribute(Ordine = 1)]
        public string RagioneSociale { get; set; }
        [CustomUIViewAttribute(Ordine = 2)]
        public string Nome { get; set; }
        [CustomUIViewAttribute(Ordine = 3)]
        public string Cognome { get; set; }
        [CustomUIViewAttribute(Width = 100, Ordine = 4)]
        public string PIVA { get; set; }
        [CustomUIViewAttribute(Ordine = 5)]
        public string CodiceFiscale { get; set; }
        [CustomUIViewAttribute(Ordine = 6)]
        public bool PersonaGiuridica { get; set; }
        [CustomUIViewAttribute(Ordine = 7)]
        public string RegimeFiscale { get; set; }
        [CustomUIViewAttribute(Ordine = 8)]
        public string Email { get; set; }

        [CustomUIViewAttribute(Ordine = 9)]
        public string Telefono { get; set; }
        [CustomUIViewAttribute(Ordine = 10)]
        public string Fax { get; set; }
        [CustomUIViewAttribute(Ordine = 11)]
        public string Cellulare { get; set; }
        [CustomUIViewAttribute(Ordine = 12)]
        public string LuogoDestinazione { get; set; }
        [CustomUIViewAttribute(Width = 120, Ordine = 13)]
        public string NomeBanca { get; set; }
        [CustomUIViewAttribute(Width = 80, Ordine = 14)]
        public int BancaAbi { get; set; }
        [CustomUIViewAttribute(Width = 80, Ordine = 15)]
        public int BancaCab { get; set; }

        [CustomUIViewAttribute(Width = 400, MultiLine = 5, Ordine = 40)]
        public string Note { get; set; }

        [CustomHideUI()]
        public virtual int CodiceClienteOld { get; set; }
        [CustomUIViewAttribute(Ordine = 20)]
        public virtual PecConfig PecConfig { get; set; } = new PecConfig();
        [CustomUIViewAttribute(Ordine = 21)]
        public virtual Indirizzo Indirizzo { get; set; } = new Indirizzo();

        public bool FatturaVersoPA { get; set; }
    }
    public enum enTipiSoggetto
    {
        Cliente,
        Fornitore,
    }
    public class PecConfig
    {
        public string CodicePec { get; set; }
        public bool RicezioneConCodicePec { get; set; }
        public string EmailPec { get; set; }
    }
    public class Indirizzo
    {
        public string IndirizzoConCivico { get; set; }
        public string Comune { get; set; }
        public string ProvinciaSigla { get; set; }
        public string Citta { get; set; }

        [CustomUIViewAttribute(Width = 80)]
        public string Cap { get; set; }
    }
}