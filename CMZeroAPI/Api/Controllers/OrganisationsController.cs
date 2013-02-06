using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMZero.API.Domain;
using CMZero.API.Messages;

namespace Api.Controllers
{
    public class OrganisationsController : ApiController
    {
        private readonly IOrganisationService _organisationService;

        public OrganisationsController(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
        }

        public Organisation Get(int id)
        {
            return new Organisation{Name = "TEST"};
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]Organisation organisation)
        {
            organisation = _organisationService.Create(organisation);
            var response = Request.CreateResponse<Organisation>(HttpStatusCode.Created, organisation);

            string uri = Url.Link("DefaultApi", new { id = organisation.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}