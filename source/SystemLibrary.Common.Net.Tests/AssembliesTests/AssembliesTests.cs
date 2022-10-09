using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Net.Tests
{
    [TestClass]
    public partial class AssembliesTests
    {
        [TestMethod]
        public void Read_Embedded_Resource()
        {
            var text = Assemblies.GetEmbeddedResource("_Files", "data.json");

            Assert.IsTrue(text.Contains("\"empty\":"));
        }

        [TestMethod]
        public void Read_Embedded_Text_AsBytes()
        {
            var bytes = Assemblies.GetEmbeddedResourceAsBytes("_Files", "data.json");

            var datetime = DateTime.Now.ToString("mm.ss.fffff");
            var path = @"C:\Temp\" + datetime + ".txt";

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (var memory = new MemoryStream(bytes))
                {
                    memory.WriteTo(fileStream);
                }
            }
            Assert.IsTrue(bytes.Length > 1);

            Assert.IsTrue(File.Exists(path));

            if (File.Exists(path))
                File.Delete(path);
        }

        [TestMethod]
        public void Read_Embedded_Image_AsBytes()
        {
            var bytes = Assemblies.GetEmbeddedResourceAsBytes("_Files", "icon.png");

            var datetime = DateTime.Now.ToString("mm.ss.fffff");
            var path = @"C:\Temp\" + datetime + ".png";

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (var memory = new MemoryStream(bytes))
                {
                    memory.WriteTo(fileStream);
                }
            }
            System.Threading.Thread.Sleep(5);

            Assert.IsTrue(bytes.Length > 1);

            Assert.IsTrue(File.Exists(path));

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
