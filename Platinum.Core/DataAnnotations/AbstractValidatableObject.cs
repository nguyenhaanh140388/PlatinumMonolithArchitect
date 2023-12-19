﻿using System.ComponentModel.DataAnnotations;

namespace Platinum.Core.DataAnnotations
{
    public abstract class AbstractValidatableObject : IValidatableObject
    {
        protected List<ValidationResult> errors = new List<ValidationResult>();

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            CancellationTokenSource source = new CancellationTokenSource();

            var task = ValidateAsync(validationContext, source.Token);

            Task.WaitAll(task);

            return task.Result;
        }

        public virtual Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext, CancellationToken cancellation)
        {
            return Task.FromResult((IEnumerable<ValidationResult>)new List<ValidationResult>());
        }
    }
}
