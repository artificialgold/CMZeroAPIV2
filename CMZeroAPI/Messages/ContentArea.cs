using System;

namespace CMZero.API.Messages
{
    public class ContentArea
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public ContentAreaType ContentType { get; set; }
    }
}