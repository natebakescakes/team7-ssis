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
        SupplierRepository supplierRepository;
        ApplicationDbContext context; 

        public SupplierService(ApplicationDbContext context)
        {
            this.context = context;
            this.supplierRepository = new SupplierRepository(context);

        }

        public Supplier FindSupplierById(string supplierCode)
        {
            //mapped from ShowSupplierDetails
            return supplierRepository.FindById(supplierCode);
        }

        public List<Supplier> FindAllSuppliers()
        {
            return supplierRepository.FindAll().ToList();
        }

        public Supplier Save(Supplier supplier)
        {
            //mapped from AddNewSupplier SaveEditDetails, EditPriceList
             return supplierRepository.Save(supplier);    

        }

        

   
   








    }
}