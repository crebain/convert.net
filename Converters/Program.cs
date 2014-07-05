using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Converters
    {
    class Model
        {
        public int Int
            {
            get;
            set;
            }

        public string String
            {
            get;
            set;
            }

        public DateTimeOffset DateTimeOffset
            {
            get;
            set;
            }

        public DateTime DateTime
            {
            get;
            set;
            }

        public TimeSpan TimeSpan
            {
            get;
            set;
            }

        public Double Double
            {
            get;
            set;
            }
        }

    class Program
        {
        static void Main (string[] args)
            {
            IDictionary<string, object> values = new Dictionary<string, object> ();
            IObjectAccessor dictAccessor = ObjectAccessor.Create (values);
            var o = new Model
                {
                    Int = 1,
                    String = "string",
                    DateTimeOffset = DateTimeOffset.Now,
                    DateTime = DateTime.Now,
                    TimeSpan = TimeSpan.Zero,
                    Double = Double.Epsilon,
                };

            IObjectAccessor clrAccessor = ObjectAccessor.Create<Model> ();

            Dictionary<string, object> dict = new Dictionary<string, object> ();
            var dictParameter = Expression.Parameter (typeof (Dictionary<string, object>));
            var objParameter = Expression.Parameter (typeof (Model));
            List<Expression> setStatements = new List<Expression> ();
            foreach (IPropertyAccessor getter in clrAccessor.Properties.Values)
                {
                IPropertyAccessor setter = dictAccessor.GetAccessor (getter.Name);
                setStatements.Add (setter.Set (dictParameter, getter.Get (objParameter)));
                }

            var body = Expression.Block (setStatements);
            var lambda = Expression.Lambda<Action<Model, Dictionary<string, object>>> (body, objParameter, dictParameter);
            var func = lambda.Compile ();

            func (o, dict);
            }
        }
    }
