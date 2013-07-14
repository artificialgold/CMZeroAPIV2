using System;

namespace CMZero.API.Domain.ApiKey
{
    public class ApiKeyCreator : IApiKeyCreator
    {
        public string Create()
        {
            return Guid.NewGuid().ToString();
        }
    }
}