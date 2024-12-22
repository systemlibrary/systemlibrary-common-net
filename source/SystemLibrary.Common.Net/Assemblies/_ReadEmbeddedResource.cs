using System;
using System.IO;
using System.Text;

namespace SystemLibrary.Common.Net;

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
        var resourcePath = GetResourcePath(folderPathInProject, fileName, asm);

        using (Stream stream = asm.GetManifestResourceStream(resourcePath))
        {
            if (stream == null) return null;

            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }

    static string GetResourcePath(string folderPathInProject, string fileName, System.Reflection.Assembly asm)
    {
        bool UseAsmNameAsRootResourcePath(string assemblyNamespace)
        {
            return folderPathInProject.IsNot() ||
                (assemblyNamespace.Is() && !assemblyNamespace.StartsWith(folderPathInProject.Split('.')[0]));
        }

        StringBuilder resourcePath = new StringBuilder();
        var defaultAsmNamespace = asm?.FullName.Split(',')[0];

        if (UseAsmNameAsRootResourcePath(defaultAsmNamespace))
            resourcePath.Append(defaultAsmNamespace + "." +
            (folderPathInProject.IsNot() ? "" : folderPathInProject + "."));

        else if (folderPathInProject?.EndsWith(".", StringComparison.Ordinal) == true)
            resourcePath.Append(folderPathInProject);

        else
            resourcePath.Append(folderPathInProject + ".");

        if (folderPathInProject.Is())
            resourcePath?
                .Replace("/", ".")
                .Replace("\\", ".");

        resourcePath.Append(fileName);
        return resourcePath.ToString();
    }
}