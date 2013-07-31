using System;
using System.Collections.Generic;
using System.Net.Http;

using CMZero.API.Messages;

namespace CMZero.API.ServiceAgent
{
    public class ApplicationsServiceAgent : BaseServiceAgent, IApplicationsServiceAgent
    {
        public ApplicationsServiceAgent(string baseUri)
            : this(baseUri, new HttpClient())
        {
        }

        public ApplicationsServiceAgent(string baseUri, HttpClient httpClient)
            : base(httpClient)
        {
            _baseUri = baseUri;
        }

        public Application Get(string id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
                                 {
                                     Path = string.Format("/application/{0}", id),
                                 };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<Application>(request);
        }

        public IEnumerable<Application> Get()
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = "/application/" };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<IEnumerable<Application>>(request);
        }

        public Application Post(Application application)
        {
            HttpRequestMessage request = CreatePostRequest(application, "/application/");

            return CheckResult<Application>(request);
        }

        public Application Put(Application application)
        {
            HttpRequestMessage request = CreatePutRequest(application, "/application/");

            return CheckResult<Application>(request);
        }

        public IEnumerable<Application> GetByOrganisation(string organisationId)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/application/organisation/{0}", organisationId)
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<IEnumerable<Application>>(request);
        }
    }
}