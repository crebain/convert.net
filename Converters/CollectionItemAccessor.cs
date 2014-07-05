﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Converters
    {
    class CollectionItemAccessor<TKey, TValue> : IPropertyAccessor
        {
        private TKey m_key;
        public CollectionItemAccessor (TKey key)
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
