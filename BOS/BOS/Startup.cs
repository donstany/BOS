using BOS.Migrations;
using BOS.Models;
using Microsoft.Owin;
using Owin;

using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(BOS.Startup))]
namespace BOS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            ConfigureAuth(app);
        }
    }
}
