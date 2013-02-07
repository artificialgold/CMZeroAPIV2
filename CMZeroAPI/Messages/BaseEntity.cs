using System;

namespace CMZero.API.Messages
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }

        public DateTime Created { get; set; }
    }
}