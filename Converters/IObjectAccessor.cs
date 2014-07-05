using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Converters
    {
    public interface IObjectAccessor
        {
        IDictionary<string, IPropertyAccessor> Properties
            {
            get;
            }

        IPropertyAccessor GetAccessor (string name);
        }

    public class ObjectAccessor
        {
        public static IObjectAccessor Create<TKey, TValue> (IDictionary<TKey, TValue> instance)
            {
            return new DictionaryAccessor<TKey, TValue> (instance);
            }

        public static IObjectAccessor Create<T> ()
            where T: class
            {
            return new ClrObjectAccessor<T> ();
            }
        }

    class ClrObjectAccessor<T> : IObjectAccessor
        where T : class
        {
        private static IDictionary<string, PropertyInfo> s_properties = typeof (T)
            .GetProperties (BindingFlags.Instance | BindingFlags.Public)
            .Where (p => p.CanRead && p.CanWrite)
            .ToList ()
            .ToDictionary (p => p.Name);

        private Lazy<IDictionary<string, IPropertyAccessor>> m_properties;

        public ClrObjectAccessor ()
            {
            m_properties = new Lazy<IDictionary<string, IPropertyAccessor>> (() =>
                GetProperties ()
                    .ToList ()
                    .ToDictionary (p => p.Name));
            }

        public IDictionary<string, IPropertyAccessor> Properties
            {
            get
                {
                return m_properties.Value;
                }
            }

        IEnumerable<IPropertyAccessor> GetProperties ()
            {
            foreach (var property in s_properties.Values)
                yield return new ClrPropertyAccessor (property);
            }

        public IPropertyAccessor GetAccessor (string name)
            {
            return new ClrPropertyAccessor (s_properties[name]);
            }
        }

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
                yield return new CollectionPropertyAccessor<TKey, TValue> (key);
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

            return new CollectionPropertyAccessor<string, TValue> (name);
            }
        }
    }
