using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliSql.Entity
{
    public class Categorie
    {
        [Key]
        public int ID { get; set; }
        public string Reparto { get; set; }
        public string Categoria { get; set; }
        public string CategoriaCondivisaCon { get; set; }

    }
}
