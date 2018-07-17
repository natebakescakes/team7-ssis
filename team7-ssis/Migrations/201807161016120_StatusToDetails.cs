namespace team7_ssis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatusToDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrderDetails", "UpdatedDateTime", c => c.DateTime());
            AddColumn("dbo.PurchaseOrderDetails", "Status_StatusId", c => c.Int());
            AddColumn("dbo.PurchaseOrderDetails", "UpdatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.RequisitionDetails", "UpdatedDateTime", c => c.DateTime());
            AddColumn("dbo.RequisitionDetails", "Status_StatusId", c => c.Int());
            AddColumn("dbo.RequisitionDetails", "UpdatedBy_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.PurchaseOrderDetails", "Status_StatusId");
            CreateIndex("dbo.PurchaseOrderDetails", "UpdatedBy_Id");
            CreateIndex("dbo.RequisitionDetails", "Status_StatusId");
            CreateIndex("dbo.RequisitionDetails", "UpdatedBy_Id");
            AddForeignKey("dbo.RequisitionDetails", "Status_StatusId", "dbo.Status", "StatusId");
            AddForeignKey("dbo.PurchaseOrderDetails", "Status_StatusId", "dbo.Status", "StatusId");
            AddForeignKey("dbo.PurchaseOrderDetails", "UpdatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.RequisitionDetails", "UpdatedBy_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequisitionDetails", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrderDetails", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PurchaseOrderDetails", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.RequisitionDetails", "Status_StatusId", "dbo.Status");
            DropIndex("dbo.RequisitionDetails", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.RequisitionDetails", new[] { "Status_StatusId" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.PurchaseOrderDetails", new[] { "Status_StatusId" });
            DropColumn("dbo.RequisitionDetails", "UpdatedBy_Id");
            DropColumn("dbo.RequisitionDetails", "Status_StatusId");
            DropColumn("dbo.RequisitionDetails", "UpdatedDateTime");
            DropColumn("dbo.PurchaseOrderDetails", "UpdatedBy_Id");
            DropColumn("dbo.PurchaseOrderDetails", "Status_StatusId");
            DropColumn("dbo.PurchaseOrderDetails", "UpdatedDateTime");
        }
    }
}
