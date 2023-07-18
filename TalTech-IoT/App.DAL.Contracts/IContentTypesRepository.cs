using App.Domain;
using Base.DAL.EF.Contracts;

namespace App.DAL.Contracts;

public interface IContentTypesRepository : IBaseRepository<ContentType>
{
    public ContentType FindByName(string name);
}