using System.Collections.Generic;

using CMZero.API.Messages;

using Raven.Client;

namespace CMZero.API.DataAccess.RepositoryInterfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Create(T organisation);

        T GetById(string id);

        void Update(T storedOrganisation);

        IEnumerable<T> GetAll();

        IDocumentSession GetSession();

        bool IdExists(string id);
    }
}