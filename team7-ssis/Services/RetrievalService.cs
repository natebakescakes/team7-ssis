using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class RetrievalService
    {
        ApplicationDbContext context;

        public RetrievalService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Retrieval ShowRetrievalDetails(string id)
        {
            throw new NotImplementedException();
        }
        public Retrieval Save(Retrieval retrieval)
        {
            //mapped to confirm retrieval, add and edit retrievals (if any) 
            throw new NotImplementedException();
        }


    }
}