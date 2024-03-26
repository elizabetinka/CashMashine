namespace Services;

public interface IUserService
{
    public Task<IList<string>> GetOperationsHistory(int id, int operationsCount = 10);

    public Task<bool> TryAddCash(int money, int id);

    public Task<bool> TryTakeCash(int money, int id);

    public Task<int> GetBalance(int idx);
}