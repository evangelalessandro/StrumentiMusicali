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

			context.Depositi.AddOrUpdate(h=>h.NomeDeposito,
				new Deposito() { ID = 1, NomeDeposito = "Depo 1" },
			    new Deposito() { ID = 2, NomeDeposito = "Depo 2" });

		}

		private static void InitCategories(Model.ModelSm context)
		{
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Synth a Tastiera", Reparto = "Tastiere", ID = 1, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Workstation a Tastiera", Reparto = "Tastiere", ID = 2, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Digital / Stage Piano", Reparto = "Tastiere", ID = 4, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Master Keyboard / Controller Midi", Reparto = "Tastiere", ID = 5, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Arranger a Tastiera", Reparto = "Tastiere", ID = 16, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Expander Rack", Reparto = "Tastiere", ID = 3, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Campionatori / Groove Box", Reparto = "Tastiere", ID = 14, CategoriaCondivisaCon = " Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Librerie Suoni", Reparto = "Tastiere", ID = 15, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pianoforti Acustici", Reparto = "Tastiere", ID = 12, CategoriaCondivisaCon = " Strumenti a Corda" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Organi / Hammond e Cloni", Reparto = "Tastiere", ID = 6, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Fisarmoniche", Reparto = "Tastiere", ID = 141, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Amplificatori per Tastiera", Reparto = "Tastiere", ID = 11, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pedali per Tastiera e Synth", Reparto = "Tastiere", ID = 7, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Stand per Tastiera", Reparto = "Tastiere", ID = 8, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Tastiere", ID = 50, CategoriaCondivisaCon = " Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Tastiere", ID = 28, CategoriaCondivisaCon = " Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Tastiere", ID = 302, CategoriaCondivisaCon = " Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Custodie per Tastiera", Reparto = "Tastiere", ID = 9, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Accessori per Tastiera", Reparto = "Tastiere", ID = 10, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cavi Audio e Adattatori", Reparto = "Tastiere", ID = 83, CategoriaCondivisaCon = " Chitarre, Bassi, Home & Studio Recording, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Chitarre Elettriche", Reparto = "Chitarre", ID = 17, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Chitarre Semiacustiche", Reparto = "Chitarre", ID = 18, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Chitarre Acustiche", Reparto = "Chitarre", ID = 19, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Chitarre Classiche", Reparto = "Chitarre", ID = 20, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mandolini / Ukulele / Banjo", Reparto = "Chitarre", ID = 68, CategoriaCondivisaCon = " Strumenti a Corda" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Amplificatori - Combo per Chitarra", Reparto = "Chitarre", ID = 21, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Amplificatori - Testata / Cassa per Chitarra", Reparto = "Chitarre", ID = 22, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Valvole per Amplificatori", Reparto = "Chitarre", ID = 250, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pedalini Singoli per Chitarra", Reparto = "Chitarre", ID = 25, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pedaliere Multieffetto per Chitarra", Reparto = "Chitarre", ID = 24, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Effetti a Rack per Chitarra", Reparto = "Chitarre", ID = 23, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Chitarre", ID = 28, CategoriaCondivisaCon = " Tastiere, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Corpi / Body per Chitarra", Reparto = "Chitarre", ID = 255, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Manici per Chitarra", Reparto = "Chitarre", ID = 254, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pick-up per Chitarra", Reparto = "Chitarre", ID = 252, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Battipenna per Chitarra", Reparto = "Chitarre", ID = 251, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Manopole / Potenziometri", Reparto = "Chitarre", ID = 309, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Ponti / Tremoli per Chitarra", Reparto = "Chitarre", ID = 257, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Meccaniche per Chitarra", Reparto = "Chitarre", ID = 256, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Corde per Chitarra", Reparto = "Chitarre", ID = 253, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Tracolle per Chitarra e Basso", Reparto = "Chitarre", ID = 306, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Plettri per chitarra e Basso", Reparto = "Chitarre", ID = 258, CategoriaCondivisaCon = " Bassi, Merchandising" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Custodie per Chitarra", Reparto = "Chitarre", ID = 26, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Stand per Chitarra e Basso", Reparto = "Chitarre", ID = 304, CategoriaCondivisaCon = " Bassi" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Chitarre", ID = 50, CategoriaCondivisaCon = " Tastiere, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Chitarre", ID = 302, CategoriaCondivisaCon = " Tastiere, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Accessori per Chitarra", Reparto = "Chitarre", ID = 27, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cavi Audio e Adattatori", Reparto = "Chitarre", ID = 83, CategoriaCondivisaCon = " Tastiere, Bassi, Home & Studio Recording, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Bassi Elettrici 4 corde", Reparto = "Bassi", ID = 29, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Bassi Elettrici 5/6/+ corde", Reparto = "Bassi", ID = 30, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Bassi Acustici", Reparto = "Bassi", ID = 31, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Amplificatori - Combo per Basso", Reparto = "Bassi", ID = 32, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Amplificatori - Testata / Cassa per Basso", Reparto = "Bassi", ID = 33, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Valvole per Amplificatori", Reparto = "Bassi", ID = 250, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pedalini Singoli per Basso", Reparto = "Bassi", ID = 36, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pedaliere Multieffetto per Basso", Reparto = "Bassi", ID = 35, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Effetti a Rack per Basso", Reparto = "Bassi", ID = 34, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Bassi", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Corpi / Body per Basso", Reparto = "Bassi", ID = 263, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Manici per Basso", Reparto = "Bassi", ID = 262, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pick-up per Basso", Reparto = "Bassi", ID = 261, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Battipenna per Basso", Reparto = "Bassi", ID = 259, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Manopole / Potenziometri", Reparto = "Bassi", ID = 309, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Ponti / Tremoli per Basso", Reparto = "Bassi", ID = 265, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Meccaniche per Basso", Reparto = "Bassi", ID = 264, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Corde per Basso", Reparto = "Bassi", ID = 260, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Tracolle per Chitarra e Basso", Reparto = "Bassi", ID = 306, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Plettri per chitarra e Basso", Reparto = "Bassi", ID = 258, CategoriaCondivisaCon = " Chitarre, Merchandising" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Custodie per Basso", Reparto = "Bassi", ID = 37, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Stand per Chitarra e Basso", Reparto = "Bassi", ID = 304, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Bassi", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Bassi", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Accessori per Basso", Reparto = "Bassi", ID = 38, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cavi Audio e Adattatori", Reparto = "Bassi", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Home & Studio Recording, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Batterie Acustiche - Set Completi", Reparto = "Batterie / Percussioni", ID = 40, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Batterie Elettroniche - Set Completi", Reparto = "Batterie / Percussioni", ID = 41, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Batterie Elettroniche - Moduli / Pad", Reparto = "Batterie / Percussioni", ID = 269, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Gran Cassa", Reparto = "Batterie / Percussioni", ID = 145, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Rullanti", Reparto = "Batterie / Percussioni", ID = 142, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Tom", Reparto = "Batterie / Percussioni", ID = 143, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Timpani", Reparto = "Batterie / Percussioni", ID = 144, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pelli / Cerchi / Ring", Reparto = "Batterie / Percussioni", ID = 267, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Set di Piatti", Reparto = "Batterie / Percussioni", ID = 266, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Piatti - HiHat", Reparto = "Batterie / Percussioni", ID = 45, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Piatti - Crash", Reparto = "Batterie / Percussioni", ID = 43, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Piatti - Ride", Reparto = "Batterie / Percussioni", ID = 44, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Piatti - Splash", Reparto = "Batterie / Percussioni", ID = 46, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Piatti - China e Altri", Reparto = "Batterie / Percussioni", ID = 47, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Percussioni / Etnici", Reparto = "Batterie / Percussioni", ID = 42, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Meccaniche / Aste / Supporti", Reparto = "Batterie / Percussioni", ID = 48, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pedali per Batteria", Reparto = "Batterie / Percussioni", ID = 49, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Batterie / Percussioni", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Batterie / Percussioni", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Custodie per Batteria", Reparto = "Batterie / Percussioni", ID = 51, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Bacchette e Spazzole", Reparto = "Batterie / Percussioni", ID = 268, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Batterie / Percussioni", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Microfoni per Batteria", Reparto = "Batterie / Percussioni", ID = 153, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Prodotti per Isolamento Acustico", Reparto = "Batterie / Percussioni", ID = 77, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Accessori per Batteria", Reparto = "Batterie / Percussioni", ID = 52, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sax", Reparto = "Strumenti a Fiato", ID = 53, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Clarinetti", Reparto = "Strumenti a Fiato", ID = 54, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Flauti", Reparto = "Strumenti a Fiato", ID = 55, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Trombe", Reparto = "Strumenti a Fiato", ID = 56, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Corni / Flicorni", Reparto = "Strumenti a Fiato", ID = 57, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Tromboni / Bassi Tuba", Reparto = "Strumenti a Fiato", ID = 58, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Oboi / Fagotti", Reparto = "Strumenti a Fiato", ID = 59, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Armonica a bocca", Reparto = "Strumenti a Fiato", ID = 272, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Strumenti a Fiato", Reparto = "Strumenti a Fiato", ID = 307, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Bocchini e Ance per Legni", Reparto = "Strumenti a Fiato", ID = 275, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Bocchini per Ottoni", Reparto = "Strumenti a Fiato", ID = 274, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sordine", Reparto = "Strumenti a Fiato", ID = 273, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Legni - Altro / Accessori", Reparto = "Strumenti a Fiato", ID = 61, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Ottoni - Altro / Accessori", Reparto = "Strumenti a Fiato", ID = 60, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Custodie per Fiati", Reparto = "Strumenti a Fiato", ID = 63, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Strumenti a Fiato", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Strumenti a Fiato", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Strumenti a Fiato", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Corda, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Fiati - Altro / Accessori", Reparto = "Strumenti a Fiato", ID = 62, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Violini / Viole", Reparto = "Strumenti a Corda", ID = 65, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Violoncelli / Contrabbassi", Reparto = "Strumenti a Corda", ID = 66, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mandolini / Ukulele / Banjo", Reparto = "Strumenti a Corda", ID = 68, CategoriaCondivisaCon = " Chitarre" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Arpe / Cetre", Reparto = "Strumenti a Corda", ID = 276, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Strumenti Silent", Reparto = "Strumenti a Corda", ID = 69, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Pianoforti Acustici", Reparto = "Strumenti a Corda", ID = 12, CategoriaCondivisaCon = " Tastiere" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Strumenti a Corda", Reparto = "Strumenti a Corda", ID = 70, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accessori - Corde", Reparto = "Strumenti a Corda", ID = 277, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accessori - Archetti", Reparto = "Strumenti a Corda", ID = 278, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Custodie per Strumenti a Corda", Reparto = "Strumenti a Corda", ID = 67, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Strumenti a Corda", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Strumenti a Corda", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Strumenti a Corda", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Accessori per Strumenti a Corda", Reparto = "Strumenti a Corda", ID = 71, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mixer Analogici", Reparto = "Home & Studio Recording", ID = 74, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mixer Digitali", Reparto = "Home & Studio Recording", ID = 75, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Equalizzatori", Reparto = "Home & Studio Recording", ID = 78, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Compressori / Limiter", Reparto = "Home & Studio Recording", ID = 85, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Preamplificatori", Reparto = "Home & Studio Recording", ID = 76, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Finali di Potenza", Reparto = "Home & Studio Recording", ID = 81, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Effetti / Altri Processori di Segnale", Reparto = "Home & Studio Recording", ID = 79, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Monitor da Studio", Reparto = "Home & Studio Recording", ID = 72, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cuffie / Auricolari", Reparto = "Home & Studio Recording", ID = 80, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Microfoni", Reparto = "Home & Studio Recording", ID = 98, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Microfoni da Studio", Reparto = "Home & Studio Recording", ID = 73, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Radiomicrofoni", Reparto = "Home & Studio Recording", ID = 301, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Workstation PC", Reparto = "Home & Studio Recording", ID = 147, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Workstation Apple", Reparto = "Home & Studio Recording", ID = 146, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Schede Audio / Interfacce Midi", Reparto = "Home & Studio Recording", ID = 84, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Software - Audio / Midi Recording", Reparto = "Home & Studio Recording", ID = 87, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Software - Virtual Instruments", Reparto = "Home & Studio Recording", ID = 86, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Software - PlugIn Effects", Reparto = "Home & Studio Recording", ID = 89, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Registratori Multitraccia", Reparto = "Home & Studio Recording", ID = 82, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Campionatori / Groove Box", Reparto = "Home & Studio Recording", ID = 14, CategoriaCondivisaCon = " Tastiere, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Librerie Suoni", Reparto = "Home & Studio Recording", ID = 15, CategoriaCondivisaCon = " Tastiere" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Player Mp3 / Midi / Karaoke", Reparto = "Home & Studio Recording", ID = 13, CategoriaCondivisaCon = " Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Basi Mp3 / Midi / Karaoke", Reparto = "Home & Studio Recording", ID = 305, CategoriaCondivisaCon = " Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Home & Studio Recording", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Aste Microfoniche", Reparto = "Home & Studio Recording", ID = 154, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Home & Studio Recording", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Home & Studio Recording", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cavi Audio e Adattatori", Reparto = "Home & Studio Recording", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Live Equipment, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Prodotti per Isolamento Acustico", Reparto = "Home & Studio Recording", ID = 77, CategoriaCondivisaCon = " Batterie / Percussioni" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mixer Analogici", Reparto = "Live Equipment", ID = 74, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mixer Digitali", Reparto = "Live Equipment", ID = 75, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Equalizzatori", Reparto = "Live Equipment", ID = 78, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Compressori / Limiter", Reparto = "Live Equipment", ID = 85, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Preamplificatori", Reparto = "Live Equipment", ID = 76, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Finali di Potenza", Reparto = "Live Equipment", ID = 81, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Effetti / Altri Processori di Segnale", Reparto = "Live Equipment", ID = 79, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Casse / Monitor Live Attivi", Reparto = "Live Equipment", ID = 93, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Casse / Monitor Live Passivi", Reparto = "Live Equipment", ID = 94, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cuffie / Auricolari", Reparto = "Live Equipment", ID = 80, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Microfoni", Reparto = "Live Equipment", ID = 98, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Radiomicrofoni", Reparto = "Live Equipment", ID = 301, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Player Mp3 / Midi / Karaoke", Reparto = "Live Equipment", ID = 13, CategoriaCondivisaCon = " Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Basi Mp3 / Midi / Karaoke", Reparto = "Live Equipment", ID = 305, CategoriaCondivisaCon = " Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accordatori / Metronomi", Reparto = "Live Equipment", ID = 28, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Strutture Rack e Flight Case", Reparto = "Live Equipment", ID = 97, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Aste Microfoniche", Reparto = "Live Equipment", ID = 154, CategoriaCondivisaCon = " Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Sgabelli e Panche", Reparto = "Live Equipment", ID = 50, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Leggii e Altri Supporti", Reparto = "Live Equipment", ID = 302, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Batterie / Percussioni, Strumenti a Fiato, Strumenti a Corda, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cavi Audio e Adattatori", Reparto = "Live Equipment", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Home & Studio Recording, DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Tralicci / Americana / Supporti", Reparto = "Live Equipment", ID = 308, CategoriaCondivisaCon = " Lighting" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altre Attrezzature Live", Reparto = "Live Equipment", ID = 155, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "CD Player / Mp3 per DJ", Reparto = "DJ Equipment", ID = 105, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Consolle / Controller per DJ", Reparto = "DJ Equipment", ID = 280, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Giradischi e Testine per DJ", Reparto = "DJ Equipment", ID = 107, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mixer per DJ", Reparto = "DJ Equipment", ID = 103, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Campionatori / Groove Box", Reparto = "DJ Equipment", ID = 14, CategoriaCondivisaCon = " Tastiere, Home & Studio Recording" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cuffie per DJ", Reparto = "DJ Equipment", ID = 106, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Player Mp3 / Midi / Karaoke", Reparto = "DJ Equipment", ID = 13, CategoriaCondivisaCon = " Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Basi Mp3 / Midi / Karaoke", Reparto = "DJ Equipment", ID = 305, CategoriaCondivisaCon = " Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Dischi in Vinile", Reparto = "DJ Equipment", ID = 113, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "CD Italiani", Reparto = "DJ Equipment", ID = 111, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "CD Stranieri", Reparto = "DJ Equipment", ID = 112, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Borse e Custodie per DJ", Reparto = "DJ Equipment", ID = 281, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Strutture Rack e Flight Case", Reparto = "DJ Equipment", ID = 97, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Porta CD / DVD", Reparto = "DJ Equipment", ID = 282, CategoriaCondivisaCon = " CD / DVD / Didattica" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Accessori per DJ", Reparto = "DJ Equipment", ID = 108, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cavi Audio e Adattatori", Reparto = "DJ Equipment", ID = 83, CategoriaCondivisaCon = " Tastiere, Chitarre, Bassi, Home & Studio Recording, Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Teste Mobili", Reparto = "Lighting", ID = 114, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Scanner", Reparto = "Lighting", ID = 115, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Laser", Reparto = "Lighting", ID = 116, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Luci Flowers", Reparto = "Lighting", ID = 296, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Proiettori Luci", Reparto = "Lighting", ID = 118, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Luci Strobo", Reparto = "Lighting", ID = 117, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Effetti Luce", Reparto = "Lighting", ID = 122, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Mixer Luci / Centraline / Dimmer", Reparto = "Lighting", ID = 123, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Macchine del Fumo / Bolle", Reparto = "Lighting", ID = 120, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accessori - Lampade", Reparto = "Lighting", ID = 297, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accessori - Gelatine", Reparto = "Lighting", ID = 300, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Accessori - Ganci / Staffe / Clamp", Reparto = "Lighting", ID = 298, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cavi per Lighting", Reparto = "Lighting", ID = 119, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Tralicci / Americana / Supporti", Reparto = "Lighting", ID = 308, CategoriaCondivisaCon = " Live Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Accessori per Lighting", Reparto = "Lighting", ID = 124, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Videoproiettori / Accessori Video", Reparto = "Lighting", ID = 121, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "CD Italiani", Reparto = "CD / DVD / Didattica", ID = 111, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "CD Stranieri", Reparto = "CD / DVD / Didattica", ID = 112, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Dischi in Vinile", Reparto = "CD / DVD / Didattica", ID = 113, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "DVD Italiani", Reparto = "CD / DVD / Didattica", ID = 127, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "DVD Stranieri", Reparto = "CD / DVD / Didattica", ID = 128, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "DVD / Video Didattici", Reparto = "CD / DVD / Didattica", ID = 294, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Porta CD / DVD", Reparto = "CD / DVD / Didattica", ID = 282, CategoriaCondivisaCon = " DJ Equipment" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Partiture Rock / Metal / Jazz / Blues", Reparto = "CD / DVD / Didattica", ID = 130, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Partiture Classica", Reparto = "CD / DVD / Didattica", ID = 129, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Metodi Studio / Didattica", Reparto = "CD / DVD / Didattica", ID = 293, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Libri - Biografie", Reparto = "CD / DVD / Didattica", ID = 132, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Libri - Altro", Reparto = "CD / DVD / Didattica", ID = 133, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Riviste Musicali", Reparto = "CD / DVD / Didattica", ID = 295, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Magliette / Felpe", Reparto = "Merchandising", ID = 283, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Cappellini", Reparto = "Merchandising", ID = 284, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Prodotti di Abbigliamento", Reparto = "Merchandising", ID = 285, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Portachiavi / Cinture", Reparto = "Merchandising", ID = 287, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Spille / Pin Badges", Reparto = "Merchandising", ID = 288, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Plettri per chitarra e Basso", Reparto = "Merchandising", ID = 258, CategoriaCondivisaCon = " Chitarre, Bassi" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Adesivi / Decalcomanie", Reparto = "Merchandising", ID = 303, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Tazze / Bicchieri", Reparto = "Merchandising", ID = 290, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Poster / Bandiere", Reparto = "Merchandising", ID = 289, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Gadget Elettronici", Reparto = "Merchandising", ID = 286, CategoriaCondivisaCon = "" });
			context.Categorie.AddOrUpdate(new Categorie() { Categoria = "Altri Gadget", Reparto = "Merchandising", ID = 291, CategoriaCondivisaCon = "" });
		}
	}
}
