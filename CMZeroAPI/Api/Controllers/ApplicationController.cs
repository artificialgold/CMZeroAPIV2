using System.Net;
using System.Net.Http;
using System.Web.Http;

using CMZero.API.Domain;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;
using CMZero.API.Messages.Exceptions.Organisations;

namespace Api.Controllers
{
    public class ApplicationController : ApiController
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public HttpResponseMessage Get()
        {
            var applications = _applicationService.GetAll();
            return Request.CreateResponse(HttpStatusCode.OK, applications);
        }

        public HttpResponseMessage Get(string id)
        {
            try
            {
                Application application = _applicationService.GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, application);
            }
            catch (ItemNotFoundException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        public HttpResponseMessage Post([FromBody]Application application)
        {
            try
            {
                application = _applicationService.Create(application);
                return Request.CreateResponse(HttpStatusCode.Created, application);
            }
            catch (OrganisationDoesNotExistException)
            {
                throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ReasonPhrases.OrganisationIdDoesNotExist });
            }
        }

        public HttpResponseMessage Put([FromBody]Application application)
        {
            try
            {
                application = _applicationService.Update(application);
                return Request.CreateResponse(HttpStatusCode.OK, application);
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
    }
}