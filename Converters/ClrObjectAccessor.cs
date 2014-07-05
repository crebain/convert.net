using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Converters
    {
    class ClrObjectAccessor<T> : IObjectAccessor<T>
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

    class ClrPropertyAccessor : IPropertyAccessor
        {
        private PropertyInfo m_property;
        public ClrPropertyAccessor (PropertyInfo property)
            {
            m_property = property;
            }

        public string Name
            {
            get
                {
                return m_property.Name;
                }
            }

        public Expression Get (ParameterExpression instance)
            {
            Contract.Requires (m_property.DeclaringType.IsAssignableFrom (instance.Type));

            return Expression.Property (instance, m_property);
            }

        public Expression Set (ParameterExpression instance, Expression value)
            {
            Contract.Requires (m_property.DeclaringType.IsAssignableFrom (instance.Type));

            return Expression.Assign (this.Get (instance), value);
            }
        }
    }
