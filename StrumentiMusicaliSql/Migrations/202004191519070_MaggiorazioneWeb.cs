namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MaggiorazioneWeb : DbMigration
    {
        public override void Up()
        {
            Sql(@"update 
                      Categorie
                      set PercMaggNegozioRispettoWeb = 1
                      where  Codice in (105
                    , 111
                    )");
            Sql(@"update 
              Categorie
              set PercMaggNegozioRispettoWeb=20
              where  Codice in (
              8
            ,50
            ,28
            ,302
            ,9
            ,10
            ,83
            ,28
            ,251
            ,309
            ,257
            ,256
            ,253
            ,306
            ,258
            ,26
            ,304
            ,50
            ,302
            ,27
            ,83
            ,28
            ,259
            ,309
            ,264
            ,260
            ,306
            ,258
            ,37
            ,304
            ,50
            ,302
            ,38
            ,83
            ,267
            ,48
            ,50
            ,302
            ,51
            ,268
            ,28
            ,52
            ,61
            ,60
            ,63
            ,28
            ,50
            ,302
            ,62
            ,277
            ,278
            ,67
            ,28
            ,50
            ,302
            ,71
            ,28
            ,154
            ,50
            ,302
            ,83
            ,28
            ,154
            ,50
            ,302
            ,83
            ,281
            ,83
            ,297
            ,300
            ,298
            ,294
            ,130
            ,129
            ,293
            ,132
            ,133
            ,283
            ,284
            ,285
            ,287
            ,288
            ,303
            ,290
            ,289
            ,286
            ,291
            )");
            Sql(@"update 
                      Categorie
                      set PercMaggNegozioRispettoWeb = 30
                      where  Codice in (258
                    )");


        }

        public override void Down()
        {
        }
    }
}
