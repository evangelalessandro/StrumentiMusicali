using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    public class ArticoloImportato :BaseEntity
    {

        [CustomUIView(Ordine = 10, Enable = false, Category = "Allineamento")]
        [MaxLength(50)]
        public string CodiceArticoloEcommerce { get; set; } = "";

        public string XmlDatiProdotto { get; set; }

        public Byte[] Immagine1 { get; set; }
        public Byte[] Immagine2 { get; set; }
        public Byte[] Immagine3 { get; set; }

    }
}
