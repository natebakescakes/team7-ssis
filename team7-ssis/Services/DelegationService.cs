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
        UserRepository userRepository;
        public DelegationService(ApplicationDbContext context)
        {
            this.context = context;
            this.delegationRepository = new DelegationRepository(context);
            this.userRepository = new UserRepository(context);
        }
        public Delegation DelegateManager(Delegation delegation)
        {
            return delegationRepository.Save(delegation);
        }
        public List<Delegation> FindAllDelegations()
        {
            return delegationRepository.FindAll().ToList();
        }
        public List<Delegation> FindDelegationsByDepartment(ApplicationUser user)
        {
            return delegationRepository.FindByDepartment(user).ToList();
        }
        public Delegation FindDelegationByDelegationId(int delegationId)
        {
            return delegationRepository.FindById(delegationId);
        }
    }
}