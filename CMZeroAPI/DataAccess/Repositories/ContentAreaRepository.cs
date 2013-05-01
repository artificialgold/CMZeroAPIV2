using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

using Raven.Client.Linq;

namespace CMZero.API.DataAccess.Repositories
{
    public class ContentAreaRepository : RepositoryBase<ContentArea>, IContentAreaRepository
    {
        public IList<ContentArea> ContentAreasInCollection(string collectionId)
        {
            try
            {
                using (var session = GetSession())
                {
                    var organisations = (from c in session.Query<ContentArea>()
                                         where c.CollectionId == collectionId
                                         select c);
                    return organisations.ToList();
                }
            }
            catch (Exception)
            {
                //TODO: Get correct exceptions
                throw new Exception();
            }
        }
    }
}