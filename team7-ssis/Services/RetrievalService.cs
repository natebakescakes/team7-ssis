﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class RetrievalService
    {
        ApplicationDbContext context;
        RetrievalRepository retrievalRepository;
        ItemService itemService;
        StockMovementService stockmovementService;

        public RetrievalService(ApplicationDbContext context)
        {
            this.context = context;
            retrievalRepository = new RetrievalRepository(context);
            itemService = new ItemService(context);
            stockmovementService = new StockMovementService(context);

        }

        public List<Retrieval> FindAllRetrievals()
        {
            return retrievalRepository.FindAll().ToList();
        }


        public Retrieval FindRetrievalById(string id)
        {
            return retrievalRepository.FindById(id);
        }
        public Retrieval Save(Retrieval retrieval)
        {
            //mapped to confirm retrieval, add and edit retrievals (if any) 
            return retrievalRepository.Save(retrieval);

        }

        public Retrieval RetrieveItems(Retrieval other)
        {
            // Get RetrieveByRetrievalId
            Retrieval retrieval = this.FindRetrievalById(other.RetrievalId);

            // Update Actual Quantity
            retrieval.Disbursements = other.Disbursements;

            // Save Retrieval
            this.Save(retrieval);

            // Update Item Quantity based on amount retrieved into Inventory
            foreach (Disbursement d in retrieval.Disbursements)
            {
                foreach (DisbursementDetail detail in d.DisbursementDetails)
                {
                   
                    //Create Stock Movement Transaction
                    stockmovementService.CreateStockMovement(detail);
                }
            }



            return retrieval;

        }



    }
}