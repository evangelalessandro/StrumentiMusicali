﻿
using System;
using System.Drawing;

namespace StrumentiMusicali.Library.Core.Attributes
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

        public Bitmap Icona { get; set; }

         
    }
}