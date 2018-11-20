using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SocialMarketplace.Controllers.api
{
    public class HomeController : ApiController
    {
        private readonly HomeBLO homeBLO = new HomeBLO();

        // GET: api/Home
        public HomeViewModel Get(int? id)
        {
            try
            {
                var viewModel = homeBLO.CreateHomeViewModel(id);
                return viewModel;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        // GET: api/Home/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Home
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Home/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Home/5
        public void Delete(int id)
        {
        }
    }
}
