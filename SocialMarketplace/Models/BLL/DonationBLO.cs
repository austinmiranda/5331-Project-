using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Entities;
using SocialMarketplace.Models.Entities.Enum;
using SocialMarketplace.Models.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.BLL
{
    public class DonationBLO
    {
        public RequestStepsViewModel CreateEmptyDonationViewModel()
        {
            return new RequestStepsViewModel
            {
                Step1 = new RequestStep1ViewModel
                {
                    Categories = GetCategories(),
                    RequestInForm = new RequestViewModel
                    {
                        DateDue = DateTime.Today.AddDays(7)
                    }
                },
                Step2 = new RequestStep2ViewModel
                {
                    RequestItemTypes = GetRequestItemTypes(),
                    ItemInForm = new RequestItemViewModel(),
                    Items = new List<RequestItemViewModel>()
                }
            };
        }

        public SelectList GetCategories()
        {
            using (var context = new ApplicationContext())
            {
                var categories = context.Categories.ToList().Select(
                    x => new { Value = x.Id, Text = x.Name });

                return new SelectList(categories, "Value", "Text");
            }
        }

        public SelectList GetRequestItemTypes()
        {
            var types = Enum.GetValues(typeof(RequestItemType)).Cast<RequestItemType>().Select(
                    x => new { Value = (int)x, Text = x.ToString() }).ToList();

            return new SelectList(types, "Value", "Text");
        }

        public void SaveRequest(RequestViewModel request)
        {
            ValidateStep1Request(request);

            int categoryId = request.CategoryId.Value;

            using (var context = new ApplicationContext())
            {
                // TODO: connect with the user

                var category = context.Categories.Where(x => x.Id == categoryId).SingleOrDefault();
                var area = context.Areas.Where(x => x.Id == 1).SingleOrDefault();

                var entity = new Request()
                {
                    Area = area,
                    Status = RequestStatus.ACTIVE,
                    DateCreated = DateTime.Now,
                    DateDue = request.DateDue,
                    Category = category,
                    Description = request.Description,
                    Progress = 0,
                    Subtitle = request.Subtitle,
                    Title = request.Title,
                    UserId = 1,
                    VisualizationCount = 0
                };

                context.Requests.Add(entity);
                context.SaveChanges();

                request.Id = entity.Id;
            }
        }

        internal void AddRequestItem(RequestItemViewModel requestItem, int Id)
        {
            ValidateStep2Request(requestItem);

            using (var context = new ApplicationContext())
            {
                // TODO: connect with the user

                var request = context.Requests.Where(x => x.Id == Id).SingleOrDefault();

                var entity = new RequestItem()
                {
                    Title = requestItem.Title,
                    Detail = requestItem.Detail,
                    Quantity = requestItem.Quantity,
                    Type = requestItem.Type,
                    Request = request
                };

                context.RequestItems.Add(entity);
                context.SaveChanges();

                requestItem.Id = entity.Id;
            }

        }

        public void ValidateStep1Request(RequestViewModel request)
        {
            // TODO: Business rules validations
            //throw new Exception("Invalid parameters");
        }

        public void ValidateStep2Request(RequestItemViewModel requestItem)
        {
            // TODO: Business rules validations
            //throw new Exception("Invalid parameters");
        }
    }
}