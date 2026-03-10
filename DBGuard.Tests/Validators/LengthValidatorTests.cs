using DBGuard.Core.Models;
using DBGuard.Validation.Validators;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DBGuard.Tests.Validators
{
    public class LengthValidatorTests
    {
        [Fact]
        public void Should_Fail_When_Length_Exceeds_Max()
        {
            var column = new ColumnSchema
            {
                TableName = "Users",
                ColumnName = "FirstName",
                MaxLength = 5
            };

            var validator = new LengthValidator();

            var result = validator.Validate(column, "ABCDEFGHI");

            result.IsValid.Should().BeFalse();
        }
    }
}
