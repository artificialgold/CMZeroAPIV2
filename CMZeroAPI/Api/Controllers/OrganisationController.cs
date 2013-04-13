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

        public GetOrganisationsResponse Get()
        {
            var organisations = _organisationService.GetAll();
            return new GetOrganisationsResponse { Organisations = organisations };
        }

        public GetOrganisationResponse Get(string id)
        {
            try
            {
                Organisation organisation = _organisationService.GetById(id);
                return new GetOrganisationResponse { Organisation = organisation };
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        // POST api/values
        public PostOrganisationResponse Post([FromBody]Organisation organisation)
        {
            organisation = _organisationService.Create(organisation);
            PostOrganisationResponse response = new PostOrganisationResponse { Organisation = organisation };

            return response;
        }

        // PUT api/values/5
        public PutOrganisationResponse Put([FromBody] Organisation organisation)
        {
            try
            {
                organisation = _organisationService.Update(organisation);
                return new PutOrganisationResponse { Organisation = organisation };
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