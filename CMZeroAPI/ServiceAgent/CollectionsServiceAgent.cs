﻿using System;
using System.Collections.Generic;
using System.Net.Http;

using CMZero.API.Messages;

namespace CMZero.API.ServiceAgent
{
    public class CollectionsServiceAgent :BaseServiceAgent, ICollectionServiceAgent
    {
        public CollectionsServiceAgent(string baseUri):this(baseUri, new HttpClient())
        {
        }

        public CollectionsServiceAgent(string baseUri, HttpClient httpClient)
            : base(httpClient)
        {
            _baseUri = baseUri;
        }

        public Collection Get(string id)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("/collection/{0}", id),
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<Collection>(request);
        }

        public Collection Post(Collection collection)
        {
            HttpRequestMessage request = CreatePostRequest(collection, "/collection/");

            return CheckResult<Collection>(request);
        }

        public Collection Put(Collection collection)
        {
            HttpRequestMessage request = CreatePutRequest(collection, "/collection/");

            return CheckResult<Collection>(request);
        }

        public IEnumerable<Collection> GetByApiKey(string apiKey)
        {
            var uriBuilder = new UriBuilder(_baseUri)
            {
                Path = string.Format("apikey/{0}/collections/", apiKey)
            };

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            return GetResult<IEnumerable<Collection>>(request);
        }
    }
}