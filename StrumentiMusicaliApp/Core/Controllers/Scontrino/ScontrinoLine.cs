using StrumentiMusicali.Library.Core.Item;
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
    internal class ScontrinoLine : ScontrinoLineItem
    {

        public decimal Qta { get; set; } = 10.5M;

        public decimal TotaleRiga { get { return Qta * PrezzoIvato; } }

        public string Pagamento { get; set; }
        /// <summary>
        /// solo per i totali
        /// </summary>
        public decimal TotaleComplessivo { get; set; } = 0;

        public string CodiceLotteria { get; set; } = "";

        public override string ToString()
        {
            StringBuilder dato = new StringBuilder("");
            if (TipoRigaScontrino == TipoRigaScontrino.Vendita)
                dato.Append(Descrizione.Replace(";", ",") + ";" + IvaPerc + ";" + Qta.ToString("0.00") + ";" + PrezzoIvato.ToString("0.00") + ";" + TotaleRiga.ToString("0.00") + ";");
            else if (TipoRigaScontrino == TipoRigaScontrino.Sconto)
                dato.Append(Descrizione.Replace(";", ",") + ";" + 0 + ";" + 0.ToString("0.00") + ";" + PrezzoIvato.ToString("0.00") + ";" + TotaleRiga.ToString("0.00") + ";");
            else if (TipoRigaScontrino == TipoRigaScontrino.Totale)
            {

                dato.Append("TOTALE" + ";" + "" + ";" + "" + ";" + ";" + TotaleComplessivo.ToString("0.00") + ";");

            }
            if (TipoRigaScontrino == TipoRigaScontrino.Vendita)
                dato.Append("V;");
            else if (TipoRigaScontrino == TipoRigaScontrino.Sconto)
                dato.Append("S;");
            else if (TipoRigaScontrino == TipoRigaScontrino.Totale)
            {
                dato.Append("T;" + Pagamento);
                if (CodiceLotteria != "")
                {
                    dato.Append("|" + CodiceLotteria);
                }
            }
            return dato.ToString();
        }
    }
}
