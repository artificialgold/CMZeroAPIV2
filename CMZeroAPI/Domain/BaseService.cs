using System;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected IRepository<T> Repository;

        public T Create(T organisation)
        {
            organisation.Created = DateTime.UtcNow;
            organisation.Updated = DateTime.UtcNow;

            Repository.Create(organisation);

            return organisation;
        }

        public T Update(T organisation)
        {
            organisation.Updated = DateTime.UtcNow;

            Repository.Update(organisation);

            return organisation;
        }

        public T GetById(string id)
        {
            return Repository.GetById(id);
        }
    }
}