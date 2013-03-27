using System;
using System.Collections.Generic;

using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;
using CMZero.API.Messages.Exceptions;

namespace CMZero.API.Domain
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected IRepository<T> Repository;

        public T Create(T entity)
        {
            entity.Created = DateTime.UtcNow;
            entity.Updated = DateTime.UtcNow;

            Repository.Create(entity);

            return entity;
        }

        public T Update(T entity)
        {
            entity.Updated = DateTime.UtcNow;

            Repository.Update(entity);

            return entity;
        }

        public T GetById(string id)
        {
            var result = Repository.GetById(id);

            if (result==null)
                throw new ItemNotFoundException();

            return result;
        }

        public IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public bool IdExists(string id)
        {
            return Repository.IdExists(id);
        }
    }
}