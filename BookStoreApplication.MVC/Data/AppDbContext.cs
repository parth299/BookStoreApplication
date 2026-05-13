using BookStoreApplication.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.MVC.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Bookreview> Bookreviews { get; set; }
    public virtual DbSet<Reviewer> Reviewers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn);
            entity.ToTable("book");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.Title).HasMaxLength(70);
            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<Reviewer>(entity =>
        {
            entity.HasKey(e => e.ReviewerId);
            entity.ToTable("reviewer");
            entity.Property(e => e.ReviewerId).HasColumnName("ReviewerID").ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.EmployedBy).HasMaxLength(30);
        });

        modelBuilder.Entity<Bookreview>(entity =>
        {
            entity.HasKey(e => new { e.Isbn, e.ReviewerId });
            entity.ToTable("bookreview");
            entity.HasIndex(e => e.ReviewerId);
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.ReviewerId).HasColumnName("ReviewerID");
            entity.Property(e => e.Comments).HasMaxLength(255);

            entity.HasOne(d => d.IsbnNavigation)
                .WithMany(p => p.Bookreviews)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookreview_ISBN");

            entity.HasOne(d => d.Reviewer)
                .WithMany(p => p.Bookreviews)
                .HasForeignKey(d => d.ReviewerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_bookreview_ReviewerID");
        });
    }
}
