namespace StrumentiMusicali.Library.Migrations
{
	using StrumentiMusicali.Library.Entity;
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<StrumentiMusicali.Library.Model.ModelSm>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			//AutomaticMigrationDataLossAllowed = true;
		}

		protected override void Seed(StrumentiMusicali.Library.Model.ModelSm context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method
			//  to avoid creating duplicate seed data.

			InitCategories(context);
			InitDepositi(context);
		}

		private static void InitDepositi(Model.ModelSm context)
		{
			context.Depositi.AddOrUpdate(h => h.NomeDeposito,
				new Deposito() { ID = 1, NomeDeposito = "Depo 1" },
				new Deposito() { ID = 2, NomeDeposito = "Depo 2" });
		}

		private static void InitCategories(Model.ModelSm context)
		{
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Synth a Tastiera", Reparto = "Tastiere", ID = 1, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Workstation a Tastiera", Reparto = "Tastiere", ID = 2, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Digital / Stage Piano", Reparto = "Tastiere", ID = 4, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Master Keyboard / Controller Midi", Reparto = "Tastiere", ID = 5, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Arranger a Tastiera", Reparto = "Tastiere", ID = 16, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Expander Rack", Reparto = "Tastiere", ID = 3, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Campionatori / Groove Box", Reparto = "Tastiere", ID = 14, CategoriaCondivisaCon = " Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Librerie Suoni", Reparto = "Tastiere", ID = 15, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pianoforti Acustici", Reparto = "Tastiere", ID = 12, CategoriaCondivisaCon = " Strumenti a Corda" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Organi / Hammond e Cloni", Reparto = "Tastiere", ID = 6, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Fisarmoniche", Reparto = "Tastiere", ID = 141, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Amplificatori per Tastiera", Reparto = "Tastiere", ID = 11, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pedali per Tastiera e Synth", Reparto = "Tastiere", ID = 7, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Stand per Tastiera", Reparto = "Tastiere", ID = 8, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Tastiere", ID = 50, CategoriaCondivisaCon = " Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Tastiere", ID = 28, CategoriaCondivisaCon = " Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Tastiere", ID = 302, CategoriaCondivisaCon = " Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Custodie per Tastiera", Reparto = "Tastiere", ID = 9, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Accessori per Tastiera", Reparto = "Tastiere", ID = 10, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cavi Audio e Adattatori", Reparto = "Tastiere", ID = 83, CategoriaCondivisaCon = " Chitarre, Bassi, Home & Studio Recording, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Chitarre Elettriche", Reparto = "Chitarre", ID = 17, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Chitarre Semiacustiche", Reparto = "Chitarre", ID = 18, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Chitarre Acustiche", Reparto = "Chitarre", ID = 19, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Chitarre Classiche", Reparto = "Chitarre", ID = 20, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mandolini / Ukulele / Banjo", Reparto = "Chitarre", ID = 68, CategoriaCondivisaCon = " Strumenti a Corda" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Amplificatori - Combo per Chitarra", Reparto = "Chitarre", ID = 21, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Amplificatori - Testata / Cassa per Chitarra", Reparto = "Chitarre", ID = 22, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Valvole per Amplificatori", Reparto = "Chitarre", ID = 250, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pedalini Singoli per Chitarra", Reparto = "Chitarre", ID = 25, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pedaliere Multieffetto per Chitarra", Reparto = "Chitarre", ID = 24, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Effetti a Rack per Chitarra", Reparto = "Chitarre", ID = 23, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Chitarre", ID = 28, CategoriaCondivisaCon = " Tastiere, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Corpi / Body per Chitarra", Reparto = "Chitarre", ID = 255, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Manici per Chitarra", Reparto = "Chitarre", ID = 254, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pick-up per Chitarra", Reparto = "Chitarre", ID = 252, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Battipenna per Chitarra", Reparto = "Chitarre", ID = 251, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Manopole / Potenziometri", Reparto = "Chitarre", ID = 309, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Ponti / Tremoli per Chitarra", Reparto = "Chitarre", ID = 257, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Meccaniche per Chitarra", Reparto = "Chitarre", ID = 256, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Corde per Chitarra", Reparto = "Chitarre", ID = 253, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Tracolle per Chitarra e Basso", Reparto = "Chitarre", ID = 306, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Plettri per chitarra e Basso", Reparto = "Chitarre", ID = 258, CategoriaCondivisaCon = " Bassi, Merchandising" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Custodie per Chitarra", Reparto = "Chitarre", ID = 26, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Stand per Chitarra e Basso", Reparto = "Chitarre", ID = 304, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Chitarre", ID = 50, CategoriaCondivisaCon = " Tastiere, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Chitarre", ID = 302, CategoriaCondivisaCon = " Tastiere, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Accessori per Chitarra", Reparto = "Chitarre", ID = 27, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cavi Audio e Adattatori", Reparto = "Chitarre", ID = 83, CategoriaCondivisaCon = " Tastiere, Bassi, Home & Studio Recording, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Bassi Elettrici 4 corde", Reparto = "Bassi", ID = 29, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Bassi Elettrici 5/6/+ corde", Reparto = "Bassi", ID = 30, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Bassi Acustici", Reparto = "Bassi", ID = 31, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Amplificatori - Combo per Basso", Reparto = "Bassi", ID = 32, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Amplificatori - Testata / Cassa per Basso", Reparto = "Bassi", ID = 33, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Valvole per Amplificatori", Reparto = "Bassi", ID = 250, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pedalini Singoli per Basso", Reparto = "Bassi", ID = 36, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pedaliere Multieffetto per Basso", Reparto = "Bassi", ID = 35, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Effetti a Rack per Basso", Reparto = "Bassi", ID = 34, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Bassi", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Corpi / Body per Basso", Reparto = "Bassi", ID = 263, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Manici per Basso", Reparto = "Bassi", ID = 262, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pick-up per Basso", Reparto = "Bassi", ID = 261, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Battipenna per Basso", Reparto = "Bassi", ID = 259, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Manopole / Potenziometri", Reparto = "Bassi", ID = 309, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Ponti / Tremoli per Basso", Reparto = "Bassi", ID = 265, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Meccaniche per Basso", Reparto = "Bassi", ID = 264, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Corde per Basso", Reparto = "Bassi", ID = 260, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Tracolle per Chitarra e Basso", Reparto = "Bassi", ID = 306, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Plettri per chitarra e Basso", Reparto = "Bassi", ID = 258, CategoriaCondivisaCon = " Chitarre, Merchandising" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Custodie per Basso", Reparto = "Bassi", ID = 37, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Stand per Chitarra e Basso", Reparto = "Bassi", ID = 304, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Bassi", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Bassi", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Accessori per Basso", Reparto = "Bassi", ID = 38, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cavi Audio e Adattatori", Reparto = "Bassi", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Home & Studio Recording, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Batterie Acustiche - Set Completi", Reparto = "Batterie / Percussioni", ID = 40, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Batterie Elettroniche - Set Completi", Reparto = "Batterie / Percussioni", ID = 41, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Batterie Elettroniche - Moduli / Pad", Reparto = "Batterie / Percussioni", ID = 269, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Gran Cassa", Reparto = "Batterie / Percussioni", ID = 145, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Rullanti", Reparto = "Batterie / Percussioni", ID = 142, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Tom", Reparto = "Batterie / Percussioni", ID = 143, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Timpani", Reparto = "Batterie / Percussioni", ID = 144, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pelli / Cerchi / Ring", Reparto = "Batterie / Percussioni", ID = 267, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Set di Piatti", Reparto = "Batterie / Percussioni", ID = 266, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Piatti - HiHat", Reparto = "Batterie / Percussioni", ID = 45, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Piatti - Crash", Reparto = "Batterie / Percussioni", ID = 43, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Piatti - Ride", Reparto = "Batterie / Percussioni", ID = 44, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Piatti - Splash", Reparto = "Batterie / Percussioni", ID = 46, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Piatti - China e Altri", Reparto = "Batterie / Percussioni", ID = 47, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Percussioni / Etnici", Reparto = "Batterie / Percussioni", ID = 42, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Meccaniche / Aste / Supporti", Reparto = "Batterie / Percussioni", ID = 48, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pedali per Batteria", Reparto = "Batterie / Percussioni", ID = 49, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Batterie / Percussioni", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Batterie / Percussioni", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Custodie per Batteria", Reparto = "Batterie / Percussioni", ID = 51, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Bacchette e Spazzole", Reparto = "Batterie / Percussioni", ID = 268, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Batterie / Percussioni", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Microfoni per Batteria", Reparto = "Batterie / Percussioni", ID = 153, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Prodotti per Isolamento Acustico", Reparto = "Batterie / Percussioni", ID = 77, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Accessori per Batteria", Reparto = "Batterie / Percussioni", ID = 52, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sax", Reparto = "Strumenti a Fiato", ID = 53, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Clarinetti", Reparto = "Strumenti a Fiato", ID = 54, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Flauti", Reparto = "Strumenti a Fiato", ID = 55, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Trombe", Reparto = "Strumenti a Fiato", ID = 56, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Corni / Flicorni", Reparto = "Strumenti a Fiato", ID = 57, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Tromboni / Bassi Tuba", Reparto = "Strumenti a Fiato", ID = 58, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Oboi / Fagotti", Reparto = "Strumenti a Fiato", ID = 59, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Armonica a bocca", Reparto = "Strumenti a Fiato", ID = 272, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Strumenti a Fiato", Reparto = "Strumenti a Fiato", ID = 307, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Bocchini e Ance per Legni", Reparto = "Strumenti a Fiato", ID = 275, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Bocchini per Ottoni", Reparto = "Strumenti a Fiato", ID = 274, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sordine", Reparto = "Strumenti a Fiato", ID = 273, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Legni - Altro / Accessori", Reparto = "Strumenti a Fiato", ID = 61, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Ottoni - Altro / Accessori", Reparto = "Strumenti a Fiato", ID = 60, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Custodie per Fiati", Reparto = "Strumenti a Fiato", ID = 63, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Strumenti a Fiato", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Strumenti a Fiato", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Strumenti a Fiato", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Fiati - Altro / Accessori", Reparto = "Strumenti a Fiato", ID = 62, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Violini / Viole", Reparto = "Strumenti a Corda", ID = 65, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Violoncelli / Contrabbassi", Reparto = "Strumenti a Corda", ID = 66, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mandolini / Ukulele / Banjo", Reparto = "Strumenti a Corda", ID = 68, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Arpe / Cetre", Reparto = "Strumenti a Corda", ID = 276, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Strumenti Silent", Reparto = "Strumenti a Corda", ID = 69, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Pianoforti Acustici", Reparto = "Strumenti a Corda", ID = 12, CategoriaCondivisaCon = " Tastiere" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Strumenti a Corda", Reparto = "Strumenti a Corda", ID = 70, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accessori - Corde", Reparto = "Strumenti a Corda", ID = 277, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accessori - Archetti", Reparto = "Strumenti a Corda", ID = 278, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Custodie per Strumenti a Corda", Reparto = "Strumenti a Corda", ID = 67, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Strumenti a Corda", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Strumenti a Corda", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Strumenti a Corda", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Accessori per Strumenti a Corda", Reparto = "Strumenti a Corda", ID = 71, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mixer Analogici", Reparto = "Home & Studio Recording", ID = 74, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mixer Digitali", Reparto = "Home & Studio Recording", ID = 75, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Equalizzatori", Reparto = "Home & Studio Recording", ID = 78, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Compressori / Limiter", Reparto = "Home & Studio Recording", ID = 85, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Preamplificatori", Reparto = "Home & Studio Recording", ID = 76, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Finali di Potenza", Reparto = "Home & Studio Recording", ID = 81, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Effetti / Altri Processori di Segnale", Reparto = "Home & Studio Recording", ID = 79, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Monitor da Studio", Reparto = "Home & Studio Recording", ID = 72, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cuffie / Auricolari", Reparto = "Home & Studio Recording", ID = 80, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Microfoni", Reparto = "Home & Studio Recording", ID = 98, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Microfoni da Studio", Reparto = "Home & Studio Recording", ID = 73, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Radiomicrofoni", Reparto = "Home & Studio Recording", ID = 301, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Workstation PC", Reparto = "Home & Studio Recording", ID = 147, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Workstation Apple", Reparto = "Home & Studio Recording", ID = 146, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Schede Audio / Interfacce Midi", Reparto = "Home & Studio Recording", ID = 84, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Software - Audio / Midi Recording", Reparto = "Home & Studio Recording", ID = 87, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Software - Virtual Instruments", Reparto = "Home & Studio Recording", ID = 86, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Software - PlugIn Effects", Reparto = "Home & Studio Recording", ID = 89, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Registratori Multitraccia", Reparto = "Home & Studio Recording", ID = 82, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Campionatori / Groove Box", Reparto = "Home & Studio Recording", ID = 14, CategoriaCondivisaCon = " Tastiere, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Librerie Suoni", Reparto = "Home & Studio Recording", ID = 15, CategoriaCondivisaCon = " Tastiere" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Player Mp3 / Midi / Karaoke", Reparto = "Home & Studio Recording", ID = 13, CategoriaCondivisaCon = " Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Basi Mp3 / Midi / Karaoke", Reparto = "Home & Studio Recording", ID = 305, CategoriaCondivisaCon = " Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Home & Studio Recording", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Aste Microfoniche", Reparto = "Home & Studio Recording", ID = 154, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Home & Studio Recording", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Home & Studio Recording", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cavi Audio e Adattatori", Reparto = "Home & Studio Recording", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Prodotti per Isolamento Acustico", Reparto = "Home & Studio Recording", ID = 77, CategoriaCondivisaCon = " Batterie / Percussioni" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mixer Analogici", Reparto = "Live Equipment", ID = 74, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mixer Digitali", Reparto = "Live Equipment", ID = 75, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Equalizzatori", Reparto = "Live Equipment", ID = 78, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Compressori / Limiter", Reparto = "Live Equipment", ID = 85, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Preamplificatori", Reparto = "Live Equipment", ID = 76, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Finali di Potenza", Reparto = "Live Equipment", ID = 81, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Effetti / Altri Processori di Segnale", Reparto = "Live Equipment", ID = 79, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Casse / Monitor Live Attivi", Reparto = "Live Equipment", ID = 93, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Casse / Monitor Live Passivi", Reparto = "Live Equipment", ID = 94, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cuffie / Auricolari", Reparto = "Live Equipment", ID = 80, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Microfoni", Reparto = "Live Equipment", ID = 98, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Radiomicrofoni", Reparto = "Live Equipment", ID = 301, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Player Mp3 / Midi / Karaoke", Reparto = "Live Equipment", ID = 13, CategoriaCondivisaCon = " Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Basi Mp3 / Midi / Karaoke", Reparto = "Live Equipment", ID = 305, CategoriaCondivisaCon = " Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accordatori / Metronomi", Reparto = "Live Equipment", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Strutture Rack e Flight Case", Reparto = "Live Equipment", ID = 97, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Aste Microfoniche", Reparto = "Live Equipment", ID = 154, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Sgabelli e Panche", Reparto = "Live Equipment", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Leggii e Altri Supporti", Reparto = "Live Equipment", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cavi Audio e Adattatori", Reparto = "Live Equipment", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Tralicci / Americana / Supporti", Reparto = "Live Equipment", ID = 308, CategoriaCondivisaCon = " Lighting" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altre Attrezzature Live", Reparto = "Live Equipment", ID = 155, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "CD Player / Mp3 per DJ", Reparto = "DJ Equipment", ID = 105, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Consolle / Controller per DJ", Reparto = "DJ Equipment", ID = 280, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Giradischi e Testine per DJ", Reparto = "DJ Equipment", ID = 107, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mixer per DJ", Reparto = "DJ Equipment", ID = 103, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Campionatori / Groove Box", Reparto = "DJ Equipment", ID = 14, CategoriaCondivisaCon = " Tastiere, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cuffie per DJ", Reparto = "DJ Equipment", ID = 106, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Player Mp3 / Midi / Karaoke", Reparto = "DJ Equipment", ID = 13, CategoriaCondivisaCon = " Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Basi Mp3 / Midi / Karaoke", Reparto = "DJ Equipment", ID = 305, CategoriaCondivisaCon = " Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Dischi in Vinile", Reparto = "DJ Equipment", ID = 113, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "CD Italiani", Reparto = "DJ Equipment", ID = 111, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "CD Stranieri", Reparto = "DJ Equipment", ID = 112, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Borse e Custodie per DJ", Reparto = "DJ Equipment", ID = 281, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Strutture Rack e Flight Case", Reparto = "DJ Equipment", ID = 97, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Porta CD / DVD", Reparto = "DJ Equipment", ID = 282, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Accessori per DJ", Reparto = "DJ Equipment", ID = 108, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cavi Audio e Adattatori", Reparto = "DJ Equipment", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Teste Mobili", Reparto = "Lighting", ID = 114, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Scanner", Reparto = "Lighting", ID = 115, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Laser", Reparto = "Lighting", ID = 116, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Luci Flowers", Reparto = "Lighting", ID = 296, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Proiettori Luci", Reparto = "Lighting", ID = 118, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Luci Strobo", Reparto = "Lighting", ID = 117, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Effetti Luce", Reparto = "Lighting", ID = 122, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Mixer Luci / Centraline / Dimmer", Reparto = "Lighting", ID = 123, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Macchine del Fumo / Bolle", Reparto = "Lighting", ID = 120, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accessori - Lampade", Reparto = "Lighting", ID = 297, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accessori - Gelatine", Reparto = "Lighting", ID = 300, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Accessori - Ganci / Staffe / Clamp", Reparto = "Lighting", ID = 298, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cavi per Lighting", Reparto = "Lighting", ID = 119, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Tralicci / Americana / Supporti", Reparto = "Lighting", ID = 308, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Accessori per Lighting", Reparto = "Lighting", ID = 124, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Videoproiettori / Accessori Video", Reparto = "Lighting", ID = 121, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "CD Italiani", Reparto = "CD / DVD / Didattica", ID = 111, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "CD Stranieri", Reparto = "CD / DVD / Didattica", ID = 112, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Dischi in Vinile", Reparto = "CD / DVD / Didattica", ID = 113, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "DVD Italiani", Reparto = "CD / DVD / Didattica", ID = 127, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "DVD Stranieri", Reparto = "CD / DVD / Didattica", ID = 128, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "DVD / Video Didattici", Reparto = "CD / DVD / Didattica", ID = 294, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Porta CD / DVD", Reparto = "CD / DVD / Didattica", ID = 282, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Partiture Rock / Metal / Jazz / Blues", Reparto = "CD / DVD / Didattica", ID = 130, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Partiture Classica", Reparto = "CD / DVD / Didattica", ID = 129, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Metodi Studio / Didattica", Reparto = "CD / DVD / Didattica", ID = 293, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Libri - Biografie", Reparto = "CD / DVD / Didattica", ID = 132, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Libri - Altro", Reparto = "CD / DVD / Didattica", ID = 133, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Riviste Musicali", Reparto = "CD / DVD / Didattica", ID = 295, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Magliette / Felpe", Reparto = "Merchandising", ID = 283, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Cappellini", Reparto = "Merchandising", ID = 284, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Prodotti di Abbigliamento", Reparto = "Merchandising", ID = 285, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Portachiavi / Cinture", Reparto = "Merchandising", ID = 287, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Spille / Pin Badges", Reparto = "Merchandising", ID = 288, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Plettri per chitarra e Basso", Reparto = "Merchandising", ID = 258, CategoriaCondivisaCon = " Chitarre, Bassi" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Adesivi / Decalcomanie", Reparto = "Merchandising", ID = 303, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Tazze / Bicchieri", Reparto = "Merchandising", ID = 290, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Poster / Bandiere", Reparto = "Merchandising", ID = 289, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Gadget Elettronici", Reparto = "Merchandising", ID = 286, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categoria() { Nome = "Altri Gadget", Reparto = "Merchandising", ID = 291, CategoriaCondivisaCon = "" });
		}
	}
}