using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
    public class SettingDocumentiPagamenti : Base.BaseEntity
    {

        [MaxLength(200)]

        public string CartellaReteDocumentiPagamenti { get; set; }



    }
}