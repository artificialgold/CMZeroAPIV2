using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public interface IBaseService<T> where T : BaseEntity
    {
        T Create(T entity);
        T Update(T entity);
        T GetById(string id);
        IEnumerable<T> GetAll();
        bool IdExists(string id);
    }
}