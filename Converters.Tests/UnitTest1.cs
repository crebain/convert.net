using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Collections.Generic;

namespace Converters.Tests
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

    [TestClass]
    public class UnitTest1
        {
        public IUnityContainer Container
            {
            get;
            private set;
            }

        [TestInitialize]
        public void TestInitialize ()
            {
            this.Container = new UnityContainer ();
            this.Container.LoadConfiguration ();
            }

        [TestMethod]
        public void TestMethod1 ()
            {
            var dtoNow = DateTimeOffset.Now;
            var dtNow = DateTime.Now;
            var o = new Model
                {
                    Int = 1,
                    String = "string",
                    DateTimeOffset = dtoNow,
                    DateTime = dtNow,
                    TimeSpan = TimeSpan.Zero,
                    Double = Double.Epsilon,
                };

            var converter = this.Container.Resolve<IObjectConverter<Model, IDictionary<string, object>>> ();
            var dict = new Dictionary<string, object> ();
            converter.Convert (o, dict);

            Assert.AreEqual (dict["Int"], 1);
            Assert.AreEqual (dict["String"], "string");
            Assert.AreEqual (dict["DateTimeOffset"], dtoNow);
            Assert.AreEqual (dict["DateTime"], dtNow);
            Assert.AreEqual (dict["TimeSpan"], TimeSpan.Zero);
            Assert.AreEqual (dict["Double"], Double.Epsilon);
            }
        }
    }
