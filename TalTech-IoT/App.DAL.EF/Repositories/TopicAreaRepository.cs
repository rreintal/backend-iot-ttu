using App.DAL.Contracts;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TopicAreaRepository : EFBaseRepository<App.Domain.TopicArea, AppDbContext>, ITopicAreaRepository
{
    public TopicAreaRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {
        
    }
}