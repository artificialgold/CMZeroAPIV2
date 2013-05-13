using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public interface ICollectionService : IBaseService<Collection>
    {
        IList<Collection> GetCollectionsForApplication(string applicationId);
    }
}