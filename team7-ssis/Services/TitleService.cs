using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class TitleService
    {
        private TitleRepository titleRepository;

        public TitleService(ApplicationDbContext context)
        {
            this.titleRepository = new TitleRepository(context);
        }

        public List<Title> FindAllTitles()
        {
            return titleRepository.FindAll().ToList();
        }

        public Title FindTitleByTitleId(int titleId)
        {
            return titleRepository.FindById(titleId);
        }
    }
}