using System.Collections.Generic;

using CMZero.API.Messages;

namespace CMZero.API.ServiceAgent
{
    public interface IOrganisationsServiceAgent
    {
        Organisation Get(string id);

        IEnumerable<Organisation> Get();

        Organisation Post(Organisation organisation);

        Organisation Put(Organisation organisation);
    }
}