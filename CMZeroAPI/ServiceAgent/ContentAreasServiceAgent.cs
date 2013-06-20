﻿using System;
using System.Net.Http;

using CMZero.API.Messages;
using CMZero.API.Messages.Responses.ContentAreas;

namespace CMZero.API.ServiceAgent
{
    public class ContentAreasServiceAgent : BaseServiceAgent, IContentAreasServiceAgent
    {
        public ContentAreasServiceAgent(string baseUri)
            : this(baseUri, new HttpClient())
        {
        }

        public ContentAreasServiceAgent(string baseUri, HttpClient httpClient)
            : base(httpClient)
        {
            _baseUri = baseUri;
        }

        public ContentArea Get(string id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/contentarea/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<ContentArea>(request);
        }

        public ContentArea Post(ContentArea contentArea)
        {
            HttpRequestMessage request = CreatePostRequest(contentArea, "/contentarea/");

            return CheckResult<ContentArea>(request);
        }

        public ContentArea Put(ContentArea contentArea)
        {
            HttpRequestMessage request = CreatePutRequest(contentArea, "/contentArea/");

            return CheckResult<ContentArea>(request);
        }
    }
}