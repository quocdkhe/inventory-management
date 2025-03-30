using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Models;

public partial class InventoryManagementContext : DbContext
{
    public static InventoryManagementContext INSTANCE = new InventoryManagementContext();
    public InventoryManagementContext()
    {
    }

    public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CashFlow> CashFlows { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DeliveryDetail> DeliveryDetails { get; set; }

    public virtual DbSet<Object> Objects { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(config.GetValue<string>("ConnectionStrings:DBContext"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CashFlow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TotalMon__3214EC07A8CE68ED");

            entity.ToTable("CashFlow");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07C5ACCE60");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContractDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.MoreInfo).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deliveri__3214EC07010EBD1C");

            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");

            entity.HasOne(d => d.Customer).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Deliverie__Custo__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Deliverie__UserI__5165187F");
        });

        modelBuilder.Entity<DeliveryDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Delivery__3214EC079461727B");

            entity.ToTable("DeliveryDetail");

            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.Delivery).WithMany(p => p.DeliveryDetails)
                .HasForeignKey(d => d.DeliveryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeliveryD__Deliv__5441852A");

            entity.HasOne(d => d.Object).WithMany(p => p.DeliveryDetails)
                .HasForeignKey(d => d.ObjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeliveryD__Objec__5535A963");
        });

        modelBuilder.Entity<Object>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Objects__3214EC07CE3361D2");

            entity.Property(e => e.BarCode).HasMaxLength(255);
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Qrcode)
                .HasMaxLength(255)
                .HasColumnName("QRCode");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Objects)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Objects__Supplie__3E52440B");

            entity.HasOne(d => d.Unit).WithMany(p => p.Objects)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Objects__UnitId__3D5E1FD2");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Receipts__3214EC0705991701");

            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receipts__Suppli__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receipts__UserId__46E78A0C");
        });

        modelBuilder.Entity<ReceiptDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReceiptD__3214EC07438D95DE");

            entity.ToTable("ReceiptDetail");

            entity.HasOne(d => d.Object).WithMany(p => p.ReceiptDetails)
                .HasForeignKey(d => d.ObjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReceiptDe__Objec__4AB81AF0");

            entity.HasOne(d => d.Receipt).WithMany(p => p.ReceiptDetails)
                .HasForeignKey(d => d.ReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReceiptDe__Recei__49C3F6B7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0728412125");

            entity.Property(e => e.DisplayName).HasMaxLength(255);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stock__3214EC077FEC0180");

            entity.ToTable("Stock");

            entity.HasOne(d => d.Object).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ObjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__ObjectId__4D94879B");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07C00CCE9E");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContractDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.MoreInfo).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Units__3214EC07EDF258A7");

            entity.Property(e => e.DisplayName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07EF739CF3");

            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
