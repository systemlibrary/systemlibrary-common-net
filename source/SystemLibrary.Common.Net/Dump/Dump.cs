using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using SystemLibrary.Common.Net;

/// <summary>
/// Global dumping of 'any' object to a local file for easy debugging and logging
/// - look at it as javascripts 'console.log'
/// - calls to Dump.Write should not go to your production environment
/// </summary>
public static class Dump
{
    static string LogFullPath;
    static string Folder;
    static bool SkipRuntimeType;
    static Dump()
    {
        LogFullPath = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.GetFullLogPath();
        if (LogFullPath.IsNot())
            LogFullPath = "C:\\Logs\\syslib-error.log";

        Folder = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.Folder;
        if (Folder.IsNot())
            Folder = "C:\\Logs\\";

        SkipRuntimeType = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.SkipRuntimeType == true;
    }

    /// <summary>
    /// Tries to delete the current log file created by Dump.Write() if such file exists, else does nothing
    /// </summary>
    public static void Clear()
    {
        try
        {
            File.Delete(LogFullPath);
        }
        catch
        {
        }
    }

    /// <summary>
    /// Write any object to disc
    /// - Look at it as javascripts 'console.log'
    /// </summary>
    /// <example>
    /// <code class="language-xml hljs">
    /// class Car {
    ///     public string Name {get;set;}
    /// }
    /// var list = new List&lt;Car&gt;();
    /// 
    /// list.Add(new Car { Name = "Vehicle 1" });
    /// list.Add(new Car { Name = "Vehicle 2" });
    /// 
    /// Dump.Write(list);
    /// //Outputs:
    /// //List of Car (1)
    /// //- Name = Vehicle 1
    /// //- Name = Vehicle 2
    /// </code>
    /// </example>
    public static void Write(object o)
    {
        try
        {
            InitializeFolders();
            if (o == null)
            {
                var t = o?.GetType();
                WriteToFileWithDateTime("(null) " + t?.Name);
                return;
            }
            Written.Clear();

            Write(o, 0);
        }
        catch (Exception ex)
        {
            File.AppendAllText(@"C:\Temp\test.txt", "ex " + ex + "\n");
        }
    }


    static void Write(object o, int level)
    {
        if (o is Exception ex)
        {
            WriteToFileWithDateTime(ex?.ToString());
            return;
        }
        if (o is string s)
        {
            WriteToFileWithDateTime(s);
            return;
        }
        if (o is StringBuilder sb)
        {
            WriteToFileWithDateTime("StringBuilder: length " + sb.Length + ", capacity " + sb.Capacity + ", value: " + sb.ToString());
            return;
        }

        if(o is Type t)
        {
            string PrintBool(string n, bool b)
            {
                if (b)
                    return ", " + n;
                return "";
            }
            WriteToFileWithDateTime(t.FullName 
                + PrintBool("IsClass", t.IsClass)
                + PrintBool("IsInterface", t.IsInterface)
                + PrintBool("IsEnum", t.IsEnum)
                + PrintBool("IsValueType", t.IsValueType)
                + PrintBool("IsAbstract", t.IsAbstract)
                + PrintBool("IsPrimitive", t.IsPrimitive)
                + PrintBool("IsArray", t.IsArray)
                + PrintBool("IsSerializable", t.IsSerializable)
                + PrintBool("IsAutoClass", t.IsAutoClass)
                + PrintBool("IsPointer", t.IsPointer)
                + PrintBool("IsGenericType", t.IsGenericType)
                + PrintBool("IsGenericParameter", t.IsGenericParameter)
                + PrintBool("IsGenericMethodParameter", t.IsGenericMethodParameter)
                );
            return;
        }

        if (level == 3) return;

        var type = o.GetType();

        if (type != typeof(int) && type != typeof(string) && type != typeof(bool)
            && type != typeof(DateTime) && type != typeof(KeyValuePair<,>) && type.BaseType != typeof(ValueType))
        {
            var hash = o.GetHashCode();

            if (hash > 1)
            {
                //Self-referenced objects if it finds itself once it is ignored, if it is found again it is logged...
                if (Written.Contains(hash))
                {
                    Written.Remove(hash);
                    return;
                }
                Written.Add(hash);
            }
        }

        if (WriteList(o, level, type)) return;

        if (WriteClass(o, level, type)) return;

        WriteVariableToFile(o, level);
    }

