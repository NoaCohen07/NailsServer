using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NailsServer.Models;

public partial class NailsDbContext : DbContext
{
    public NailsDbContext()
    {
    }

    public NailsDbContext(DbContextOptions<NailsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Treatment> Treatments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=NailsDB;User ID=NailsLogin;Password=12345;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C037C100B72C1");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ChatMessageReceivers).HasConstraintName("FK__ChatMessa__Recei__2A4B4B5E");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatMessageSenders).HasConstraintName("FK__ChatMessa__Sende__29572725");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAAE33D855E");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments).HasConstraintName("FK__Comment__PostID__33D4B598");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Favorite__AA1260389AE54315");

            entity.Property(e => e.PostId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Post).WithOne(p => p.Favorite)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorites__PostI__30F848ED");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites).HasConstraintName("FK__Favorites__UserI__300424B4");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => new { e.PostId, e.UserId }).HasName("PK__Likes__7B6AECF2C36C3CE2");

            entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Likes__PostID__36B12243");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__AA126038A02A2406");

            entity.HasOne(d => d.User).WithMany(p => p.Posts).HasConstraintName("FK__Post__UserID__2D27B809");
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentId).HasName("PK__Treatmen__1A57B71109614577");

            entity.HasOne(d => d.User).WithMany(p => p.Treatments).HasConstraintName("FK__Treatment__UserI__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACCFB26C14");

            entity.Property(e => e.DateOfBirth).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Gender).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
