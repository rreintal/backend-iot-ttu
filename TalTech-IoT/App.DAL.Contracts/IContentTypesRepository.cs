using App.Domain;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IContentTypesRepository : IBaseRepository<ContentType>
{
    public Task<ContentType> FindByName(string name);
}