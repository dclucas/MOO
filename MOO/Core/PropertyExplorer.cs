namespace Moo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class PropertyExplorer
    {
        public IEnumerable<PropertyInfo> GetSourceProps<TSource>()
        {
            return typeof(TSource).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.GetProperty);
        }

        public IEnumerable<PropertyInfo> GetTargetProps<TTarget>()
        {
            return typeof(TTarget).GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.SetProperty);
        }

        public IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> GetMatches<TSource, TTarget>()
        {
            return GetMatches<TSource, TTarget>((s, t) => s.PropertyType == t.PropertyType);
        }

        public IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> GetMatches<TSource, TTarget>(Func<PropertyInfo, PropertyInfo, bool> checkAction)
        {
            return from s in GetSourceProps<TSource>()
                   from t in GetTargetProps<TTarget>()
                   where checkAction(s, t)
                   select new KeyValuePair<PropertyInfo, PropertyInfo>(s, t);
        }
    }
}
