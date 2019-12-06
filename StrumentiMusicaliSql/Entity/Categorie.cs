using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
    public class Categoria : BaseEntity
    {

        [MaxLength(50)]
        public string Reparto { get; set; }
        [MaxLength(50)]
        public string Nome { get; set; }
        public string CategoriaCondivisaCon { get; set; }
        public int Codice { get; set; }
    }
}