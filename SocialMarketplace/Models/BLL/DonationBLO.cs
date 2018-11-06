using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Entities;
using SocialMarketplace.Models.Entities.Enum;
using SocialMarketplace.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.BLL
{
    public class DonationBLO
    {
        public RequestViewModel CreateEmptyDonationViewModel()
        {
            return new RequestViewModel
            {
                Categories = GetCategories(),
                Step1 = new RequestStep1ViewModel
                {
                    DateDue = DateTime.Today.AddDays(7)
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

        public bool SaveStep1Request(RequestViewModel request)
        {
            if (!IsStep1RequestValid(request))
                return false;

            int categoryId = request.Step1.CategoryId ?? 0;

            using (var context = new ApplicationContext())
            {
                var category = context.Categories.Where(x => x.Id == categoryId).SingleOrDefault();
                var area = context.Areas.Where(x => x.Id == 1).SingleOrDefault();

                var entity = new Request()
                {
                    Area = area,
                    Status = RequestStatus.ACTIVE,
                    DateCreated = DateTime.Now,
                    DateDue = request.Step1.DateDue,
                    Category = category,
                    Description = request.Step1.Description,
                    Progress = 0,
                    Subtitle = request.Step1.Subtitle,
                    Title = request.Step1.Title,
                    UserId = 1,
                    VisualizationCount = 0
                };

                context.Requests.Add(entity);
                context.SaveChanges();
            }

            return true;
        }

        public bool IsStep1RequestValid(RequestViewModel request)
        {
            //if(request.Step1 == null)
            //{
            throw new Exception("Invalid parameters");
            //}

            //if(String.IsNullOrEmpty(request.Step1.Title))
            //{
            //    request.Error = new ErrorViewModel(1, "Title must be informed.");
            //    return false;
            //}

            //if (String.IsNullOrEmpty(request.Step1.Subtitle))
            //{
            //    request.Error = new ErrorViewModel(1, "Subtitle must be informed.");
            //    return false;
            //}

            return true;
        }
    }
}