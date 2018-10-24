using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;
using XDbAccess.Demo.Repositories;
using XDbAccess.AutoTrans;
using XDbAccess.Common;

namespace XDbAccess.Demo.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _UserRepo;

        private DapperTestDbContext _DbContext;

        public UserService(IUserRepository userRepo, DapperTestDbContext dbContext)
        {
            _UserRepo = userRepo;
            _DbContext = dbContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _UserRepo.InsertUserAsync(user);
        }

        public async Task<List<User>> AllUsersAsync(string name)
        {
            return await _UserRepo.AllUsersAsync(name);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _UserRepo.GetUserAsync(id);
        }

        public async Task<int> ModifyUserAsync(User user)
        {
            return await _UserRepo.UpdateUserAsync(user);
        }

        public async Task<int> RemoveUserAsync(int id)
        {
            return await _UserRepo.DeleteUserAsync(id);
        }

        public async Task<int> SingleTransDemoAsync()
        {
            int result = 0;
            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Michael",
                    Birthday = new DateTime(1986, 1, 20),
                    Description = "SingleTransTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                user.Description = "SingleTransDemoUser";
                result = await _UserRepo.UpdateUserAsync(user);
                if(result == 0)
                {
                    throw new Exception("Modify User Fail");
                }
                scope.Commit();
            }
            return result;
        }

        public async Task<int> SingleTwoTransDemoAsync()
        {
            int result = 0;

            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Kaka",
                    Birthday = new DateTime(1982, 6, 21),
                    Description = "SingleTwoTransTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                user = new User()
                {
                    Name = "Jerry",
                    Birthday = new DateTime(1990, 3, 2),
                    Description = "SingleTwoTransTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }
                scope.Commit();
            }

            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Ronaldo",
                    Birthday = new DateTime(1976, 6, 30),
                    Description = "SingleTwoTransTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                user.Description = "SingleTwoTransDemoUser";
                result = await _UserRepo.UpdateUserAsync(user);
                if (result == 0)
                {
                    throw new Exception("Modify User Fail");
                }
                scope.Commit();
            }

            return result;
        }

        public async Task<int> DoubleTwoTransDemoAsync()
        {
            int result = 0;

            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Jim",
                    Birthday = new DateTime(1975, 8, 12),
                    Description = "DoubleTwoTransTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                using (var nestScope = _DbContext.TransScope())
                {
                    var user1 = new User()
                    {
                        Name = "Neymar",
                        Birthday = new DateTime(1992, 2, 27),
                        Description = "DoubleTwoTransTestUser"
                    };

                    await _UserRepo.InsertUserAsync(user1);
                    if (user1.Id == 0)
                    {
                        throw new Exception("Add User Fail");

                    }
                    nestScope.Commit();
                }

                user = new User()
                {
                    Name = "Adriano",
                    Birthday = new DateTime(1981, 5, 30),
                    Description = "DoubleTwoTransTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }
                scope.Commit();
            }

            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Messi",
                    Birthday = new DateTime(1987, 7, 30),
                    Description = "DoubleTwoTransTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                user.Description = "DoubleTwoTransDemoUser";
                result = await _UserRepo.UpdateUserAsync(user);
                if (result == 0)
                {
                    throw new Exception("Modify User Fail");
                }
                scope.Commit();
            }

            return result;
        }

        public async Task<int> DoubleTwoTransNestRollbackDemoAsync()
        {
            int result = 0;

            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Jim",
                    Birthday = new DateTime(1975, 8, 12),
                    Description = "DoubleTwoTransNestRollbackTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                using (var nestScope = _DbContext.TransScope())
                {
                    var user1 = new User()
                    {
                        Name = "Neymar",
                        Birthday = new DateTime(1992, 2, 27),
                        Description = "DoubleTwoTransNestRollbackTestUser"
                    };

                    await _UserRepo.InsertUserAsync(user1);
                    if (user1.Id == 0)
                    {
                        throw new Exception("Add User Fail");

                    }
                    nestScope.Rollback();
                }

                user = new User()
                {
                    Name = "Adriano",
                    Birthday = new DateTime(1981, 5, 30),
                    Description = "DoubleTwoTransNestRollbackTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }
                scope.Commit();
            }

            using (var scope = _DbContext.TransScope(TransScopeOption.RequireNew))
            {
                var user = new User()
                {
                    Name = "Messi",
                    Birthday = new DateTime(1987, 7, 30),
                    Description = "DoubleTwoTransNestRollbackTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                user.Description = "DoubleTwoTransNestRollbackDemoUser";
                result = await _UserRepo.UpdateUserAsync(user);
                if (result == 0)
                {
                    throw new Exception("Modify User Fail");
                }
                scope.Commit();
            }

            return result;
        }

        public async Task<int> DoubleTwoTransRootRollbackDemoAsync()
        {
            int result = 0;

            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Mary",
                    Birthday = new DateTime(1975, 8, 12),
                    Description = "DoubleTwoTransRootRollbackTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                using (var nestScope = _DbContext.TransScope())
                {
                    var user1 = new User()
                    {
                        Name = "Neymar",
                        Birthday = new DateTime(1992, 2, 27),
                        Description = "DoubleTwoTransRootRollbackTestUser"
                    };

                    await _UserRepo.InsertUserAsync(user1);
                    if (user1.Id == 0)
                    {
                        throw new Exception("Add User Fail");

                    }
                    nestScope.Commit();
                }

                user = new User()
                {
                    Name = "Adriano",
                    Birthday = new DateTime(1981, 5, 30),
                    Description = "DoubleTwoTransRootRollbackTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }
                scope.Rollback();
            }

            using (var scope = _DbContext.TransScope())
            {
                var user = new User()
                {
                    Name = "Messi",
                    Birthday = new DateTime(1987, 7, 30),
                    Description = "DoubleTwoTransRootRollbackTestUser"
                };

                await _UserRepo.InsertUserAsync(user);
                if (user.Id == 0)
                {
                    throw new Exception("Add User Fail");

                }

                user.Description = "DoubleTwoTransRootRollbackDemoUser";
                result = await _UserRepo.UpdateUserAsync(user);
                if (result == 0)
                {
                    throw new Exception("Modify User Fail");
                }
                scope.Commit();
            }

            return result;
        }

        public async Task AddUserEntityAsync(User user)
        {
            await _UserRepo.InsertUserEntityAsync(user);
        }

        public async Task<int> ModifyUserEntityAsync(User user)
        {
            return await _UserRepo.UpdateUserEntityAsync(user);
        }

        public async Task<PagedQueryResult<User>> PagedQueryDemoAsync(string name, int pageIndex, int pageSize)
        {
            return await _UserRepo.PagedQueryAsync(name, pageIndex, pageSize);
        }
    }
}
