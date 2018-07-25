using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class DelegationService
    {
        ApplicationDbContext context;
        DelegationRepository delegationRepository;
        public DelegationService(ApplicationDbContext context)
        {
            this.context = context;
            this.delegationRepository = new DelegationRepository(context);
        }
        public Delegation DelegateManager(Delegation delegation)
        {
            return delegationRepository.Save(delegation);
        }
    }
}