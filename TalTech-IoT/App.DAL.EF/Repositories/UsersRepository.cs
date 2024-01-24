using App.DAL.Contracts;
using App.Domain.Identity;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class UsersRepository : EFBaseRepository<App.Domain.Identity.AppUser, AppDbContext>, IUsersRepository
{
    public UsersRepository(AppDbContext dataContext, IMapper mapper) : base(dataContext, mapper)
    {}
}