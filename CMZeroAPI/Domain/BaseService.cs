using CMZero.API.Domain.RepositoryInterfaces;

namespace CMZero.API.Domain
{
    public class BaseService<T>
    {
        protected IRepository<T> Repository;

        public T Create(T organisation)
        {
            Repository.Create(organisation);

            return organisation;
        }

        public T Update(T organisation)
        {
            Repository.Update(organisation);

            return organisation;
        }

        public T GetById(string id)
        {
            return Repository.GetById(id);
        }
    }
}