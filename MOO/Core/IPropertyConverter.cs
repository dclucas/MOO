namespace Moo.Core
{
    using System;
    using System.Reflection;

    /// <summary>Interface for property conversion.</summary>
    public interface IPropertyConverter
    {
        bool CanConvert(PropertyInfo sourceProperty, PropertyInfo targetProperty);
        void Convert(object source, PropertyInfo sourceProperty, object target, PropertyInfo targetProperty);
        void Convert(object source, PropertyInfo sourceProperty, object target, PropertyInfo targetProperty, bool strict);
    }
}
