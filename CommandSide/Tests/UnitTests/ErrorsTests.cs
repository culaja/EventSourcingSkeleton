using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain;
using FluentAssertions;
using Framework;
using Xunit;

namespace Tests.UnitTests
{
    public sealed class ErrorsTests
    {
        [Fact]
        public void Error_codes_must_be_unique()
        {
            List<MethodInfo> methods = typeof(Errors).Assembly.GetTypes()
                .SelectMany(t => t
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(x => x.ReturnType == typeof(Error)))
                .ToList();

            int numberOfUniqueCodes = methods.Select(GetErrorCode)
                .Distinct()
                .Count();

            numberOfUniqueCodes.Should().Be(methods.Count);
        }

        private string GetErrorCode(MethodInfo method)
        {
            object[] parameters = method.GetParameters()
                .Select<ParameterInfo, object>(x =>
                {
                    if (x.ParameterType == typeof(string))
                        return string.Empty;
                    
                    if (x.ParameterType == typeof(long))
                        return 0;

                    throw new Exception();
                })
                .ToArray();

            var error = (Error)method.Invoke(null, parameters);
            return error.Code;
        }
    }
}