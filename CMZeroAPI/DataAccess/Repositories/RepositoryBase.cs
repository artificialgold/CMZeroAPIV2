using System;

using Raven.Client;
using Raven.Client.Document;

namespace CMZero.API.DataAccess.Repositories
{
    public class RepositoryBase<T>
    {
        public void Create(T organisation)
        {
            try
            {
                using (var session = GetSession())
                {
                    session.Store(organisation);
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //TODO Get correct exceptions
            }
        }

        public T GetById(Guid id)
        {
            try
            {
                using (var session = GetSession())
                {
                    return session.Load<T>(id);
                }
            }
            catch (Exception ex)
            {
                //TODO: Get correct exceptions
                throw new Exception();
            }
        }

        public void Update(T storedOrganisation)
        {
            try
            {
                using (var session = GetSession())
                {
                    session.Store(storedOrganisation);
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected static IDocumentSession GetSession()
        {
            var documentStore = new DocumentStore
                                    {
                                        Url = "https://aeo.ravenhq.com/databases/artificialgold-organisations",
                                        ApiKey = "1b66e4cb-a050-4d74-89f3-82fbe946627b"
                                    };
            documentStore.Initialize();
            return documentStore.OpenSession();
        }
    }
}