using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using XDbAccess.Common;
using XDbAccess.Dapper;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;

namespace XDbAccess.Demo.Repositories
{
    public class UserPostgreSQLRepository : BaseRepository<DapperTestDbContext>, IUserRepository
    {
        public UserPostgreSQLRepository(IDbHelper<DapperTestDbContext> dbHelper) : base(dbHelper) { }

        public async Task InsertUserAsync(User user)
        {
            string sql = $@"
                insert into ""User""(""Name"",""Birthday"",""Description"",""OrgId"") values(@Name,@Birthday,@Description,@OrgId) RETURNING CAST(""Id"" AS BIGINT);
            ";
            user.Id = Convert.ToInt32(await DbHelper.ExecuteScalarAsync(sql, new { Name = user.Name, Birthday = user.Birthday, Description = user.Description, OrgId = user.OrgId }));
        }

        public async Task<List<User>> AllUsersAsync(string name)
        {
            //IEnumerable<User> data;
            //string sql = "select * from `user` where 1=1";
            //if (!string.IsNullOrWhiteSpace(name))
            //{
            //    sql += " and Name like @Name";
            //}
            //data = await DbHelper.QueryAsync<User>(sql, new { Name = "%" + name + "%" });
            //return data.ToList();

            var data = await DbHelper.QuerySingleTableAsync<User>("\"Name\" like @Name", new { Name = "%" + name + "%" });
            return data.ToList();
        }

        public async Task<User> GetUserAsync(int id)
        {
            string sql = "select * from \"User\" where \"Id\"=@Id";
            return await DbHelper.QuerySingleAsync<User>(sql, new { Id = id });
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            string sql = $@"
                update ""User"" set ""Name""=@Name,""Birthday""=@Birthday,""Description""=@Description
                where ""Id""=@Id
            ";
            return await DbHelper.ExecuteAsync(sql, new
            {
                Name = user.Name,
                Birthday = Convert.ToDateTime(user.Birthday.ToString("yyyy-MM-dd")),
                Description = user.Description,
                Id = user.Id
            });
        }

        public Task<int> DeleteUserAsync(int id)
        {
            //string sql = "delete from `user` where Id=@Id";
            //return await DbHelper.ExecuteAsync(sql, new { Id = id });
            return DbHelper.DeleteAsync<User>(new { Id = id });
        }

        public async Task InsertUserEntityAsync(User user)
        {
            user.Id = Convert.ToInt32(await DbHelper.InsertAsync<User>(user));
        }

        public async Task<int> UpdateUserEntityAsync(User user)
        {
            return await DbHelper.UpdateAsync<User>(user);
        }

        public async Task<PagedQueryResult<User>> PagedQueryAsync(string name, int pageIndex, int pageSize)
        {
            PagedQueryOptions opt = new PagedQueryOptions()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SqlFieldsPart = "\"Id\",\"Name\",\"Birthday\",\"Description\"",
                SqlFromPart = "\"User\"",
                SqlConditionPart = "\"Name\" like @Name",
                SqlOrderPart = "\"Birthday\" desc,\"Id\""
            };

            return await DbHelper.PagedQueryAsync<User>(opt, new { Name = "%" + name + "%" });
        }
    }
}
