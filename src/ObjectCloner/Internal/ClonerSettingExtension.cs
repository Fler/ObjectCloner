using System;
using System.Linq;
using System.Reflection;

namespace ObjectCloner.Internal
{
    /// <summary>
    ///     Contains settings for cloner, for change clone logic
    /// </summary>
    internal static class ClonerSettingExtension
    {
        private const string BackingFieldFormat = ">k__BackingField";
        private const string IgnoreAttribute = "JsonIgnore";

        public static bool IsIgnored(this FieldInfo field)
        {
            var isProperty = field.Name.EndsWith(BackingFieldFormat);

            var isPrivate = field.IsPrivate && !isProperty;
            if (isPrivate)
            {
                return true;
            }

            bool needIgnore = false;

            if (isProperty)
            {
                var propertyName = field.Name.Remove(0, 1).Replace(BackingFieldFormat, string.Empty);
                var property = field.DeclaringType?.GetProperty(propertyName);
                if (property != null)
                {
                    needIgnore = property.GetCustomAttributes().Any(x => x.GetType().Name.StartsWith(IgnoreAttribute, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                needIgnore = field.GetCustomAttributes().Any(x => x.GetType().Name.StartsWith(IgnoreAttribute, StringComparison.OrdinalIgnoreCase));
            }

            return needIgnore;
        }
    }
}
