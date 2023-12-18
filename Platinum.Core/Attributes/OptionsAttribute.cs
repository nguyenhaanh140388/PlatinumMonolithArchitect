namespace Anhny010920.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OptionsAttribute : ValidationAttribute
    {
        private readonly string[] _availableOptions;
        private readonly bool _ignoreCase;

        public OptionsAttribute(params string[] availableOptions)
            : this(true, availableOptions)
        {
        }

        public OptionsAttribute(bool ignoreCase, params string[] availableOptions)
            : base(errorMessage: "The {0} field value must be one of the following options: {1}.")
        {
            this._availableOptions = availableOptions;
            this._ignoreCase = ignoreCase;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                base.ErrorMessageString,
                name,
                string.Join(", ", this._availableOptions));
        }

        public override bool IsValid(object value)
        {
            return this._availableOptions.Contains(
                value.ToString(),
                (this._ignoreCase) ?
                    StringComparer.InvariantCultureIgnoreCase :
                    StringComparer.InvariantCulture);
        }
    }
}
