using System.Collections.Generic;

namespace CMZero.API.Messages.Responses.Applications
{
    public class GetApplicationsResponse
    {
        public IEnumerable<Application> Applications { get; set; }
    }
}