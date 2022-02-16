using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        [TestMethod]
        public void Array_Add_Strings_To_Empty()
        {
            var names = new string[] { };

            var res = names.Add(new string[] { "World", "world", "w0rld" });

            Assert.IsTrue(res.Length == 3);
        }

        [TestMethod]
        public void Array_Add_Empty_To_Empty()
        {
            var names = new string[] { };

            var res = names.Add(new string[] { });

            Assert.IsTrue(res.Length == 0);
        }

        [TestMethod]
        public void Array_Add_Empty_To_Null()
        {
            string[] names = null;

            var res = names.Add(new string[] { });

            Assert.IsTrue(res.Length == 0);
        }

        [TestMethod]
        public void Array_Add_Null_To_Null()
        {
            string[] names = null;

            string[] add = null;
            var res = names.Add(add);

            Assert.IsTrue(res == null);
        }

        [TestMethod]
        public void Array_Add_Strings_To_Strings()
        {
            var names = new string[] { "Hello", "hello", "hell0" };

            var res = names.Add(new string[] { "World", "world", "w0rld" });

            Assert.IsTrue(res.Length == 6);
        }

        [TestMethod]
        public void Array_Add_Ints_To_Ints()
        {
            var names = new int[] { 1, 2, 3, 4 };

            var res = names.Add(new int[] { 1, 2, 3, 4, 5 });

            Assert.IsTrue(res.Length == 9);
        }

        static bool Array_Add_ints_Predicate(int number)
        {
            return number > 2;
        }

        [TestMethod]
        public void Array_Add_Ints_To_Ints_With_Predicate()
        {
            var names = new int[] { 1, 2, 3, 4 };

            var res = names.Add(Array_Add_ints_Predicate, new int[] { 1, 2, 3, 4, 5 });

            Assert.IsTrue(res.Length == 5);
        }
    }
}
