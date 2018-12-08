using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.BLL
{
    public class AdminBLO
    {
        public DashboardSeriesViewModel CalculateActiveUsersPerDay()
        {
            var aWeekAgo = DateTime.Today.AddDays(-7);

            using (var context = new ApplicationDbContext())
            {
                var data = context.Users
                    .Where(x => x.CreatedAt >= aWeekAgo)
                    .GroupBy(x => x.CreatedAt)
                    .Select(l => new
                    {
                        CreatedAt = l.Key,
                        Count = l.Count()
                    })
                    .OrderBy(x => x.CreatedAt)
                    .ToList();

                string[] xvalues = data.Select(x => x.CreatedAt.ToString("MM/dd/yyyy")).ToArray();
                string[] yvalues = data.Select(x => x.Count.ToString()).ToArray();

                return new DashboardSeriesViewModel
                {
                    Xvalues = xvalues,
                    Yvalues = yvalues
                };
            }
        }

        public DashboardSeriesViewModel CalculateDonationsPerDay()
        {
            var aWeekAgo = DateTime.Today.AddDays(-28);

            using (var context = new ApplicationContext())
            {
                var data = context.Requests
                    .Where(x => x.DateCreated >= aWeekAgo)
                    .GroupBy(x => x.DateCreated.Month + "/" + x.DateCreated.Day)
                    .Select(l => new
                    {
                        Date = l.Key,
                        Count = l.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToList();

                string[] xvalues = data.Select(x => x.Date.ToString()).ToArray();
                string[] yvalues = data.Select(x => x.Count.ToString()).ToArray();

                return new DashboardSeriesViewModel
                {
                    Xvalues = xvalues,
                    Yvalues = yvalues
                };
            }
        }


        public DashboardSeriesViewModel CalculateResponsesPerDay()
        {
            var aWeekAgo = DateTime.Today.AddDays(-28);

            using (var context = new ApplicationContext())
            {
                var data = context.Responses
                    .Where(x => x.DateCreated >= aWeekAgo)
                    .GroupBy(x => x.DateCreated.Month + "/" + x.DateCreated.Day)
                    .Select(l => new
                    {
                        Date = l.Key,
                        Count = l.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToList();

                string[] xvalues = data.Select(x => x.Date.ToString()).ToArray();
                string[] yvalues = data.Select(x => x.Count.ToString()).ToArray();

                return new DashboardSeriesViewModel
                {
                    Xvalues = xvalues,
                    Yvalues = yvalues
                };
            }
        }
    }
}