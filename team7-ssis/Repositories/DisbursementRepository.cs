using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class DisbursementRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public Disbursement Save(Disbursement disbursement)
        {
            throw new NotImplementedException();
        }

        public Disbursement FindById(int disbursementId)
        {
            throw new NotImplementedException();
        }

        public List<Disbursement> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Disbursement.Count();
        }

        public void Delete(Disbursement disbursement)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int disbursementId)
        {
            throw new NotImplementedException();
        }
    }
}
