namespace Anhny010920.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinimumAttribute : ValidationAttribute
    {
        private readonly int _minimumValue;

        public MinimumAttribute(int minimum) :
            base(errorMessage: "The {0} field value must be minimum {1}.")
        {
            this._minimumValue = minimum;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                base.ErrorMessageString,
                name,
                this._minimumValue);
        }

        public override bool IsValid(object value)
        {
            if (value != null && int.TryParse(value.ToString(), out int intValue))
            {
                return (intValue >= this._minimumValue);
            }

            return false;
        }
    }
}
