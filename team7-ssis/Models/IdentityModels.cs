using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace team7_ssis.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Title Title { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        public virtual ApplicationUser Supervisor { get; set; }
        public virtual Department Department { get; set; }
        public virtual Status Status { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual List<ItemCategory> ItemCategoriesCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<ItemCategory> ItemCategoriesUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Supplier> SuppliersCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Supplier> SuppliersUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<DeliveryOrder> DeliveryOrdersCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<DeliveryOrder> DeliveryOrdersUpdated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<DeliveryOrderDetail> DeliveryOrderDetailsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Item> ItemsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Item> ItemsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<StockAdjustment> StockAdjustmentsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<StockAdjustment> StockAdjustmentsUpdated { get; set; }
        [InverseProperty("ApprovedBySupervisor")]
        public virtual List<StockAdjustment> StockAdjustmentsSupervisorApproved { get; set; }
        [InverseProperty("ApprovedByManager")]
        public virtual List<StockAdjustment> StockAdjustmentsManagerApproved { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<StockAdjustmentDetail> StockAdjustmentDetailsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<PurchaseOrder> PurchaseOrdersCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<PurchaseOrder> PurchaseOrdersUpdated { get; set; }
        [InverseProperty("ApprovedBy")]
        public virtual List<PurchaseOrder> PurchaseOrdersApproved { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<PurchaseOrderDetail> PurchaseOrderDetailsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Requisition> RequisitionsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Requisition> RequisitionsUpdated { get; set; }
        [InverseProperty("ApprovedBy")]
        public virtual List<Requisition> RequisitionsApproved { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<RequisitionDetail> RequisitionDetailsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<ItemPrice> ItemPricesCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<ItemPrice> ItemPricesUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Disbursement> DisbursementsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Disbursement> DisbursementsUpdated { get; set; }
        [InverseProperty("CollectedBy")]
        public virtual List<Disbursement> DisbursementsCollected { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<DisbursementDetail> DisbursementDetailsUpdated { get; set; }
        [InverseProperty("Representative")]
        public virtual List<Department> RepresentativeOf { get; set; }
        [InverseProperty("Head")]
        public virtual List<Department> HeadOf { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Department> DepartmentsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Department> DepartmentsUpdated { get; set; }
        [InverseProperty("ClerkInCharge")]
        public virtual List<CollectionPoint> CollectionPointsInCharge { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<CollectionPoint> CollectionPointsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<CollectionPoint> CollectionPointsUpdated { get; set; }
        [InverseProperty("Receipient")]
        public virtual List<Delegation> DelegationsReceived { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Delegation> DelegationsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Delegation> DelegationsUpdated { get; set; }
        [InverseProperty("CreatedFor")]
        public virtual List<Notification> NotificationsCreatedFor { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<NotificationType> NotificationTypesCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<NotificationType> NotificationTypesUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Status> StatusCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Status> StatusUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public virtual List<Retrieval> RetrievalsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public virtual List<Retrieval> RetrievalsUpdated { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CollectionPoint> CollectionPoint { get; set; }
        public DbSet<Delegation> Delegation { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrder { get; set; }
        public DbSet<DeliveryOrderDetail> DeliveryOrderDetail { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Disbursement> Disbursement { get; set; }
        public DbSet<DisbursementDetail> DisbursementDetail { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemCategory> ItemCategory { get; set; }
        public DbSet<ItemPrice> ItemPrice { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public DbSet<Requisition> Requisition { get; set; }
        public DbSet<RequisitionDetail> RequisitionDetail { get; set; }
        public DbSet<Retrieval> Retrieval { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StockAdjustment> StockAdjustment { get; set; }
        public DbSet<StockAdjustmentDetail> StockAdjustmentDetail { get; set; }
        public DbSet<StockMovement> StockMovement { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Title> Title { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        { 
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}