using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace OpenSongWeb.Data
{
    public class SongDbContext : IdentityDbContext<AppUser, AppUserRole, string>
    {
        public SongDbContext(DbContextOptions<SongDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            

            modelbuilder.Entity<OSSong>()
                .Property(b => b.ID)
                .ValueGeneratedOnAdd();

            modelbuilder.Entity<OSSong>()
                .Property(b => b.CreatedDateUTC)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql(this.Database.IsSqlite() ? "SELECT DATETIME('now')" :  "GETUTCDATE()");

            modelbuilder.Entity<OSSong>()
                .Property(b => b.LastUpdatedDateUTC)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql(this.Database.IsSqlite() ? "SELECT DATETIME('now')" : "GETUTCDATE()");

            modelbuilder.Entity<OSSong>()
                .Property(b => b.Copyright)
                .HasDefaultValueSql("''");


            modelbuilder.Entity<OSSong>()
                .HasIndex(m => m.Author);

            modelbuilder.Entity<OSSong>()
                .HasOne(b => b.CreatedBy)
                .WithMany(u => u.SongsCreated)
                .HasForeignKey(m => m.CreatedByID);

            modelbuilder.Entity<OSSong>()
                .HasIndex(m => m.CreatedDateUTC);

            modelbuilder.Entity<OSSong>()
                .HasIndex(m => m.Title);

            modelbuilder.Entity<OSSong>()
                .HasIndex(m => new { m.Title, m.Author })
                .IsUnique();

            modelbuilder.Entity<OSSong>()
                .HasIndex(m => m.Filename)
                .IsUnique();

            modelbuilder.Entity<OSSong>()
                .Property(e => e.VideoLinkType)
                .HasConversion(
                    v => v.ToString(),
                    v => (EmbedLinkType)Enum.Parse(typeof(EmbedLinkType), v));

            modelbuilder.Entity<SetEntry>()
                .HasOne(e => e.SongSet)
                .WithMany(set => set.SetEntries)
                .HasForeignKey(s => s.SongSetID);


            modelbuilder.Query<AppUserBrief>().ToView("View_AppUserBriefs");
                //.HasOne(e => e.Id)
        }

        public DbSet<OSSong> OSSongs { get; set; }
        public DbSet<SetType> SetTypes { get; set; }
        public DbSet<SongSet> Sets { get; set; }

        public DbSet<SetEntry> SetEntries { get; set; }

        public DbQuery<AppUserBrief> AppUserBriefs { get; set; }
    }
}
