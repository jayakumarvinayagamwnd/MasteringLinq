using LinqExercises.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinqExercises.Shared.Data;
public sealed class ChinookDbContext : DbContext
{
    public ChinookDbContext(DbContextOptions<ChinookDbContext> options) : base(options)
    {
    }

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; } 
    public DbSet<MediaType> MediaTypes { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
    public DbSet<Track> Tracks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       base.OnModelCreating(modelBuilder);
       
       // Album entity configuration
       modelBuilder.Entity<Album>( entity =>
       {
            entity.ToTable("Albums");
            entity.HasKey(e => e.AlbumId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(160);
            entity.HasOne(e => e.Artist)
                  .WithMany(a => a.Albums)
                  .HasForeignKey(e => e.ArtistId);
       });
       
       // Artist entity configuration
       modelBuilder.Entity<Artist>( entity =>
       {
            entity.ToTable("Artists");
            entity.HasKey(e => e.ArtistId);
            entity.Property(e => e.Name).HasMaxLength(120);
       });

       // Customer entity configuration
       modelBuilder.Entity<Customer>( entity =>
       {
            entity.ToTable("Customers");
            entity.HasKey(e => e.CustomerId);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(40);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Company).HasMaxLength(80);
            entity.Property(e => e.Address).HasMaxLength(70);
            entity.Property(e => e.City).HasMaxLength(40);
            entity.Property(e => e.State).HasMaxLength(40);
            entity.Property(e => e.Country).HasMaxLength(40);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(24);
            entity.Property(e => e.Fax).HasMaxLength(24);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(60);
            entity.HasOne(e => e.SupportRep)
                  .WithMany(emp => emp.Customers)
                  .HasForeignKey(e => e.SupportRepId)
                  .OnDelete(DeleteBehavior.SetNull);
       });

       // Employee entity configuration
       modelBuilder.Entity<Employee>( entity =>
       {
            entity.ToTable("Employees");
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(20);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(30);
            entity.Property(e => e.Address).HasMaxLength(70);
            entity.Property(e => e.City).HasMaxLength(40);
            entity.Property(e => e.State).HasMaxLength(40);
            entity.Property(e => e.Country).HasMaxLength(40);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(24);
            entity.Property(e => e.Fax).HasMaxLength(24);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(60);
            entity.HasOne(e => e.Manager)
                  .WithMany(m => m.Subordinates)
                  .HasForeignKey(e => e.ReportsTo)
                  .OnDelete(DeleteBehavior.SetNull);
       });

       // Genre entity configuration
       modelBuilder.Entity<Genre>( entity =>
       {
            entity.ToTable("Genres");
            entity.HasKey(e => e.GenreId);
            entity.Property(e => e.Name).HasMaxLength(120);
       });

       // Invoice entity configuration
       modelBuilder.Entity<Invoice>( entity =>
       {
            entity.ToTable("Invoices");
            entity.HasKey(e => e.InvoiceId);
            entity.Property(e => e.BillingAddress).HasMaxLength(70);
            entity.Property(e => e.BillingCity).HasMaxLength(40);
            entity.Property(e => e.BillingState).HasMaxLength(40);
            entity.Property(e => e.BillingCountry).HasMaxLength(40);
            entity.Property(e => e.BillingPostalCode).HasMaxLength(10);
            entity.Property(e => e.Total).HasColumnType("decimal(10,2)");
            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Invoices)
                  .HasForeignKey(e => e.CustomerId);
       });

       // InvoiceItem entity configuration
         modelBuilder.Entity<InvoiceItem>( entity =>
         {
                entity.ToTable("Invoice_Items");
                entity.HasKey(e => e.InvoiceLineId);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Invoice)
                    .WithMany(i => i.InvoiceItems)
                    .HasForeignKey(e => e.InvoiceId);
                entity.HasOne(e => e.Track)
                    .WithMany(t => t.InvoiceItems)
                    .HasForeignKey(e => e.TrackId);
         });

        // MediaType entity configuration
         modelBuilder.Entity<MediaType>( entity =>
         {
                entity.ToTable("Media_Types");
                entity.HasKey(e => e.MediaTypeId);
                entity.Property(e => e.Name).HasMaxLength(120);
         });

        // Playlist entity configuration
         modelBuilder.Entity<Playlist>( entity =>
         {
                entity.ToTable("Playlists");
                entity.HasKey(e => e.PlaylistId);
                entity.Property(e => e.Name).HasMaxLength(120);
         });

         // PlaylistTrack entity configuration
         modelBuilder.Entity<PlaylistTrack>( entity =>
         {
                entity.ToTable("Playlist_Track");
                entity.HasKey(e => new { e.PlaylistId, e.TrackId });
                entity.HasOne(e => e.Playlist)
                    .WithMany(p => p.PlaylistTracks)
                    .HasForeignKey(e => e.PlaylistId);
                entity.HasOne(e => e.Track)
                    .WithMany(t => t.Playlists)
                    .HasForeignKey(e => e.TrackId);
         });

         // Track entity configuration
         modelBuilder.Entity<Track>( entity =>
         {
                entity.ToTable("Tracks");
                entity.HasKey(e => e.TrackId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Album)
                      .WithMany(a => a.Tracks)
                      .HasForeignKey(e => e.AlbumId);
                entity.HasOne(e => e.Genre)
                      .WithMany(g => g.Tracks)
                      .HasForeignKey(e => e.GenreId)
                      .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.MediaType)
                      .WithMany(m => m.Tracks)
                      .HasForeignKey(e => e.MediaTypeId);
         });
    }
}