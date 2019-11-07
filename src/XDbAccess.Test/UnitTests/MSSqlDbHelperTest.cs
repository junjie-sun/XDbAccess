using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;
using XDbAccess.Common;
using XDbAccess.Dapper;
using XDbAccess.MSSql;
using Xunit;

namespace XDbAccess.Test.UnitTests
{
    public class MSSqlDbHelperTest
    {
        public MSSqlDbHelperTest()
        {
            var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DapperTemp;Trusted_Connection=True;MultipleActiveResultSets=true";
            DbContext = new DbContext(new DbContextOptions()
            {
                ConnectionString = connectionString,
                DbFactory = new MSSqlDbFactory(connectionString)
            });
            DbHelper = new MSSqlDbHelper<DbContext>(DbContext);

            DbHelper.Execute("truncate table [user]; truncate table [org];");
        }

        public DbContext DbContext { get; }

        public MSSqlDbHelper<DbContext> DbHelper { get; }

        #region Execute

        [Fact]
        public void ExecuteSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            var r = DbHelper.Execute(sql, new { Name = "ExecuteSqlOrg" });
            Assert.Equal(1, r);
        }

        [Fact]
        public async void ExecuteAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            var r = await DbHelper.ExecuteAsync(sql, new { Name = "ExecuteAsyncSqlOrg" });
            Assert.Equal(1, r);
        }

        [Fact]
        public void ExecuteSpTest()
        {
            var sp = "ExecuteSqlTestSP";
            var r = DbHelper.Execute(sp, new { Name = "ExecuteSpOrg" }, null, System.Data.CommandType.StoredProcedure);
            Assert.Equal(1, r);
        }

        [Fact]
        public async void ExecuteAsyncSpTest()
        {
            var sp = "ExecuteSqlTestSP";
            var r = await DbHelper.ExecuteAsync(sp, new { Name = "ExecuteAsyncSpOrg" }, null, System.Data.CommandType.StoredProcedure);
            Assert.Equal(1, r);
        }

        #endregion

        #region Reader

        [Fact]
        public void ExecuteReaderSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "ExecuteReaderSql" });

            sql = "select * from [org]";
            string orgName = null;
            using (var reader = DbHelper.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    orgName = reader["Name"].ToString();
                }
            }

            Assert.Equal("ExecuteReaderSql", orgName);
        }

        [Fact]
        public async void ExecuteReaderAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "ExecuteReaderAsyncSql" });

            sql = "select * from [org]";
            string orgName = null;
            using (var reader = await DbHelper.ExecuteReaderAsync(sql))
            {
                if (reader.Read())
                {
                    orgName = reader["Name"].ToString();
                }
            }

            Assert.Equal("ExecuteReaderAsyncSql", orgName);
        }

        #endregion

        #region ExecuteScalar

        [Fact]
        public void ExecuteScalarSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "ExecuteScalarSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarSqlOrg3" });

            sql = "select count(1) from [org]";
            var r = Convert.ToInt32(DbHelper.ExecuteScalar(sql));
            Assert.Equal(3, r);
        }

        [Fact]
        public async void ExecuteScalarAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "ExecuteScalarAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarAsyncSqlOrg3" });

            sql = "select count(1) from [org]";
            var r = Convert.ToInt32(await DbHelper.ExecuteScalarAsync(sql));
            Assert.Equal(3, r);
        }

        [Fact]
        public void ExecuteScalarGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "ExecuteScalarGenericSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarGenericSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarGenericSqlOrg3" });

            sql = "select count(1) from [org]";
            var r = DbHelper.ExecuteScalar<int>(sql);
            Assert.Equal(3, r);
        }

        [Fact]
        public async void ExecuteScalarAsyncGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "ExecuteScalarAsyncGenericSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarAsyncGenericSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "ExecuteScalarAsyncGenericSqlOrg3" });

            sql = "select count(1) from [org]";
            var r = await DbHelper.ExecuteScalarAsync<int>(sql);
            Assert.Equal(3, r);
        }

        #endregion

        #region Query

        [Fact]
        public void QuerySqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = DbHelper.Query(sql, new { Id = 1 });
            Assert.Equal(2, r.Count());
            Assert.Single(r.Where(d => d.Id == 2));
            Assert.Single(r.Where(d => d.Id == 3));
        }

        [Fact]
        public async void QueryAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryAsyncSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = await DbHelper.QueryAsync(sql, new { Id = 1 });
            Assert.Equal(2, r.Count());
            Assert.Single(r.Where(d => d.Id == 2));
            Assert.Single(r.Where(d => d.Id == 3));
        }

        [Fact]
        public void QueryGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryGenericSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryGenericSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryGenericSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = DbHelper.Query<Org>(sql, new { Id = 1 });
            Assert.Equal(2, r.Count());
            Assert.Single(r.Where(d => d.Id == 2));
            Assert.Single(r.Where(d => d.Id == 3));
        }

        [Fact]
        public async void QueryGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryGenericAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryGenericAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryGenericAsyncSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = await DbHelper.QueryAsync<Org>(sql, new { Id = 1 });
            Assert.Equal(2, r.Count());
            Assert.Single(r.Where(d => d.Id == 2));
            Assert.Single(r.Where(d => d.Id == 3));
        }

        #endregion

        #region QueryFirst

        [Fact]
        public void QueryFirstSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryFirstSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryFirstSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryFirstSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = DbHelper.QueryFirst(sql, new { Id = 1 });
            Assert.Equal(2, r.Id);
        }

        [Fact]
        public async void QueryFirstAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryFirstAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryFirstAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryFirstAsyncSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = await DbHelper.QueryFirstAsync(typeof(Org), sql, new { Id = 1 });
            Assert.Equal(2, ((Org)r).Id);
        }

        [Fact]
        public void QueryFirstGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryFirstGenericSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryFirstGenericSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryFirstGenericSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = DbHelper.QueryFirst<Org>(sql, new { Id = 1 });
            Assert.Equal(2, r.Id);
        }

        [Fact]
        public async void QueryFirstGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryFirstGenericAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryFirstGenericAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryFirstGenericAsyncSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = await DbHelper.QueryFirstAsync<Org>(sql, new { Id = 1 });
            Assert.Equal(2, r.Id);
        }

        #endregion

        #region QueryFirstOrDefault

        [Fact]
        public void QueryFirstOrDefaultSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = DbHelper.QueryFirstOrDefault(sql, new { Id = 1 });
            Assert.Equal(2, r.Id);
        }

        [Fact]
        public void QueryFirstOrDefaultGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultGenericSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultGenericSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultGenericSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = DbHelper.QueryFirstOrDefault<Org>(sql, new { Id = 1 });
            Assert.Equal(2, r.Id);
        }

        [Fact]
        public async void QueryFirstOrDefaultGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultGenericAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultGenericAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QueryFirstOrDefaultGenericAsyncSqlOrg3" });

            sql = "select * from [org] where Id>@Id";
            var r = await DbHelper.QueryFirstOrDefaultAsync<Org>(sql, new { Id = 1 });
            Assert.Equal(2, r.Id);
        }

        #endregion

        #region QueryMultiple

        [Fact]
        public void QueryMultipleSqlTest()
        {
            var sql = @"
                insert into [org](Name) values(@Name);
                SELECT CAST(SCOPE_IDENTITY() AS bigint) as Id;
            ";
            var orgId = DbHelper.ExecuteScalar<int>(sql, new { Name = "QueryMultipleOrg" });

            sql = @"
                insert into [user](Name,Birthday,Description,OrgId)
                    values(@Name,@Birthday,@Description,@OrgId);";
            DbHelper.Execute(sql, new { Name = "QueryMultipleUser", Birthday = new DateTime(1986, 8, 7), Description = "Test", OrgId = orgId });

            sql = @"
                select * from [org];
                select * from [user];
            ";
            using (var gridReaderWrap = DbHelper.QueryMultiple(sql, new { Id = 1 }))
            {
                var orgList = gridReaderWrap.GridReader.Read<Org>();
                var userList = gridReaderWrap.GridReader.Read<User>();
                Assert.Single(orgList);
                Assert.Single(userList);
            }
        }

        [Fact]
        public async void QueryMultipleAsyncSqlTest()
        {
            var sql = @"
                insert into [org](Name) values(@Name);
                SELECT CAST(SCOPE_IDENTITY() AS bigint) as Id;
            ";
            var orgId = DbHelper.ExecuteScalar<int>(sql, new { Name = "QueryMultipleAsyncOrg" });

            sql = @"
                insert into [user](Name,Birthday,Description,OrgId)
                    values(@Name,@Birthday,@Description,@OrgId);";
            DbHelper.Execute(sql, new { Name = "QueryMultipleAsyncUser", Birthday = new DateTime(1986, 8, 7), Description = "Test", OrgId = orgId });

            sql = @"
                select * from [org];
                select * from [user];
            ";
            using (var gridReaderWrap = await DbHelper.QueryMultipleAsync(sql, new { Id = 1 }))
            {
                var orgList = await gridReaderWrap.GridReader.ReadAsync<Org>();
                var userList = await gridReaderWrap.GridReader.ReadAsync<User>();
                Assert.Single(orgList);
                Assert.Single(userList);
            }
        }

        #endregion

        #region QuerySingle

        [Fact]
        public void QuerySingleSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySingleSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySingleSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySingleSqlOrg3" });

            sql = "select * from [org] where Id=@Id";
            var r = DbHelper.QuerySingle(sql, new { Id = 3 });
            Assert.Equal(3, r.Id);
        }

        [Fact]
        public void QuerySingleGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySingleGenericSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySingleGenericSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySingleGenericSqlOrg3" });

            sql = "select * from [org] where Id=@Id";
            var r = DbHelper.QuerySingle<Org>(sql, new { Id = 3 });
            Assert.Equal(3, r.Id);
        }

        [Fact]
        public async void QuerySingleGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySingleGenericAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySingleGenericAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySingleGenericAsyncSqlOrg3" });

            sql = "select * from [org] where Id=@Id";
            var r = await DbHelper.QuerySingleAsync<Org>(sql, new { Id = 3 });
            Assert.Equal(3, r.Id);
        }

        #endregion

        #region QuerySingleOrDefault

        [Fact]
        public void QuerySingleOrDefaultSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultSqlOrg3" });

            sql = "select * from [org] where Id=@Id";
            var r = DbHelper.QuerySingleOrDefault(sql, new { Id = 3 });
            Assert.Equal(3, r.Id);
        }

        [Fact]
        public void QuerySingleOrDefaultGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultGenericSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultGenericSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultGenericSqlOrg3" });

            sql = "select * from [org] where Id=@Id";
            var r = DbHelper.QuerySingleOrDefault<Org>(sql, new { Id = 3 });
            Assert.Equal(3, r.Id);
        }

        [Fact]
        public async void QuerySingleOrDefaultAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultAsyncSqlOrg3" });

            sql = "select * from [org] where Id=@Id";
            var r = await DbHelper.QuerySingleOrDefaultAsync(typeof(Org), sql, new { Id = 3 });
            Assert.Equal(3, ((Org)r).Id);
        }

        [Fact]
        public async void QuerySingleOrDefaultGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultGenericAsyncSqlOrg1" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultGenericAsyncSqlOrg2" });
            DbHelper.Execute(sql, new { Name = "QuerySingleOrDefaultGenericAsyncSqlOrg3" });

            sql = "select * from [org] where Id=@Id";
            var r = await DbHelper.QuerySingleOrDefaultAsync<Org>(sql, new { Id = 3 });
            Assert.Equal(3, r.Id);
        }

        #endregion

        #region Insert

        [Fact]
        public void InsertTest()
        {
            var org = new Org()
            {
                Name = "InsertOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "InsertUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);
        }

        [Fact]
        public async void InsertAsyncTest()
        {
            var org = new Org()
            {
                Name = "InsertAsyncOrg"
            };

            org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "InsertAsyncUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(await DbHelper.InsertAsync<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);
        }

        [Fact]
        public void BatchInsertTest()
        {
            var org = new Org()
            {
                Name = "InsertOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var list = new List<User>();
            list.Add(new User()
            {
                Name = "InsertUser1",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test1",
                OrgId = org.Id
            });
            list.Add(new User()
            {
                Name = "InsertUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test2",
                OrgId = org.Id
            });
            var count = DbHelper.BatchInsert<User>(list);
            Assert.Equal(2, count);

            var sql = "select * from [user]";
            var queryResult = DbHelper.Query<User>(sql);
            Assert.Equal(2, queryResult.Count());
        }

        [Fact]
        public async Task BatchInsertAsyncTest()
        {
            var org = new Org()
            {
                Name = "InsertAsyncOrg"
            };

            org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));
            Assert.Equal(1, org.Id);

            var list = new List<User>();
            list.Add(new User()
            {
                Name = "InsertAsyncUser1",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test1",
                OrgId = org.Id
            });
            list.Add(new User()
            {
                Name = "InsertAsyncUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test2",
                OrgId = org.Id
            });
            var count = await DbHelper.BatchInsertAsync<User>(list);
            Assert.Equal(2, count);

            var sql = "select * from [user]";
            var queryResult = await DbHelper.QueryAsync<User>(sql);
            Assert.Equal(2, queryResult.Count());
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateByPrimaryKeyTest()
        {
            var org = new Org()
            {
                Name = "UpdateOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            user.Description = "UpdateTest";
            var r =DbHelper.Update<User>(user);
            Assert.Equal(1, r);

            var sql = "select * from [user] where Id=@Id";
            var queryResult = DbHelper.QueryFirst<User>(sql, new { Id = user.Id });
            Assert.Equal("UpdateTest", queryResult.Description);

            var sql2 = "select * from [user] where Id=@Id";
            var queryResult2 = DbHelper.QueryFirst<User>(sql2, new { Id = user2.Id });
            Assert.Equal("Test", queryResult2.Description);
        }

        [Fact]
        public async void UpdateByPrimaryKeyAsyncTest()
        {
            var org = new Org()
            {
                Name = "UpdateAsyncOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateAsyncUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            user.Description = "UpdateAsyncTest";
            var r = await DbHelper.UpdateAsync<User>(user);
            Assert.Equal(1, r);

            var sql = "select * from [user] where Id=@Id";
            var queryResult = DbHelper.QueryFirst<User>(sql, new { Id = user.Id });
            Assert.Equal("UpdateAsyncTest", queryResult.Description);

            var sql2 = "select * from [user] where Id=@Id";
            var queryResult2 = DbHelper.QueryFirst<User>(sql2, new { Id = user2.Id });
            Assert.Equal("Test", queryResult2.Description);
        }

        [Fact]
        public void UpdateByConditionTest()
        {
            var org = new Org()
            {
                Name = "UpdateOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test2",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            user2.Description = "UpdateTest";
            var r = DbHelper.Update<User>(user2, false, "CAST([Description] AS VARCHAR)=@Description", new { Description = "Test2" });
            Assert.Equal(1, r);

            var sql = "select * from [user] where Id=@Id";
            var queryResult = DbHelper.QueryFirst<User>(sql, new { Id = user.Id });
            Assert.Equal("Test", queryResult.Description);

            var sql2 = "select * from [user] where Id=@Id";
            var queryResult2 = DbHelper.QueryFirst<User>(sql2, new { Id = user2.Id });
            Assert.Equal("UpdateTest", queryResult2.Description);
        }

        [Fact]
        public async void UpdateByCondtionAsyncTest()
        {
            var org = new Org()
            {
                Name = "UpdateAsyncOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateAsyncUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateAsyncUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test2",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            user2.Description = "UpdateAsyncTest";
            var r = await DbHelper.UpdateAsync<User>(user2, false, "CAST([Description] AS VARCHAR)=@Description", new { Description = "Test2" });
            Assert.Equal(1, r);

            var sql = "select * from [user] where Id=@Id";
            var queryResult = DbHelper.QueryFirst<User>(sql, new { Id = user.Id });
            Assert.Equal("Test", queryResult.Description);

            var sql2 = "select * from [user] where Id=@Id";
            var queryResult2 = DbHelper.QueryFirst<User>(sql2, new { Id = user2.Id });
            Assert.Equal("UpdateAsyncTest", queryResult2.Description);
        }

        [Fact]
        public void UpdateAllTest()
        {
            var org = new Org()
            {
                Name = "UpdateOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            Exception result = null;
            user.Description = "UpdateTest";
            try
            {
                DbHelper.Update<User>(user, false);
            }
            catch (InvalidOperationException ex)
            {
                result = ex;
            }
            Assert.NotNull(result);
        }

        [Fact]
        public async void UpdateAllAsyncTest()
        {
            var org = new Org()
            {
                Name = "UpdateOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            Exception result = null;
            user.Description = "UpdateTest";
            try
            {
                await DbHelper.UpdateAsync<User>(user, false);
            }
            catch (InvalidOperationException ex)
            {
                result = ex;
            }
            Assert.NotNull(result);
        }

        [Fact]
        public void BatchUpdateByPrimaryKeyTest()
        {
            var org = new Org()
            {
                Name = "UpdateOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            user.Description = "UpdateTest";
            user2.Description = "UpdateTest2";
            var r = DbHelper.BatchUpdate<User>(new List<User>() { user, user2 });
            Assert.Equal(2, r);

            var sql = "select * from [user] where Id=@Id";
            var queryResult = DbHelper.QueryFirst<User>(sql, new { Id = user.Id });
            Assert.Equal("UpdateTest", queryResult.Description);

            var sql2 = "select * from [user] where Id=@Id";
            var queryResult2 = DbHelper.QueryFirst<User>(sql2, new { Id = user2.Id });
            Assert.Equal("UpdateTest2", queryResult2.Description);
        }

        [Fact]
        public async Task BatchUpdateByPrimaryKeyAsyncTest()
        {
            var org = new Org()
            {
                Name = "UpdateOrg"
            };

            org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "UpdateUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(await DbHelper.InsertAsync<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "UpdateUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(await DbHelper.InsertAsync<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            user.Description = "UpdateTest";
            user2.Description = "UpdateTest2";
            var r = await DbHelper.BatchUpdateAsync<User>(new List<User>() { user, user2 });
            Assert.Equal(2, r);

            var sql = "select * from [user] where Id=@Id";
            var queryResult = await DbHelper.QueryFirstAsync<User>(sql, new { Id = user.Id });
            Assert.Equal("UpdateTest", queryResult.Description);

            var sql2 = "select * from [user] where Id=@Id";
            var queryResult2 = await DbHelper.QueryFirstAsync<User>(sql2, new { Id = user2.Id });
            Assert.Equal("UpdateTest2", queryResult2.Description);
        }

        #endregion

        #region Delete

        [Fact]
        public void DeleteByPrimaryKeyTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            var sql1 = "select count(1) from [user]";
            var result1 = DbHelper.ExecuteScalar<long>(sql1);
            Assert.Equal(2, result1);

            DbHelper.Delete<User>(new { Id = 1 });

            var sql2 = "select * from [user]";
            var result2 = DbHelper.QuerySingle<User>(sql2);
            Assert.Equal(2, result2.Id);
        }

        [Fact]
        public async void DeleteByPrimaryKeyAsyncTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            var sql1 = "select count(1) from [user]";
            var result1 = DbHelper.ExecuteScalar<long>(sql1);
            Assert.Equal(2, result1);

            await DbHelper.DeleteAsync<User>(new { Id = 1 });

            var sql2 = "select * from [user]";
            var result2 = DbHelper.QuerySingle<User>(sql2);
            Assert.Equal(2, result2.Id);
        }

        [Fact]
        public void DeleteByConditionTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            var sql1 = "select count(1) from [user]";
            var result1 = DbHelper.ExecuteScalar<long>(sql1);
            Assert.Equal(2, result1);

            DbHelper.Delete<User>(new { Name = "DeleteUser2" }, false, "[Name]=@Name");

            var sql2 = "select * from [user]";
            var result2 = DbHelper.QuerySingle<User>(sql2);
            Assert.Equal(1, result2.Id);
        }

        [Fact]
        public async void DeleteByConditionAsyncTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            var sql1 = "select count(1) from [user]";
            var result1 = DbHelper.ExecuteScalar<long>(sql1);
            Assert.Equal(2, result1);

            await DbHelper.DeleteAsync<User>(new { Name = "DeleteUser2" }, false, "[Name]=@Name");

            var sql2 = "select * from [user]";
            var result2 = DbHelper.QuerySingle<User>(sql2);
            Assert.Equal(1, result2.Id);
        }

        [Fact]
        public void DeleteAllTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            Exception result = null;
            try
            {
                DbHelper.Delete<User>(null, false);
            }
            catch (InvalidOperationException ex)
            {
                result = ex;
            }
            Assert.NotNull(result);
        }

        [Fact]
        public async void DeleteAllAsyncTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            Exception result = null;
            try
            {
                await DbHelper.DeleteAsync<User>(null, false);
            }
            catch (InvalidOperationException ex)
            {
                result = ex;
            }
            Assert.NotNull(result);
        }

        [Fact]
        public void BatchDeleteByPrimaryKeyTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(DbHelper.Insert<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            var sql1 = "select count(1) from [user]";
            var result1 = DbHelper.ExecuteScalar<long>(sql1);
            Assert.Equal(2, result1);

            DbHelper.BatchDelete<User>(new List<object>() { new { Id = 1 }, new { Id = 2 } });

            var sql2 = "select count(1) from [user]";
            var result2 = DbHelper.ExecuteScalar<long>(sql2);
            Assert.Equal(0, result2);
        }

        [Fact]
        public async Task BatchDeleteByPrimaryKeyAsyncTest()
        {
            var org = new Org()
            {
                Name = "DeleteOrg"
            };

            org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));
            Assert.Equal(1, org.Id);

            var user = new User()
            {
                Name = "DeleteUser",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user.Id = Convert.ToInt32(await DbHelper.InsertAsync<User>(user));
            Assert.Equal(1, user.Id);
            Assert.Equal(1, user.OrgId);

            var user2 = new User()
            {
                Name = "DeleteUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = org.Id
            };
            user2.Id = Convert.ToInt32(await DbHelper.InsertAsync<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(1, user2.OrgId);

            var sql1 = "select count(1) from [user]";
            var result1 = await DbHelper.ExecuteScalarAsync<long>(sql1);
            Assert.Equal(2, result1);

            await DbHelper.BatchDeleteAsync<User>(new List<object>() { new { Id = 1 }, new { Id = 2 } });

            var sql2 = "select count(1) from [user]";
            var result2 = await DbHelper.ExecuteScalarAsync<long>(sql2);
            Assert.Equal(0, result2);
        }

        #endregion

        #region PagedQuery

        [Fact]
        public void PagedQueryTest()
        {
            var org = new Org()
            {
                Name = "PagedQueryOrg"
            };
            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

            var sql = @"
                insert into [user](Name,Birthday,Description,OrgId)
                    values(@Name,@Birthday,@Description,@OrgId)
            ";

            for (var i = 1; i <= 20; i++)
            {
                DbHelper.Execute(sql, new
                {
                    Name = "PagedQueryUser" + i,
                    Birthday = new DateTime(1990, 1, i),
                    Description = "TestUser" + i,
                    OrgId = org.Id
                });
            }

            var result = DbHelper.PagedQuery<OrgUser>(new PagedQueryOptions()
            {
                PageIndex = 0,
                PageSize = 3,
                SqlFromPart = "[org] o join [user] u on o.Id=u.OrgId",
                SqlFieldsPart = "u.*,o.Name OrgName",
                SqlConditionPart = "u.Id > @UserId",
                SqlOrderPart = "u.Id"
            }, new { UserId = 3 });
            Assert.Equal(17, result.Total);
            var user = result.Data[1];
            Assert.Equal("PagedQueryUser5", user.Name);
            Assert.Equal("PagedQueryOrg", user.OrgName);

            //测试第2页
            result = DbHelper.PagedQuery<OrgUser>(new PagedQueryOptions()
            {
                PageIndex = 1,
                PageSize = 3,
                SqlFromPart = "[org] o join [user] u on o.Id=u.OrgId",
                SqlFieldsPart = "u.*,o.Name OrgName",
                SqlConditionPart = "u.Id > @UserId",
                SqlOrderPart = "u.Id"
            }, new { UserId = 3 });
            Assert.Equal(0, result.Total);
            user = result.Data[1];
            Assert.Equal("PagedQueryUser8", user.Name);
            Assert.Equal("PagedQueryOrg", user.OrgName);

            //测试第2页及AlwayQueryCount
            result = DbHelper.PagedQuery<OrgUser>(new PagedQueryOptions()
            {
                PageIndex = 1,
                PageSize = 3,
                SqlFromPart = "[org] o join [user] u on o.Id=u.OrgId",
                SqlFieldsPart = "u.*,o.Name OrgName",
                SqlConditionPart = "u.Id > @UserId",
                SqlOrderPart = "u.Id",
                AlwayQueryCount = true
            }, new { UserId = 3 });
            Assert.Equal(17, result.Total);
            user = result.Data[1];
            Assert.Equal("PagedQueryUser8", user.Name);
            Assert.Equal("PagedQueryOrg", user.OrgName);
        }

        [Fact]
        public async void PagedQueryAsyncTest()
        {
            var org = new Org()
            {
                Name = "PagedQueryAsyncOrg"
            };
            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

            var sql = @"
                insert into [user](Name,Birthday,Description,OrgId)
                    values(@Name,@Birthday,@Description,@OrgId)
            ";

            for (var i = 1; i <= 20; i++)
            {
                DbHelper.Execute(sql, new
                {
                    Name = "PagedQueryAsyncUser" + i,
                    Birthday = new DateTime(1990, 1, i),
                    Description = "TestUser" + i,
                    OrgId = org.Id
                });
            }

            var result = await DbHelper.PagedQueryAsync<OrgUser>(new PagedQueryOptions()
            {
                PageIndex = 0,
                PageSize = 3,
                SqlFromPart = "[org] o join [user] u on o.Id=u.OrgId",
                SqlFieldsPart = "u.*,o.Name OrgName",
                SqlConditionPart = "u.Id > @UserId",
                SqlOrderPart = "u.Id"
            }, new { UserId = 3 });
            Assert.Equal(17, result.Total);
            var user = result.Data[1];
            Assert.Equal("PagedQueryAsyncUser5", user.Name);
            Assert.Equal("PagedQueryAsyncOrg", user.OrgName);

            //测试第2页
            result = await DbHelper.PagedQueryAsync<OrgUser>(new PagedQueryOptions()
            {
                PageIndex = 1,
                PageSize = 3,
                SqlFromPart = "[org] o join [user] u on o.Id=u.OrgId",
                SqlFieldsPart = "u.*,o.Name OrgName",
                SqlConditionPart = "u.Id > @UserId",
                SqlOrderPart = "u.Id"
            }, new { UserId = 3 });
            Assert.Equal(0, result.Total);
            user = result.Data[1];
            Assert.Equal("PagedQueryAsyncUser8", user.Name);
            Assert.Equal("PagedQueryAsyncOrg", user.OrgName);

            //测试第2页及AlwayQueryCount
            result = await DbHelper.PagedQueryAsync<OrgUser>(new PagedQueryOptions()
            {
                PageIndex = 1,
                PageSize = 3,
                SqlFromPart = "[org] o join [user] u on o.Id=u.OrgId",
                SqlFieldsPart = "u.*,o.Name OrgName",
                SqlConditionPart = "u.Id > @UserId",
                SqlOrderPart = "u.Id",
                AlwayQueryCount = true
            }, new { UserId = 3 });
            Assert.Equal(17, result.Total);
            user = result.Data[1];
            Assert.Equal("PagedQueryAsyncUser8", user.Name);
            Assert.Equal("PagedQueryAsyncOrg", user.OrgName);
        }

        #endregion

        #region Trans

        [Fact]
        public void TransCommitTest()
        {
            string sql = "";
            using (var scope = DbContext.TransScope())
            {
                var org = new Org()
                {
                    Name = "TransCommitOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransCommitUser",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Single(userList);
        }

        [Fact]
        public async void TransCommitAsyncTest()
        {
            string sql = "";
            using (var scope = DbContext.TransScope())
            {
                var org = new Org()
                {
                    Name = "TransCommitAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransCommitAsyncUser",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Single(userList);
        }

        [Fact]
        public void TransRollbackTest()
        {
            string sql = "";
            using (var scope = DbContext.TransScope())
            {
                var org = new Org()
                {
                    Name = "TransRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransRollbackUser",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Empty(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransRollbackAsyncTest()
        {
            string sql = "";
            using (var scope = DbContext.TransScope())
            {
                var org = new Org()
                {
                    Name = "TransRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransRollbackAsyncUser",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Empty(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransCommitRollbackTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransCommitRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransCommitRollbackUser",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransCommitRollbackAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransCommitRollbackAsyncUser",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransNestCommitTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransNestCommitUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransNestCommitUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Equal(2, userList.Count());
        }

        [Fact]
        public async void TransNestCommitAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransNestCommitAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransNestCommitAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Equal(2, userList.Count());
        }

        [Fact]
        public void TransNestCommitTest2()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransNestCommitUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransNestCommitUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Equal(2, userList.Count());
        }

        [Fact]
        public async void TransNestCommitAsyncTest2()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransNestCommitAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransNestCommitAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Equal(2, userList.Count());
        }

        [Fact]
        public void TransNestCommitRollbackTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransNestCommitRollbackUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Rollback();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransNestCommitRollbackUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransNestCommitRollbackAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransNestCommitRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Rollback();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransNestCommitRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransNestCommitRollbackTest2()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransNestCommitRollbackUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransNestCommitRollbackUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransNestCommitRollbackAsyncTest2()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransNestCommitRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransNestCommitRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransNestCommitRollbackTest3()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransNestCommitRollbackUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransNestCommitRollbackUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Rollback();
                }

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransNestCommitRollbackAsyncTest3()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransNestCommitRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransNestCommitRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Rollback();
                }

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransNestCommitRollbackTest4()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransNestCommitRollbackUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransNestCommitRollbackUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransNestCommitRollbackAsyncTest4()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransNestCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransNestCommitRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransNestCommitRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransTwoNestCommitTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestCommitOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestCommitUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestCommitUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransTwoNestCommitUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Equal(3, userList.Count());
        }

        [Fact]
        public async void TransTwoNestCommitAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestCommitAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestCommitAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestCommitAsyncUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransTwoNestCommitAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Equal(3, userList.Count());
        }

        [Fact]
        public void TransTwoNestCommitRollbackTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestCommitRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransTwoNestCommitRollbackUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransTwoNestCommitRollbackAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackAsyncUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransTwoNestCommitRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransTwoNestCommitRollbackTest2()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestCommitRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Rollback();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransTwoNestCommitRollbackUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransTwoNestCommitRollbackAsyncTest2()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestCommitRollbackAsyncUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Rollback();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransTwoNestCommitRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public void TransTwoNestRollbackTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestRollbackOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestRollbackUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Rollback();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    DbHelper.Execute(sql, new
                    {
                        Name = "TransTwoNestRollbackUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Rollback();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                DbHelper.Execute(sql, new
                {
                    Name = "TransTwoNestRollbackUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = DbHelper.Query<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void TransTwoNestRollbackAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoNestRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Rollback();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "TransTwoNestRollbackAsyncUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Rollback();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "TransTwoNestRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Empty(userList);
        }

        [Fact]
        public async void RequireNewCommitAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "RequireNewCommitAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitAsyncUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Commit();
                }
                using (var scope4 = DbContext.TransScope(TransScopeOption.RequireNew))
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitAsyncUser4",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope4.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "RequireNewCommitAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Equal(4, userList.Count());
        }

        [Fact]
        public async void RequireNewCommitRollbackAsyncTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "RequireNewCommitRollbackAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitRollbackAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitRollbackAsyncUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Commit();
                }
                using (var scope4 = DbContext.TransScope(TransScopeOption.RequireNew))
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitRollbackAsyncUser4",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope4.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "RequireNewCommitRollbackAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Rollback();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Single(userList);
        }

        [Fact]
        public async void RequireNewCommitRollbackAsyncTest2()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "RequireNewCommitAsyncOrg"
                };
                org.Id = Convert.ToInt32(await DbHelper.InsertAsync<Org>(org));

                scope.Commit();
            }

            using (var scope = DbContext.TransScope())
            {
                using (var scope2 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitAsyncUser1",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope2.Commit();
                }
                using (var scope3 = DbContext.TransScope())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitAsyncUser3",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    scope3.Commit();
                }
                using (var scope4 = DbContext.TransScope(TransScopeOption.RequireNew))
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    await DbHelper.ExecuteAsync(sql, new
                    {
                        Name = "RequireNewCommitAsyncUser4",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    });

                    using (var scope5 = DbContext.TransScope())
                    {
                        sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                        ";

                        await DbHelper.ExecuteAsync(sql, new
                        {
                            Name = "RequireNewCommitAsyncUser5",
                            Birthday = new DateTime(1991, 2, 1),
                            Description = "TestUser",
                            OrgId = org.Id
                        });

                        scope5.Rollback();
                    }

                    scope4.Commit();
                }
                sql = @"
                    insert into [user](Name,Birthday,Description,OrgId)
                        values(@Name,@Birthday,@Description,@OrgId)
                ";

                await DbHelper.ExecuteAsync(sql, new
                {
                    Name = "RequireNewCommitAsyncUser2",
                    Birthday = new DateTime(1991, 2, 1),
                    Description = "TestUser",
                    OrgId = org.Id
                });

                scope.Commit();
            }

            sql = "select * from [org]";
            var orgList = await DbHelper.QueryAsync<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user]";
            var userList = await DbHelper.QueryAsync<User>(sql);
            Assert.Equal(3, userList.Count());
        }

        [Fact]
        public void TransTwoTaskTest()
        {
            string sql = "";
            Org org = null;
            using (var scope = DbContext.TransScope())
            {
                org = new Org()
                {
                    Name = "TransTwoTaskOrg"
                };
                org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));

                scope.Commit();
            }

            Task.WaitAll(TransTwoTask1(org), TransTwoTask2(org));

            sql = "select * from [org]";
            var orgList = DbHelper.Query<Org>(sql);
            Assert.Single(orgList);

            sql = "select * from [user] where Name=@Name";
            var userList = DbHelper.Query<User>(sql, new { Name = "TransTwoTask1" });
            Assert.Equal(30, userList.Count());

            sql = "select * from [user] where Name=@Name";
            var userList2 = DbHelper.Query<User>(sql, new { Name = "TransTwoTask2" });
            Assert.Empty(userList2);
        }

        private Task TransTwoTask1(Org org)
        {
            return Task.Run(async () =>
            {
                using (var scope = DbContext.TransScope())
                {
                    var sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    for (var i = 0; i < 30; i++)
                    {
                        await DbHelper.ExecuteAsync(sql, new
                        {
                            Name = "TransTwoTask1",
                            Birthday = new DateTime(1991, 2, 1),
                            Description = "TestUser",
                            OrgId = org.Id
                        });
                    }

                    scope.Commit();
                }
            });
        }

        private Task TransTwoTask2(Org org)
        {
            return Task.Run(async () =>
            {
                using (var scope = DbContext.TransScope())
                {
                    var sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";

                    for (var i = 0; i < 30; i++)
                    {
                        await DbHelper.ExecuteAsync(sql, new
                        {
                            Name = "TransTwoTask2",
                            Birthday = new DateTime(1991, 2, 1),
                            Description = "TestUser",
                            OrgId = org.Id
                        });
                    }

                    scope.Rollback();
                }
            });
        }

        #endregion

        #region QuerySingleTable

        [Fact]
        public void QuerySingleTableTest()
        {
            var org = new Org()
            {
                Name = "QuerySingleTableOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user1 = new User()
            {
                Name = "QuerySingleTableUser1",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = 1
            };
            user1.Id = Convert.ToInt32(DbHelper.Insert<User>(user1));
            Assert.Equal(1, user1.Id);
            Assert.Equal(1, user1.OrgId);

            var user2 = new User()
            {
                Name = "QuerySingleTableUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = 2
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(2, user2.OrgId);

            var result1 = DbHelper.QuerySingleTable<User>();
            Assert.Equal(2, result1.Count());

            var condition2 = new
            {
                Id = 2
            };
            var result2 = DbHelper.QuerySingleTable<User>("Id=@Id", condition2).Single();
            Assert.Equal(2, result2.Id);

            var condition3 = new
            {
                OrgId = 2
            };
            var result3 = DbHelper.QuerySingleTable<User>("OrgId=@OrgId", condition3).Single();
            Assert.Equal(2, result2.Id);

            var result4 = DbHelper.QuerySingleTable<User>(null, null, "[Name] DESC").ToArray();
            Assert.Equal("QuerySingleTableUser2", result4[0].Name);

            var condition5 = new
            {
                Name = "%User%"
            };
            var result5 = DbHelper.QuerySingleTable<User>("[Name] like @Name", condition5, "[Name] DESC").ToArray();
            Assert.Equal(2, result5.Count());
            Assert.Equal("QuerySingleTableUser2", result5[0].Name);
        }

        [Fact]
        public void QuerySingleTableAsyncTest()
        {
            var org = new Org()
            {
                Name = "QuerySingleTableOrg"
            };

            org.Id = Convert.ToInt32(DbHelper.Insert<Org>(org));
            Assert.Equal(1, org.Id);

            var user1 = new User()
            {
                Name = "QuerySingleTableUser1",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = 1
            };
            user1.Id = Convert.ToInt32(DbHelper.Insert<User>(user1));
            Assert.Equal(1, user1.Id);
            Assert.Equal(1, user1.OrgId);

            var user2 = new User()
            {
                Name = "QuerySingleTableUser2",
                Birthday = new DateTime(1996, 8, 9),
                Description = "Test",
                OrgId = 2
            };
            user2.Id = Convert.ToInt32(DbHelper.Insert<User>(user2));
            Assert.Equal(2, user2.Id);
            Assert.Equal(2, user2.OrgId);

            var result1 = DbHelper.QuerySingleTableAsync<User>().Result;
            Assert.Equal(2, result1.Count());

            var condition2 = new
            {
                Id = 2
            };
            var result2 = DbHelper.QuerySingleTableAsync<User>("Id=@Id", condition2).Result.Single();
            Assert.Equal(2, result2.Id);

            var condition3 = new
            {
                OrgId = 2
            };
            var result3 = DbHelper.QuerySingleTableAsync<User>("OrgId=@OrgId", condition3).Result.Single();
            Assert.Equal(2, result2.Id);

            var result4 = DbHelper.QuerySingleTableAsync<User>(null, null, "[Name] DESC").Result.ToArray();
            Assert.Equal("QuerySingleTableUser2", result4[0].Name);

            var condition5 = new
            {
                Name = "%User%"
            };
            var result5 = DbHelper.QuerySingleTableAsync<User>("[Name] like @Name", condition5, "[Name] DESC").Result.ToArray();
            Assert.Equal(2, result5.Count());
            Assert.Equal("QuerySingleTableUser2", result5[0].Name);
        }

        #endregion QuerySingleTable
    }
}
