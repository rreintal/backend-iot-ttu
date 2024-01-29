using App.DAL.Contracts;
using App.Domain.Helpers;
using Base.BLL.Contracts;
using Base.DAL.EF.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Contracts;

public interface INewsService : ITranslateableEntityService<News>, INewsRepositoryCustom<App.Domain.News> // App.Domain.News mby sketchy
{
    // add your custom service methods here!
    public Task<List<ContentType>> GetContentTypes();
    
    public Task<IEnumerable<News>> AllAsyncFiltered(NewsFilterSet filterSet, string languageCulture);
    public Task<UpdateNews> UpdateNews(UpdateNews entity);

    public Task<News> FindByIdAllTranslationsAsync(Guid id);
    public Task<News> AddAsync(News entity);



}