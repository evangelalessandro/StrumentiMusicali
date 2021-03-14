using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Setting
{
    public class WooCommerceSetting
    {
        [MaxLength(200)]
        public string keyPrivate { get; set; }
        [MaxLength(200)]
        public string keyPublic { get; set; }

        [MaxLength(200)]
        public string WebServiceUrl { get; set; }

        [CustomUIView(DateTimeView = true, Enable = false)]
        public System.DateTime UltimoAggiornamento { get; set; } = new DateTime(1900, 1, 1);

        [CustomUIView(DateTimeView = true, Enable = false, Titolo = @"Data dell'ultimo ordine modificato\creato verificato per le giacenze")]
        public System.DateTime DataUltimoOrdineGiacenza { get; set; } = new DateTime(1900, 1, 1);

    }
}