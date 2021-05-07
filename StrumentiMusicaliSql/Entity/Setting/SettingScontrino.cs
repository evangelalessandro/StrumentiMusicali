using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Setting
{
    public class SettingScontrino : Base.BaseEntity
    {
        
        [CustomUIView(Ordine = 20,Titolo ="Cartella destinazione")]
        public string FolderDestinazione { get; set; } = "";

        [CustomUIView(Ordine = 21, Titolo = "Pulsante per inserire articolo generico, non anagrafico")]
        public bool ArticoloGenerico { get; set; } = true;
    }

}