    static bool WriteClass(object o, int level, Type type)
    {
        if (type.IsClass
                && type.IsEnum == false
                && type.IsArray == false
                && IsNativeType(type) == false
                && type is Exception == false
                && type.Name != "NullReferenceException"
            )
        {
            if (SkipRuntimeType && type.Name == "RuntimeType")
            {
                //string runtimeTypeSkippedText = type.FullName + " (level " + level + ")" + ", isPublic " + type.IsPublic + ", isGeneric " + type.IsGenericType;
                //WriteToFile(runtimeTypeSkippedText, level);
                return false;
            }

            var arguments = type?.GetGenericArguments();
            var genericType = (Type)null;
            if (arguments != null && arguments.Length > 0)
                genericType = arguments[0];

            string typeName = type?.Name;
            if (genericType != null)
                typeName = typeName + "<" + genericType?.Name + ">";

            if (level == 0)
                WriteToFileWithDateTime(typeName);
            else
                WriteToFile(typeName + " (level " + level + ")", level);


            level = level + 1;
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
            if (props != null && props.Length > 0)
            {
                foreach (var prop in props)
                {
                    if (prop.PropertyType == typeof(char)) continue;

                    if (SkipRuntimeType && prop.Name == "RuntimeType") continue;

                    try
                    {
                        var v = prop.GetValue(o);

                        if (IsNativeType(prop.PropertyType) || IsNullableType(prop.PropertyType))
                            WriteToFile(prop.Name + "=" + v, level);
                        else
                        {
                            if (v == null)
                                WriteToFile(prop.Name + "=(null)", level);
                            else
                                Write(v, level);
                        }
                    }
                    catch
                    {
                        WriteToFile(prop.Name + "... could not be retrieved, continuing...", level);
                    }
                }
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
            if (fields != null && fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(char)) continue;

                    var v = field.GetValue(o);

                    if (IsNativeType(field.FieldType) || IsNullableType(field.FieldType))
                        WriteToFile(field.Name + "=" + v, level);
                    else
                    {
                        if (v == null)
                            WriteToFile(field.Name + "=(null)", level);
                        else
                            Write(v, level);
                    }
                }
            }

            return true;
        }
        return false;
    }

    static bool WriteList(object o, int level, Type type)
    {
        if (o is string == false && o is IEnumerable)
        {
            var e = o as IEnumerable;

            var arguments = type?.GetGenericArguments();
            var genericType = type;
            if (arguments != null && arguments.Length > 0)
                genericType = arguments[0];

            if (e is IDictionary d)
                WriteToFileWithDateTime(type.Name + " contains total pairs: " + d.Count);

            else if (e is IList l)
                WriteToFileWithDateTime("IList<" + genericType.Name + "> count: " + l.Count);

            else if (e is Array a)
                WriteToFileWithDateTime(type.Name + " length: " + a.Length);

            else if (e is ICollection ic)
                WriteToFileWithDateTime(type.Name + " count " + ic.Count);
            else
                WriteToFileWithDateTime(type.Name + " (unknown count)");

            level = level + 1;
            foreach (var v in e)
                if (v != null)
                    Write(v, level);

            return true;
        }
        return false;
    }

    static void WriteVariableToFile(object o, int level)
    {
        if (level == 0)
            WriteToFileWithDateTime(o);
        else
        {
            WriteToFile(o, level);
        }
    }

    static void InitializeFolders()
    {
        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);
    }

    static bool IsNullableType(Type type)
    {
        return type == typeof(DateTime?) ||
            type == typeof(int?) ||
            type == typeof(DateTimeOffset?) ||
            type == typeof(TimeSpan?) ||
            type == typeof(bool?) ||
            type == typeof(double?);
    }

    static List<int> Written = new List<int>();
    static bool IsNativeType(Type type)
    {
        return type == typeof(string)
            || type == typeof(char)
            || type == typeof(int)
            || type == typeof(bool)
            || type == typeof(DateTime)
            || type == typeof(TimeSpan)
            || type == typeof(DateTimeOffset)
            || type == typeof(long)
            || type == typeof(double)
            || type == typeof(decimal);
    }

    static void WriteToFileWithDateTime(object o)
    {
        var message = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + o + "\n";
        SafeWrite(message);
    }

    static void WriteToFile(object o, int level)
    {
        var tabs = "";
        for (int i = 0; i < level; i++)
        {
            tabs += "\t";
        }

        var message = tabs + o + "\n";

        SafeWrite(message);
    }

    static ReaderWriterLock readWriteLock = new ReaderWriterLock();

    static void SafeWrite(string message)
    {
        try
        {
            try
            {
                readWriteLock.AcquireWriterLock(10);
            }
            catch
            {
            }

            File.AppendAllText(LogFullPath, message, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            try
            {
                File.AppendAllText("C:\\Logs\\errror-writing-to-log-" + DateTime.Now.Millisecond + ".txt", "Error reading to file..." + ex.Message, Encoding.UTF8);
            }
            catch
            {
            }
        }
        finally
        {
            try
            {
                readWriteLock.ReleaseWriterLock();
            }
            catch
            {
            }
        }
    }
}


