using System;
using System.Net.Http;

using CMZero.API.Messages;
using CMZero.API.Messages.Responses;
using CMZero.API.Messages.Responses.Organisations;

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

        public GetOrganisationResponse Get(string id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/organisation/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetOrganisationResponse>(request);
        }

        public GetOrganisationsResponse Get()
        {
            var uriBuilder = new UriBuilder(_baseUri) { Path = "/organisation/" };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetOrganisationsResponse>(request);
        }

        public PostOrganisationResponse Post(Organisation organisation)
        {
            HttpRequestMessage request = CreatePostRequest(organisation, "/organisation/");

            return CheckResult<PostOrganisationResponse>(request);
        }

        public PutOrganisationResponse Put(Organisation organisation)
        {
            HttpRequestMessage request = CreatePutRequest(organisation, "/organisation/");

            return CheckResult<PutOrganisationResponse>(request);
        }
    }
}
