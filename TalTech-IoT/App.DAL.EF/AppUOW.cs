using App.DAL.Contracts;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : EFBaseUOW<AppDbContext>, IAppUOW
{
    private IMapper _mapper { get; set; }
    public AppUOW(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
    {
        _mapper = mapper;
    }

    private INewsRepository? _newsRepository;
    public INewsRepository NewsRepository =>
        _newsRepository ??= new NewsRepository(UowDbContext, _mapper);
}