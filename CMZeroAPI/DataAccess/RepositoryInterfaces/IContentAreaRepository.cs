using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.DataAccess.RepositoryInterfaces
{
    public interface IContentAreaRepository : IRepository<ContentArea>
    {
        IList<ContentArea> ContentAreasInCollection(string collectionId);
    }
}