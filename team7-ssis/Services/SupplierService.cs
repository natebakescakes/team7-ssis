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
        ApplicationDbContext context; 

        public SupplierService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Supplier FindSupplierById(string supplierCode)
        {
            //mapped from ShowSupplierDetails
            throw new NotImplementedException();
        }

        public List<Supplier> FindAllSuppliers(string query)
        {
            throw new NotImplementedException();
        }

        public Supplier Save(string supplierCode)
        {
            //mapped from AddNewSupplier, SaveEditDetails, EditPriceList
            throw new NotImplementedException();

        }








    }
}