using CleanCodeTest.Domain.Entities;
using Microsoft.Xrm.Sdk;

namespace CleanCodeTest.Domain.Repositories
{
    public interface IComicRepository
    {
        void UpdateByMarvelApiId(gap_Comic data, Entity comic);
    }
}
