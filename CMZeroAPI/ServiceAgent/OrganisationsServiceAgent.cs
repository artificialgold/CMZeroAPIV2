using System;
using System.Net.Http;

using CMZero.API.Messages.Responses;

namespace CMZero.API.ServiceAgent
{
    public class OrganisationsServiceAgent :BaseServiceAgent
    {
        private readonly string _baseUri;

        public OrganisationsServiceAgent(string baseUri)
            : this(baseUri, new HttpClient())
		{
		}

        public OrganisationsServiceAgent(string baseUri, HttpClient httpClient) :base(httpClient)
        {
            _baseUri = baseUri;
        }

        public GetOrganisationResponse Get(int id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/organisation/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<GetOrganisationResponse>(request);
        }
    }
}
