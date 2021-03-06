﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

using Raven.Client;
using Raven.Client.Document;

namespace CMZero.API.DataAccess.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        public void Create(T entity)
        {
            try
            {
                entity.Id = Guid.NewGuid().ToString();

                using (var session = GetSession())
                {
                    session.Store(entity);
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //TODO Get correct exceptions
                throw ex;
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
            {
                //TODO: Get correct exceptions
                throw new Exception();
            }
        }

        public IDocumentSession GetSession()
        {
            DocumentStore documentStore;

            if (ConfigurationManager.AppSettings["RavenDBUseLocal"] == "true")
            {
                documentStore = new DocumentStore
                {
                    ConnectionStringName = "RavenDB"
                };
            }
            else
            {
                documentStore = new DocumentStore
                                    {
                                        Url = ConfigurationManager.AppSettings["RavenDBLocalAddress"],
                                        ApiKey = ConfigurationManager.AppSettings["RavenDBApiKey"],
                                        DefaultDatabase = ConfigurationManager.AppSettings["DefaultDatabase"]
                                    };
            }

            documentStore.Initialize();
            return documentStore.OpenSession();
        }

        public bool IdExists(string id)
        {
            try
            {
                if (id == null) return false;

                using (var session = GetSession())
                {
                    return session.Load<T>(id) != null;
                }
            }
            catch (Exception)
            {
                //TODO: LogException
            }

            throw new Exception("Unable to determine if Id is existing");
        }
    }
}