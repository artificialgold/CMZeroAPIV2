using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Responses;
using CMZero.API.Messages.Responses.Organisations;

namespace Api.Controllers
{
    public class OrganisationController : ApiController
    {
        private readonly IOrganisationService _organisationService;

        public OrganisationController(IOrganisationService organisationService)
        {
            _organisationService = organisationService;
        }

        public HttpResponseMessage Get()
        {
            IEnumerable<Organisation> organisations = _organisationService.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, organisations);
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {
                Organisation organisation = _organisationService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, organisation);
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]Organisation organisation)
        {
            organisation = _organisationService.Create(organisation);
            return Request.CreateResponse(HttpStatusCode.Created, organisation);
        }

        // PUT api/values/5
        public HttpResponseMessage Put([FromBody] Organisation organisation)
        {
            try
            {
                organisation = _organisationService.Update(organisation);
                return Request.CreateResponse(HttpStatusCode.OK, organisation);
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}