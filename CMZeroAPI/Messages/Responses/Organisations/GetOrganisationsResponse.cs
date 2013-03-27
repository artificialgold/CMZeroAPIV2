using System.Collections.Generic;

namespace CMZero.API.Messages.Responses.Organisations
{
    public class GetOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }
    }
}