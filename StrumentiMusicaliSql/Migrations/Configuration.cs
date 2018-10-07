using System.Data.Entity.Migrations;

namespace StrumentiMusicaliSql.Migrations
{

	internal sealed class Configuration : DbMigrationsConfiguration<StrumentiMusicaliSql.Model.ModelSm>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StrumentiMusicaliSql.Model.ModelSm context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
