using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using DBGuard.Validation.Engine;
using DBGuard.Validation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DBGuard.Tests.Validators
{
    public class ValidationEngineTests
    {
        [Fact]
        public void Should_Detect_Length_Error()
        {
            var schema = new DatabaseSchema();

            var table = new TableSchema
            {
                TableName = "Users"
            };

            table.Columns["FirstName"] = new ColumnSchema
            {
                TableName = "Users",
                ColumnName = "FirstName",
                MaxLength = 5,
                IsNullable = false
            };

            schema.Tables["Users"] = table;

            var engine = new ValidationEngine(
                schema,
                new List<IColumnValidator>
                {
                new LengthValidator(),
                new NullValidator()
                });

            var values = new Dictionary<string, object>
        {
            { "FirstName", "ABCDEFG" }
        };

            var result = engine.Validate("Users", values);

            Assert.False(result.IsValid);
        }
    }
}
