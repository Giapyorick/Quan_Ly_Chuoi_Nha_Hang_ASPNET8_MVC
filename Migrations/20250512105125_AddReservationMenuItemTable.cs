using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiapDepTrai.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationMenuItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAccount",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PassWords = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    Roles = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblAccou__349DA5866BAF9571", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "tblAdminMenus",
                columns: table => new
                {
                    ADMenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ADMenuName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    AreaName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Levels = table.Column<int>(type: "int", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MenuOrder = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    ItemTaget = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IDName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Roles = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblAdmin__39139EBD6526398C", x => x.ADMenuID);
                });

            migrationBuilder.CreateTable(
                name: "tblBranches",
                columns: table => new
                {
                    BranchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    StatusBranch = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Hoạt động")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblBranc__A1682FA5191BD799", x => x.BranchID);
                });

            migrationBuilder.CreateTable(
                name: "tblCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblCateg__19093A2B59272378", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "tblCustomers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    BirthofDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IdentityCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Img = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblCusto__A4AE64B81EB7191F", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "tblMenus",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Levels = table.Column<int>(type: "int", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MenuOrder = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblMenus__C99ED250156B0DC8", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "tblMessageContact",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "ntext", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMessageContact", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "tblNavigation_Home",
                columns: table => new
                {
                    NavigationID = table.Column<int>(type: "int", nullable: true),
                    Nav1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PicNav1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Nav2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PicNav2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Nav3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PicNav3 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tblPromotions",
                columns: table => new
                {
                    PromotionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiscountValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MinOrderAmount = table.Column<decimal>(type: "decimal(12,2)", nullable: true, defaultValue: 0.00m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblPromo__52C42F2FA094B17D", x => x.PromotionID);
                });

            migrationBuilder.CreateTable(
                name: "tblSlide_Home",
                columns: table => new
                {
                    Slide1ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contents1 = table.Column<string>(type: "ntext", nullable: true),
                    Contents2 = table.Column<string>(type: "ntext", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Img1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Img2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSlide_Home", x => x.Slide1ID);
                });

            migrationBuilder.CreateTable(
                name: "tblStory_Home",
                columns: table => new
                {
                    StoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contents1 = table.Column<string>(type: "ntext", nullable: true),
                    Pic1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Title2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Contents2 = table.Column<string>(type: "ntext", nullable: true),
                    Pic2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    MainContents = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStory_Home", x => x.StoryID);
                });

            migrationBuilder.CreateTable(
                name: "tblTableType",
                columns: table => new
                {
                    TableTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblTable__2B99D0998CCDC1F3", x => x.TableTypeID);
                });

            migrationBuilder.CreateTable(
                name: "tblEmployees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateofBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    IdentityCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addresss = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    Img = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblEmplo__7AD04FF1E4E63ECE", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK__tblEmploy__Branc__440B1D61",
                        column: x => x.BranchID,
                        principalTable: "tblBranches",
                        principalColumn: "BranchID");
                });

            migrationBuilder.CreateTable(
                name: "tblMenuItems",
                columns: table => new
                {
                    MenuItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    Count_Saled = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblMenuI__8943F70247A71D4F", x => x.MenuItemID);
                    table.ForeignKey(
                        name: "FK__tblMenuIt__Branc__5629CD9C",
                        column: x => x.BranchID,
                        principalTable: "tblBranches",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK__tblMenuIt__Categ__571DF1D5",
                        column: x => x.CategoryID,
                        principalTable: "tblCategories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tblTable",
                columns: table => new
                {
                    TableID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TableTypeID = table.Column<int>(type: "int", nullable: true),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblTable__7D5F018E0663EAE2", x => x.TableID);
                    table.ForeignKey(
                        name: "FK__tblTable__Branch__4E88ABD4",
                        column: x => x.BranchID,
                        principalTable: "tblBranches",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK__tblTable__TableT__4D94879B",
                        column: x => x.TableTypeID,
                        principalTable: "tblTableType",
                        principalColumn: "TableTypeID");
                });

            migrationBuilder.CreateTable(
                name: "TblBranchTblEmployee",
                columns: table => new
                {
                    BranchesBranchId = table.Column<int>(type: "int", nullable: false),
                    EmployeesEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBranchTblEmployee", x => new { x.BranchesBranchId, x.EmployeesEmployeeId });
                    table.ForeignKey(
                        name: "FK_TblBranchTblEmployee_tblBranches_BranchesBranchId",
                        column: x => x.BranchesBranchId,
                        principalTable: "tblBranches",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblBranchTblEmployee_tblEmployees_EmployeesEmployeeId",
                        column: x => x.EmployeesEmployeeId,
                        principalTable: "tblEmployees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblEmployeeOrder",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    TableID = table.Column<int>(type: "int", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Chờ xử lý"),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BranchID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblEmplo__C3905BAF8C718683", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK__tblEmploy__Branc__5CD6CB2B",
                        column: x => x.BranchID,
                        principalTable: "tblBranches",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__tblEmploy__Emplo__5DCAEF64",
                        column: x => x.EmployeeID,
                        principalTable: "tblEmployees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__tblEmploy__Table__5EBF139D",
                        column: x => x.TableID,
                        principalTable: "tblTable",
                        principalColumn: "TableID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReservations",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    TableID = table.Column<int>(type: "int", nullable: false),
                    ReservationTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Chờ xác nhận"),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MenuItemID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblReser__B7EE5F04FC81EB99", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK__tblReserv__Custo__0D7A0286",
                        column: x => x.CustomerID,
                        principalTable: "tblCustomers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__tblReserv__Table__0E6E26BF",
                        column: x => x.TableID,
                        principalTable: "tblTable",
                        principalColumn: "TableID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblReservations_tblMenuItems",
                        column: x => x.MenuItemID,
                        principalTable: "tblMenuItems",
                        principalColumn: "MenuItemID");
                });

            migrationBuilder.CreateTable(
                name: "tblEmployeeOrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MenuItemID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblEmplo__D3B9D30C4350D324", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK__tblEmploy__MenuI__6383C8BA",
                        column: x => x.MenuItemID,
                        principalTable: "tblMenuItems",
                        principalColumn: "MenuItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__tblEmploy__Order__628FA481",
                        column: x => x.OrderID,
                        principalTable: "tblEmployeeOrder",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblInvoices",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0.00m),
                    FinalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Chua thanh toán")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblInvoi__D796AAD514C3EA63", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK__tblInvoic__Order__41EDCAC5",
                        column: x => x.OrderID,
                        principalTable: "tblEmployeeOrder",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblPayments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceID = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblPayme__9B556A5816EED28F", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK__tblPaymen__Invoi__4B7734FF",
                        column: x => x.InvoiceID,
                        principalTable: "tblInvoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblBranchTblEmployee_EmployeesEmployeeId",
                table: "TblBranchTblEmployee",
                column: "EmployeesEmployeeId");

            migrationBuilder.CreateIndex(
                name: "UQ__tblCateg__8517B2E054B7D385",
                table: "tblCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__tblCusto__5C7E359EE4D1E68E",
                table: "tblCustomers",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__tblCusto__A9D10534A220FC12",
                table: "tblCustomers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeOrder_BranchID",
                table: "tblEmployeeOrder",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeOrder_EmployeeID",
                table: "tblEmployeeOrder",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeOrder_TableID",
                table: "tblEmployeeOrder",
                column: "TableID");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeOrderDetails_MenuItemID",
                table: "tblEmployeeOrderDetails",
                column: "MenuItemID");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeOrderDetails_OrderID",
                table: "tblEmployeeOrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployees_BranchID",
                table: "tblEmployees",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "UQ__tblEmplo__5C7E359E1A89A23E",
                table: "tblEmployees",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblInvoices_OrderID",
                table: "tblInvoices",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_tblMenuItems_BranchID",
                table: "tblMenuItems",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_tblMenuItems_CategoryID",
                table: "tblMenuItems",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_tblPayments_InvoiceID",
                table: "tblPayments",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_tblReservations_CustomerID",
                table: "tblReservations",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_tblReservations_MenuItemID",
                table: "tblReservations",
                column: "MenuItemID");

            migrationBuilder.CreateIndex(
                name: "IX_tblReservations_TableID",
                table: "tblReservations",
                column: "TableID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTable_BranchID",
                table: "tblTable",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_tblTable_TableTypeID",
                table: "tblTable",
                column: "TableTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAccount");

            migrationBuilder.DropTable(
                name: "tblAdminMenus");

            migrationBuilder.DropTable(
                name: "TblBranchTblEmployee");

            migrationBuilder.DropTable(
                name: "tblEmployeeOrderDetails");

            migrationBuilder.DropTable(
                name: "tblMenus");

            migrationBuilder.DropTable(
                name: "tblMessageContact");

            migrationBuilder.DropTable(
                name: "tblNavigation_Home");

            migrationBuilder.DropTable(
                name: "tblPayments");

            migrationBuilder.DropTable(
                name: "tblPromotions");

            migrationBuilder.DropTable(
                name: "tblReservations");

            migrationBuilder.DropTable(
                name: "tblSlide_Home");

            migrationBuilder.DropTable(
                name: "tblStory_Home");

            migrationBuilder.DropTable(
                name: "tblInvoices");

            migrationBuilder.DropTable(
                name: "tblCustomers");

            migrationBuilder.DropTable(
                name: "tblMenuItems");

            migrationBuilder.DropTable(
                name: "tblEmployeeOrder");

            migrationBuilder.DropTable(
                name: "tblCategories");

            migrationBuilder.DropTable(
                name: "tblEmployees");

            migrationBuilder.DropTable(
                name: "tblTable");

            migrationBuilder.DropTable(
                name: "tblBranches");

            migrationBuilder.DropTable(
                name: "tblTableType");
        }
    }
}
