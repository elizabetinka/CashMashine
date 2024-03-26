using System.Collections;
using System.Collections.Generic;
using Models;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class DecreaseTestsBad : IEnumerable<object[]>
{
private readonly List<object[]> _data = new List<object[]>
{
    new object[] { new User("Liza", "111", 1, 0), 1, 0 },
    new object[] { new User("Liza1", "112", 2, 1), 2, 1 },
    new object[] { new User("Liza2", "113", 3, 9), 10, 9 },
    new object[] { new User("Liza3", "114", 4, 7), 10, 7 },
    new object[] { new Admin("Liza4", "115", 5), 10, 0 },
    new object[] { new User("Liza5", "116", 6, 1), 999, 1 },
    new object[] { new User("Liza6", "117", 7, 27), 30, 27 },
    new object[] { new User("Liza7", "118", 8, 100), 340, 100 },
    new object[] { new User("Liza8", "119", 9, 100), 101, 100 },
    new object[] { new User("Liza9", "120", 10, 1), 10, 1 },
    new object[] { new User("Liza10", "121", 11, 1), 100000000, 1 },
};

public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}