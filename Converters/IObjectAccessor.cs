using System.Collections.Generic;

namespace Converters
    {
    public interface IObjectAccessor<in T>
        {
        IPropertyAccessor GetAccessor (string name);

        IDictionary<string, IPropertyAccessor> GetProperties (T instance);
        }

    public class ObjectAccessor
        {
        public static IObjectAccessor<IDictionary<TKey, TValue>> Create<TKey, TValue> ()
            {
            return new DictionaryAccessor<TKey, TValue> ();
            }

        public static IObjectAccessor<T> Create<T> ()
            where T: class
            {
            return new ClrObjectAccessor<T> ();
            }
        }
    }
