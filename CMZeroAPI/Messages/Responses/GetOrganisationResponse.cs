using System.Collections.Generic;

namespace CMZero.API.Messages.Responses
{
    public class GetOrganisationResponse
    {
        public Organisation Organisation { get; set; }
    }

    public class PostOrganisationResponse
    {
        public Organisation Organisation { get; set; }
    }

    public class PutOrganisationResponse
    {
        public Organisation Organisation { get; set; }
    }

    public class GetOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }
    }

    public class GetApplicationResponse
    {
        public Application Application { get; set; }
    }

    public class PostApplicationResponse
    {
        public Application Application { get; set; }
    }

    public class PutApplicationResponse
    {
        public Application Application { get; set; }
    }

    public class GetApplicationsResponse
    {
        public IEnumerable<Application> Applications { get; set; }
    }
}
