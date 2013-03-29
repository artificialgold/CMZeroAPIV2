using CMZero.API.Messages;
using CMZero.API.Messages.Responses;
using CMZero.API.Messages.Responses.Organisations;

namespace CMZero.API.ServiceAgent
{
    public interface IOrganisationsServiceAgent
    {
        GetOrganisationResponse Get(string id);

        GetOrganisationsResponse Get();

        PostOrganisationResponse Post(Organisation organisation);

        PutOrganisationResponse Put(Organisation organisation);
    }
}