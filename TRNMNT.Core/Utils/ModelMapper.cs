using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.Category;
using TRNMNT.Core.Model.Event;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Utils
{
    public static class ModelMapper
    {
        public static Event UpdateFromModel(this Event @event, EventModelFull eventModel)
        {
            @event.EventDate = eventModel.EventDate;
            @event.AdditionalData = eventModel.AdditionalData;
            @event.Address = eventModel.Address;
            @event.ContactEmail = eventModel.ContactEmail;
            @event.Description = eventModel.Description;
            @event.ImgPath = eventModel.ImgPath;
            @event.TNCFilePath = eventModel.TNCFilePath;
            @event.ContactPhone = eventModel.ContactPhone;
            @event.Title = eventModel.Title;
            @event.FBLink = eventModel.FBLink;
            @event.RegistrationEndTS = eventModel.RegistrationEndTS;
            @event.RegistrationStartTS = eventModel.RegistrationStartTS;
            @event.UrlPrefix = eventModel.UrlPrefix;
            @event.VKLink = eventModel.VKLink;
            @event.EarlyRegistrationEndTS = eventModel.EarlyRegistrationEndTS;
            @event.EarlyRegistrationPrice = eventModel.EarlyRegistrationPrice;
            @event.EarlyRegistrationPriceForMembers = eventModel.EarlyRegistrationPriceForMembers;
            @event.LateRegistrationPrice = eventModel.EarlyRegistrationPriceForMembers;
            @event.LateRegistrationPriceForMembers = eventModel.LateRegistrationPriceForMembers;
            @event.PromoCodeEnabled = eventModel.PromoCodeEnabled;
            @event.PromoCodeListPath = eventModel.PromoCodeListPath;
            //@event.Categories = eventModel.CategoryModels.GetCategoriesFromModels();
            @event.Categories.UpdateCategories(eventModel.CategoryModels);
            return @event;
        }


        public static ICollection<Category> GetCategoriesFromModels(this IEnumerable<CategoryModel> models)
        {
            var categories = new List<Category>();
            foreach (var model in models)
            {
                var category = new Category
                {
                    EventId = model.EventId,
                    CategoryId = string.IsNullOrEmpty(model.CategoryId) ? Guid.NewGuid() : Guid.Parse(model.CategoryId),
                    Name = model.Name
                };
                category.WeightDivisions = GetWeightDeivisionsFromModels(model.WeightDivisionModels, category.CategoryId);
                categories.Add(category);
            }

            return categories;
        }

        private static ICollection<Category> UpdateCategories(this ICollection<Category> categories,
            ICollection<CategoryModel> models)
        {
            var newCategories = models.Where(m => string.IsNullOrEmpty(m.CategoryId)).Select(model => new Category()
            {
                EventId = model.EventId,
                RoundTime = model.RoundTime,
                CategoryId = Guid.NewGuid(),
                Name = model.Name
            });
            foreach (var category in categories)
            {
                var model = models.FirstOrDefault(m => m.CategoryId == category.CategoryId.ToString());
                if (model != null)
                {
                    category.Name = model.Name;
                    category.RoundTime = model.RoundTime;
                }

            }

            foreach (var newCategory in newCategories)
            {
                categories.Add(newCategory);
            }
            return categories;

        }

        private static ICollection<WeightDivision> GetWeightDeivisionsFromModels(this IEnumerable<WeightDivisionModel> models, Guid categoryId)
        {
            return models.Select(model => new WeightDivision
            {
                WeightDivisionId = string.IsNullOrEmpty(model.WeightDivisionId) ? Guid.NewGuid() : Guid.Parse(model.WeightDivisionId),
                Weight = model.Weight,
                Descritpion = model.Descritpion,
                Name = model.Name,
                CategoryId = categoryId
            }).ToList();
        }
    }



}
