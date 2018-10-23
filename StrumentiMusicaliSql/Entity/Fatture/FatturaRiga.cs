using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public class FatturaRiga : RigaDoc
	{
		[Required]
		public int FatturaID { get; set; }

		public virtual Fattura Fattura { get; set; }


		[Required]
		public decimal PrezzoUnitario { get; set; }
		 
		public string IvaApplicata { get; set; }
	}
}