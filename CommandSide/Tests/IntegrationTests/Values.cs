﻿using System;
using System.Collections.Generic;
using Framework;
using Framework.Commanding;

namespace Tests.IntegrationTests
{
    internal static class Values
    {
        public static TestAggregateId GenerateTestAggregateId() => TestAggregateId.Of(Guid.NewGuid());
        
        public sealed class TestAggregate : AggregateRoot
        {
            public string Value { get; private set; } = "";
            
            public static TestAggregate NewFrom(TestAggregateId id) => 
                (TestAggregate)new TestAggregate().ApplyChange(TestAggregateCreated.Of(id));

            private void Apply(TestAggregateCreated e)
            {
                SetIdentity(TestAggregateId.Of(Guid.Parse(e.AggregateId)));
            }

            public Result<TestAggregate> DoTransformationWith(string value)
            {
                ApplyChange(TransformationDone.Of(Id.Id, value));
                return Result.Ok(this);
            }
            
            private void Apply(TransformationDone e)
            {
                Value = e.Value;
            }

            protected override void When(IDomainEvent domainEvent)
            {
                switch (domainEvent)
                {
                    case TestAggregateCreated testAggregateCreated:
                        SetIdentity(TestAggregateId.Of(Guid.Parse(testAggregateCreated.AggregateId)));
                        break;
                    case TransformationDone transformationDone:
                        Value = transformationDone.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(domainEvent));
                }
            }
        }
        
        public sealed class TestAggregateId : AggregateId<TestAggregate>
        {
            public TestAggregateId(string aggregateName) : base(aggregateName)
            {
            }
        
            public static TestAggregateId Of(Guid testAggregateName) => new TestAggregateId(testAggregateName.ToString());
        }

        public sealed class TestAggregateCreated : DomainEvent
        {
            public TestAggregateCreated(string aggregateName) : base(aggregateName)
            {
            }
            
            public static TestAggregateCreated Of(string aggregateName) => new TestAggregateCreated(aggregateName);
        }
        
        public sealed class TransformationDone : DomainEvent
        {
            public string Value { get; }

            public TransformationDone(string aggregateName, string value) : base(aggregateName)
            {
                Value = value;
            }
            
            public static TransformationDone Of(string aggregateName, string value) => new TransformationDone(
                aggregateName,
                value);

            protected override IEnumerable<object> GetEqualityComponents()
            {
                foreach (var item in base.GetEqualityComponents()) yield return item;
                yield return Value;
            }
        }
    }
}