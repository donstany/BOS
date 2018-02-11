namespace BOS.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BOS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BOS.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Gifts.AddOrUpdate(x => x.GiftId,
                    new Models_Data.Gift() { GiftId = 1, Description = "Екскурзия за двама до Барселона" },
                    new Models_Data.Gift() { GiftId = 2, Description = "Спа уикенд във Велинград" },
                    new Models_Data.Gift() { GiftId = 3, Description = "Комплект луксозни писалки" },
                    new Models_Data.Gift() { GiftId = 4, Description = "Ваучер за екстремно изживяване на клуб - Адреналин" }
        );

        }
    }
}
