using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.BLL
{
    public class DonationGivenBLO
    {
        
        //public IList<ResponseUserGivenModel> GetResponseListTest(int userId)
        //{
        //    using (var context = new ApplicationContext())
        //    {
        //        var query = context.Responses
        //                    .Where(x => x.UserId == userId)
        //                    .Select(x => new ResponseUserGivenModel
        //                    {
        //                        Id = x.Id,
        //                        Description = x.Description,
        //                        DataCreated = x.DateCreated

        //                    }).ToList();
        //        return query;
        //    }
        //}

        public IList<ResponseUserGivenModel> GetResponseList(int userId)
        {
            using (var context = new ApplicationContext())
            {
                var query = from RS in context.Responses
                            join RQ in context.Requests on RS.Request.Id equals RQ.Id
                            where RS.UserId == userId
                            select new
                            {
                                RQ.Title,
                                RS.Id,
                                RS.Description,
                                RS.DateCreated
                            };
                var responseList = query.Select(x => new ResponseUserGivenModel
                {
                    Id = x.Id,
                    RequestTitle = x.Title,
                    Description = x.Description,
                    DataCreated = x.DateCreated
                }).ToList();

                return responseList;
            }
        }


        public IList<ResponseItemUserGivenModel> GetItemList(int responseId)
        {
            using (var context = new ApplicationContext())
            {
                var query = from RS in context.Responses
                            join RSI in context.ResponseItems on RS.Id equals RSI.Response.Id
                            where RS.Id == responseId
                            select new
                            {
                                RSI.Id,
                                RSI.RequestItem.Title,
                                RSI.Quantity
                            };

                var itemList = query.Select(x => new ResponseItemUserGivenModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Quantity = x.Quantity
                }).ToList();

                return itemList;

            }
        }
    }
}