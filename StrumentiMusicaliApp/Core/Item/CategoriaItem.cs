namespace StrumentiMusicali.App.Core
{
	public class CategoriaItem
	{
		public string Descrizione { get; set; }
		public int ID { get; set; }
		public string Reparto { get; set; }

		public override string ToString()
		{
			return Descrizione;
		}
	}
}