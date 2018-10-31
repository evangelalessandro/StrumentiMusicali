using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class FatturaRiga : RigaDoc
	{
		[Required]
		public int FatturaID { get; set; }

		public virtual Fattura Fattura { get; set; }

		[NotMapped]
		public decimal Importo { get { return PrezzoUnitario * Qta; } set { } }

		[Required]
		public decimal PrezzoUnitario { get; set; }

		public string IvaApplicata { get; set; }
	}
}