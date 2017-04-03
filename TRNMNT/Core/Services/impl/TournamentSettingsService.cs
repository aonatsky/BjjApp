using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class TournamentSettingsService : ITournamentSettingsService
    {

        private IRepository<Category> categoryRepository;
        private IRepository<WeightDivision> weightDivisionRepository;
        private ILogger<TournamentSettingsService> logger;
        public TournamentSettingsService(IRepository<Category> categoryRepository, IRepository<WeightDivision> weightDivisionRepository, ILogger<TournamentSettingsService> logger)
        {
            this.categoryRepository = categoryRepository;
            this.weightDivisionRepository = weightDivisionRepository;
            this.logger = logger;
        }

        public IQueryable<Category> GetCatogiries()
        {
            return categoryRepository.GetAll();
        }

        public void AddCatogiries(IEnumerable<Category> categories)
        {
            categoryRepository.AddRange(categories);
        }

        public void AddCatogiries(Category category)
        {
            categoryRepository.Add(category);
        }

        public void DeleteCatogory(Category category)
        {
            categoryRepository.Delete(category);
        }

        public void UpdateCatogory(Category category)
        {
            categoryRepository.Update(category);
        }


        public void AddWeightDivisions(IEnumerable<WeightDivision> weightDivisions)
        {
            weightDivisionRepository.AddRange(weightDivisions);
        }

        public void AddWeightDivisions(WeightDivision weightDivision)
        {
            weightDivisionRepository.Add(weightDivision);
        }


        public void DeleteWeightDivision(WeightDivision weightDivision)
        {
            weightDivisionRepository.Delete(weightDivision);
        }



        public IQueryable<WeightDivision> GetWeightDivisions()
        {
            return weightDivisionRepository.GetAll();
        }


        public void UpdateWeightDivisions(WeightDivision weightDivision)
        {
            weightDivisionRepository.Update(weightDivision);
        }
    }
}
