using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class Model1Context : DbContext
{
    public Model1Context()
    {
    }

    public Model1Context(DbContextOptions<Model1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<MapPoint> MapPoints { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewImage> ReviewImages { get; set; }

    public virtual DbSet<RouteSegment> RouteSegments { get; set; }

    public virtual DbSet<ScenicSpot> ScenicSpots { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=10.106.130.93,1433;Database=Model1Db;User Id=sa;Password=Senai@123;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MapPoint>(entity =>
        {
            entity.HasKey(e => e.PointId).HasName("PK__MapPoint__40A977E1577649A6");

            entity.Property(e => e.Latitude).HasColumnType("decimal(10, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(10, 6)");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PointType)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454C953F5347E1");

            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusinessHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BusinessType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("restaurant");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Distance).HasColumnType("decimal(4, 1)");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PriceRange)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.RestaurantName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RestaurantType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79CEDBE17C78");

            entity.Property(e => e.BusinessType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ReviewContent)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ReviewTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserId__440B1D61");
        });

        modelBuilder.Entity<ReviewImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReviewIm__3214EC07F2BA67A1");

            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Review).WithMany(p => p.ReviewImages)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK__ReviewIma__Revie__4AB81AF0");
        });

        modelBuilder.Entity<RouteSegment>(entity =>
        {
            entity.HasKey(e => e.SegmentId).HasName("PK__RouteSeg__C680677BF15F9F65");

            entity.Property(e => e.ColorHex)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TrafficCondition)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.EndPoint).WithMany(p => p.RouteSegmentEndPoints)
                .HasForeignKey(d => d.EndPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteSegm__EndPo__5165187F");

            entity.HasOne(d => d.StartPoint).WithMany(p => p.RouteSegmentStartPoints)
                .HasForeignKey(d => d.StartPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RouteSegm__Start__5070F446");
        });

        modelBuilder.Entity<ScenicSpot>(entity =>
        {
            entity.HasKey(e => e.SpotId).HasName("PK__ScenicSp__61645F87CA1F7857");

            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusinessType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("scenic");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Distance).HasColumnType("decimal(4, 1)");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Latitude).HasColumnType("decimal(10, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(10, 6)");
            entity.Property(e => e.OpeningHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Rating).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.SpotName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketPrice)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TouristPeriod)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C37445870");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F2845657B4CAD6").IsUnique();

            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
