﻿using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;
using Framework;

namespace Tests
{
    public static class AssertionExtensions
    {
        public static void BeSuccess(this ObjectAssertions objectAssertions)
        {
            if (objectAssertions.Subject is Result result)
            {
                result.IsSuccess.Should().BeTrue();
            }
            else
            {
                throw new InvalidOperationException($"Expected {nameof(Result)} type but found {objectAssertions.Subject.GetType().Name}");
            }
        }

        public static void BeFailureWith(this ObjectAssertions objectAssertions, Error expectedError)
        {
            if (objectAssertions.Subject is Result result)
            {
                result.IsFailure.Should().BeTrue();
                CompareErrors(result.Error, expectedError);
            }
            else
            {
                throw new InvalidOperationException($"Expected {nameof(Result)} type but found {objectAssertions.Subject.GetType().Name}");
            }
        }

        private static void CompareErrors(Error actualError, Error expectedError)
        {
            switch (actualError)
            {
                case CompositeError compositeError:
                    compositeError.Errors.Should().HaveCount(1);
                    compositeError.Errors.First().Should().Be(expectedError);
                    break;
                default:
                    actualError.Should().Be(expectedError);
                    break;
            }
        }
        
    }
}