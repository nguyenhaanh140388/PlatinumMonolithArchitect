﻿namespace Platinum.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaximumAttribute : ValidationAttribute
    {
        private readonly int _maximumValue;

        public MaximumAttribute(int maximum) :
            base(errorMessage: "The {0} field value must be maximum {1}.")
        {
            _maximumValue = maximum;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                ErrorMessageString,
                name,
                _maximumValue);
        }

        public override bool IsValid(object value)
        {
            if (value != null && int.TryParse(value.ToString(), out int intValue))
            {
                return intValue <= _maximumValue;
            }

            return false;
        }
    }
}
