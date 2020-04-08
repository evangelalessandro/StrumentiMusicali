using StrumentiMusicali.Library.Core;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Setting
{
    public class SettingSito : Base.BaseEntity
    {

        [MaxLength(200)]

        public string UrlSito { get; set; }
        [MaxLength(200)]

        public string UrlCompletaImmagini { get; set; }
        [MaxLength(200)]

        public string CartellaLocaleImmagini { get; set; }

        [MaxLength(200)]

        public string SoloNomeFileMercatino { get; set; }
        [MaxLength(200)]

        public string SoloNomeFileEcommerce { get; set; }
        [MaxLength(200)]

        public string UrlCompletoFileMercatino { get; set; }
        [MaxLength(200)]
        public string UrlCompletoFileEcommerce { get; set; }

        public PrestaShopSetting prestaShopSetting { get; set; } = new PrestaShopSetting();

    }
    public class PrestaShopSetting
    {
        [MaxLength(200)]
        public string AuthKey { get; set; }
        [MaxLength(200)]
        public string WebServiceUrl { get; set; }

        [CustomUIView(DateTimeView = true, Enable = false)]
        public System.DateTime UltimoAggiornamento { get; set; }

    }
}