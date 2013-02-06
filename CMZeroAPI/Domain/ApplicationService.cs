using CMZero.API.DataAccess.RepositoryInterfaces;
using CMZero.API.Messages;

namespace CMZero.API.Domain
{
    public class ApplicationService : BaseService<Application>, IApplicationService
    {
        public ApplicationService(IApplicationRepository applicationRepository)
        {
            Repository = applicationRepository;
        }
    }
}