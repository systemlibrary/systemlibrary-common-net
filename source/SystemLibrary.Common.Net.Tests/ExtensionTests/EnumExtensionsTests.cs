using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Tests.ExtensionTests
{
    [TestClass]
    public class EnumExtensionsTests
    {
        public enum Colors
        {
            Black, White, Red, Blue
        }

        [TestMethod]
        public void Enum_IsAny()
        {
            var red = Colors.Red;

            var result = red.IsAny(Colors.Black, Colors.Blue);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Enum_Cast()
        {
            var color = Colors.Red;
            
            var colorEnum = color.AsEnum();

            Assert.IsTrue(colorEnum != null);
            Assert.IsTrue(colorEnum.GetType().Name == nameof(Colors));
        }

        [TestMethod]
        public void Enum_ToArray()
        {
            var integers = new object[] { 1, 2, 3, 4 };
            
            var colors = integers.AsEnumArray<Colors>();

            Assert.IsTrue(colors.Length == integers.Length);
            Assert.IsTrue(colors[2] == Colors.Blue);
        }
    }
}
