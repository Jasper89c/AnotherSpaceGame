using System;
using System.ComponentModel;
using System.Reflection;

namespace AnotherSpaceGame.Models
{
    internal static class Extensions
    {
        public static string ToDescription(this Enum value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field == null)
            {
                return value.ToString();
            }

            DescriptionAttribute? attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}