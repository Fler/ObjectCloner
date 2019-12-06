using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;

namespace ObjectCloner.Tests.DeepClone
{
    
    public class SettingCloneTest
    {
        [Fact]
        public void HandlesCloneWithoutSettingCopyAllFields()
        {
            var testClass = GetTestClass();
            var copyClass = ObjectCloner.DeepClone(testClass);

            Assert.NotSame(testClass, copyClass);
            Assert.NotSame(testClass.AnoverClassNotIgnore, copyClass.AnoverClassNotIgnore);
            Assert.Equal(testClass.AnoverClassNotIgnore.Value, copyClass.AnoverClassNotIgnore.Value);

            Assert.NotSame(testClass.AnoverClassIgnore, copyClass.AnoverClassIgnore);
            Assert.Null(copyClass.AnoverClassIgnore);

            Assert.NotSame(testClass.ListClassesIgnore, copyClass.ListClassesIgnore);
            Assert.NotSame(testClass.ListClassesNotIgnore, copyClass.ListClassesNotIgnore);

            Assert.Null(copyClass.ListClassesIgnore);
            Assert.Equal(copyClass.ListClassesNotIgnore.Count, testClass.ListClassesNotIgnore.Count);

            for (int i = 0; i < 2; i++)
            {
                Assert.NotSame(copyClass.ListClassesNotIgnore[i], testClass.ListClassesNotIgnore[i]);
                Assert.Equal(copyClass.ListClassesNotIgnore[i].Value, testClass.ListClassesNotIgnore[i].Value);
            }

            Assert.Equal(copyClass.PublicFieldIgnore, (int)0);
            Assert.Equal(copyClass.PublicFieldNotIgnore, (decimal)20.6);

            Assert.False(copyClass.PublicPropertyIgnore);
            Assert.Equal(copyClass.PublicPropertyNotIgnore, 40);

            Assert.False(testClass.EqualsPrivateFieldNotIgnore(copyClass));
            Assert.False(testClass.EqualsPrivateFieldIgnore(copyClass));
        }
        
        private SettingTestClass GetTestClass()
        {
            var testClass = new SettingTestClass
            {
                PublicFieldIgnore = 10,
                PublicFieldNotIgnore = (decimal)20.6,
                PublicPropertyIgnore = true,
                PublicPropertyNotIgnore = 40,
                AnoverClassNotIgnore = new SecondClass
                {
                    Value = 50
                },
                AnoverClassIgnore = new SecondClass
                {
                    Value = 60
                },
                ListClassesNotIgnore = new List<SecondClass>
                {
                    new SecondClass
                    {
                        Value = 70
                    },
                    new SecondClass
                    {
                        Value = 80
                    }
                },
                ListClassesIgnore = new List<SecondClass>
                {
                    new SecondClass
                    {
                        Value = 90
                    },
                    new SecondClass
                    {
                        Value = 100
                    }
                }
            };
            testClass.SetPrivateFieldNotIgnore(50);
            testClass.SetPrivateFieldIgnore("JustString");

            return testClass;
        }

        public class SecondClass
        {
            public int Value { get; set; }
        }

        private class SettingTestClass
        {
            private int _privateFieldNotIgnore;
            public decimal PublicFieldNotIgnore;

            [JsonIgnore]
            private string _privatePrimitiveFieldIgnore;
            [JsonIgnore]
            public int PublicFieldIgnore;

            public int PublicPropertyNotIgnore { get; set; }
            [JsonIgnore]
            public bool PublicPropertyIgnore { get; set; }

            public SecondClass AnoverClassNotIgnore { get; set; }

            [JsonIgnore]
            public SecondClass AnoverClassIgnore { get; set; }

            public IList<SecondClass> ListClassesNotIgnore { get; set; }

            [JsonIgnore]
            public IList<SecondClass> ListClassesIgnore { get; set; }

            public void SetPrivateFieldNotIgnore(int privateValue)
            {
                _privateFieldNotIgnore = privateValue;
            }

            public void SetPrivateFieldIgnore(string privateValue)
            {
                _privatePrimitiveFieldIgnore = privateValue;
            }

            public bool EqualsPrivateFieldNotIgnore(SettingTestClass over)
            {
                return _privateFieldNotIgnore == over._privateFieldNotIgnore;
            }

            public bool EqualsPrivateFieldIgnore(SettingTestClass over)
            {
                return _privatePrimitiveFieldIgnore == over._privatePrimitiveFieldIgnore;
            }
        }
    }
}
