using App.BLL.Contracts;
using App.BLL.Contracts.ImageStorageModels.Save;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;
using DAL.DTO.V1;
using UpdateHomePageBanner = BLL.DTO.V1.UpdateHomePageBanner;

namespace App.BLL.Services;

public class HomePageBannerService : BaseEntityService<HomePageBanner, Domain.HomePageBanner, IHomePageBannerRepository>, IHomePageBannerService
{
    private IAppUOW _uow;
    private IMapper _mapper;
    private IImageStorageService _imageStorageService { get; set; }

    public HomePageBannerService(IAppUOW uow, IMapper<HomePageBanner, Domain.HomePageBanner> mapper, IMapper autoMapper) : base(uow.HomePageBannerRepository, mapper)
    {
        _uow = uow;
        _mapper = autoMapper;
        _imageStorageService = new ImageStorageService.ImageStorageService();
    }
    // TODO: here do CDN magic!
    public async Task<IEnumerable<HomePageBanner>> AllAsync(string? languageCulture)
    {
        return (await _uow.HomePageBannerRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }


    public override HomePageBanner Add(HomePageBanner entity)
    {
        var data = new SaveContent()
        {
            Items = new List<SaveItem>()
        };
        var banner = new SaveItem()
        {
            Sequence = 1,
            Content = entity.Image,
            IsAlreadyBase64 = true
        };
        data.Items.Add(banner);
        var cdnResult = _imageStorageService.Save(data);
        if (cdnResult != null)
        {
            var link = cdnResult.Items.FirstOrDefault(e => e.Sequence == 1)?.Content;
            entity.Image = link;
        }
        return base.Add(entity);
    }

    public async Task<HomePageBanner?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.HomePageBannerRepository.FindAsync(id, languageCulture));
    }

    public async Task<HomePageBanner> UpdateAsync(HomePageBanner entity)
    {
        var domainObject = _mapper.Map<Domain.HomePageBanner>(entity);
        var result = await _uow.HomePageBannerRepository.UpdateAsync(domainObject);
        return _mapper.Map<HomePageBanner>(result);
    }
    

    public async Task UpdateSequenceBulkAsync(List<HomePageBannerSequence> data)
    {
        await _uow.HomePageBannerRepository.UpdateSequenceBulkAsync(data);
    }
}