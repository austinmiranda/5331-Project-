﻿using SocialMarketplace.Models.DAL;
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
using SocialMarketplace.Models.ViewModels.Response;

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

        public void SaveResponse(int userId, ResponseViewModel responseViewModel)
        {
            ValidateResponse(responseViewModel);

            using (var context = new ApplicationContext())
            {
                var request = context.Requests.Where(x => x.Id == responseViewModel.RequestId).SingleOrDefault();

                var response = new Response
                {
                    Description = responseViewModel.Description,
                    Status = ResponseStatus.NEW,
                    Request = request,
                    DateCreated = DateTime.Now,
                    Items = new List<ResponseItem>(),
                    UserId = userId
                };

                foreach (var item in responseViewModel.Items)
                {
                    var requestItem = request.Items.Where(x => x.Id == item.RequestItemId).SingleOrDefault();

                    response.Items.Add(new ResponseItem
                    {
                        Quantity = item.Quantity,
                        RequestItem = requestItem
                    });
                }

                context.Responses.Add(response);

                context.SaveChanges();

                responseViewModel.Id = response.Id;
            };
        }

        private void ValidateResponse(ResponseViewModel responseViewModel)
        {
            if(string.IsNullOrEmpty(responseViewModel.Description))
                throw new Exception("Description must be provided.");

            if (responseViewModel.Items.Count == 0)
                throw new Exception("At least one item must be donated.");

            foreach(var responseItem in responseViewModel.Items)
            {
                if(responseItem.Quantity == 0)
                    throw new Exception("At least one item must be donated.");

                if(responseItem.RequestItemId == 0)
                    throw new Exception("Invalid item.");
            }

        }

        public void SaveRequest(int userId, RequestMandatoryViewModel requestMandatory, RequestOptionalViewModel requestOptional = null)
        {
            ValidateStep1Request(requestMandatory);

            RequestStatus status = RequestStatus.IN_PROGRESS;

            if (requestOptional != null)
            {
                ValidateStep3Request(requestOptional);
                status = RequestStatus.ACTIVE;
            }

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
                    entity.Status = status;
                    entity.DateCreated = DateTime.Now;
                    entity.DateDue = requestMandatory.DateDue;
                    entity.Category = category;
                    entity.Description = requestMandatory.Description;
                    entity.Progress = 0;
                    entity.Subtitle = requestMandatory.Subtitle;
                    entity.Title = requestMandatory.Title;
                    entity.UserId = userId;
                    entity.VisualizationCount = 0;
                }
                else
                {
                    entity = new Request()
                    {
                        Area = area,
                        Status = status,
                        DateCreated = DateTime.Now,
                        DateDue = requestMandatory.DateDue,
                        Category = category,
                        Description = requestMandatory.Description,
                        Progress = 0,
                        Subtitle = requestMandatory.Subtitle,
                        Title = requestMandatory.Title,
                        UserId = userId,
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

        internal void DeleteItem(int requestId, int requestItemId)
        {
            using (var context = new ApplicationContext())
            {
                var item = context.RequestItems
                    .Where(x => x.Id == requestItemId && x.Request.Id == requestId).SingleOrDefault();

                if(item != null)
                {
                    context.RequestItems.Remove(item);
                    context.SaveChanges();
                }
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

        public DetailViewModel GetRequest(int id)
        {
            using (var context = new ApplicationContext())
            {
                var request = context.Requests
                    .Where(x => x.Id == id && (x.Status == RequestStatus.ACTIVE || x.Status == RequestStatus.COMPLETED)).SingleOrDefault();

                if (request == null)
                    return null;

                request.VisualizationCount++;
                context.SaveChanges();

                var detailViewModel = new DetailViewModel
                {
                    Id = request.Id,
                    CategoryId = request.Category.Id,
                    CategoryName = request.Category.Name,
                    Title = request.Title,
                    Subtitle = request.Subtitle,
                    Description = request.Description,
                    DateDue = request.DateDue,
                    Keywords = request.Keywords,
                    PhotoURL = $"{SessionFacade.RootUrl}/Donation/Photo/{request.Id}",
                    VideoURL = request.VideoURL,
                    Progress = request.Progress,
                    Items = new List<RequestItemViewModel>()
                };

                foreach (var item in request.Items)
                {
                    detailViewModel.Items.Add(new RequestItemViewModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Detail = item.Detail,
                        Type = item.Type,
                        Quantity = item.Quantity
                    });
                };

                return detailViewModel;
            }
        }

        internal void SaveQuestion(int userId, QuestionViewModel viewModel)
        {
            using (var context = new ApplicationContext())
            {
                var request = context.Requests.Where(x => x.Id == viewModel.RequestId).SingleOrDefault();

                context.Questions.Add(new Question
                {
                    Description = viewModel.Description,
                    Type = QuestionType.NOT_ANSWERED,
                    Request = request,
                    UserId = userId
                });

                context.SaveChanges();
            }
        }

        internal QuestionViewModel CreateEmptyQuestionViewModel(int id)
        {
            using (var context = new ApplicationContext())
            {
                var request = context.Requests.Where(x => x.Id == id).SingleOrDefault();

                return new QuestionViewModel
                {
                    RequestId = request.Id,
                    Title = request.Title,
                    Subtitle = request.Subtitle,
                    Description = String.Empty
                };
            }
        }

        public void ValidateStep1Request(RequestMandatoryViewModel request)
        {
            if(request.DateDue <= DateTime.Today)
                throw new Exception("Due date must be in the future.");
        }

        public void ValidateStep2Request(RequestItemViewModel requestItem)
        {
            if(requestItem.Quantity == 0)
                throw new Exception("Item quantity cannot be zero.");
        }

        public void ValidateStep3Request(RequestOptionalViewModel request)
        {
            if(request.Photo != null)
            {
                String filename = request.Photo.FileName.ToLower();

                if (!filename.EndsWith(".jpg") && !filename.EndsWith(".jpeg") && !filename.EndsWith(".png"))
                    throw new Exception("Invalid photo format. Must be JPG or PNG.");
            }
        }

        public byte[] Photo(int id)
        {
            using (var context = new ApplicationContext())
            {
                var request = context.Requests.Where(x => x.Id == id).SingleOrDefault();

                if (request != null)
                    return request.Photo;
                else
                    return null;
            }
        }
    }
}