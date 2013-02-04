namespace CMZero.API.DataAccess.RepositoryInterfaces
{
    public interface IRepository<T>
    {
        void Create(T organisation);

        T GetById(string id);

        void Update(T storedOrganisation);
    }
}