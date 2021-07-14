using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Setting
{
    public class SettingPostazioni : Base.BaseEntity
    {
        public int Versione { get; set; } = 0;

        public string PostazioneServer { get; set; } = "SERVERSM";

    }
}