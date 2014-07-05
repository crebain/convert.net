using System;
using System.Collections.Generic;
using System.Linq;

namespace Converters
    {
    class DictionaryAccessor<TKey, TValue> : IObjectAccessor
        {
        private IDictionary<TKey, TValue> m_instance;

        public DictionaryAccessor (IDictionary<TKey, TValue> instance)
            {
            m_instance = instance;
            }

        private IEnumerable<IPropertyAccessor> GetProperties ()
            {
            foreach (TKey key in m_instance.Keys)
                yield return new CollectionItemAccessor<TKey, TValue> (key);
            }

        public IDictionary<string, IPropertyAccessor> Properties
            {
            get
                {
                return GetProperties ()
                    .ToList ()
                    .ToDictionary (p => p.Name);
                }
            }

        public IPropertyAccessor GetAccessor (string name)
            {
            if (typeof (TKey) != typeof (string))
                throw new NotSupportedException ("Only dictionaries with string keys are supported");

            return new CollectionItemAccessor<string, TValue> (name);
            }
        }
    }
