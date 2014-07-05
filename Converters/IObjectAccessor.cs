using System.Collections.Generic;

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

    public interface IObjectAccessor<T> : IObjectAccessor
        {
        }

    public class ObjectAccessor
        {
        public static IObjectAccessor<IDictionary<TKey, TValue>> Create<TKey, TValue> (IDictionary<TKey, TValue> instance)
            {
            return new DictionaryAccessor<TKey, TValue> (instance);
            }

        public static IObjectAccessor<T> Create<T> ()
            where T: class
            {
            return new ClrObjectAccessor<T> ();
            }
        }
    }
