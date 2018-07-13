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
        public Title Title { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        public ApplicationUser Supervisor { get; set; }
        public Department Department { get; set; }
        public Status Status { get; set; }

        [InverseProperty("CreatedBy")]
        public List<ItemCategory> ItemCategoriesCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<ItemCategory> ItemCategoriesUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Supplier> SuppliersCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Supplier> SuppliersUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<DeliveryOrder> DeliveryOrdersCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<DeliveryOrder> DeliveryOrdersUpdated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<DeliveryOrderDetail> DeliveryOrderDetailsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Item> ItemsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Item> ItemsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<StockAdjustment> StockAdjustmentsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<StockAdjustment> StockAdjustmentsUpdated { get; set; }
        [InverseProperty("ApprovedBySupervisor")]
        public List<StockAdjustment> StockAdjustmentsSupervisorApproved { get; set; }
        [InverseProperty("ApprovedByManager")]
        public List<StockAdjustment> StockAdjustmentsManagerApproved { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<StockAdjustmentDetail> StockAdjustmentDetailsUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<PurchaseOrder> PurchaseOrdersCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<PurchaseOrder> PurchaseOrdersUpdated { get; set; }
        [InverseProperty("ApprovedBy")]
        public List<PurchaseOrder> PurchaseOrdersApproved { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Requisition> RequisitionsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Requisition> RequisitionsUpdated { get; set; }
        [InverseProperty("ApprovedBy")]
        public List<Requisition> RequisitionsApproved { get; set; }
        [InverseProperty("CreatedBy")]
        public List<ItemPrice> ItemPricesCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<ItemPrice> ItemPricesUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Disbursement> DisbursementsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Disbursement> DisbursementsUpdated { get; set; }
        [InverseProperty("CollectedBy")]
        public List<Disbursement> DisbursementsCollected { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<DisbursementDetail> DisbursementDetailsUpdated { get; set; }
        [InverseProperty("Representative")]
        public List<Department> RepresentativeOf { get; set; }
        [InverseProperty("Head")]
        public List<Department> HeadOf { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Department> DepartmentsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Department> DepartmentsUpdated { get; set; }
        [InverseProperty("ClerkInCharge")]
        public List<CollectionPoint> CollectionPointsInCharge { get; set; }
        [InverseProperty("CreatedBy")]
        public List<CollectionPoint> CollectionPointsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<CollectionPoint> CollectionPointsUpdated { get; set; }
        [InverseProperty("Receipient")]
        public List<Delegation> DelegationsReceived { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Delegation> DelegationsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Delegation> DelegationsUpdated { get; set; }
        [InverseProperty("CreatedFor")]
        public List<Notification> NotificationsCreatedFor { get; set; }
        [InverseProperty("CreatedBy")]
        public List<NotificationType> NotificationTypesCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<NotificationType> NotificationTypesUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Status> StatusCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Status> StatusUpdated { get; set; }
        [InverseProperty("CreatedBy")]
        public List<Retrieval> RetrievalsCreated { get; set; }
        [InverseProperty("UpdatedBy")]
        public List<Retrieval> RetrievalsUpdated { get; set; }

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