using System.Collections.Generic;

namespace CMZero.API.Messages.Responses.ContentAreas
{
    public class GetContentAreasResponse
    {
        public IEnumerable<ContentArea> ContentAreas { get; set; }
    }
}