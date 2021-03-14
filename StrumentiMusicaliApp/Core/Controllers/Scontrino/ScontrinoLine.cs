using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Controllers.Scontrino
{

    /*
     
        Descrizione : per il tipo di riga V deve sempre esserci , per il resto è opzionale
        Aliquota Iva : per il tipo di riga V deve sempre esserci , per il resto è opzionale
        Quantità : per il tipo di riga V deve sempre esserci , per il resto è opzionale
        Totale : è sempre obbligatoria
        Tipo Riga (V,T) : è sempre obbligatoria
        Extra : è sempre opzionale
    

        Vino Lambrusco ; 22 ;2;0,75;1,50;V;     
     */
    internal class ScontrinoLine
    {
        public string Descrizione { get; set; } = "";
        public decimal Aliquota { get; set; } = 22;
        public decimal PrezzoSingoloIvato { get; set; } = 0;

        public bool TipoTotale { get; set; } = false;
        public decimal Qta { get; set; } = 10.5M;

        public decimal TotaleRiga { get { return Qta * PrezzoSingoloIvato; } }

        public string Pagamento { get; set; }
        /// <summary>
        /// solo per i totali
        /// </summary>
        public decimal TotaleComplessivo { get; set; } = 0;

        public override string ToString()
        {
            StringBuilder dato = new StringBuilder("");
            if (TipoTotale == false)
                dato.Append( Descrizione.Replace(";",",") + ";" + Aliquota + ";" + Qta.ToString("0.00") + ";" + PrezzoSingoloIvato.ToString("0.00") + ";" + TotaleRiga.ToString("0.00") + ";");
            else
            {

                dato.Append("TOTALE" + ";" + ""+ ";" + "" + ";" + ";" + TotaleComplessivo.ToString("0.00") + ";");

            }
            if (TipoTotale == false)
                dato.Append("V;");
            else
                dato.Append("T;" + Pagamento);
            return dato.ToString();
        }
    }
}
