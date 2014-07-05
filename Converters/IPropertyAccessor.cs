using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Converters
    {
    public interface IPropertyAccessor
        {
        string Name
            {
            get;
            }

        Expression Get (ParameterExpression instance);
        Expression Set (ParameterExpression instance, Expression value);
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

    class CollectionPropertyAccessor<TKey, TValue> : IPropertyAccessor
        {
        private TKey m_key;
        public CollectionPropertyAccessor (TKey key)
            {
            m_key = key;
            }

        public string Name
            {
            get
                {
                return m_key.ToString ();
                }
            }

        public Expression Get (ParameterExpression instance)
            {
            DefaultMemberAttribute item = (DefaultMemberAttribute)Attribute.GetCustomAttribute (instance.Type, typeof (DefaultMemberAttribute));
            if (null == item)
                throw new NotSupportedException ("The instance must be indexable");

            return Expression.Property (instance, item.MemberName, Expression.Constant (m_key));
            }

        public Expression Set (ParameterExpression instance, Expression value)
            {
            return Expression.Assign (this.Get (instance), Expression.Convert (value, typeof (TValue)));
            }
        }
    }
