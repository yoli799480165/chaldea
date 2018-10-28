using MongoDB.Bson;

namespace Chaldea.Core.Repositories
{
    public class RepositoryHelper
    {
        public static ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}