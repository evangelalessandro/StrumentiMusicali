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

   

        public PrestaShopSetting PrestaShopSetting { get; set; } = new PrestaShopSetting();


        public WooCommerceSetting WooCommerceSetting { get; set; } = new WooCommerceSetting();
    }
}