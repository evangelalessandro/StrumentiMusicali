using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Ecomm
{
    public class OrdineWeb : BaseEntity
    {

        public int OrdineID { get; set; }
        public DateTime DataUltimoAggiornamentoWeb { get; set; } = new DateTime(1900, 1, 1);

        public string CodiciProdotti { get; set; }
        List<int> _IdProdotti = new List<int>();
        public List<int> IdProdotti {
            get {
                _IdProdotti = CodiciProdotti.Split(";".ToCharArray()).Select(a => int.Parse(a)).ToList();
                return _IdProdotti;
            }
            set {
                CodiciProdotti=string.Join(";", value);
                _IdProdotti = value;
            }
        }
    }
}