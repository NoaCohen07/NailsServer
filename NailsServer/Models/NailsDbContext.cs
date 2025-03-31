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
            entity.HasKey(e => e.MessageId).HasName("PK__ChatMess__C87C037CC7055EA3");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ChatMessageReceivers).HasConstraintName("FK__ChatMessa__Recei__2A4B4B5E");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatMessageSenders).HasConstraintName("FK__ChatMessa__Sende__29572725");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAAC3869FDE");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments).HasConstraintName("FK__Comment__PostID__5FB337D6");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PostId }).HasName("PK__Favorite__8D29EAAFBC6832DF");

            entity.HasOne(d => d.Post).WithMany(p => p.Favorites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorites__PostI__30F848ED");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favorites__UserI__31EC6D26");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => new { e.PostId, e.UserId }).HasName("PK__Likes__7B6AECF2C439D41D");

            entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Likes__PostID__628FA481");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__AA126038D9022A8A");

            entity.HasOne(d => d.User).WithMany(p => p.Posts).HasConstraintName("FK__Post__UserID__2E1BDC42");
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentId).HasName("PK__Treatmen__1A57B7111809D8A9");

            entity.HasOne(d => d.User).WithMany(p => p.Treatments).HasConstraintName("FK__Treatment__UserI__3A81B327");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC57BF2914");

            entity.Property(e => e.DateOfBirth).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Gender).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
