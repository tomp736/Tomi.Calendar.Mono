using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tomi.Calendar.Mono.Server.Models;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Data
{
    public class AppNpgSqlDataContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<CalendarItem> CalendarItems { get; set; }
        public DbSet<CalendarItemTag> CalendarItemTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public AppNpgSqlDataContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Keys
            modelBuilder.Entity<CalendarItem>().Property(f => f.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CalendarItem>().Property(f => f.Key).ValueGeneratedOnAdd();

            modelBuilder.Entity<Tag>().Property(f => f.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Tag>().Property(f => f.Key).ValueGeneratedOnAdd();

            modelBuilder.Entity<Note>().Property(f => f.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Note>().Property(f => f.Key).ValueGeneratedOnAdd();


            // Relationships
            modelBuilder.Entity<CalendarItemTag>()
                .HasKey(bc => new { bc.CalendarItemKey, bc.TagKey });

            modelBuilder.Entity<CalendarItemTag>()
                .HasOne(bc => bc.CalendarItem)
                .WithMany(b => b.CalendarItemTags)
                .HasForeignKey(bc => bc.CalendarItemKey);

            modelBuilder.Entity<CalendarItemTag>()
                .HasOne(bc => bc.Tag)
                .WithMany(c => c.CalendarItemTags)
                .HasForeignKey(bc => bc.TagKey);


            // Relationships
            modelBuilder.Entity<CalendarItemNote>()
                .HasKey(bc => new { bc.CalendarItemKey, bc.NoteKey });

            modelBuilder.Entity<CalendarItemNote>()
                .HasOne(bc => bc.CalendarItem)
                .WithMany(b => b.CalendarItemNotes)
                .HasForeignKey(bc => bc.CalendarItemKey);

            modelBuilder.Entity<CalendarItemNote>()
                .HasOne(bc => bc.Note)
                .WithMany(c => c.CalendarItemNotes)
                .HasForeignKey(bc => bc.NoteKey);

            base.OnModelCreating(modelBuilder);
        }
    }
}
