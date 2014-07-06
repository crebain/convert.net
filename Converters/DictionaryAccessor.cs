using System;
using System.Collections.Generic;
using System.Linq;

namespace Converters
    {
    class DictionaryAccessor<TKey, TValue> : IObjectAccessor<IDictionary<TKey, TValue>>
        {
        private IEnumerable<IPropertyAccessor> GetPropertiesInternal (IDictionary<TKey, TValue> instance)
            {
            foreach (TKey key in instance.Keys)
                yield return new CollectionItemAccessor<TKey> (key, typeof (TValue));
            }

        public IPropertyAccessor GetAccessor (string name)
            {
            if (typeof (TKey) != typeof (string))
                throw new NotSupportedException ("Only dictionaries with string keys are supported");

            return new CollectionItemAccessor<string> (name, typeof (TValue));
            }

        public IDictionary<string, IPropertyAccessor> GetProperties (IDictionary<TKey, TValue> instance)
            {
            return GetPropertiesInternal (instance)
                .ToList ()
                .ToDictionary (p => p.Name);
            }
        }
    }
