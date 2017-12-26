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
            return @event;
        }
        
        
        public static ICollection<Category> GetCategoriesFromModels(this IEnumerable<CategoryModel> models)
        {
            return models.Select(model => new Category
            {
                EventId = model.EventId,
                CategoryId = string.IsNullOrEmpty(model.CategoryId) ? Guid.NewGuid() : Guid.Parse(model.CategoryId),
                Name = model.Name,
                WeightDivisions = GetWeightDeivisionsFromModels(model.WeightDivisionModels)
            }).ToList();
        }

        private static ICollection<WeightDivision> GetWeightDeivisionsFromModels(this IEnumerable<WeightDivisionModel> models)
        {
            return models.Select(model => new WeightDivision
            {
                WeightDivisionId = Guid.Parse(model.WeightDivisionId),
                Weight = model.Weight,
                Descritpion = model.Descritpion,
                Name = model.Name,
                CategoryId = Guid.Parse(model.CategoryId)
            }).ToList();
        }
    }


   
}
