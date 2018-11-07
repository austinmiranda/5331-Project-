using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Entities;
using SocialMarketplace.Models.Entities.Enum;
using SocialMarketplace.Models.Utils;
using SocialMarketplace.Models.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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
                    RequestInForm = new RequestMandatoryViewModel
                    {
                        DateDue = DateTime.Today.AddDays(7)
                    }
                },
                Step2 = new RequestStep2ViewModel
                {
                    RequestItemTypes = GetRequestItemTypes(),
                    ItemInForm = new RequestItemViewModel(),
                    Items = new List<RequestItemViewModel>()
                },
                Step3 = new RequestStep3ViewModel
                {
                    RequestOptional = new RequestOptionalViewModel()
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

        public void SaveRequest(RequestMandatoryViewModel requestMandatory, RequestOptionalViewModel requestOptional = null)
        {
            ValidateStep1Request(requestMandatory);

            int categoryId = requestMandatory.CategoryId.Value;

            using (var context = new ApplicationContext())
            {
                var category = context.Categories.Where(x => x.Id == categoryId).SingleOrDefault();
                var area = context.Areas.Where(x => x.Id == SessionFacade.AreaId).SingleOrDefault();

                Request entity;
                
                if(requestMandatory.Id != null)
                {
                    entity = context.Requests.Where(x => x.Id == requestMandatory.Id).SingleOrDefault();

                    entity.Area = area;
                    entity.Status = RequestStatus.ACTIVE;
                    entity.DateCreated = DateTime.Now;
                    entity.DateDue = requestMandatory.DateDue;
                    entity.Category = category;
                    entity.Description = requestMandatory.Description;
                    entity.Progress = 0;
                    entity.Subtitle = requestMandatory.Subtitle;
                    entity.Title = requestMandatory.Title;
                    entity.UserId = SessionFacade.User.Identity.GetUserId<int>();
                    entity.VisualizationCount = 0;
                }
                else
                {
                    entity = new Request()
                    {
                        Area = area,
                        Status = RequestStatus.ACTIVE,
                        DateCreated = DateTime.Now,
                        DateDue = requestMandatory.DateDue,
                        Category = category,
                        Description = requestMandatory.Description,
                        Progress = 0,
                        Subtitle = requestMandatory.Subtitle,
                        Title = requestMandatory.Title,
                        UserId = SessionFacade.User.Identity.GetUserId<int>(),
                        VisualizationCount = 0
                    };

                    context.Requests.Add(entity);
                }

                if(requestOptional != null)
                {
                    entity.Keywords = requestOptional.Keywords;
                    entity.VideoURL = requestOptional.VideoURL;

                    if(requestOptional.Photo != null)
                    {
                        byte[] fileData = null;

                        using (var binaryReader = new BinaryReader(requestOptional.Photo.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(requestOptional.Photo.ContentLength);
                        }

                        entity.Photo = fileData;
                    }
                }

                context.SaveChanges();

                requestMandatory.Id = entity.Id;
            }
        }

        internal void AddRequestItem(RequestItemViewModel requestItem, int Id)
        {
            ValidateStep2Request(requestItem);

            using (var context = new ApplicationContext())
            {
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

        public void ValidateStep1Request(RequestMandatoryViewModel request)
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