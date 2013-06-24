using System;
using System.Collections.Generic;
using System.Net.Http;

using CMZero.API.Messages;

namespace CMZero.API.ServiceAgent
{
    public class OrganisationsServiceAgent : BaseServiceAgent, IOrganisationsServiceAgent
    {
        public OrganisationsServiceAgent(string baseUri)
            : this(baseUri, new HttpClient())
        {
        }

        public OrganisationsServiceAgent(string baseUri, HttpClient httpClient)
            : base(httpClient)
        {
            _baseUri = baseUri;
        }

        public Organisation Get(string id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/organisation/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<Organisation>(request);
        }

        public IEnumerable<Organisation> Get()
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = "/organisation/" };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<IEnumerable<Organisation>>(request);
        }

        public Organisation Post(Organisation organisation)
        {
            HttpRequestMessage request = CreatePostRequest(organisation, "/organisation/");

            return CheckResult<Organisation>(request);
        }

        public Organisation Put(Organisation organisation)
        {
            HttpRequestMessage request = CreatePutRequest(organisation, "/organisation/");

            return CheckResult<Organisation>(request);
        }
    }
}
