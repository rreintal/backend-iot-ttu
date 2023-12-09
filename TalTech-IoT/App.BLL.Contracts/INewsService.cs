using Base.BLL.Contracts;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface INewsService : ITranslateableEntityService<News>
{
    // add your custom service methods here!
    public Task<List<ContentType>> GetContentTypes();
    
    public Task<IEnumerable<News>> AllAsyncFiltered(int? page, int? size, string languageCulture);
    public Task<UpdateNews> UpdateNews(UpdateNews entity);

    public Task<News> FindByIdAllTranslationsAsync(Guid id);
}