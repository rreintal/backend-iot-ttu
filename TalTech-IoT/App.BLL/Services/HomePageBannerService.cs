using App.BLL.Contracts;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Services;

public class HomePageBannerService : BaseEntityService<HomePageBanner, Domain.HomePageBanner, IHomePageBannerRepository>, IHomePageBannerService
{
    private IAppUOW _uow;
    private IMapper _mapper;

    public HomePageBannerService(IAppUOW uow, IMapper<HomePageBanner, Domain.HomePageBanner> mapper, IMapper autoMapper) : base(uow.HomePageBannerRepository, mapper)
    {
        _uow = uow;
        _mapper = autoMapper;
    }
    // TODO: here do CDN magic!
    public async Task<IEnumerable<HomePageBanner>> AllAsync(string? languageCulture)
    {
        return (await _uow.HomePageBannerRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<HomePageBanner?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.HomePageBannerRepository.FindAsync(id, languageCulture));
    }

    public async Task<UpdateHomePageBanner?> Update(UpdateHomePageBanner entity)
    {
        // Here do image stuff!
        var dalEntity = _mapper.Map<global::DAL.DTO.V1.UpdateHomePageBanner>(entity);
        var dalResult = await _uow.HomePageBannerRepository.Update(dalEntity);
        var bllResult = _mapper.Map<UpdateHomePageBanner>(dalResult);
        return bllResult;
    }
}