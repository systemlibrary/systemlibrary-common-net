using System;
using System.IO;

namespace SystemLibrary.Common.Net
{
    partial class Assemblies
    {
        static byte[] ReadEmbeddedResourceAsBytes(string folderPathInProject, string fileName, System.Reflection.Assembly asm)
        {
            byte[] ReadResourceFromAssemblyStream(string path)
            {
                using (Stream stream = asm.GetManifestResourceStream(path))
                {
                    if (stream == null) return null;
                    
                    if (stream.Length > int.MaxValue)
                        throw new Exception("The embedded resource is too large, max bytes supported is: " + int.MaxValue);

                    var bytes = new byte[stream.Length];

                    stream.Read(bytes, 0, (int)stream.Length);

                    return bytes;
                }
            }

            string resourcePath = GetResourcePath(folderPathInProject, fileName, asm);

            return ReadResourceFromAssemblyStream(resourcePath);
        }

        static string ReadEmbeddedResourceAsString(string folderPathInProject, string fileName, System.Reflection.Assembly asm)
        {
            string ReadResourceFromAssemblyStream(string path)
            {
                using (Stream stream = asm.GetManifestResourceStream(path))
                {
                    if (stream == null) return null;

                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }

            string resourcePath = GetResourcePath(folderPathInProject, fileName, asm);

            return ReadResourceFromAssemblyStream(resourcePath);
        }

        static string GetResourcePath(string folderPathInProject, string fileName, System.Reflection.Assembly asm)
        {
            bool UseAsmNameAsRootResourcePath(string assemblyNamespace)
            {
                return folderPathInProject.IsNot() ||
                    (assemblyNamespace.Is() && !assemblyNamespace.StartsWith(folderPathInProject.Split('.')[0]));
            }

            var resourcePath = "";
            var defaultAsmNamespace = asm?.FullName.Split(',')[0];

            if (UseAsmNameAsRootResourcePath(defaultAsmNamespace))
                resourcePath = defaultAsmNamespace + "." +
                (folderPathInProject.IsNot() ? "" : folderPathInProject + ".");

            else if (folderPathInProject?.EndsWith(".") == true)
                resourcePath += folderPathInProject;

            else
                resourcePath += folderPathInProject + ".";

            if (folderPathInProject.Is())
                resourcePath = resourcePath?.Replace("/", ".").Replace("\\", ".");

            resourcePath += fileName;
            return resourcePath;
        }
    }
}