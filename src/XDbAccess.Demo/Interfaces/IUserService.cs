using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Models;
using XDbAccess.Dapper;

namespace XDbAccess.Demo.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(User user);

        Task<List<User>> AllUsersAsync(string name);

        Task<User> GetUserAsync(int id);

        Task<int> ModifyUserAsync(User user);

        Task<int> RemoveUserAsync(int id);

        Task<int> SingleTransDemoAsync();

        Task<int> SingleTwoTransDemoAsync();

        Task<int> DoubleTwoTransDemoAsync();

        Task<int> DoubleTwoTransNestRollbackDemoAsync();

        Task<int> DoubleTwoTransRootRollbackDemoAsync();

        Task AddUserEntityAsync(User user);

        Task<int> ModifyUserEntityAsync(User user);

        Task<PagedQueryResult<User>> PagedQueryDemoAsync(string name, int pageIndex, int pageSize);
    }
}
