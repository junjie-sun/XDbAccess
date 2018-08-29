using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Models;
using XDbAccess.Dapper;

namespace XDbAccess.Demo.Interfaces
{
    public interface IUserRepository
    {
        Task InsertUserAsync(User user);

        Task<List<User>> AllUsersAsync(string name);

        Task<User> GetUserAsync(int id);

        Task<int> UpdateUserAsync(User user);

        Task<int> DeleteUserAsync(int id);

        Task InsertUserEntityAsync(User user);

        Task<int> UpdateUserEntityAsync(User user);

        Task<PagedQueryResult<User>> PagedQueryAsync(string name, int pageIndex, int pageSize);
    }
}
