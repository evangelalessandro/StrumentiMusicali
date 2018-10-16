﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Entity
{
	public class Magazzino
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }

		[Required]
		public string ArticoloID { get; set; }

		public virtual Articolo Articolo { get; set; }

		[Required]
		public int DepositoID { get; set; }

		public virtual Deposito Deposito { get; set; }

		[Required]
		public int Giacenza { get; set; }

	}
}
