using System;
using System.Collections.Generic;
using GiapDepTrai1.Models;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Models;

public partial class QlChuoiNhaHang1Context : DbContext
{
    public QlChuoiNhaHang1Context()
    {
    }

    public QlChuoiNhaHang1Context(DbContextOptions<QlChuoiNhaHang1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<SessionState> SessionStates { get; set; }

    public virtual DbSet<TblAccount> TblAccounts { get; set; }

    public virtual DbSet<TblAdminMenu> TblAdminMenus { get; set; }

    public virtual DbSet<TblBranch> TblBranches { get; set; }

    public virtual DbSet<TblCart> TblCarts { get; set; }

    public virtual DbSet<TblCartItem> TblCartItems { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblComment> TblComments { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblEmployee> TblEmployees { get; set; }

    public virtual DbSet<TblEmployeeOrder> TblEmployeeOrders { get; set; }

    public virtual DbSet<TblEmployeeOrderDetail> TblEmployeeOrderDetails { get; set; }

    public virtual DbSet<TblInvoice> TblInvoices { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblMenuItem> TblMenuItems { get; set; }

    public virtual DbSet<TblMessageContact> TblMessageContacts { get; set; }

    public virtual DbSet<TblNavigationHome> TblNavigationHomes { get; set; }

    public virtual DbSet<TblPayment> TblPayments { get; set; }

    public virtual DbSet<TblPromotion> TblPromotions { get; set; }

    public virtual DbSet<TblReservation> TblReservations { get; set; }

    public virtual DbSet<TblReservationMenuItem> TblReservationMenuItems { get; set; }

    public virtual DbSet<TblReservationPayment> TblReservationPayments { get; set; }

    public virtual DbSet<TblSlideHome> TblSlideHomes { get; set; }

    public virtual DbSet<TblStoryHome> TblStoryHomes { get; set; }

    public virtual DbSet<TblTable> TblTables { get; set; }

    public virtual DbSet<TblTableType> TblTableTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-8T25JED;Initial Catalog=QL_Chuoi_Nha_Hang1;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SessionS__3214EC07BFB4F798");

            entity.ToTable("SessionState");

            entity.HasIndex(e => e.ExpiresAtTime, "Index_ExpiresAtTime");

            entity.Property(e => e.Id)
                .HasMaxLength(449)
                .UseCollation("SQL_Latin1_General_CP1_CS_AS");
        });

        modelBuilder.Entity<TblAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tblAccou__349DA5866BAF9571");

            entity.ToTable("tblAccount");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.PassWords).HasMaxLength(250);
            entity.Property(e => e.Roles).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblAdminMenu>(entity =>
        {
            entity.HasKey(e => e.AdmenuId).HasName("PK__tblAdmin__39139EBD6526398C");

            entity.ToTable("tblAdminMenus");

            entity.Property(e => e.AdmenuId).HasColumnName("ADMenuID");
            entity.Property(e => e.ActionName).HasMaxLength(50);
            entity.Property(e => e.AdmenuName)
                .HasMaxLength(20)
                .HasColumnName("ADMenuName");
            entity.Property(e => e.AreaName).HasMaxLength(50);
            entity.Property(e => e.ControllerName).HasMaxLength(50);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Idname)
                .HasMaxLength(50)
                .HasColumnName("IDName");
            entity.Property(e => e.ItemTaget).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(250);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Roles).HasMaxLength(50);
        });

        modelBuilder.Entity<TblBranch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__tblBranc__A1682FA5191BD799");

            entity.ToTable("tblBranches");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.BranchName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.StatusBranch)
                .HasMaxLength(20)
                .HasDefaultValue("Hoạt động");
        });

        modelBuilder.Entity<TblCart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B73379E77F");

            entity.ToTable("tblCart");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

            entity.HasOne(d => d.Customers).WithMany(p => p.TblCarts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_tblCart_tblCustomers");
        });

        modelBuilder.Entity<TblCartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A5CB3D920");

            entity.ToTable("tblCartItem");

            entity.Property(e => e.Note).HasMaxLength(250);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItem)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__CartId__2334397B");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItem__MenuIt__24285DB4");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tblCateg__19093A2B59272378");

            entity.ToTable("tblCategories");

            entity.HasIndex(e => e.CategoryName, "UQ__tblCateg__8517B2E054B7D385").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__tblComme__C3B4DFAACA5DEB66");

            entity.ToTable("tblComment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblComments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__tblCommen__Custo__1D7B6025");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.TblComments)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblCommen__MenuI__1C873BEC");
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__tblCusto__A4AE64B81EB7191F");

            entity.ToTable("tblCustomers");

            entity.HasIndex(e => e.Phone, "UQ__tblCusto__5C7E359EE4D1E68E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__tblCusto__A9D10534A220FC12").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IdentityCard).HasMaxLength(50);
            entity.Property(e => e.Img).HasMaxLength(250);
            entity.Property(e => e.Passwork).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(15);
        });

        modelBuilder.Entity<TblEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__tblEmplo__7AD04FF1E4E63ECE");

            entity.ToTable("tblEmployees");

            entity.HasIndex(e => e.Phone, "UQ__tblEmplo__5C7E359E1A89A23E").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Addresss).HasMaxLength(50);
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.IdentityCard).HasMaxLength(50);
            entity.Property(e => e.Img).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Position).HasMaxLength(50);

            entity.HasOne(d => d.Branch).WithMany(p => p.TblEmployees)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblEmploy__Branc__440B1D61");
        });

        modelBuilder.Entity<TblEmployeeOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__tblEmplo__C3905BAF8C718683");

            entity.ToTable("tblEmployeeOrder");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.OrderTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ xử lý");
            entity.Property(e => e.TableId).HasColumnName("TableID");

            entity.HasOne(d => d.Branch).WithMany(p => p.TblEmployeeOrders)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__tblEmploy__Branc__5CD6CB2B");

            entity.HasOne(d => d.Employee).WithMany(p => p.TblEmployeeOrders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__tblEmploy__Emplo__5DCAEF64");

            entity.HasOne(d => d.Table).WithMany(p => p.TblEmployeeOrders)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__tblEmploy__Table__5EBF139D");
        });

        modelBuilder.Entity<TblEmployeeOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__tblEmplo__D3B9D30C4350D324");

            entity.ToTable("tblEmployeeOrderDetails");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.TblEmployeeOrderDetails)
                .HasForeignKey(d => d.MenuItemId)
                .HasConstraintName("FK__tblEmploy__MenuI__6383C8BA");

            entity.HasOne(d => d.Order).WithMany(p => p.TblEmployeeOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__tblEmploy__Order__628FA481");
        });

        modelBuilder.Entity<TblInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__tblInvoi__D796AAD514C3EA63");

            entity.ToTable("tblInvoices");

            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FinalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.InvoiceDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Chua thanh toán");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.TblInvoices)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__tblInvoic__Order__41EDCAC5");
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__tblMenus__C99ED250156B0DC8");

            entity.ToTable("tblMenus");

            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.ActionName).HasMaxLength(50);
            entity.Property(e => e.ControllerName).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(250);
            entity.Property(e => e.MenuName).HasMaxLength(20);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
        });

        modelBuilder.Entity<TblMenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("PK__tblMenuI__8943F70247A71D4F");

            entity.ToTable("tblMenuItems");

            entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Count_Saled).HasColumnName("Count_Saled");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.IsAvailable).HasDefaultValue(false);
            entity.Property(e => e.ItemName).HasMaxLength(150);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Branch).WithMany(p => p.TblMenuItems)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__tblMenuIt__Branc__5629CD9C");

            entity.HasOne(d => d.Category).WithMany(p => p.TblMenuItems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__tblMenuIt__Categ__571DF1D5");
        });

        modelBuilder.Entity<TblMessageContact>(entity =>
        {
            entity.HasKey(e => e.ContactID);

            entity.ToTable("tblMessageContact");

            entity.Property(e => e.ContactID).HasColumnName("ContactID");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Message).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<TblNavigationHome>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblNavigation_Home");

            entity.Property(e => e.Nav1).HasMaxLength(50);
            entity.Property(e => e.Nav2).HasMaxLength(50);
            entity.Property(e => e.Nav3).HasMaxLength(50);
            entity.Property(e => e.NavigationId).HasColumnName("NavigationID");
            entity.Property(e => e.PicNav1).HasMaxLength(250);
            entity.Property(e => e.PicNav2).HasMaxLength(250);
            entity.Property(e => e.PicNav3).HasMaxLength(250);
        });

        modelBuilder.Entity<TblPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__tblPayme__9B556A5816EED28F");

            entity.ToTable("tblPayments");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.HasOne(d => d.Invoice).WithMany(p => p.TblPayments)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__tblPaymen__Invoi__4B7734FF");
        });

        modelBuilder.Entity<TblPromotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__tblPromo__52C42F2FA094B17D");

            entity.ToTable("tblPromotions");

            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.DiscountType).HasMaxLength(50);
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinOrderAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.PromotionName).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

         modelBuilder.Entity<TblReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__tblReser__B7EE5F04FC81EB99");

            entity.ToTable("tblReservations");

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.ReservationTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ xác nhận");
            entity.Property(e => e.TableId).HasColumnName("TableID");

            entity.HasOne(d => d.Branch).WithMany(p => p.TblReservations)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_tblReservations_tblBranches");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblReservations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__tblReserv__Custo__0D7A0286");

            entity.HasOne(d => d.Table).WithMany(p => p.TblReservations)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__tblReserv__Table__0E6E26BF");
        });

        modelBuilder.Entity<TblReservationMenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblReser__3214EC072B15C84F");

            entity.ToTable("TblReservationMenuItem");

            entity.Property(e => e.Note).HasColumnType("ntext");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.TblReservationMenuItems)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservationMenuItem_MenuItem");

            entity.HasOne(d => d.Reservation).WithMany(p => p.TblReservationMenuItems)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK_ReservationMenuItem_Reservation");
        });

         modelBuilder.Entity<TblReservationPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__tblReser__9B556A586FB8EC46");

            entity.ToTable("tblReservationPayments");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

            entity.HasOne(d => d.CustomerPayment).WithMany(p => p.TblReservationPayments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_tblReservationPayments_tblCustomers");

            entity.HasOne(d => d.Reservation).WithMany(p => p.TblReservationPayments)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK__tblReserv__Reser__27F8EE98");
        });

        modelBuilder.Entity<TblSlideHome>(entity =>
        {
            entity.HasKey(e => e.Slide1Id);

            entity.ToTable("tblSlide_Home");

            entity.Property(e => e.Slide1Id).HasColumnName("Slide1ID");
            entity.Property(e => e.Contents1).HasColumnType("ntext");
            entity.Property(e => e.Contents2).HasColumnType("ntext");
            entity.Property(e => e.Img1).HasMaxLength(250);
            entity.Property(e => e.Img2).HasMaxLength(250);
            entity.Property(e => e.Title1).HasMaxLength(50);
            entity.Property(e => e.Title2).HasMaxLength(50);
        });

        modelBuilder.Entity<TblStoryHome>(entity =>
        {
            entity.HasKey(e => e.StoryId);

            entity.ToTable("tblStory_Home");

            entity.Property(e => e.StoryId).HasColumnName("StoryID");
            entity.Property(e => e.Contents1).HasColumnType("ntext");
            entity.Property(e => e.Contents2).HasColumnType("ntext");
            entity.Property(e => e.MainContents).HasColumnType("ntext");
            entity.Property(e => e.Pic1).HasMaxLength(150);
            entity.Property(e => e.Pic2).HasMaxLength(150);
            entity.Property(e => e.Title1).HasMaxLength(50);
            entity.Property(e => e.Title2).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTable>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__tblTable__7D5F018E0663EAE2");

            entity.ToTable("tblTable");

            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TableName).HasMaxLength(50);
            entity.Property(e => e.TableTypeId).HasColumnName("TableTypeID");

            entity.HasOne(d => d.Branch).WithMany(p => p.TblTables)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__tblTable__Branch__4E88ABD4");

            entity.HasOne(d => d.TableType).WithMany(p => p.TblTables)
                .HasForeignKey(d => d.TableTypeId)
                .HasConstraintName("FK__tblTable__TableT__4D94879B");
        });

        modelBuilder.Entity<TblTableType>(entity =>
        {
            entity.HasKey(e => e.TableTypeId).HasName("PK__tblTable__2B99D0998CCDC1F3");

            entity.ToTable("tblTableType");

            entity.Property(e => e.TableTypeId).HasColumnName("TableTypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
