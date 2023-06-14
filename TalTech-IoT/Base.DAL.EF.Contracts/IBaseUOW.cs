namespace Base.DAL.EF.Contracts;

public interface IBaseUOW
{
    Task<int> SaveChangesAsync();
}