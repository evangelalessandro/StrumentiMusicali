using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Ecomm
{
    public class OrdineRigaWeb : BaseEntity
    {
        public int ArticoloID { get; set; }

        public virtual Articolo Articolo { get; set; }

        public int OrdineWebID { get; set; }

        public virtual OrdineWeb OrdineWeb { get; set; }
         
        public int Quantita { get; set; }
         
    }
}