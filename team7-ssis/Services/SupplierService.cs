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
        public Supplier FindSupplierById(ApplicationDbContext context, string suppliercode)
        {
            //mapped from ShowSupplierDetails
            throw new NotImplementedException();
        }

        public List<Supplier> FindSupplierBySearchQuery(ApplicationDbContext context, string query)
        {
            throw new NotImplementedException();
        }

        public bool Save(ApplicationDbContext context, string suppliercode)
        {
            //mapped from AddNewSupplier, SaveEditDetails, EditPriceList
            throw new NotImplementedException();

        }








    }
}