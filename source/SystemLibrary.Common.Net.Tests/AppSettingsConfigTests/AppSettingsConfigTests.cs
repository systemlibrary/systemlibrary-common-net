using System;
using System.Reflection;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests.AppSettingsTests
{
    [TestClass]
    public class AppSettingsConfigTests
    {
        [TestMethod]
        public void Read_Config_Extension()
        {
            var config = Configs.AppSettingsConfigTests.Current;
            Assert.IsTrue(config != null, "config is null: ");
            Assert.IsTrue(config.DummyUser?.Contains("Hello"), "DummyUser: " + config.DummyUser);
            Assert.IsTrue(config.Parent != null, "Parent is null");
            Assert.IsTrue(config.Parent.Color == "orange", "Parent color");
        }

        [TestMethod]
        public void A2()
        {
            Func(() => "hah2a");
            Func(() => "hah2a");
            Func(() => "hah2A");

            string a = "99999";
            int b = 6666;
            bool c = true;
            var cached = Func(() => a + b + c);
            var c3ached = Func(() => a + b + c);
        }

        static StringBuilder Func<T>(Func<T> getItem)
        {
            var key = new StringBuilder("common.web.cache");

            var getItemMethod = getItem.Method;

            key.Append(getItemMethod.Name);
            key.Append(getItemMethod.DeclaringType?.GetHashCode());
            key.Append(getItemMethod.ReturnType?.GetHashCode());

            var target = getItem.Target;
            if (target != null)
            {
                var type = target.GetType();
                var fields = type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (fields.Length > 0)
                {
                    foreach (var field in fields)
                    {
                        key.Append(field.Name);

                        var value = field.GetValue(target);

                        if (value != null)
                        {
                            key.Append(value.GetHashCode());
                        }
                    }
                }
            }
            return key;
        }

    }
}

