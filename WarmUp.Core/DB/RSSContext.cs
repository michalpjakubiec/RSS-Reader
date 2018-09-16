using System.Data.Entity;
using WarmUp.Core.Models;

namespace WarmUp.Core.DB
{
    public class RSSContext : DbContext
    {
        public DbSet<RSSChannel> RSSChannels { get; set; }
        public DbSet<RSSItem> RSSItems { get; set; }

        public RSSContext() : base("RSSContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<RSSContext>());
            base.OnModelCreating(modelBuilder);
        }

        public void Configure()
        {
            if (Database.Exists())
                return;
            Database.Create();
        }

    }
}
