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
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.CollectionPointId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id)
                .Index(t => t.Status_StatusId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    UserName = c.String(nullable: false, maxLength: 256),
                    Title_TitleId = c.Int(),
                    FirstName = c.String(maxLength: 30),
                    LastName = c.String(maxLength: 30),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    Department_DepartmentCode = c.String(maxLength: 4),
                    Supervisor_Id = c.String(maxLength: 128),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    Status_StatusId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentCode)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.Supervisor_Id)
                .ForeignKey("dbo.Titles", t => t.Title_TitleId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Department_DepartmentCode)
                .Index(t => t.Status_StatusId)
                .Index(t => t.Supervisor_Id)
                .Index(t => t.Title_TitleId);

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
                    Receipient_Id = c.String(maxLength: 128),
                    StartDate = c.DateTime(nullable: false),
                    EndDate = c.DateTime(nullable: false),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.DelegationId)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Receipient_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Receipient_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.Status",
                c => new
                {
                    StatusId = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 30),
                    Description = c.String(maxLength: 200),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.DeliveryOrderDetails",
                c => new
                {
                    DeliveryOrderNo = c.String(nullable: false, maxLength: 20),
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    PlanQuantity = c.Int(nullable: false),
                    ActualQuantity = c.Int(nullable: false),
                    Remarks = c.String(maxLength: 200),
                    Status_StatusId = c.Int(),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => new { t.DeliveryOrderNo, t.ItemCode })
                .ForeignKey("dbo.DeliveryOrders", t => t.DeliveryOrderNo, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.DeliveryOrderNo)
                .Index(t => t.ItemCode)
                .Index(t => t.Status_StatusId)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.DeliveryOrders",
                c => new
                {
                    DeliveryOrderNo = c.String(nullable: false, maxLength: 20),
                    PurchaseOrder_PurchaseOrderNo = c.String(maxLength: 20),
                    Supplier_SupplierCode = c.String(maxLength: 4),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.DeliveryOrderNo)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrder_PurchaseOrderNo)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.Suppliers", t => t.Supplier_SupplierCode)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.PurchaseOrder_PurchaseOrderNo)
                .Index(t => t.Status_StatusId)
                .Index(t => t.Supplier_SupplierCode)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                {
                    PurchaseOrderNo = c.String(nullable: false, maxLength: 20),
                    SupplierCode = c.String(maxLength: 4),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    ApprovedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                    ApprovedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.PurchaseOrderNo)
                .ForeignKey("dbo.Suppliers", t => t.SupplierCode)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.SupplierCode)
                .Index(t => t.Status_StatusId)
                .Index(t => t.ApprovedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.PurchaseOrderDetails",
                c => new
                {
                    PurchaseOrderNo = c.String(nullable: false, maxLength: 20),
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    Quantity = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.PurchaseOrderNo, t.ItemCode })
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderNo, cascadeDelete: true)
                .Index(t => t.PurchaseOrderNo)
                .Index(t => t.ItemCode);

            CreateTable(
                "dbo.Items",
                c => new
                {
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    Name = c.String(maxLength: 30),
                    Description = c.String(maxLength: 200),
                    Uom = c.String(maxLength: 30),
                    itemCategory_ItemCategoryId = c.Int(),
                    Bin = c.String(maxLength: 8),
                    ReorderLevel = c.Int(nullable: false),
                    ReorderQuantity = c.Int(nullable: false),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.ItemCode)
                .ForeignKey("dbo.ItemCategories", t => t.itemCategory_ItemCategoryId)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.itemCategory_ItemCategoryId)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

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
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.ItemCategoryId)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.ItemPrices",
                c => new
                {
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    SupplierCode = c.String(nullable: false, maxLength: 4),
                    PrioritySequence = c.Int(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => new { t.ItemCode, t.SupplierCode })
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierCode, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.ItemCode)
                .Index(t => t.SupplierCode)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.Suppliers",
                c => new
                {
                    SupplierCode = c.String(nullable: false, maxLength: 4),
                    Name = c.String(maxLength: 30),
                    Address = c.String(maxLength: 200),
                    ContactName = c.String(maxLength: 30),
                    PhoneNumber = c.String(maxLength: 30),
                    FaxNumber = c.String(maxLength: 30),
                    GstRegistrationNo = c.String(maxLength: 30),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.SupplierCode)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.StockMovements",
                c => new
                {
                    StockMovementId = c.Int(nullable: false, identity: true),
                    Item_ItemCode = c.String(maxLength: 4),
                    DeliveryOrderNo = c.String(maxLength: 20),
                    DeliveryOrderDetailItemCode = c.String(maxLength: 4),
                    DisbursementId = c.String(maxLength: 20),
                    DisbursementDetailItemCode = c.String(maxLength: 4),
                    StockAdjustmentId = c.String(maxLength: 20),
                    StockAdjustmentDetailItemCode = c.String(maxLength: 4),
                    OriginalQuantity = c.Int(nullable: false),
                    AfterQuantity = c.Int(nullable: false),
                    CreatedDateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.StockMovementId)
                .ForeignKey("dbo.DisbursementDetails", t => new { t.DisbursementId, t.DisbursementDetailItemCode })
                .ForeignKey("dbo.StockAdjustmentDetails", t => new { t.StockAdjustmentId, t.StockAdjustmentDetailItemCode })
                .ForeignKey("dbo.Items", t => t.Item_ItemCode)
                .ForeignKey("dbo.DeliveryOrderDetails", t => new { t.DeliveryOrderNo, t.DeliveryOrderDetailItemCode })
                .Index(t => new { t.DeliveryOrderNo, t.DeliveryOrderDetailItemCode })
                .Index(t => new { t.DisbursementId, t.DisbursementDetailItemCode })
                .Index(t => new { t.StockAdjustmentId, t.StockAdjustmentDetailItemCode })
                .Index(t => t.Item_ItemCode);

            CreateTable(
                "dbo.DisbursementDetails",
                c => new
                {
                    DisbursementId = c.String(nullable: false, maxLength: 20),
                    ItemCode = c.String(nullable: false, maxLength: 4),
                    Bin = c.String(maxLength: 8),
                    PlanQuantity = c.Int(nullable: false),
                    ActualQuantity = c.Int(nullable: false),
                    Status_StatusId = c.Int(),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => new { t.DisbursementId, t.ItemCode })
                .ForeignKey("dbo.Disbursements", t => t.DisbursementId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemCode, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.DisbursementId)
                .Index(t => t.ItemCode)
                .Index(t => t.Status_StatusId)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.Disbursements",
                c => new
                {
                    DisbursementId = c.String(nullable: false, maxLength: 20),
                    Retrieval_RetrievalId = c.String(maxLength: 20),
                    Department_DepartmentCode = c.String(maxLength: 4),
                    Remarks = c.String(maxLength: 200),
                    Status_StatusId = c.Int(),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: false),
                    CollectedDateTime = c.DateTime(nullable: false),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CollectedBy_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.DisbursementId)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentCode)
                .ForeignKey("dbo.Retrievals", t => t.Retrieval_RetrievalId)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CollectedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.Department_DepartmentCode)
                .Index(t => t.Retrieval_RetrievalId)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CollectedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.Retrievals",
                c => new
                {
                    RetrievalId = c.String(nullable: false, maxLength: 20),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.RetrievalId)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.Requisitions",
                c => new
                {
                    RequisitionId = c.String(nullable: false, maxLength: 20),
                    Retrieval_RetrievalId = c.String(maxLength: 20),
                    Department_DepartmentCode = c.String(maxLength: 4),
                    CollectionPoint_CollectionPointId = c.Int(),
                    EmployeeRemarks = c.String(maxLength: 200),
                    HeadRemarks = c.String(maxLength: 200),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    ApprovedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                    ApprovedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.RequisitionId)
                .ForeignKey("dbo.CollectionPoints", t => t.CollectionPoint_CollectionPointId)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentCode)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.Retrievals", t => t.Retrieval_RetrievalId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.CollectionPoint_CollectionPointId)
                .Index(t => t.Department_DepartmentCode)
                .Index(t => t.Status_StatusId)
                .Index(t => t.Retrieval_RetrievalId)
                .Index(t => t.ApprovedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

            CreateTable(
                "dbo.Departments",
                c => new
                {
                    DepartmentCode = c.String(nullable: false, maxLength: 4),
                    Name = c.String(maxLength: 30),
                    Head_Id = c.String(maxLength: 128),
                    Representative_Id = c.String(maxLength: 128),
                    CollectionPoint_CollectionPointId = c.Int(),
                    ContactName = c.String(maxLength: 30),
                    PhoneNumber = c.String(maxLength: 30),
                    FaxNumber = c.String(maxLength: 30),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.DepartmentCode)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Head_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Representative_Id)
                .ForeignKey("dbo.CollectionPoints", t => t.CollectionPoint_CollectionPointId)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id)
                .Index(t => t.Head_Id)
                .Index(t => t.Representative_Id)
                .Index(t => t.CollectionPoint_CollectionPointId);

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
                    StockAdjustmentId = c.String(nullable: false, maxLength: 20),
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
                    StockAdjustmentId = c.String(nullable: false, maxLength: 20),
                    Remarks = c.String(maxLength: 200),
                    Status_StatusId = c.Int(),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    ApprovedBySupervisor_Id = c.String(maxLength: 128),
                    ApprovedByManager_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                    ApprovedSupervisorDateTime = c.DateTime(nullable: true),
                    ApprovedManagerDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.StockAdjustmentId)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedByManager_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApprovedBySupervisor_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.ApprovedByManager_Id)
                .Index(t => t.ApprovedBySupervisor_Id)
                .Index(t => t.UpdatedBy_Id);

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
                    NotificationType_NotificationTypeId = c.Int(),
                    Contents = c.String(maxLength: 200),
                    Status_StatusId = c.Int(),
                    CreatedFor_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationType_NotificationTypeId)
                .ForeignKey("dbo.Status", t => t.Status_StatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedFor_Id)
                .Index(t => t.NotificationType_NotificationTypeId)
                .Index(t => t.Status_StatusId)
                .Index(t => t.CreatedFor_Id);

            CreateTable(
                "dbo.NotificationTypes",
                c => new
                {
                    NotificationTypeId = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 30),
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.NotificationTypeId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

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
                    CreatedBy_Id = c.String(maxLength: 128),
                    UpdatedBy_Id = c.String(maxLength: 128),
                    CreatedDateTime = c.DateTime(nullable: false),
                    UpdatedDateTime = c.DateTime(nullable: true),
                })
                .PrimaryKey(t => t.TitleId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);

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
            DropForeignKey("dbo.CollectionPoints", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.Departments", "CollectionPoint_CollectionPointId", "dbo.CollectionPoints");
            DropForeignKey("dbo.AspNetUsers", "Title_TitleId", "dbo.Titles");
            DropForeignKey("dbo.Titles", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Titles", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Suppliers", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Suppliers", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Supervisor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "ApprovedBySupervisor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "ApprovedByManager_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockAdjustments", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Status", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Status", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Retrievals", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Retrievals", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requisitions", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requisitions", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requisitions", "ApprovedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "Representative_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrders", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrders", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrders", "ApprovedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationTypes", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationTypes", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "CreatedFor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.Notifications", "NotificationType_NotificationTypeId", "dbo.NotificationTypes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemPrices", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemPrices", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemCategories", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemCategories", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "Head_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Disbursements", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Disbursements", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Disbursements", "CollectedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DisbursementDetails", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Departments", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryOrders", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryOrders", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DeliveryOrderDetails", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StockMovements", new[] { "DeliveryOrderNo", "DeliveryOrderDetailItemCode" }, "dbo.DeliveryOrderDetails");
            DropForeignKey("dbo.DeliveryOrderDetails", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.DeliveryOrderDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.DeliveryOrders", "Supplier_SupplierCode", "dbo.Suppliers");
            DropForeignKey("dbo.DeliveryOrders", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.PurchaseOrders", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.PurchaseOrderDetails", "PurchaseOrderNo", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrderDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.StockMovements", "Item_ItemCode", "dbo.Items");
            DropForeignKey("dbo.StockMovements", new[] { "StockAdjustmentId", "StockAdjustmentDetailItemCode" }, "dbo.StockAdjustmentDetails");
            DropForeignKey("dbo.StockAdjustmentDetails", "StockAdjustmentId", "dbo.StockAdjustments");
            DropForeignKey("dbo.StockAdjustments", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.StockAdjustmentDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.StockMovements", new[] { "DisbursementId", "DisbursementDetailItemCode" }, "dbo.DisbursementDetails");
            DropForeignKey("dbo.DisbursementDetails", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.DisbursementDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.Disbursements", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.Retrievals", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.Requisitions", "Retrieval_RetrievalId", "dbo.Retrievals");
            DropForeignKey("dbo.Requisitions", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.RequisitionDetails", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.RequisitionDetails", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.Requisitions", "Department_DepartmentCode", "dbo.Departments");
            DropForeignKey("dbo.Departments", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.AspNetUsers", "Department_DepartmentCode", "dbo.Departments");
            DropForeignKey("dbo.Requisitions", "CollectionPoint_CollectionPointId", "dbo.CollectionPoints");
            DropForeignKey("dbo.Disbursements", "Retrieval_RetrievalId", "dbo.Retrievals");
            DropForeignKey("dbo.DisbursementDetails", "DisbursementId", "dbo.Disbursements");
            DropForeignKey("dbo.Items", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.ItemPrices", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.Suppliers", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.PurchaseOrders", "SupplierCode", "dbo.Suppliers");
            DropForeignKey("dbo.ItemPrices", "SupplierCode", "dbo.Suppliers");
            DropForeignKey("dbo.ItemPrices", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.ItemCategories", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.Items", "itemCategory_ItemCategoryId", "dbo.ItemCategories");
            DropForeignKey("dbo.Inventory", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.DeliveryOrders", "PurchaseOrder_PurchaseOrderNo", "dbo.PurchaseOrders");
            DropForeignKey("dbo.DeliveryOrderDetails", "DeliveryOrderNo", "dbo.DeliveryOrders");
            DropForeignKey("dbo.Delegations", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delegations", "Receipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delegations", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delegations", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.CollectionPoints", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CollectionPoints", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Titles", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Titles", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.NotificationTypes", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.NotificationTypes", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Notifications", new[] { "CreatedFor_Id" });
            DropIndex("dbo.Notifications", new[] { "Status_StatusId" });
            DropIndex("dbo.Notifications", new[] { "NotificationType_NotificationTypeId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.StockAdjustments", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.StockAdjustments", new[] { "ApprovedBySupervisor_Id" });
            DropIndex("dbo.StockAdjustments", new[] { "ApprovedByManager_Id" });
            DropIndex("dbo.StockAdjustments", new[] { "CreatedBy_Id" });
            DropIndex("dbo.StockAdjustments", new[] { "Status_StatusId" });
            DropIndex("dbo.StockAdjustmentDetails", new[] { "ItemCode" });
            DropIndex("dbo.StockAdjustmentDetails", new[] { "StockAdjustmentId" });
            DropIndex("dbo.RequisitionDetails", new[] { "ItemCode" });
            DropIndex("dbo.RequisitionDetails", new[] { "RequisitionId" });
            DropIndex("dbo.Departments", new[] { "CollectionPoint_CollectionPointId" });
            DropIndex("dbo.Departments", new[] { "Representative_Id" });
            DropIndex("dbo.Departments", new[] { "Head_Id" });
            DropIndex("dbo.Departments", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Departments", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Departments", new[] { "Status_StatusId" });
            DropIndex("dbo.Requisitions", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Requisitions", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Requisitions", new[] { "ApprovedBy_Id" });
            DropIndex("dbo.Requisitions", new[] { "Retrieval_RetrievalId" });
            DropIndex("dbo.Requisitions", new[] { "Status_StatusId" });
            DropIndex("dbo.Requisitions", new[] { "Department_DepartmentCode" });
            DropIndex("dbo.Requisitions", new[] { "CollectionPoint_CollectionPointId" });
            DropIndex("dbo.Retrievals", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Retrievals", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Retrievals", new[] { "Status_StatusId" });
            DropIndex("dbo.Disbursements", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Disbursements", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Disbursements", new[] { "CollectedBy_Id" });
            DropIndex("dbo.Disbursements", new[] { "Status_StatusId" });
            DropIndex("dbo.Disbursements", new[] { "Retrieval_RetrievalId" });
            DropIndex("dbo.DisbursementDetails", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.DisbursementDetails", new[] { "Status_StatusId" });
            DropIndex("dbo.DisbursementDetails", new[] { "ItemCode" });
            DropIndex("dbo.DisbursementDetails", new[] { "DisbursementId" });
            DropIndex("dbo.StockMovements", new[] { "Item_ItemCode" });
            DropIndex("dbo.StockMovements", new[] { "StockAdjustmentId", "StockAdjustmentDetailItemCode" });
            DropIndex("dbo.StockMovements", new[] { "DisbursementId", "DisbursementDetailItemCode" });
            DropIndex("dbo.StockMovements", new[] { "DeliveryOrderNo", "DeliveryOrderDetailItemCode" });
            DropIndex("dbo.Suppliers", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Suppliers", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Suppliers", new[] { "Status_StatusId" });
            DropIndex("dbo.ItemPrices", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.ItemPrices", new[] { "CreatedBy_Id" });
            DropIndex("dbo.ItemPrices", new[] { "Status_StatusId" });
            DropIndex("dbo.ItemPrices", new[] { "SupplierCode" });
            DropIndex("dbo.ItemPrices", new[] { "ItemCode" });
            DropIndex("dbo.ItemCategories", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.ItemCategories", new[] { "CreatedBy_Id" });
            DropIndex("dbo.ItemCategories", new[] { "Status_StatusId" });
            DropIndex("dbo.Inventory", new[] { "ItemCode" });
            DropIndex("dbo.Items", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Items", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Items", new[] { "Status_StatusId" });
            DropIndex("dbo.Items", new[] { "itemCategory_ItemCategoryId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "ItemCode" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "PurchaseOrderNo" });
            DropIndex("dbo.PurchaseOrders", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "CreatedBy_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "ApprovedBy_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "Status_StatusId" });
            DropIndex("dbo.PurchaseOrders", new[] { "SupplierCode" });
            DropIndex("dbo.DeliveryOrders", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.DeliveryOrders", new[] { "CreatedBy_Id" });
            DropIndex("dbo.DeliveryOrders", new[] { "Supplier_SupplierCode" });
            DropIndex("dbo.DeliveryOrders", new[] { "Status_StatusId" });
            DropIndex("dbo.DeliveryOrders", new[] { "PurchaseOrder_PurchaseOrderNo" });
            DropIndex("dbo.DeliveryOrderDetails", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.DeliveryOrderDetails", new[] { "Status_StatusId" });
            DropIndex("dbo.DeliveryOrderDetails", new[] { "ItemCode" });
            DropIndex("dbo.DeliveryOrderDetails", new[] { "DeliveryOrderNo" });
            DropIndex("dbo.Status", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Status", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Delegations", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Delegations", new[] { "Receipient_Id" });
            DropIndex("dbo.Delegations", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Delegations", new[] { "Status_StatusId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Title_TitleId" });
            DropIndex("dbo.AspNetUsers", new[] { "Supervisor_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Status_StatusId" });
            DropIndex("dbo.AspNetUsers", new[] { "Department_DepartmentCode" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CollectionPoints", new[] { "Status_StatusId" });
            DropIndex("dbo.CollectionPoints", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.CollectionPoints", new[] { "CreatedBy_Id" });
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
            DropTable("dbo.Retrievals");
            DropTable("dbo.Disbursements");
            DropTable("dbo.DisbursementDetails");
            DropTable("dbo.StockMovements");
            DropTable("dbo.Suppliers");
            DropTable("dbo.ItemPrices");
            DropTable("dbo.ItemCategories");
            DropTable("dbo.Inventory");
            DropTable("dbo.Items");
            DropTable("dbo.PurchaseOrderDetails");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.DeliveryOrders");
            DropTable("dbo.DeliveryOrderDetails");
            DropTable("dbo.Status");
            DropTable("dbo.Delegations");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CollectionPoints");
        }
    }
}
