using System.Net;
using System.Net.Http;
using System.Web.Http;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;
using CMZero.API.Messages.Responses.Applications;

namespace Api.Controllers
{
    public class ApplicationController : ApiController
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public GetApplicationsResponse Get()
        {
            var applications = _applicationService.GetAll();
            return new GetApplicationsResponse { Applications = applications };
        }

        public GetApplicationResponse Get(string id)
        {
            try
            {
                Application application = _applicationService.GetById(id);
                return new GetApplicationResponse() { Application = application };
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        // POST api/values
        public PostApplicationResponse Post([FromBody]Application application)
        {
            try
            {
                application = _applicationService.Create(application);
                PostApplicationResponse response = new PostApplicationResponse { Application = application };

                return response;
            }
            catch (OrganisationDoesNotExistException)
            {
                throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ReasonPhrases.OrganisationIdDoesNotExist });
            }
        }

        // PUT api/values/5
        public PutApplicationResponse Put([FromBody]Application application)
        {
            try
            {
                application = _applicationService.Update(application);
                return new PutApplicationResponse { Application = application };
            }
            catch (OrganisationIdNotValidException)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            ReasonPhrase = ReasonPhrases.OrganisationIdNotValid,
                            StatusCode = HttpStatusCode.BadRequest
                        });
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}