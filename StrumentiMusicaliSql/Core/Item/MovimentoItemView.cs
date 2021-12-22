using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;

namespace StrumentiMusicali.Library.Core.Item
{
    public class MovimentoItemView : BaseItem<Magazzino>
    {
        public MovimentoItemView()
        { }

        public MovimentoItemView(Magazzino magazzino)
        {
            ID = magazzino.ID;
            NomeDeposito = magazzino.Deposito.NomeDeposito;
            Qta = magazzino.Qta;
            Data = magazzino.DataCreazione;
            Articolo = magazzino.Articolo.Titolo;
            Note = magazzino.Note;
            OperazioneWeb = magazzino.OperazioneWeb;
        }

        public int ID { get; set; }
        public string NomeDeposito { get; set; }
        public int Qta { get; set; }
        public DateTime Data { get; set; }
        public string Articolo { get; set; }
 
        public string Note { get; set; } = "";

        public bool OperazioneWeb { get; set; } = false;
    }
}