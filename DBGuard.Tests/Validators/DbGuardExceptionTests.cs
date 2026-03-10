using DBGuard.Core.Exceptions;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DBGuard.Tests.Validators
{
    public class DbGuardExceptionTests
    {
        [Fact]
        public void Should_Format_Error_Message()
        {
            var errors = new List<DbValidationError>
        {
            new DbValidationError
            {
                TableName = "Users",
                ColumnName = "FirstName",
                Message = "Length exceeded"
            }
        };

            var ex = new DbGuardValidationException(errors);

            Assert.Contains("Users", ex.Message);
        }
    }
}
