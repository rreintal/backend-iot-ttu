using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService.Models.Delete;
using App.DAL.Contracts;
using App.Domain;
using Base.BLL;
using Base.Contracts;
using ImageResource = BLL.DTO.V1.ImageResource;

namespace App.BLL.Services;

public class FeedPagePostService : BaseEntityService<global::BLL.DTO.V1.FeedPagePost,FeedPagePost, IFeedPagePostRepository>, IFeedPagePostService
{
    private readonly IAppUOW _uow;
    private readonly IImageStorageService _imageStorageService;
    public FeedPagePostService(IAppUOW uow, IMapper<global::BLL.DTO.V1.FeedPagePost, FeedPagePost> mapper, IImageStorageService imageStorageService) : base(uow.FeedPagePostRepository, mapper)
    {
        _uow = uow;
        _imageStorageService = imageStorageService;
    }

    public async Task<IEnumerable<global::BLL.DTO.V1.FeedPagePost>> AllAsync(string? languageCulture)
    {
        return (await _uow.FeedPagePostRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public override global::BLL.DTO.V1.FeedPagePost Add(global::BLL.DTO.V1.FeedPagePost entity)
    {
        var result = _imageStorageService.ProccessSave(entity);
        if (result != null && result.SavedLinks != null)
        {
            entity.ImageResources = new List<ImageResource>();
            foreach (var imgResource in result.SavedLinks)
            {
                entity.ImageResources.Add(new ImageResource()
                {
                    FeedPagePostId = entity.Id,
                    Link = imgResource
                });
            }
            
        }
        return base.Add(entity);
    }

    public override global::BLL.DTO.V1.FeedPagePost Update(global::BLL.DTO.V1.FeedPagePost entity)
    {
        _imageStorageService.ProccessUpdate(entity);
        return base.Update(entity);
    }

    public async override Task<global::BLL.DTO.V1.FeedPagePost?> RemoveAsync(Guid id)
    {
        var entity = await _uow.FeedPagePostRepository.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        if (entity.ImageResources != null)
        {
            var deleteContent = new DeleteContent()
            {
                Links = new List<string>()
            };
            foreach (var resource in entity.ImageResources)
            {
                deleteContent.Links.Add(resource.Link);
            }

            _imageStorageService.ProcessDelete(deleteContent);
        }
        return await base.RemoveAsync(id);
    }

    public async Task<global::BLL.DTO.V1.FeedPagePost?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.FeedPagePostRepository.FindAsync(id, languageCulture));
    }

    public async Task<global::BLL.DTO.V1.FeedPagePost> UpdateAsync(global::BLL.DTO.V1.FeedPagePost entity)
    {
        var existingEntity = await _uow.FeedPagePostRepository.FindAsync(entity.Id);

        // THIS CAN BE REFACTORED //
        if (existingEntity != null)
        {
            entity.ImageResources = existingEntity.ImageResources.Select(e => new ImageResource()
            {
                FeedPagePostId = entity.Id,
                Link = e.Link
            }).ToList();
        }

        var updateResult = _imageStorageService.ProccessUpdate(entity);
        if (updateResult != null)
        {
            if (updateResult.DeletedLinks != null)
            {
                var deleteContent = new DeleteContent()
                {
                    Links = new List<string>()
                };
                foreach (var deletedLink in updateResult.DeletedLinks)
                {
                    deleteContent.Links.Add(deletedLink);
                }

                _imageStorageService.ProcessDelete(deleteContent);
            }

            if (updateResult.SavedLinks != null)
            {
                entity.ImageResources = updateResult.SavedLinks.Select(e => new ImageResource()
                {
                    FeedPagePostId = entity.Id,
                    Link = e
                }).ToList();
            }
        }
        // THIS CAN BE REFACTORED //
        var domainObject = Mapper.Map(entity);
        var domainResult = await _uow.FeedPagePostRepository.UpdateAsync(domainObject);
        return Mapper.Map(domainResult)!;
    }
}