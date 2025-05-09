using System;
using System.ComponentModel.DataAnnotations;

namespace dufeksoft.lib.Model
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base("^[a-zA-Z0-9_\\+-]+(\\.[a-zA-Z0-9_\\+-]+)*@[a-zA-Z0-9-]+(\\.[a-zA-Z0-9-]+)*\\.([a-zA-Z]{2,4})$")
        {
        }
    }

    public class UserNameAttribute : RegularExpressionAttribute
    {
        public UserNameAttribute()
            : base("^[a-z0-9_-]+(\\.[a-z0-9_-]+)*$")
        {
        }
    }

    public class NumberAttribute : RegularExpressionAttribute
    {
        public NumberAttribute()
            : base("^[0-9]*$")
        {
        }
    }

    public class DecimalNumberAttribute : RegularExpressionAttribute
    {
        public DecimalNumberAttribute()
            : base("^[+-]?[0-9.,]*$")
        {
        }
    }

    public class RequiredGuidDropDownAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (!base.IsValid(value))
            {
                return false;
            }

            return value.ToString() != Guid.Empty.ToString();
        }

        public static bool IsValidKey(string ddKey)
        {
            if (string.IsNullOrEmpty(ddKey))
            {
                return false;
            }

            return ddKey.ToString() != Guid.Empty.ToString();
        }
    }

    public class IcoAttribute : RegularExpressionAttribute
    {
        public IcoAttribute()
            : base("^[0-9]{8,8}$")
        {
        }
    }
    public class DicAttribute : RegularExpressionAttribute
    {
        public DicAttribute()
            : base("^[0-9]{10,10}$")
        {
        }
    }
    public class IcdphAttribute : RegularExpressionAttribute
    {
        public IcdphAttribute()
            : base("^[a-zA-Z]{2,2}[0-9]{10,10}$")
        {
        }
    }
}