using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moo.Core
{
    internal class MappingValueConverter : ValueConverter
    {
        public override bool CanConvert(Type sourceType, Type targetType)
        {
            return 
                sourceType.Assembly.Equals(targetType.Assembly) 
                || base.CanConvert(sourceType, targetType);
        }

        public override object Convert(object sourceValue, Type targetType)
        {
            if (sourceValue == null)
                return null;

            var srcType = sourceValue.GetType();

            if (base.CanConvert(srcType, targetType))
                return base.Convert(sourceValue, targetType);

            return Convert(sourceValue, targetType, MappingRepository.Default.ResolveMapper(srcType, targetType));
        }

        private object Convert(object sourceValue, Type targetType, IMapper mapper)
        {
            return mapper.Map(sourceValue);
        }
    }
}
