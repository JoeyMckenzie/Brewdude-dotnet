using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Brewdude.Application.Exceptions
{
    public class ValidatorException : Exception
    {
        public IDictionary<string, string[]> Failures { get; }

        public ValidatorException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidatorException(IReadOnlyCollection<ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }
    }
}