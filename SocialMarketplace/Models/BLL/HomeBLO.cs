using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Entities.Enum;
using SocialMarketplace.Models.ViewModels.Home;
using SocialMarketplace.Models.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.BLL
{
    public class HomeBLO
    {
        public HomeViewModel CreateHomeViewModel(int? categoryId) {

            var homeViewModel = new HomeViewModel
            {
                Statistics = GetStatistics(),
                CategoryId = categoryId,
                Categories = GetCategories(categoryId),
                MainRequest = GetMainRequest(categoryId),
                CategoryRequests = GetCategoryRequestList(categoryId),
                UrgentRequests = GetUrgentRequestList()
            };

            return homeViewModel;
        }

        public StatisticsViewModel GetStatistics()
        {
            using (var context = new ApplicationContext())
            {
                return new StatisticsViewModel
                {
                    TotalDonations = context.Responses
                        .Where(x => x.Status != ResponseStatus.CANCELLED)
                        .Count(),

                    ActiveDonations = context.Requests
                        .Where(x => x.Status == RequestStatus.ACTIVE)
                        .Count(),

                    TotalDonors = context.Responses
                        .Where(x => x.Status != ResponseStatus.CANCELLED)
                        .Select(x => x.UserId)
                        .Distinct()
                        .Count()
                };
            }
        }

        internal SearchFormViewModel CreateEmptySearchViewModel()
        {
            return new SearchFormViewModel
            {
                Categories = GetCategories(),
                Page = 0,
                PageSize = 5                
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

        public IList<CategoryViewModel> GetCategories(int? categoryId)
        {
            using (var context = new ApplicationContext())
            {
                var categories = context.Categories
                    .Select(x => new CategoryViewModel {
                        Id = x.Id,
                        Name = x.Name,
                        Selected = x.Id == categoryId
                    }).ToList();

                return categories;
            }
        }

        public RequestViewModel GetMainRequest(int? Id)
        {
            using (var context = new ApplicationContext())
            {
                var query = context.Requests
                    .Where(x => x.Status == Entities.Enum.RequestStatus.ACTIVE);

                if (Id.HasValue && Id > 0)
                    query = query
                        .Where(x => x.Category.Id == Id);

                RequestViewModel mainRequest = query
                    .OrderByDescending(x => x.VisualizationCount)
                    .Take(1)
                    .Select(x => new RequestViewModel
                    {
                        Id = x.Id,
                        Title = x.Title, 
                        Subtitle = x.Subtitle,
                        Photo = Utils.SessionFacade.RootUrl + "/Donation/Photo/" + x.Id,
                        Progress = x.Progress,
                        CategoryId = x.Category.Id
                    }).SingleOrDefault();

                return mainRequest;
            }
        }
        public IList<RequestViewModel> GetCategoryRequestList(int? Id)
        {
            using (var context = new ApplicationContext())
            {
                var query = context.Requests
                    .Where(x => x.Status == Entities.Enum.RequestStatus.ACTIVE);

                if (Id.HasValue && Id > 0)
                    query = query
                        .Where(x => x.Category.Id == Id);

                List<RequestViewModel> topThreeRequests = query
                    .OrderByDescending(x => x.VisualizationCount)
                    .Skip(1)
                    .Take(3)
                    .Select(x => new RequestViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Subtitle = x.Subtitle,
                        Photo = Utils.SessionFacade.RootUrl + "/Donation/Photo/" + x.Id,
                        Progress = x.Progress,
                        CategoryId = x.Category.Id
                    }).ToList();

                return topThreeRequests;
            }
        }

        public IList<RequestViewModel> GetUrgentRequestList()
        {
            using (var context = new ApplicationContext())
            {
                List<RequestViewModel> urgentThreeRequests = context.Requests
                    .Where(x => x.Status == Entities.Enum.RequestStatus.ACTIVE)
                    .OrderBy(x => x.DateDue).Take(3)
                    .Select(x => new RequestViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Subtitle = x.Subtitle,
                        Photo = Utils.SessionFacade.RootUrl + "/Donation/Photo/" + x.Id,
                        Progress = x.Progress,
                        CategoryId = x.Category.Id
                    }).ToList();

                return urgentThreeRequests;

            }
        }
    }
}