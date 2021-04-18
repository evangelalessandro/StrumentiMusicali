
using StrumentiMusicali.Core.Enum;
using System;
using System.Drawing;

namespace StrumentiMusicali.Core.Attributes
{
    public class UIAmbienteAttribute : Attribute
    {
        public UIAmbienteAttribute(bool viewInForm)
        {
            OnlyViewInForm = viewInForm;
        }
        public bool Exclusive { get; set; } = true;
        public bool OnlyViewInForm { get; set; }
        /*Richiede chiusura di questo ambiente se si apre l'altro*/
        public enAmbiente AmbienteMutuale { get; set; } = enAmbiente.NonSpecificato;

        public string NomeAmbiente { get; set; } = "Niente";

        public string Icona { get; set; }

         
    }
}