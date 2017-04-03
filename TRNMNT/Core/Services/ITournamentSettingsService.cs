using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface ITournamentSettingsService
    {
      IQueryable<Category>GetCatogiries();
      void AddCatogiries(IEnumerable<Category> categories);
      void AddCatogiries(Category category);
      void UpdateCatogory(Category category);
      void DeleteCatogory(Category category);
      IQueryable<WeightDivision>GetWeightDivisions();
      void AddWeightDivisions(IEnumerable<WeightDivision> weightDivision);
      void AddWeightDivisions(WeightDivision weightDivision);
      void UpdateWeightDivisions(WeightDivision weightDivision);
      void DeleteWeightDivision(WeightDivision weightDivision);
    }
}
