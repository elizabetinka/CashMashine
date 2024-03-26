using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Services;

namespace Lab5.Tests.Data;

#pragma warning disable CA2007
public class UserService : IUserService
{
private IPersonRepository _personRepository;
public UserService(IPersonRepository personRepository)
{
    _personRepository = personRepository;
}

public Task<IList<string>> GetOperationsHistory(int id, int operationsCount = 10)
{
    throw new NotImplementedException();
}

public async Task<bool> TryAddCash(int money, int id)
{
    IPerson? person = await _personRepository.FindById(id);
    if (person is User u)
    {
        return u.IncreaseMoney(money);
    }

    return false;
}

public async Task<bool> TryTakeCash(int money, int id)
{
    IPerson? person = await _personRepository.FindById(id);
    if (person is User u)
    {
        return u.DecreaseMoney(money);
    }

    return false;
}

public async Task<int> GetBalance(int idx)
{
    IPerson? person = await _personRepository.FindById(idx);
    if (person is User u)
    {
        return u.Balance;
    }

    throw new ArgumentException("Вы не пользователь");
}
}