using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public interface IBaseService<T> where T : BaseEntity
    {
        T Create(T organisation);
        T Update(T organisation);
        T GetById(string id);
    }
}