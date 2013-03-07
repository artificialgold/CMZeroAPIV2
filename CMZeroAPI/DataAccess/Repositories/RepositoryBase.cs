using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

using Raven.Client;
using Raven.Client.Document;

namespace CMZero.API.DataAccess.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        public void Create(T organisation)
        {
            try
            {
                organisation.Id = new Guid().ToString();

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

        public T GetById(string id)
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

        public IEnumerable<T> GetAll()
        {
            try
            {
                using (var session = GetSession())
                {
                    return session.Query<T>().Take(100);
                }
            }
            catch (Exception ex)
            {
                //TODO: Get correct exceptions
                throw new Exception();
            }
        }

        protected IDocumentSession GetSession()
        {
            var documentStore = new DocumentStore
                                    {
                                        Url = ConfigurationManager.AppSettings["RavenDBAddress"],
                                        ApiKey = ConfigurationManager.AppSettings["RavenDBApiKey"]
                                    };
            documentStore.Initialize();
            return documentStore.OpenSession();
        }
    }
}