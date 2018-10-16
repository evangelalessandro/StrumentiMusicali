﻿using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
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
