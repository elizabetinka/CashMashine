using System.Collections;
using System.Collections.Generic;
using Models;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class DecreaseTestsGood : IEnumerable<object[]>
{
private readonly List<object[]> _data = new List<object[]>
{
    new object[] { new User("Liza", "111", 1, 0), 0, 0 },
    new object[] { new User("Liza1", "112", 2, 10), 1, 9 },
    new object[] { new User("Liza2", "113", 3, 1000), 100, 900 },
    new object[] { new User("Liza3", "114", 4, 1000000), 1000000, 0 },
    new object[] { new User("Liza4", "115", 5, 10), 10, 0 },
    new object[] { new User("Liza5", "116", 6, 1000), 999, 1 },
    new object[] { new User("Liza6", "117", 7, 30), 27, 3 },
    new object[] { new User("Liza7", "118", 8, 340), 100, 240 },
    new object[] { new User("Liza8", "119", 9, 101), 100, 1 },
    new object[] { new User("Liza9", "120", 10, 2), 1, 1 },
    new object[] { new User("Liza10", "121", 11, 100000000), 1, 99999999 },
};

public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}