namespace SystemLibrary.Common.Net.Global
{

    /// <summary>
    /// Global dumping of 'any' object to a local file for easy debugging and logging
    /// - look at it as javascripts 'console.log'
    /// - calls to Dump.Write should not go to your production environment
    /// </summary>
    public static class Dump
    {
        static string LogFullPath;
        static string Folder;
        static bool SkipRuntimeType;
        static Dump()
        {
            LogFullPath = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.GetFullLogPath();
            if (LogFullPath.IsNot())
                LogFullPath = "C:\\Logs\\syslib-error.log";

            Folder = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.Folder;
            if (Folder.IsNot())
                Folder = "C:\\Logs\\";

            SkipRuntimeType = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.SkipRuntimeType == true;
        }

        /// <summary>
        /// Tries to delete the current log file created by Dump.Write() if such file exists, else does nothing
        /// </summary>
        public static void Clear()
        {
            try
            {
                File.Delete(LogFullPath);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Write any object to disc
        /// - Look at it as javascripts 'console.log'
        /// </summary>
        /// <example>
        /// <code class="language-xml hljs">
        /// class Car {
        ///     public string Name {get;set;}
        /// }
        /// var list = new List&lt;Car&gt;();
        /// 
        /// list.Add(new Car { Name = "Vehicle 1" });
        /// list.Add(new Car { Name = "Vehicle 2" });
        /// 
        /// Dump.Write(list);
        /// //Outputs:
        /// //List of Car (1)
        /// //- Name = Vehicle 1
        /// //- Name = Vehicle 2
        /// </code>
        /// </example>
        public static void Write(object o)
        {
            try
            {
                InitializeFolders();
                if (o == null)
                {
                    var t = o?.GetType();
                    WriteToFileWithDateTime("(null) " + t?.Name);
                    return;
                }
                Written.Clear();

                Write(o, 0);
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"C:\Temp\test.txt", "ex " + ex + "\n");
            }
        }


        static void Write(object o, int level)
        {
            if (o is Exception ex)
            {
                WriteToFileWithDateTime(ex?.ToString());
                return;
            }
            if (o is string s)
            {
                WriteToFileWithDateTime(s);
                return;
            }
            if (o is StringBuilder sb)
            {
                WriteToFileWithDateTime("StringBuilder: length " + sb.Length + ", capacity " + sb.Capacity + ", value: " + sb.ToString());
                return;
            }

            if (o is Type t)
            {
                string PrintBool(string n, bool b)
                {
                    if (b)
                        return ", " + n;
                    return "";
                }
                WriteToFileWithDateTime(t.FullName
                    + PrintBool("IsClass", t.IsClass)
                    + PrintBool("IsInterface", t.IsInterface)
                    + PrintBool("IsEnum", t.IsEnum)
                    + PrintBool("IsValueType", t.IsValueType)
                    + PrintBool("IsAbstract", t.IsAbstract)
                    + PrintBool("IsPrimitive", t.IsPrimitive)
                    + PrintBool("IsArray", t.IsArray)
                    + PrintBool("IsSerializable", t.IsSerializable)
                    + PrintBool("IsAutoClass", t.IsAutoClass)
                    + PrintBool("IsPointer", t.IsPointer)
                    + PrintBool("IsGenericType", t.IsGenericType)
                    + PrintBool("IsGenericParameter", t.IsGenericParameter)
                    + PrintBool("IsGenericMethodParameter", t.IsGenericMethodParameter)
                    );
                return;
            }

            if (level == 3) return;

            var type = o.GetType();

            if (type != typeof(int) && type != typeof(string) && type != typeof(bool)
                && type != typeof(DateTime) && type != typeof(KeyValuePair<,>) && type.BaseType != typeof(ValueType))
            {
                var hash = o.GetHashCode();

                if (hash > 1)
                {
                    //Self-referenced objects if it finds itself once it is ignored, if it is found again it is logged...
                    if (Written.Contains(hash))
                    {
                        Written.Remove(hash);
                        return;
                    }
                    Written.Add(hash);
                }
            }

            if (WriteList(o, level, type)) return;

            if (WriteClass(o, level, type)) return;

            WriteVariableToFile(o, level);
        }

        static bool WriteClass(object o, int level, Type type)
        {
            if (type.IsClass
                    && type.IsEnum == false
                    && type.IsArray == false
                    && IsNativeType(type) == false
                    && type is Exception == false
                    && type.Name != "NullReferenceException"
                )
            {
                if (SkipRuntimeType && type.Name == "RuntimeType")
                {
                    //string runtimeTypeSkippedText = type.FullName + " (level " + level + ")" + ", isPublic " + type.IsPublic + ", isGeneric " + type.IsGenericType;
                    //WriteToFile(runtimeTypeSkippedText, level);
                    return false;
                }

                var arguments = type?.GetGenericArguments();
                var genericType = (Type)null;
                if (arguments != null && arguments.Length > 0)
                    genericType = arguments[0];

                string typeName = type?.Name;
                if (genericType != null)
                    typeName = typeName + "<" + genericType?.Name + ">";

                if (level == 0)
                    WriteToFileWithDateTime(typeName);
                else
                    WriteToFile(typeName + " (level " + level + ")", level);


                level = level + 1;
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
                if (props != null && props.Length > 0)
                {
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(char)) continue;

                        if (SkipRuntimeType && prop.Name == "RuntimeType") continue;

                        try
                        {
                            var v = prop.GetValue(o);

                            if (IsNativeType(prop.PropertyType) || IsNullableType(prop.PropertyType))
                                WriteToFile(prop.Name + "=" + v, level);
                            else
                            {
                                if (v == null)
                                    WriteToFile(prop.Name + "=(null)", level);
                                else
                                    Write(v, level);
                            }
                        }
                        catch
                        {
                            WriteToFile(prop.Name + "... could not be retrieved, continuing...", level);
                        }
                    }
                }

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
                if (fields != null && fields.Length > 0)
                {
                    foreach (var field in fields)
                    {
                        if (field.FieldType == typeof(char)) continue;

                        var v = field.GetValue(o);

                        if (IsNativeType(field.FieldType) || IsNullableType(field.FieldType))
                            WriteToFile(field.Name + "=" + v, level);
                        else
                        {
                            if (v == null)
                                WriteToFile(field.Name + "=(null)", level);
                            else
                                Write(v, level);
                        }
                    }
                }

                return true;
            }
            return false;
        }

