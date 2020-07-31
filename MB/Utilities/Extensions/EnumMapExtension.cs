using System;

namespace MB.Utilities.Extensions
{
    public static class EnumMapExtensions
    {
        /// <summary>
        /// Will map one emun to another enum
        /// This will be done via the STRING value of the enum not via the interger value
        /// When the string can't be mapped, the default enum value is returned
        /// </summary>
        public static T ToEnum<T>(this Enum inputEnum, bool throwWhenUnknown = true) where T : struct
        {
            if (inputEnum == null)
            {
                if (throwWhenUnknown)
                {
                    throw new Exception($"ToEnum<{typeof(T).Name}> failed because of null input value.");
                }
                return default;
            }

            if (Enum.TryParse(inputEnum.ToString(), out T value))
            {
                return value;
                
            }

            if (throwWhenUnknown)
            {
                throw new Exception($"ToEnum<{typeof(T).Name}> failed because inputEnum value [{inputEnum}] could not be mapped.");
            }
            return default;
        }

        /// <summary>
        /// Will map a string value to an enum
        /// When the string can't be mapped, the default enum value is returned
        /// </summary>
        public static T ToEnum<T>(this string inputString, bool throwWhenUnknown = true) where T : struct
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                if (throwWhenUnknown)
                {
                    throw new Exception($"ToEnum<{typeof(T).Name}> failed because of null/empty/whitespace input value.");
                }
                return default;
            }

            if (Enum.TryParse(inputString, out T value))
            {
                return value;

            }

            if (throwWhenUnknown)
            {
                throw new Exception($"ToEnum<{typeof(T).Name}> failed because inputString [{inputString}] could not be mapped.");
            }
            return default;
        }
    }
}
