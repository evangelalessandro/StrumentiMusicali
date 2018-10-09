using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliSql.Entity
{
    public class Articolo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Required]
        public int Categoria { get; set; }
        public CondizioneArticolo Condizione { get; set; } = CondizioneArticolo.Nuovo;
        [MaxLength(100),Required]
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
		public DateTime DataCreazione { get; set; }
		public DateTime DataUltimaModifica { get; set; }
		public bool Pinned { get; set; }
		public int Giacenza { get; set; }

		public virtual ICollection<FotoArticolo> getFotoArticolo { get; set; } 

		 
	}
    public enum CondizioneArticolo
    {
        Nuovo=1,
        ExDemo=3,
        UsatoGarantito=5
    }
}
