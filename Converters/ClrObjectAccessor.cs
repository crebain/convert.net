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
        private static Lazy<IDictionary<string, IPropertyAccessor>> s_properties = new Lazy<IDictionary<string, IPropertyAccessor>> (() =>
            typeof (T)
            .GetProperties (BindingFlags.Instance | BindingFlags.Public)
            .Where (p => p.CanRead && p.CanWrite)
            .Select (p => (IPropertyAccessor)new ClrPropertyAccessor (p))
                .ToList ()
                .ToDictionary (p => p.Name));

        public IPropertyAccessor GetAccessor (string name)
            {
            return s_properties.Value[name];
            }

        public IDictionary<string, IPropertyAccessor> GetProperties (T instance)
            {
            return s_properties.Value;
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
