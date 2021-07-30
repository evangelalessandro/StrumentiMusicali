using System;
using System.Collections.Generic;
using System.Linq;

namespace StrumentiMusicali.Library.Entity
{
    public enum EnTipoDocumento
    {
        [TipoDocFiscaleAttribute(1,"Non Specificato",false,"", VerificaCodiceUnivoco = false)]
        NonSpecificato = 0,
        [TipoDocFiscaleAttribute(3,"Fattura di Cortesia", true,"F", VerificaCodiceUnivoco = true)]
        FatturaDiCortesia = 1,
        [TipoDocFiscaleAttribute(4,"Fattura proforma", true, "F", VerificaCodiceUnivoco = true)]
        FatturaProforma = 5,
        [TipoDocFiscaleAttribute(5,"Ricevuta Fiscale", true, "F", VerificaCodiceUnivoco = true)]
        RicevutaFiscale = 4,
        [TipoDocFiscaleAttribute(100,"Nota di credito", false,"NC", VerificaCodiceUnivoco = true)]
        NotaDiCredito = 2,
        [TipoDocFiscaleAttribute(2,"DDT", false,"D",ScriviRigheETotaliStampa =false)]
        DDT = 3,
    }
    
    public static class UtilityTipoDoc
    {
        public static  List<EnTipoDocumento> CodiciNonDuplicabili()
        {
            var list = Enum.GetValues(typeof(EnTipoDocumento)).Cast<EnTipoDocumento>();



            var listToCh = list.Select(a => new
            {
                enumVal = a,
                attrib =
               ((EnTipoDocumento)a).GetAttrTipoFiscaleAttr()
            }).Where(a => a.attrib.VerificaCodiceUnivoco == true).Select(a => a.enumVal).ToList();
            return listToCh;
        }

        public static TipoDocFiscaleAttribute GetAttrTipoFiscaleAttr(this EnTipoDocumento tipoDocumento)
        {
            return Utility.EnumAttributi.GetAttribute<TipoDocFiscaleAttribute>(tipoDocumento);
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class TipoDocFiscaleAttribute : System.Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string positionalString;

        // This is a positional argument
        public TipoDocFiscaleAttribute(int ordineCombo,string descrizione, bool  pagamentoObbligatorio,string preCodice)
        {
            PagamentoObbligatorio = pagamentoObbligatorio;
            PreCodice = preCodice;
            Descrizione = descrizione;
            OrdineCombo = OrdineCombo;
        }


        public string PreCodice{ get; set; } = "";
        public bool VerificaCodiceUnivoco { get; set; } = false;
        public bool PagamentoObbligatorio { get; private set; } = false;

        public bool ScriviRigheETotaliStampa { get; set; } = true;

        public string Descrizione { get; set; }

        public int OrdineCombo { get; set; } = -1;
    }
}