using PropertyChanged;
using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Base
{

    [AddINotifyPropertyChangedInterface]
    public class BaseEntity
    {
        public BaseEntity()
        {
        }

        [CustomHideUIAttribute]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [CustomUIViewAttribute(DateTimeView = true, Width = 200, Enable = false, Category = "Info record")]
        public DateTime DataCreazione { get; set; } = DateTime.Now;
        [CustomUIViewAttribute(DateTimeView = true, Width = 200, Enable = false, Category = "Info record")]
        public DateTime DataUltimaModifica { get; set; } = DateTime.Now;



    }
}