namespace team7_ssis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollectionPoints",
                c => new
                {
                    CollectionPointId = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 30),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.CollectionPointId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.StatusId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    UserName = c.String(nullable: false, maxLength: 256),
                    TitleId = c.Int(),
                    FirstName = c.String(maxLength: 30),
                    LastName = c.String(maxLength: 30),
                    SupervisorId = c.String(maxLength: 128),
                    DepartmentCode = c.String(maxLength: 4),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    StatusId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentCode)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.SupervisorId)
                .ForeignKey("dbo.Titles", t => t.TitleId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.DepartmentCode)
                .Index(t => t.StatusId)
                .Index(t => t.SupervisorId)
                .Index(t => t.TitleId);

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Delegations",
                c => new
                    {
                        DelegationId = c.Int(nullable: false, identity: true),
                        ReceipientId = c.String(maxLength: 128),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        StatusId = c.Int(),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedBy = c.String(maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.DelegationId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceipientId)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ReceipientId)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Description = c.String(maxLength: 200),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedBy = c.String(maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.DeliveryOrders",
                c => new
                {
                    DeliveryOrderNo = c.String(nullable: false, maxLength: 9),
                    PurchaseOrderNo = c.String(maxLength: 6),
                    SupplierCode = c.String(maxLength: 128),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.DeliveryOrderNo)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderNo)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierCode)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.PurchaseOrderNo)
                .Index(t => t.StatusId)
                .Index(t => t.SupplierCode)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.DeliveryOrderDetails",
                c => new
                {
                    DeliveryOrderNo = c.String(nullable: false, maxLength: 9),
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    Quantity = c.Int(nullable: false),
                    Remarks = c.String(maxLength: 200),
                })
                .PrimaryKey(t => new { t.DeliveryOrderNo, t.ItemCode })
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.DeliveryOrders", t => t.DeliveryOrderNo, cascadeDelete: true)
                .Index(t => t.DeliveryOrderNo)
                .Index(t => t.ItemCode);

            CreateTable(
                "dbo.Items",
                c => new
                {
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    Name = c.String(maxLength: 30),
                    Description = c.String(maxLength: 200),
                    Uom = c.String(maxLength: 30),
                    ItemCategoryId = c.Int(),
                    Bin = c.String(maxLength: 8),
                    ReorderLevel = c.Int(nullable: false),
                    ReorderQuantity = c.Int(nullable: false),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.ItemCode)
                .ForeignKey("dbo.ItemCategories", t => t.ItemCategoryId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.ItemCategoryId)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.Inventory",
                c => new
                    {
                        ItemCode = c.String(nullable: false, maxLength: 4),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemCode)
                .ForeignKey("dbo.Items", t => t.ItemCode)
                .Index(t => t.ItemCode);

            CreateTable(
                "dbo.ItemCategories",
                c => new
                {
                    ItemCategoryId = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 30),
                    Description = c.String(maxLength: 200),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.ItemCategoryId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.ItemPrices",
                c => new
                {
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    SupplierCode = c.String(nullable: false, maxLength: 128),
                    PrioritySequence = c.Int(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => new { t.ItemCode, t.SupplierCode })
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierCode, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.ItemCode)
                .Index(t => t.SupplierCode)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.Suppliers",
                c => new
                {
                    SupplierCode = c.String(nullable: false, maxLength: 128),
                    Name = c.String(maxLength: 30),
                    Address = c.String(maxLength: 200),
                    ContactName = c.String(maxLength: 30),
                    PhoneNumber = c.String(maxLength: 30),
                    FaxNumber = c.String(maxLength: 30),
                    GstRegistrationNo = c.String(maxLength: 30),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.SupplierCode)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                {
                    PurchaseOrderNo = c.String(nullable: false, maxLength: 6),
                    SupplierCode = c.String(maxLength: 128),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    ApprovedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                    ApprovedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.PurchaseOrderNo)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierCode)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.StatusId)
                .Index(t => t.SupplierCode)
                .Index(t => t.ApprovedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.PurchaseOrderDetails",
                c => new
                {
                    PurchaseOrderNo = c.String(nullable: false, maxLength: 6),
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    Quantity = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.PurchaseOrderNo, t.ItemCode })
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderNo, cascadeDelete: true)
                .Index(t => t.PurchaseOrderNo)
                .Index(t => t.ItemCode);

            CreateTable(
                "dbo.StockMovements",
                c => new
                {
                    StockMovementId = c.Int(nullable: false, identity: true),
                    ItemCode = c.String(maxLength: 4),
                    DeliveryOrderNo = c.String(maxLength: 9),
                    DeliveryOrderDetailItemCode = c.String(maxLength: 4),
                    DisbursementId = c.Int(nullable: false),
                    DisbursementDetailItemCode = c.String(maxLength: 4),
                    StockAdjustmentId = c.Int(nullable: false),
                    StockAdjustmentDetailItemCode = c.String(maxLength: 4),
                    OriginalQuantity = c.Int(nullable: false),
                    AfterQuantity = c.Int(nullable: false),
                    CreatedDateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.StockMovementId)
                .ForeignKey("dbo.DisbursementDetails", t => new { t.DisbursementId, t.DisbursementDetailItemCode })
                .ForeignKey("dbo.StockAdjustmentDetails", t => new { t.StockAdjustmentId, t.StockAdjustmentDetailItemCode })
                .ForeignKey("dbo.Items", t => t.ItemCode)
                .ForeignKey("dbo.DeliveryOrderDetails", t => new { t.DeliveryOrderNo, t.DeliveryOrderDetailItemCode })
                .Index(t => new { t.DeliveryOrderNo, t.DeliveryOrderDetailItemCode })
                .Index(t => new { t.DisbursementId, t.DisbursementDetailItemCode })
                .Index(t => new { t.StockAdjustmentId, t.StockAdjustmentDetailItemCode })
                .Index(t => t.ItemCode);

            CreateTable(
                "dbo.DisbursementDetails",
                c => new
                {
                    DisbursementId = c.Int(nullable: false),
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    Bin = c.String(maxLength: 8),
                    Quantity = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.DisbursementId, t.ItemCode })
                .ForeignKey("dbo.Disbursements", t => t.DisbursementId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .Index(t => t.DisbursementId)
                .Index(t => t.ItemCode);

            CreateTable(
                "dbo.Disbursements",
                c => new
                {
                    DisbursementId = c.Int(nullable: false, identity: true),
                    RequisitionId = c.String(nullable: false, maxLength: 20),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.DisbursementId)
                .ForeignKey("dbo.Requisitions", t => t.RequisitionId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.RequisitionId)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.Requisitions",
                c => new
                {
                    RequisitionId = c.String(nullable: false, maxLength: 20),
                    DepartmentCode = c.String(maxLength: 4),
                    CollectionPointId = c.Int(),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    ApprovedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                    ApprovedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.RequisitionId)
                .ForeignKey("dbo.CollectionPoints", t => t.CollectionPointId)
                .ForeignKey("dbo.Departments", t => t.DepartmentCode)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CollectionPointId)
                .Index(t => t.DepartmentCode)
                .Index(t => t.StatusId)
                .Index(t => t.ApprovedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.Departments",
                c => new
                {
                    DepartmentCode = c.String(nullable: false, maxLength: 4),
                    Name = c.String(maxLength: 30),
                    HeadId = c.String(maxLength: 128),
                    RepresentativeId = c.String(maxLength: 128),
                    CollectionPointId = c.Int(),
                    ContactName = c.String(maxLength: 30),
                    PhoneNumber = c.String(maxLength: 30),
                    FaxNumber = c.String(maxLength: 30),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.DepartmentCode)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.HeadId)
                .ForeignKey("dbo.AspNetUsers", t => t.RepresentativeId)
                .ForeignKey("dbo.CollectionPoints", t => t.CollectionPointId)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.HeadId)
                .Index(t => t.RepresentativeId)
                .Index(t => t.CollectionPointId);

            CreateTable(
                "dbo.RequisitionDetails",
                c => new
                    {
                        RequisitionId = c.String(nullable: false, maxLength: 20),
                        ItemCode = c.String(nullable: false, maxLength: 4),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequisitionId, t.ItemCode })
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.Requisitions", t => t.RequisitionId, cascadeDelete: true)
                .Index(t => t.RequisitionId)
                .Index(t => t.ItemCode);
            
            CreateTable(
                "dbo.StockAdjustmentDetails",
                c => new
                    {
                        StockAdjustmentId = c.Int(nullable: false),
                        ItemCode = c.String(nullable: false, maxLength: 4),
                        OriginalQuantity = c.Int(nullable: false),
                        AfterQuantity = c.Int(nullable: false),
                        Reason = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => new { t.StockAdjustmentId, t.ItemCode })
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.StockAdjustments", t => t.StockAdjustmentId, cascadeDelete: true)
                .Index(t => t.StockAdjustmentId)
                .Index(t => t.ItemCode);

            CreateTable(
                "dbo.StockAdjustments",
                c => new
                {
                    StockAdjustmentId = c.Int(nullable: false, identity: true),
                    Remarks = c.String(maxLength: 200),
                    StatusId = c.Int(),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    ApprovedBySupervisor = c.String(maxLength: 128),
                    ApprovedByManager = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                    ApprovedSupervisorDateTime = c.DateTime(nullable: true),
                    ApprovedManagerDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.StockAdjustmentId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedByManager)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedBySupervisor)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ApprovedByManager)
                .Index(t => t.ApprovedBySupervisor)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        NotificationTypeId = c.Int(),
                        Contents = c.String(maxLength: 200),
                        StatusId = c.Int(),
                        CreatedFor = c.String(maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationTypeId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedFor)
                .Index(t => t.NotificationTypeId)
                .Index(t => t.StatusId)
                .Index(t => t.CreatedFor);
            
            CreateTable(
                "dbo.NotificationTypes",
                c => new
                    {
                        NotificationTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedBy = c.String(maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(nullable: true),
                    })
                .PrimaryKey(t => t.NotificationTypeId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.Titles",
                c => new
                {
                    TitleId = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 5),
                    CreatedBy = c.String(maxLength: 128),
                    UpdatedBy = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.TitleId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CollectionPoints", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Departments", "CollectionPointId", "dbo.CollectionPoints");
            DropForeignKey("dbo.AspNetUsers", "TitleId", "dbo.Titles");
            DropForeignKey("dbo.Titles", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Titles", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Suppliers", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Suppliers", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "SupervisorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "ApprovedBySupervisor", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "ApprovedByManager", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Status", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Status", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "StatusId", "dbo.Status");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requisitions", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requisitions", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requisitions", "ApprovedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "RepresentativeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrders", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrders", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrders", "ApprovedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationTypes", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationTypes", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "CreatedFor", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Notifications", "NotificationTypeId", "dbo.NotificationTypes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemPrices", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemPrices", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemCategories", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemCategories", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "HeadId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Disbursements", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Disbursements", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryOrders", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryOrders", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryOrders", "SupplierCode", "dbo.Suppliers");
            DropForeignKey("dbo.DeliveryOrders", "StatusId", "dbo.Status");
            DropForeignKey("dbo.DeliveryOrders", "PurchaseOrderNo", "dbo.PurchaseOrders");
            DropForeignKey("dbo.DeliveryOrderDetails", "DeliveryOrderNo", "dbo.DeliveryOrders");
            DropForeignKey("dbo.StockMovements", new[] { "DeliveryOrderNo", "DeliveryOrderDetailItemCode" }, "dbo.DeliveryOrderDetails");
            DropForeignKey("dbo.DeliveryOrderDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.StockMovements", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.StockMovements", new[] { "StockAdjustmentId", "StockAdjustmentDetailItemCode" }, "dbo.StockAdjustmentDetails");
            DropForeignKey("dbo.StockAdjustmentDetails", "StockAdjustmentId", "dbo.StockAdjustments");
            DropForeignKey("dbo.StockAdjustments", "StatusId", "dbo.Status");
            DropForeignKey("dbo.StockAdjustmentDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.StockMovements", new[] { "DisbursementId", "DisbursementDetailItemCode" }, "dbo.DisbursementDetails");
            DropForeignKey("dbo.DisbursementDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.Disbursements", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Requisitions", "StatusId", "dbo.Status");
            DropForeignKey("dbo.RequisitionDetails", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.RequisitionDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.Disbursements", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.Requisitions", "DepartmentCode", "dbo.Departments");
            DropForeignKey("dbo.Departments", "StatusId", "dbo.Status");
            DropForeignKey("dbo.AspNetUsers", "DepartmentCode", "dbo.Departments");
            DropForeignKey("dbo.Requisitions", "CollectionPointId", "dbo.CollectionPoints");
            DropForeignKey("dbo.DisbursementDetails", "DisbursementId", "dbo.Disbursements");
            DropForeignKey("dbo.Items", "StatusId", "dbo.Status");
            DropForeignKey("dbo.ItemPrices", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.Suppliers", "StatusId", "dbo.Status");
            DropForeignKey("dbo.PurchaseOrders", "SupplierCode", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrders", "StatusId", "dbo.Status");
            DropForeignKey("dbo.PurchaseOrderDetails", "PurchaseOrderNo", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrderDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.ItemPrices", "SupplierCode", "dbo.Suppliers");
            DropForeignKey("dbo.ItemPrices", "StatusId", "dbo.Status");
            DropForeignKey("dbo.ItemCategories", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Items", "ItemCategoryId", "dbo.ItemCategories");
            DropForeignKey("dbo.Inventory", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.Delegations", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delegations", "ReceipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delegations", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delegations", "StatusId", "dbo.Status");
            DropForeignKey("dbo.CollectionPoints", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.CollectionPoints", "CreatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Titles", new[] { "UpdatedBy" });
            DropIndex("dbo.Titles", new[] { "CreatedBy" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.NotificationTypes", new[] { "UpdatedBy" });
            DropIndex("dbo.NotificationTypes", new[] { "CreatedBy" });
            DropIndex("dbo.Notifications", new[] { "CreatedFor" });
            DropIndex("dbo.Notifications", new[] { "StatusId" });
            DropIndex("dbo.Notifications", new[] { "NotificationTypeId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.StockAdjustments", new[] { "UpdatedBy" });
            DropIndex("dbo.StockAdjustments", new[] { "ApprovedBySupervisor" });
            DropIndex("dbo.StockAdjustments", new[] { "ApprovedByManager" });
            DropIndex("dbo.StockAdjustments", new[] { "CreatedBy" });
            DropIndex("dbo.StockAdjustments", new[] { "StatusId" });
            DropIndex("dbo.StockAdjustmentDetails", new[] { "ItemCode" });
            DropIndex("dbo.StockAdjustmentDetails", new[] { "StockAdjustmentId" });
            DropIndex("dbo.RequisitionDetails", new[] { "ItemCode" });
            DropIndex("dbo.RequisitionDetails", new[] { "RequisitionId" });
            DropIndex("dbo.Departments", new[] { "CollectionPointId" });
            DropIndex("dbo.Departments", new[] { "RepresentativeId" });
            DropIndex("dbo.Departments", new[] { "HeadId" });
            DropIndex("dbo.Departments", new[] { "UpdatedBy" });
            DropIndex("dbo.Departments", new[] { "CreatedBy" });
            DropIndex("dbo.Departments", new[] { "StatusId" });
            DropIndex("dbo.Requisitions", new[] { "UpdatedBy" });
            DropIndex("dbo.Requisitions", new[] { "CreatedBy" });
            DropIndex("dbo.Requisitions", new[] { "ApprovedBy" });
            DropIndex("dbo.Requisitions", new[] { "StatusId" });
            DropIndex("dbo.Requisitions", new[] { "DepartmentCode" });
            DropIndex("dbo.Requisitions", new[] { "CollectionPointId" });
            DropIndex("dbo.Disbursements", new[] { "UpdatedBy" });
            DropIndex("dbo.Disbursements", new[] { "CreatedByd" });
            DropIndex("dbo.Disbursements", new[] { "StatusId" });
            DropIndex("dbo.Disbursements", new[] { "RequisitionId" });
            DropIndex("dbo.DisbursementDetails", new[] { "ItemCode" });
            DropIndex("dbo.DisbursementDetails", new[] { "DisbursementId" });
            DropIndex("dbo.StockMovements", new[] { "ItemCode" });
            DropIndex("dbo.StockMovements", new[] { "StockAdjustmentId", "StockAdjustmentDetailItemCode" });
            DropIndex("dbo.StockMovements", new[] { "DisbursementId", "DisbursementDetailItemCode" });
            DropIndex("dbo.StockMovements", new[] { "DeliveryOrderNo", "DeliveryOrderDetailItemCode" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "ItemCode" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "PurchaseOrderNo" });
            DropIndex("dbo.PurchaseOrders", new[] { "UpdatedBy" });
            DropIndex("dbo.PurchaseOrders", new[] { "CreatedBy" });
            DropIndex("dbo.PurchaseOrders", new[] { "ApprovedBy" });
            DropIndex("dbo.PurchaseOrders", new[] { "SupplierCode" });
            DropIndex("dbo.PurchaseOrders", new[] { "StatusId" });
            DropIndex("dbo.Suppliers", new[] { "UpdatedBy" });
            DropIndex("dbo.Suppliers", new[] { "CreatedBy" });
            DropIndex("dbo.Suppliers", new[] { "StatusId" });
            DropIndex("dbo.ItemPrices", new[] { "UpdatedBy" });
            DropIndex("dbo.ItemPrices", new[] { "CreatedBy" });
            DropIndex("dbo.ItemPrices", new[] { "StatusId" });
            DropIndex("dbo.ItemPrices", new[] { "SupplierCode" });
            DropIndex("dbo.ItemPrices", new[] { "ItemCode" });
            DropIndex("dbo.ItemCategories", new[] { "UpdatedBy" });
            DropIndex("dbo.ItemCategories", new[] { "CreatedBy" });
            DropIndex("dbo.ItemCategories", new[] { "StatusId" });
            DropIndex("dbo.Inventory", new[] { "ItemCode" });
            DropIndex("dbo.Items", new[] { "UpdatedBy" });
            DropIndex("dbo.Items", new[] { "CreatedBy" });
            DropIndex("dbo.Items", new[] { "StatusId" });
            DropIndex("dbo.Items", new[] { "ItemCategoryId" });
            DropIndex("dbo.DeliveryOrderDetails", new[] { "ItemCode" });
            DropIndex("dbo.DeliveryOrderDetails", new[] { "DeliveryOrderNo" });
            DropIndex("dbo.DeliveryOrders", new[] { "UpdatedBy" });
            DropIndex("dbo.DeliveryOrders", new[] { "CreatedBy" });
            DropIndex("dbo.DeliveryOrders", new[] { "SupplierCode" });
            DropIndex("dbo.DeliveryOrders", new[] { "StatusId" });
            DropIndex("dbo.DeliveryOrders", new[] { "PurchaseOrderNo" });
            DropIndex("dbo.Status", new[] { "UpdatedBy" });
            DropIndex("dbo.Status", new[] { "CreatedBy" });
            DropIndex("dbo.Delegations", new[] { "UpdatedBy" });
            DropIndex("dbo.Delegations", new[] { "ReceipientId" });
            DropIndex("dbo.Delegations", new[] { "CreatedBy" });
            DropIndex("dbo.Delegations", new[] { "StatusId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "TitleId" });
            DropIndex("dbo.AspNetUsers", new[] { "SupervisorId" });
            DropIndex("dbo.AspNetUsers", new[] { "StatusId" });
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentCode" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CollectionPoints", new[] { "StatusId" });
            DropIndex("dbo.CollectionPoints", new[] { "UpdatedBy" });
            DropIndex("dbo.CollectionPoints", new[] { "CreatedBy" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Titles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.NotificationTypes");
            DropTable("dbo.Notifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.StockAdjustments");
            DropTable("dbo.StockAdjustmentDetails");
            DropTable("dbo.RequisitionDetails");
            DropTable("dbo.Departments");
            DropTable("dbo.Requisitions");
            DropTable("dbo.Disbursements");
            DropTable("dbo.DisbursementDetails");
            DropTable("dbo.StockMovements");
            DropTable("dbo.PurchaseOrderDetails");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.Suppliers");
            DropTable("dbo.ItemPrices");
            DropTable("dbo.ItemCategories");
            DropTable("dbo.Inventory");
            DropTable("dbo.Items");
            DropTable("dbo.DeliveryOrderDetails");
            DropTable("dbo.DeliveryOrders");
            DropTable("dbo.Status");
            DropTable("dbo.Delegations");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CollectionPoints");
        }
    }
}
