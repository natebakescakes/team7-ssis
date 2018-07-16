using System;
using team7_ssis.Models;
using team7_ssis.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.Services
{
    public class SupplierService
    {
        public Supplier FindSupplierById(ApplicationDbContext context, string supplierCode)
        {
            //mapped from ShowSupplierDetails
            throw new NotImplementedException();
        }

        public List<Supplier> FindSupplierBySearchQuery(ApplicationDbContext context, string query)
        {
            throw new NotImplementedException();
        }

        public Supplier Save(ApplicationDbContext context, string supplierCode)
        {
            //mapped from AddNewSupplier, SaveEditDetails, EditPriceList
            throw new NotImplementedException();

        }








    }
}