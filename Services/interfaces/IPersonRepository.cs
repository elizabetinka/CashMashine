using Models;

namespace Services;

public interface IPersonRepository
{
    public Task<IPerson?> FindByUsername(string username);
    public Task<IPerson?> FindById(int id);
}