using CleanCodeTest.Domain.Entities;
using Microsoft.Xrm.Sdk;

namespace CleanCodeTest.Domain.Repositories
{
    public interface IComicRepository
    {
        void UpdateComic(Gap_Comic data, Entity comic);

        string GetComicAttribute(string entityName, string[] column, string conditionColum, string conditionValue);
    }
}
