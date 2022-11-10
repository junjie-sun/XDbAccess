using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using XDbAccess.AutoTrans;
using XDbAccess.MSSql;
using Xunit;

namespace XDbAccess.Test.UnitTests
{
    public class MSSqlXDbAccessTest
    {
        public MSSqlXDbAccessTest()
        {
            var connectionString = "Server=.;Database=DapperTemp;Trusted_Connection=True;MultipleActiveResultSets=true";
            DbContext = new DbContext(new DbContextOptions()
            {
                ConnectionString = connectionString,
                DbFactory = new MSSqlDbFactory(connectionString)
            });

            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute("truncate table [user]; truncate table [org];");
            }
        }

        public DbContext DbContext { get; }

        #region Execute

        [Fact]
        public void ExecuteSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                var r = conn.Execute(sql, new { Name = "ExecuteSqlOrg" });
                Assert.Equal(1, r);
            }
        }

        [Fact]
        public async void ExecuteAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                var r = await conn.ExecuteAsync(sql, new { Name = "ExecuteAsyncSqlOrg" });
                Assert.Equal(1, r);
            }
        }

        [Fact]
        public void ExecuteSpTest()
        {
            var sp = "ExecuteSqlTestSP";
            using (var conn = DbContext.GetOpenedConnection())
            {
                var r = conn.Execute(sp, new { Name = "ExecuteSpOrg" }, null, null, System.Data.CommandType.StoredProcedure);
                Assert.Equal(1, r);
            }
        }

        [Fact]
        public async void ExecuteAsyncSpTest()
        {
            var sp = "ExecuteSqlTestSP";
            using (var conn = DbContext.GetOpenedConnection())
            {
                var r = await conn.ExecuteAsync(sp, new { Name = "ExecuteSpOrg" }, null, null, System.Data.CommandType.StoredProcedure);
                Assert.Equal(1, r);
            }
        }

        #endregion

        #region Reader

        [Fact]
        public void ExecuteReaderSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "ExecuteReaderSql" });
            }

            sql = "select * from [org]";
            string orgName = null;
            var connReader = DbContext.GetOpenedConnection();
            using (var reader = connReader.ExecuteReader(sql))
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
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "ExecuteReaderAsyncSql" });
            }

            sql = "select * from [org]";
            string orgName = null;
            var connReader = DbContext.GetOpenedConnection();
            using (var reader = await connReader.ExecuteReaderAsync(sql))
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
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "ExecuteScalarSqlOrg1" });
                conn.Execute(sql, new { Name = "ExecuteScalarSqlOrg2" });
                conn.Execute(sql, new { Name = "ExecuteScalarSqlOrg3" });

                sql = "select count(1) from [org]";
                var r = Convert.ToInt32(conn.ExecuteScalar(sql));
                Assert.Equal(3, r);
            }
        }

        [Fact]
        public async void ExecuteScalarAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "ExecuteScalarAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "ExecuteScalarAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "ExecuteScalarAsyncSqlOrg3" });

                sql = "select count(1) from [org]";
                var r = Convert.ToInt32(await conn.ExecuteScalarAsync(sql));
                Assert.Equal(3, r);
            }
        }

        [Fact]
        public void ExecuteScalarGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "ExecuteScalarGenericSqlOrg1" });
                conn.Execute(sql, new { Name = "ExecuteScalarGenericSqlOrg2" });
                conn.Execute(sql, new { Name = "ExecuteScalarGenericSqlOrg3" });

                sql = "select count(1) from [org]";
                var r = conn.ExecuteScalar<int>(sql);
                Assert.Equal(3, r);
            }
        }

        [Fact]
        public async void ExecuteScalarAsyncGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "ExecuteScalarAsyncGenericSqlOrg1" });
                conn.Execute(sql, new { Name = "ExecuteScalarAsyncGenericSqlOrg2" });
                conn.Execute(sql, new { Name = "ExecuteScalarAsyncGenericSqlOrg3" });

                sql = "select count(1) from [org]";
                var r = await conn.ExecuteScalarAsync<int>(sql);
                Assert.Equal(3, r);
            }
        }

        #endregion

        #region Query

        [Fact]
        public void QuerySqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = conn.Query(sql, new { Id = 1 });
                Assert.Equal(2, r.Count());
                Assert.Single(r.Where(d => d.Id == 2));
                Assert.Single(r.Where(d => d.Id == 3));
            }
        }

        [Fact]
        public async void QueryAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryAsyncSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = await conn.QueryAsync(sql, new { Id = 1 });
                Assert.Equal(2, r.Count());
                Assert.Single(r.Where(d => d.Id == 2));
                Assert.Single(r.Where(d => d.Id == 3));
            }
        }

        [Fact]
        public void QueryGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryGenericSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryGenericSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryGenericSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = conn.Query<Org>(sql, new { Id = 1 });
                Assert.Equal(2, r.Count());
                Assert.Single(r.Where(d => d.Id == 2));
                Assert.Single(r.Where(d => d.Id == 3));
            }
        }

        [Fact]
        public async void QueryGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryGenericAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryGenericAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryGenericAsyncSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = await conn.QueryAsync<Org>(sql, new { Id = 1 });
                Assert.Equal(2, r.Count());
                Assert.Single(r.Where(d => d.Id == 2));
                Assert.Single(r.Where(d => d.Id == 3));
            }
        }

        #endregion

        #region QueryFirst

        [Fact]
        public void QueryFirstSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryFirstSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryFirstSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryFirstSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = conn.QueryFirst(sql, new { Id = 1 });
                Assert.Equal(2, r.Id);
            }
        }

        [Fact]
        public async void QueryFirstAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryFirstAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryFirstAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryFirstAsyncSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = await conn.QueryFirstAsync(typeof(Org), sql, new { Id = 1 });
                Assert.Equal(2, ((Org)r).Id);
            }
        }

        [Fact]
        public void QueryFirstGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryFirstGenericSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryFirstGenericSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryFirstGenericSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = conn.QueryFirst<Org>(sql, new { Id = 1 });
                Assert.Equal(2, r.Id);
            }
        }

        [Fact]
        public async void QueryFirstGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryFirstGenericAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryFirstGenericAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryFirstGenericAsyncSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = await conn.QueryFirstAsync<Org>(sql, new { Id = 1 });
                Assert.Equal(2, r.Id);
            }
        }

        #endregion

        #region QueryFirstOrDefault

        [Fact]
        public void QueryFirstOrDefaultSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = conn.QueryFirstOrDefault(sql, new { Id = 1 });
                Assert.Equal(2, r.Id);
            }
        }

        [Fact]
        public void QueryFirstOrDefaultGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultGenericSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultGenericSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultGenericSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = conn.QueryFirstOrDefault<Org>(sql, new { Id = 1 });
                Assert.Equal(2, r.Id);
            }
        }

        [Fact]
        public async void QueryFirstOrDefaultGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultGenericAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultGenericAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QueryFirstOrDefaultGenericAsyncSqlOrg3" });

                sql = "select * from [org] where Id>@Id";
                var r = await conn.QueryFirstOrDefaultAsync<Org>(sql, new { Id = 1 });
                Assert.Equal(2, r.Id);
            }
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
            var conn = DbContext.GetOpenedConnection();
            var orgId = conn.ExecuteScalar<int>(sql, new { Name = "QueryMultipleOrg" });

            sql = @"
                insert into [user](Name,Birthday,Description,OrgId)
                    values(@Name,@Birthday,@Description,@OrgId);";
            conn.Execute(sql, new { Name = "QueryMultipleUser", Birthday = new DateTime(1986, 8, 7), Description = "Test", OrgId = orgId });

            sql = @"
                select * from [org];
                select * from [user];
            ";
            using (var gridReader = conn.QueryMultiple(sql, new { Id = 1 }))
            {
                var orgList = gridReader.Read<Org>();
                var userList = gridReader.Read<User>();
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
            var conn = DbContext.GetOpenedConnection();
            var orgId = conn.ExecuteScalar<int>(sql, new { Name = "QueryMultipleAsyncOrg" });

            sql = @"
                insert into [user](Name,Birthday,Description,OrgId)
                    values(@Name,@Birthday,@Description,@OrgId);";
            conn.Execute(sql, new { Name = "QueryMultipleAsyncUser", Birthday = new DateTime(1986, 8, 7), Description = "Test", OrgId = orgId });

            sql = @"
                select * from [org];
                select * from [user];
            ";
            using (var gridReader = await conn.QueryMultipleAsync(sql, new { Id = 1 }))
            {
                var orgList = gridReader.Read<Org>();
                var userList = gridReader.Read<User>();
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
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySingleSqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySingleSqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySingleSqlOrg3" });

                sql = "select * from [org] where Id=@Id";
                var r = conn.QuerySingle(sql, new { Id = 3 });
                Assert.Equal(3, r.Id);
            }
        }

        [Fact]
        public void QuerySingleGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySingleGenericSqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySingleGenericSqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySingleGenericSqlOrg3" });

                sql = "select * from [org] where Id=@Id";
                var r = conn.QuerySingle<Org>(sql, new { Id = 3 });
                Assert.Equal(3, r.Id);
            }
        }

        [Fact]
        public async void QuerySingleGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySingleGenericAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySingleGenericAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySingleGenericAsyncSqlOrg3" });

                sql = "select * from [org] where Id=@Id";
                var r = await conn.QuerySingleAsync<Org>(sql, new { Id = 3 });
                Assert.Equal(3, r.Id);
            }
        }

        #endregion

        #region QuerySingleOrDefault

        [Fact]
        public void QuerySingleOrDefaultSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultSqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultSqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultSqlOrg3" });

                sql = "select * from [org] where Id=@Id";
                var r = conn.QuerySingleOrDefault(sql, new { Id = 3 });
                Assert.Equal(3, r.Id);
            }
        }

        [Fact]
        public void QuerySingleOrDefaultGenericSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultGenericSqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultGenericSqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultGenericSqlOrg3" });

                sql = "select * from [org] where Id=@Id";
                var r = conn.QuerySingleOrDefault<Org>(sql, new { Id = 3 });
                Assert.Equal(3, r.Id);
            }
        }

        [Fact]
        public async void QuerySingleOrDefaultAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultAsyncSqlOrg3" });

                sql = "select * from [org] where Id=@Id";
                var r = await conn.QuerySingleOrDefaultAsync(typeof(Org), sql, new { Id = 3 });
                Assert.Equal(3, ((Org)r).Id);
            }
        }

        [Fact]
        public async void QuerySingleOrDefaultGenericAsyncSqlTest()
        {
            var sql = "insert into [org](Name) values(@Name)";
            using (var conn = DbContext.GetOpenedConnection())
            {
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultGenericAsyncSqlOrg1" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultGenericAsyncSqlOrg2" });
                conn.Execute(sql, new { Name = "QuerySingleOrDefaultGenericAsyncSqlOrg3" });

                sql = "select * from [org] where Id=@Id";
                var r = await conn.QuerySingleOrDefaultAsync<Org>(sql, new { Id = 3 });
                Assert.Equal(3, r.Id);
            }
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
                using (var conn1 = DbContext.GetOpenedConnection())
                {
                    sql = @"
                        insert into [org](Name) values(@Name);
                        SELECT CAST(SCOPE_IDENTITY() AS bigint) as Id;
                    ";
                    org.Id = conn1.ExecuteScalar<int>(sql, new { Name = org.Name }, conn1.TransScope.Trans);
                }

                using (var conn2 = DbContext.GetOpenedConnection())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";
                    conn2.Execute(sql, new
                    {
                        Name = "TransCommitUser",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    }, conn2.TransScope.Trans);
                }

                scope.Commit();
            }

            using (var conn3 = DbContext.GetOpenedConnection())
            {
                sql = "select * from [org]";
                var orgList = conn3.Query<Org>(sql);
                Assert.Single(orgList);

                sql = "select * from [user]";
                var userList = conn3.Query<User>(sql);
                Assert.Single(userList);
            }   
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
                using (var conn1 = await DbContext.GetOpenedConnectionAsync())
                {
                    sql = @"
                        insert into [org](Name) values(@Name);
                        SELECT CAST(SCOPE_IDENTITY() AS bigint) as Id;
                    ";
                    org.Id = await conn1.ExecuteScalarAsync<int>(sql, new { Name = org.Name }, conn1.TransScope.Trans);
                }

                using (var conn2 = await DbContext.GetOpenedConnectionAsync())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";
                    await conn2.ExecuteAsync(sql, new
                    {
                        Name = "TransCommitAsyncUser",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    }, conn2.TransScope.Trans);
                }

                scope.Commit();
            }

            using (var conn3 = await DbContext.GetOpenedConnectionAsync())
            {
                sql = "select * from [org]";
                var orgList = await conn3.QueryAsync<Org>(sql);
                Assert.Single(orgList);

                sql = "select * from [user]";
                var userList = await conn3.QueryAsync<User>(sql);
                Assert.Single(userList);
            }
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
                using (var conn1 = DbContext.GetOpenedConnection())
                {
                    sql = @"
                        insert into [org](Name) values(@Name);
                        SELECT CAST(SCOPE_IDENTITY() AS bigint) as Id;
                    ";
                    org.Id = conn1.ExecuteScalar<int>(sql, new { Name = org.Name }, conn1.TransScope.Trans);
                }

                using (var conn2 = DbContext.GetOpenedConnection())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";
                    conn2.Execute(sql, new
                    {
                        Name = "TransRollbackUser",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    }, conn2.TransScope.Trans);
                }

                scope.Rollback();
            }

            using (var conn3 = DbContext.GetOpenedConnection())
            {
                sql = "select * from [org]";
                var orgList = conn3.Query<Org>(sql);
                Assert.Empty(orgList);

                sql = "select * from [user]";
                var userList = conn3.Query<User>(sql);
                Assert.Empty(userList);
            }
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
                using (var conn1 = await DbContext.GetOpenedConnectionAsync())
                {
                    sql = @"
                        insert into [org](Name) values(@Name);
                        SELECT CAST(SCOPE_IDENTITY() AS bigint) as Id;
                    ";
                    org.Id = await conn1.ExecuteScalarAsync<int>(sql, new { Name = org.Name }, conn1.TransScope.Trans);
                }

                using (var conn2 = await DbContext.GetOpenedConnectionAsync())
                {
                    sql = @"
                        insert into [user](Name,Birthday,Description,OrgId)
                            values(@Name,@Birthday,@Description,@OrgId)
                    ";
                    await conn2.ExecuteAsync(sql, new
                    {
                        Name = "TransRollbackAsyncUser",
                        Birthday = new DateTime(1991, 2, 1),
                        Description = "TestUser",
                        OrgId = org.Id
                    }, conn2.TransScope.Trans);
                }

                scope.Rollback();
            }
        }

        #endregion
    }
}
