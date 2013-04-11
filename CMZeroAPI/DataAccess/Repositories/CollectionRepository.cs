using System;
using System.Collections.Generic;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

using Raven.Client.Linq;

namespace CMZero.API.DataAccess.Repositories
{
    public class CollectionRepository : RepositoryBase<Collection>, ICollectionRepository
    {
        public IList<Collection> GetCollectionsForApplication(string applicationId, string organisationId)
        {
            try
            {
                using (var session = GetSession())
                {
                    var result =
                        (from collection in session.Query<Collection>() 
                         where collection.ApplicationId == applicationId && collection.OrganisationId == organisationId
                         select collection);

                    return result.ToList();
                }
            }
            catch(Exception ex)
            {
                //TODO: This properly with logging
                throw ex;
            }
        }
    }
}
