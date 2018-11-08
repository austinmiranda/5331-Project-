using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.ViewModels.Home;
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
            var homeViewModel = new HomeViewModel();

            homeViewModel.Categories = GetCategories();

            homeViewModel.MainRequest = GetMainRequest(categoryId ?? 1);
            homeViewModel.CategoryRequests = GetCategoryList(categoryId ?? 1);
            homeViewModel.UrgentRequests = GetUrgentCategoryList();

            return homeViewModel;
        }

        public Dictionary<int,String> GetCategories()
        {
            using (var context = new ApplicationContext())
            {
                var categories = context.Categories.ToDictionary(x => x.Id, x => x.Name);
                
                return categories;
            }
        }
        public RequestViewModel GetMainRequest(int? Id)
        {
            using (var context = new ApplicationContext())
            {
                RequestViewModel mainRequest = context.Requests
                    .Where(x => x.Category.Id == Id &&
                        x.Status == Entities.Enum.RequestStatus.ACTIVE)
                    .OrderByDescending(x => x.VisualizationCount)
                    .Take(1)
                    .Select(x => new RequestViewModel
                    {
                        Id = x.Id,
                        Title = x.Title, 
                        Subtitle = x.Subtitle,
                        Photo = "/Donation/Photo/" + x.Id,
                        Progress = x.Progress,
                        CategoryId = x.Category.Id
                    }).SingleOrDefault();

                return mainRequest;
            }
        }
        public IList<RequestViewModel> GetCategoryList(int? Id)
        {
            using (var context = new ApplicationContext())
            {
                List<RequestViewModel> topThreeRequests = context.Requests
                    .Where(x => x.Category.Id == Id &&
                        x.Status == Entities.Enum.RequestStatus.ACTIVE)
                    .OrderByDescending(x => x.VisualizationCount)
                    .Skip(1)
                    .Take(3)
                    .Select(x => new RequestViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Subtitle = x.Subtitle,
                        Photo = "/Donation/Photo/" + x.Id,
                        Progress = x.Progress,
                        CategoryId = x.Category.Id
                    }).ToList();

                return topThreeRequests;
            }
        }

        public IList<RequestViewModel> GetUrgentCategoryList()
        {
            using (var context = new ApplicationContext())
            {
                List<RequestViewModel> urgentThreeRequests = context.Requests
                    .OrderByDescending(x => x.DateDue).Take(3)
                    .Select(x => new RequestViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Subtitle = x.Subtitle,
                        Photo = "/Donation/Photo/" + x.Id,
                        Progress = x.Progress,
                        CategoryId = x.Category.Id
                    }).ToList();

                return urgentThreeRequests;

            }
        }
    }
}