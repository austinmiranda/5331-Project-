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
using SocialMarketplace.Models.ViewModels.Response;
using SocialMarketplace.Models.ViewModels.Home;

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

        public RequestListViewModel MyRequests(int userId, int skip, int take)
        {
            var result = new RequestListViewModel();

            using (var context = new ApplicationContext())
            {
                var query = context.Requests
                    .Where(x => x.UserId == userId);

                result.Quantity = query.Count();

                result.Requests = query
                    .Select(request => new RequestViewModel
                    {
                        Id = request.Id,
                        CategoryId = request.Category.Id,
                        Title = request.Title,
                        Subtitle = request.Subtitle,
                        Progress = request.Progress,
                        Photo = Utils.SessionFacade.RootUrl + "/Donation/Photo/" + request.Id
                    })
                    .OrderByDescending(x => x.Id)
                    .Skip(skip).Take(take).ToList();
            }

            return result;
        }

        internal SearchResultViewModel Search(SearchViewModel searchViewModel, int skip, int take)
        {
            var result = new SearchResultViewModel
            {
                Attributes = searchViewModel
            };

            using (var context = new ApplicationContext())
            {
                IQueryable<Request> query = context.Requests;

                if (searchViewModel.CategoryId != 0)
                    query = query.Where(x => x.Category.Id == searchViewModel.CategoryId);

                if (!string.IsNullOrEmpty(searchViewModel.Keywords))
                    query = query.Where(x => x.Keywords.Contains(searchViewModel.Keywords));

                if (!string.IsNullOrEmpty(searchViewModel.Query))
                    query = query.Where(x => x.Title.Contains(searchViewModel.Query) ||
                    x.Subtitle.Contains(searchViewModel.Query));

                result.Quantity = query.Count();

                result.Requests = query.Select(request => new RequestViewModel {
                    Id = request.Id,
                    CategoryId = request.Category.Id,
                    Title = request.Title,
                    Subtitle = request.Subtitle,
                    Progress = request.Progress,
                    Photo = Utils.SessionFacade.RootUrl + "/Donation/Photo/" + request.Id
                })
                .OrderByDescending(x => x.Id)
                .Skip(skip).Take(take).ToList();
            }

            return result;
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

        public void SaveRequest(int userId, RequestMandatoryViewModel requestMandatory, IList<RequestItemViewModel> itemsToMerge = null, RequestOptionalViewModel requestOptional = null)
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

                    // Caution! Accessing the request of another user!
                    if (userId != entity.UserId)
                        throw new UnauthorizedAccessException();

                    if (entity.Status == RequestStatus.COMPLETED)
                        throw new Exception("Request already completed");

                    // If already started or was suspendend, keep that way
                    if (entity.Status == RequestStatus.IN_PROGRESS || entity.Status == RequestStatus.IN_PROGRESS)
                        status = entity.Status;

                    entity.Area = area;
                    entity.Status = status;
                    entity.DateDue = requestMandatory.DateDue;
                    entity.Category = category;
                    entity.Description = requestMandatory.Description;
                    entity.Subtitle = requestMandatory.Subtitle;
                    entity.Title = requestMandatory.Title;
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
                        Items = new List<RequestItem>(),
                        UserId = userId,
                        VisualizationCount = 0
                    };

                    context.Requests.Add(entity);
                }

                if(itemsToMerge != null)
                {
                    var currentList = entity.Items;
                    var newList = itemsToMerge;

                    var removeList = currentList
                        .Where(currentItem => newList.All(newItem => currentItem.Id != newItem.Id))
                        .ToList();

                    var addList = newList
                        .Where(newItem => currentList.All(currentItem => currentItem.Id != newItem.Id))
                        .ToList();

                    var updateList = currentList
                        .Join(newList,
                            currentItem => currentItem.Id,
                            newItem => newItem.Id,
                            (currentItem, newItem) => new { CurrentItem = currentItem, NewItem = newItem })
                        .ToList();

                    foreach (var itemToAdd in addList)
                    {
                        var item = new RequestItem
                        {
                            Quantity = itemToAdd.Quantity,
                            Request = entity,
                            Title = itemToAdd.Title,
                            Detail = itemToAdd.Detail,
                            Type = itemToAdd.Type
                        };

                        context.RequestItems.Add(item);
                    }

                    foreach (var itemToRemove in removeList)
                        context.RequestItems.Remove(itemToRemove);

                    foreach(var item in updateList)
                    {
                        item.CurrentItem.Title = item.NewItem.Title;
                        item.CurrentItem.Detail = item.NewItem.Detail;
                        item.CurrentItem.Quantity = item.NewItem.Quantity;
                        item.CurrentItem.Type = item.NewItem.Type;
                    }
                }

                if (requestOptional != null)
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

        internal void SaveRequestPhoto(int userId, int requestId, HttpPostedFile file)
        {
            using (var context = new ApplicationContext())
            {
                Request entity = context.Requests.Where(x => x.Id == requestId).SingleOrDefault();

                // Caution! Accessing the request of another user!
                if (userId != entity.UserId)
                    throw new UnauthorizedAccessException();

                if (entity.Status == RequestStatus.COMPLETED)
                    throw new Exception("Request already completed");

                if (file != null)
                {
                    byte[] fileData = null;

                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    entity.Photo = fileData;

                    context.SaveChanges();
                }
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