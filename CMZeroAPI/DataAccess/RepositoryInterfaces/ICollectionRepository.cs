using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.DataAccess.RepositoryInterfaces
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        IList<Collection> GetCollectionsForApplication(string applicationId, string organisationId);
    }
}