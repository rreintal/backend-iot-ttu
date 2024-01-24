using App.DAL.Contracts;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class UsersRepository : EFBaseRepository<App.Domain.Identity.AppUser, AppDbContext>, IUsersRepository
{
    public UsersRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {}
    
}