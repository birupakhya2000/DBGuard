using DBGuard.SqlServer.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DBGuard.Tests.Validators
{
    public class SchemaTests
    {
        private readonly ITestOutputHelper _output;

        public SchemaTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Should_Load_Schema()
        {
            var provider = new SqlServerSchemaProvider();

            var schema = await provider.LoadSchemaAsync("Data Source=10.66.13.24;Initial Catalog=journalitrack_uat;Persist Security Info=True;user id=skeltadbusr;password=skelta@bjd;Encrypt=False");

            foreach (var table in schema.Tables)
            {
                _output.WriteLine($"Table: {table.Key}");

                foreach (var column in table.Value.Columns)
                {
                    _output.WriteLine($"Column: {column.Key}");
                }
            }

            Assert.True(schema.Tables.Count > 0);
        }
    }
}
