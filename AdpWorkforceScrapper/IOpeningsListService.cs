using System.Collections.Generic;

namespace AdpWorkforceScrapper
{
    public interface IOpeningsListService
    {
        IEnumerable<Opening> FetchOpenings();
    }
}
