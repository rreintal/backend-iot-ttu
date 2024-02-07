using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Contracts;
using BLL.DTO.V1;

namespace App.BLL.Services;

public class ContactPersonService : BaseEntityService<ContactPerson , Domain.ContactPerson, IContactPersonRepository>, IContactPersonService
{
    private IAppUOW _uow;
    public ContactPersonService(IAppUOW uow, IMapper<ContactPerson, Domain.ContactPerson> mapper) : base(uow.ContactPersonRepository, mapper)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<ContactPerson>> AllAsync(string? languageCulture)
    {
        return (await _uow.ContactPersonRepository.AllAsync(languageCulture)).Select(e => Mapper.Map(e));
    }

    public async Task<ContactPerson?> FindAsync(Guid id, string? languageCulture)
    {
        return Mapper.Map(await _uow.ContactPersonRepository.FindAsync(id, languageCulture));
    }
}