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
            var dictAccessor = ObjectAccessor.Create<string, object> ();
            var o = new Model
                {
                    Int = 1,
                    String = "string",
                    DateTimeOffset = DateTimeOffset.Now,
                    DateTime = DateTime.Now,
                    TimeSpan = TimeSpan.Zero,
                    Double = Double.Epsilon,
                };

            var clrAccessor = ObjectAccessor.Create<Model> ();
            var converter = new ObjectConverter<Model, IDictionary<string, object>> (clrAccessor, dictAccessor);
            Dictionary<string, object> dict = new Dictionary<string, object> ();
            converter.Convert (o, dict);
            }
        }
    }
