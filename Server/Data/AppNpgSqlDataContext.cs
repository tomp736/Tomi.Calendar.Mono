using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Server.DataServices;
using Tomi.Calendar.Mono.Server.Models;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Data
{
    public class AppNpgSqlDataContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        private readonly DbContextEvents _dbContextEvents;

        public DbSet<ApplicationUserCalendarItem> ApplicationUserCalendarItem { get; set; }
        public DbSet<ApplicationUserNote> ApplicationUserNotes { get; set; }

        public DbSet<CalendarItem> CalendarItems { get; set; }

        public DbSet<CalendarItemTag> CalendarItemTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<CalendarItemNote> CalendarItemNotes { get; set; }
        public DbSet<Note> Notes { get; set; }

        public AppNpgSqlDataContext(
            DbContextOptions options,
            DbContextEvents dbContextEvents,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            _dbContextEvents = dbContextEvents;
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
            modelBuilder.Entity<ApplicationUserCalendarItem>()
                .HasKey(bc => new { bc.CalendarItemKey, bc.UserKey });

            modelBuilder.Entity<ApplicationUserCalendarItem>()
                .HasOne(c => c.User)
                .WithMany(bc => bc.UserCalendarItems)
                .HasForeignKey(k => k.UserKey);

            // Relationships
            modelBuilder.Entity<ApplicationUserNote>()
                .HasKey(bc => new { bc.NoteKey, bc.UserKey });

            modelBuilder.Entity<ApplicationUserNote>()
                .HasOne(c => c.User)
                .WithMany(bc => bc.UserNotes)
                .HasForeignKey(k => k.UserKey);

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

        public override int SaveChanges()
        {
            StateChanged();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StateChanged();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            StateChanged();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            StateChanged();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void StateChanged()
        {
            var changedEntities = ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged);
            List<Type> typesChanged = new List<Type>();
            foreach(var changedEntity in changedEntities)
            {
                var typeChanged = changedEntity.Entity.GetType();
                if (!typesChanged.Contains(typeChanged))
                {
                    typesChanged.Add(typeChanged);
                }
            }
            _dbContextEvents.EntitiesChanged?.Invoke(typesChanged);
        }
    }
}
