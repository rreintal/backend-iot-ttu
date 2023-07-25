using Base.DAL.EF.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface INewsService : IBaseRepository<News>
{
    // add your custom service methods here!
    public Task<List<ContentType>> GetContentTypes();
}