        static bool WriteList(object o, int level, Type type)
        {
            if (o is string == false && o is IEnumerable)
            {
                var e = o as IEnumerable;

                var arguments = type?.GetGenericArguments();
                var genericType = type;
                if (arguments != null && arguments.Length > 0)
                    genericType = arguments[0];

                if (e is IDictionary d)
                    WriteToFileWithDateTime(type.Name + " contains total pairs: " + d.Count);

                else if (e is IList l)
                    WriteToFileWithDateTime("IList<" + genericType.Name + "> count: " + l.Count);

                else if (e is Array a)
                    WriteToFileWithDateTime(type.Name + " length: " + a.Length);

                else if (e is ICollection ic)
                    WriteToFileWithDateTime(type.Name + " count " + ic.Count);
                else
                    WriteToFileWithDateTime(type.Name + " (unknown count)");

                level = level + 1;
                foreach (var v in e)
                    if (v != null)
                        Write(v, level);

                return true;
            }
            return false;
        }

        static void WriteVariableToFile(object o, int level)
        {
            if (level == 0)
                WriteToFileWithDateTime(o);
            else
            {
                WriteToFile(o, level);
            }
        }

        static void InitializeFolders()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
        }

        static bool IsNullableType(Type type)
        {
            return type == typeof(DateTime?) ||
                type == typeof(int?) ||
                type == typeof(DateTimeOffset?) ||
                type == typeof(TimeSpan?) ||
                type == typeof(bool?) ||
                type == typeof(double?);
        }

        static List<int> Written = new List<int>();
        static bool IsNativeType(Type type)
        {
            return type == typeof(string)
                || type == typeof(char)
                || type == typeof(int)
                || type == typeof(bool)
                || type == typeof(DateTime)
                || type == typeof(TimeSpan)
                || type == typeof(DateTimeOffset)
                || type == typeof(long)
                || type == typeof(double)
                || type == typeof(decimal);
        }

        static void WriteToFileWithDateTime(object o)
        {
            var message = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + o + "\n";
            SafeWrite(message);
        }

        static void WriteToFile(object o, int level)
        {
            var tabs = "";
            for (int i = 0; i < level; i++)
            {
                tabs += "\t";
            }

            var message = tabs + o + "\n";

            SafeWrite(message);
        }

        static ReaderWriterLock readWriteLock = new ReaderWriterLock();

        static void SafeWrite(string message)
        {
            try
            {
                try
                {
                    readWriteLock.AcquireWriterLock(10);
                }
                catch
                {
                }

                File.AppendAllText(LogFullPath, message, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                try
                {
                    File.AppendAllText("C:\\Logs\\errror-writing-to-log-" + DateTime.Now.Millisecond + ".txt", "Error reading to file..." + ex.Message, Encoding.UTF8);
                }
                catch
                {
                }
            }
            finally
            {
                try
                {
                    readWriteLock.ReleaseWriterLock();
                }
                catch
                {
                }
            }
        }
    }
}