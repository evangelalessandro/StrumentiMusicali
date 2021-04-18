namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sottoscortaefornitori : DbMigration
    {
        public override void Up()
        {
            Sql(@"Update Clienti set tipiSoggetto='Cliente'");
        }
        
        public override void Down()
        {
        }
    }
}
