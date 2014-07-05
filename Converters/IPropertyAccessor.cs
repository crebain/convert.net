using System.Linq.Expressions;

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
    }
