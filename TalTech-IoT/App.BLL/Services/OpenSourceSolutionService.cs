using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using DAL.DTO.V1;
using OpenSourceSolution = BLL.DTO.V1.OpenSourceSolution;

namespace App.BLL.Services;

public class OpenSourceSolutionService : BaseEntityService<OpenSourceSolution, Domain.OpenSourceSolution, IOpenSourceSolutionRepository>, IOpenSourceSolutionService
{
    private readonly IAppUOW _uow;
    
    public OpenSourceSolutionService(IAppUOW uow, IMapper<OpenSourceSolution, Domain.OpenSourceSolution> mapper) : base(uow.OpenSourceSolutionRepository, mapper)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<OpenSourceSolution>> AllAsync(string? languageCulture)
    {
        return (await _uow.OpenSourceSolutionRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<OpenSourceSolution?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.OpenSourceSolutionRepository.FindAsync(id, languageCulture));
    }

    public async Task<int> GetCount()
    {
        return await _uow.OpenSourceSolutionRepository.GetCount();
    }
}