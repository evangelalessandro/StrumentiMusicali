using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Setting
{
    public class SettingScontrino : Base.BaseEntity
    {
        
        [CustomUIView(Ordine = 20,Titolo ="Cartella destinazione")]
        public string FolderDestinazione { get; set; } = "";

         
    }

}