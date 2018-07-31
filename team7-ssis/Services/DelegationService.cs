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
        UserService userService;
        public DelegationService(ApplicationDbContext context)
        {
            this.context = context;
            this.delegationRepository = new DelegationRepository(context);
            this.userService = new UserService(context);
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

        public void DelegateManagerRole(string recipientEmail, string headEmail, string startDate, string endDate)
        {
            // Date Format: Sat Jul 28 00:00:00 GMT + 08:00 2018
            var startDateActual = new DateTime(
                Int32.Parse(startDate.Substring(startDate.Length - 4)),
                ConvertMonth(startDate.Substring(4, 3)),
                Int32.Parse(startDate.Substring(8, 2))
            );

            var endDateActual = new DateTime(
                Int32.Parse(endDate.Substring(endDate.Length - 4)),
                ConvertMonth(endDate.Substring(4, 3)),
                Int32.Parse(endDate.Substring(8, 2))
            );

            if (startDateActual > endDateActual)
                throw new ArgumentException("Invalid dates");

            if (userService.FindUserByEmail(recipientEmail).Department.Name != userService.FindUserByEmail(headEmail).Department.Name)
                throw new ArgumentException("Representative and Department Head not from same Department");

            if (headEmail != userService.FindUserByEmail(headEmail).Department.Head.Email)
                throw new ArgumentException("Only Department Heads can delegate roles");

            var delegation = new Delegation()
            {
                DelegationId = IdService.GetNewDelegationId(context),
                Receipient = userService.FindUserByEmail(recipientEmail),
                StartDate = startDateActual,
                EndDate = endDateActual,
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedBy = userService.FindUserByEmail(headEmail),
                CreatedDateTime = DateTime.Now,
            };

            delegationRepository.Save(delegation);

            new UserService(context).AddDepartmentHeadRole(recipientEmail);
        }

        private static int ConvertMonth(string s)
        {
            int month;
            switch (s)
            {
                case "Jan":
                    month = 1;
                    break;
                case "Feb":
                    month = 2;
                    break;
                case "Mar":
                    month = 3;
                    break;
                case "Apr":
                    month = 4;
                    break;
                case "May":
                    month = 5;
                    break;
                case "Jun":
                    month = 6;
                    break;
                case "Jul":
                    month = 7;
                    break;
                case "Aug":
                    month = 8;
                    break;
                case "Sep":
                    month = 9;
                    break;
                case "Oct":
                    month = 10;
                    break;
                case "Nov":
                    month = 11;
                    break;
                case "Dec":
                    month = 12;
                    break;
                default:
                    month = 0;
                    break;
            }

            return month;
        }

        public void CancelDelegation(int delegationId, string headEmail)
        {
            if (headEmail != userService.FindUserByEmail(headEmail).Department.Head.Email)
                throw new ArgumentException("Only Department Heads can cancel delegations");

            if (!delegationRepository.ExistsById(delegationId))
                throw new ArgumentException("Delegation does not exist");

            var delegation = FindDelegationByDelegationId(delegationId);
            userService.RemoveDepartmentHeadRole(delegation.Receipient.Email);
            delegation.Status = new StatusService(context).FindStatusByStatusId(3);
            delegation.UpdatedBy = userService.FindUserByEmail(headEmail);
            delegation.UpdatedDateTime = DateTime.Now;
            delegationRepository.Save(delegation);
        }
    }
}