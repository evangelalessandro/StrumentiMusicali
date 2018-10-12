﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicaliSql.Entity
{
    public class FotoArticolo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

		public string ArticoloID { get; set; }

		public virtual Articolo Articolo { get; set; }
	 
        [Required]
        public string UrlFoto { get; set; }

		[Required]
		public int Ordine { get; set; } = -1;

	}
}
