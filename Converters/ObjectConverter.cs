using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Converters
    {
    class ObjectConverter<TFrom, TTo>: IObjectConverter<TFrom, TTo>
        {
        private IObjectAccessor<TFrom> m_accessorFrom;
        private IObjectAccessor<TTo> m_accessorTo;

        public ObjectConverter (IObjectAccessor<TFrom> accessorFrom, IObjectAccessor<TTo> accessorTo)
            {
            m_accessorFrom = accessorFrom;
            m_accessorTo = accessorTo;
            }

        public void Convert (TFrom from, TTo to)
            {
            var parameterFrom = Expression.Parameter (typeof (TFrom));
            var parameterTo = Expression.Parameter (typeof (TTo));
            List<Expression> setStatements = new List<Expression> ();
            foreach (IPropertyAccessor getter in m_accessorFrom.Properties.Values)
                {
                IPropertyAccessor setter = m_accessorTo.GetAccessor (getter.Name);
                setStatements.Add (setter.Set (parameterTo, getter.Get (parameterFrom)));
                }

            var body = Expression.Block (setStatements);
            var lambda = Expression.Lambda<Action<TFrom, TTo>> (body, parameterFrom, parameterTo);
            var action = lambda.Compile ();
            action (from, to);
            }
        }
    }
