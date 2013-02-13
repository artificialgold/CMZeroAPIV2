using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;

namespace CMZero.API.ServiceAgent
{
    public abstract class BaseServiceAgent
    {
        private readonly HttpClient _client;

        protected BaseServiceAgent(HttpClient client)
        {
            _client = client;
        }

        protected T GetResult<T>(HttpRequestMessage request)
        {
            HttpResponseMessage response = _client.SendAsync(request).Result;

            return GetResultFromResponse<T>(response);
        }

        private T GetResultFromResponse<T>(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default(T);
            }

            HandleBadResponses(response);

            try
            {
                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsAsync<T>(new[] { new JsonMediaTypeFormatter() }).Result;
            }
            catch (HttpRequestException ex)
            {
                throw new BadResponseException(ex.Message, ex);
            }
        }

        private void HandleBadResponses(HttpResponseMessage response)
        {
            HandleBadRequest(response);
            HandleConflict(response);
            HandleForbidden(response);
            HandleUnauthorised(response);
            HandleServiceUnavailable(response);
        }

        private void HandleBadRequest(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var validationErrors = response.Content.ReadAsAsync<ValidationErrors>(new[] { new JsonMediaTypeFormatter() }).Result;

                throw new BadRequestException(validationErrors);
            }
        }

        private void HandleConflict(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new ConflictException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private void HandleForbidden(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessForbiddenException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private void HandleUnauthorised(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AccessUnauthorisedException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private void HandleServiceUnavailable(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ServiceUnavailableException(response.ReasonPhrase, GetContentAsString(response));
            }
        }

        private string GetContentAsString(HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return string.Empty;
            }

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}