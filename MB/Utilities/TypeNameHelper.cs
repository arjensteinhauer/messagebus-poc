using System;
using System.Text;

namespace MB.Utilities
{
    /// <summary>
    /// Borrowed from https://dotnetfiddle.net/LxVGpZ
    /// </summary>
    public static class TypeNameHelper
    {
        public static string GetTypeName(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsGenericType)
            {
                return type.GetNestedTypeName();
            }

            var stringBuilder = new StringBuilder();
            BuildTypeName(type, stringBuilder);
            return stringBuilder.ToString();
        }

        public static string GetNestedTypeName(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsNested)
            {
                return type.Name;
            }

            var nestedName = new StringBuilder();
            while (type != null)
            {
                if (nestedName.Length > 0)
                {
                    nestedName.Insert(0, '.');
                }

                nestedName.Insert(0, GetSplitTypeName(type));

                type = type.DeclaringType;
            }
            return nestedName.ToString();
        }

        private static void BuildTypeName(Type type, StringBuilder typeNameBuilder, int genericParameterIndex = 0)
        {
            if (type.IsGenericParameter)
            {
                typeNameBuilder.AppendFormat("T{0}", genericParameterIndex + 1);
            }
            else if (type.IsGenericType)
            {
                typeNameBuilder.Append(GetNestedTypeName(type)).Append('[');
                var subIndex = 0;
                foreach (var genericTypeArgument in type.GetGenericArguments())
                {
                    if (subIndex > 0)
                    {
                        typeNameBuilder.Append(":");
                    }

                    BuildTypeName(genericTypeArgument, typeNameBuilder, subIndex++);
                }
                typeNameBuilder.Append("]");
            }
            else
            {
                typeNameBuilder.Append(type.GetNestedTypeName());
            }
        }

        private static string GetSplitTypeName(Type type)
        {
            return type.IsGenericType ? type.Name.Split('`')[0] : type.Name;
        }
    }
}
