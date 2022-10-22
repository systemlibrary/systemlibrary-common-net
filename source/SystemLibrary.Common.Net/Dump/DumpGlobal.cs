using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using SystemLibrary.Common.Net.Extensions;

namespace SystemLibrary.Common.Net.Global;

/// <summary>
/// Global dumping of 'any' object to a local file for easy debugging and logging
/// - look at it as javascripts 'console.log'
/// - calls to Dump.Write should not go to your production environment
/// </summary>
public static class Dump
{
    static string LogFullPath;
    static string Folder;
    static bool DirExists;
    static List<int> WrittenQueue = new List<int>();

    static Dump()
    {
        LogFullPath = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.GetFullLogPath();
        if (LogFullPath.IsNot())
            LogFullPath = "C:\\Logs\\syslib-error.log";

        Folder = AppSettings.Current?.SystemLibraryCommonNet?.Dump?.Folder;
        if (Folder.IsNot())
            Folder = "C:\\Logs\\";
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

            StringBuilder logString = new StringBuilder();

            logString.Append(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t");

            Build(logString, o, 0, 3);

            WriteToFileWithDateTime(logString);
        }
        catch (Exception ex)
        {
            try
            {
                if (Directory.Exists(@"C:\Temp"))
                    File.AppendAllText(@"C:\Temp\syslib-log.txt", "ex " + ex + "\n");
                else if (Directory.Exists(@"C:\Logs"))
                    File.AppendAllText(@"C:\Logs\syslib-log.txt", "ex " + ex + "\n");
                else if (Directory.Exists(@"C:\"))
                    File.AppendAllText(@"C:\syslib-log.txt", "ex " + ex + "\n");
                else if (Directory.Exists(Environment.CurrentDirectory))
                    File.AppendAllText(Environment.CurrentDirectory + @"\syslib-log" + DateTime.Now.Millisecond + ".txt", ex.Message + "\n");
            }
            catch
            {
                //Swallow infinite loop, in case of:
                //Write access exception
                //Full disk exception
            }
        }
    }

    static void InitializeFolders()
    {
        if (DirExists) return;

        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);

        DirExists = true;
    }

    static string WriteBoolProperty(string n, bool b)
    {
        if (b)
            return ", " + n;
        return "";
    }

    static string WriteType(Type t)
    {
        return t.FullName + " "
            + WriteBoolProperty("IsClass", t.IsClass)
            + WriteBoolProperty("IsInterface", t.IsInterface)
            + WriteBoolProperty("IsEnum", t.IsEnum)
            + WriteBoolProperty("IsValueType", t.IsValueType)
            + WriteBoolProperty("IsAbstract", t.IsAbstract)
            + WriteBoolProperty("IsPrimitive", t.IsPrimitive)
            + WriteBoolProperty("IsArray", t.IsArray)
            + WriteBoolProperty("IsSerializable", t.IsSerializable)
            + WriteBoolProperty("IsAutoClass", t.IsAutoClass)
            + WriteBoolProperty("IsPointer", t.IsPointer)
            + WriteBoolProperty("IsGenericType", t.IsGenericType)
            + WriteBoolProperty("IsGenericParameter", t.IsGenericParameter);
    }

    static void WriteList(StringBuilder logString, Type type, int level, IEnumerable e)
    {
        var arguments = type.GetGenericArguments();
        var genericType = type;
        if (arguments != null && arguments.Length > 0)
            genericType = arguments[0];

        if (e is IDictionary d)
            logString.Append(" dictionary count: " + d.Count + "\n");

        else if (e is IList l)
            logString.Append("IList<" + genericType.Name + "> count: " + l.Count + "\n");

        else if (e is Array a)
            logString.Append(" array length: " + a.Length + "\n");

        else if (e is ICollection ic)
            logString.Append(" collection count: " + ic.Count + "\n");
        else
            logString.Append(" unknown count" + "\n");

        logString.Append(GetTabs(level));
        foreach (var item in e)
        {
            Build(logString, item, level, 3);
            var t = item.GetType();
            if (IsNativeType(t))
                logString.Append(" ");
            else
                logString.Append("\n");
        }
    }

    static void WriteClass(StringBuilder logString, object value, Type type, int level)
    {
        if (type == SystemType.ExceptionType ||
            type.Name == "NullReferenceException" ||
            type.Name == "RuntimeType")
            return;

        var arguments = type.GetGenericArguments();
        var genericType = (Type)null;
        if (arguments != null && arguments.Length > 0)
            genericType = arguments[0];

        var typeName = type.Name;
        if (genericType != null)
            typeName = typeName + "<" + genericType?.Name + ">";

        logString.Append(typeName + (IsClassType(type) ? " (class)" : "") + ", level " + level + "\n");

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

        if (properties != null && properties.Length > 0)
        {
            foreach (var property in properties)
            {
                if (property?.PropertyType == null) continue;
                if (property.PropertyType == SystemType.CharType) continue;
                if (property.PropertyType.Name == "RuntimeType") continue;

                try
                {
                    var propertyValue = property.GetValue(value);

                    AppendVariable(logString, property.PropertyType, property.Name, propertyValue, level);
                }
                catch
                {
                    logString.Append(property.Name + ": could not retrieve value, continuing...");
                }
                logString.Append("\n");
            }
        }

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

        if (fields != null && fields.Length > 0)
        {
            foreach (var field in fields)
            {
                if (field?.FieldType == null) continue;

                try
                {
                    var fieldValue = field.GetValue(value);

                    AppendVariable(logString, field.FieldType, field.Name, fieldValue, level);
                }
                catch
                {
                    logString.Append(field.Name + ": could not retrieve value, continuing...");
                }
                logString.Append("\n");
            }
        }
    }

    static void AppendVariable(StringBuilder logString, Type variableType, string name, object variableValue, int level)
    {
        logString.Append(GetTabs(level) + name + ": ");

        if (IsClassType(variableType) && !IsListType(variableValue))
        {
            Build(logString, variableValue, level + 1, 3);
        }
        else
        {
            Build(logString, variableValue, level, 3);
        }
    }

    static void Build(StringBuilder logString, object value, int level, int maxDepth = 3)
    {
        if (level >= maxDepth)
            return;

        var v = GetVariableValue(value);
        if (v != null)
        {
            logString.Append(v);
        }
        else if (value is Type t)
        {
            logString.Append(WriteType(t));
        }
        else
        {
            var type = value.GetType();

            if (IsListType(value) && value is IEnumerable e)
            {
                WriteList(logString, type, level, e);
                return;
            }

            if (IsClassType(type))
            {
                if (type.BaseType != typeof(ValueType))
                {
                    var hash = value.GetHashCode();

                    if (hash > 1)
                    {
                        // Self-referenced objects are added to a 'already written queue' so it is ignored, every other time
                        if (WrittenQueue.Contains(hash))
                        {
                            logString.Append("Object already logged, continuing...\n");
                            WrittenQueue.Remove(hash);
                            return;
                        }
                        WrittenQueue.Add(hash);
                    }
                }
                WriteClass(logString, value, type, level);
                return;
            }

            Append(logString, type, value, level);
        }
    }

    static string GetVariableValue(object value)
    {
        if (value == null)
            return "(null)";
        else if (value is string str)
            return str + " (Length: " + str.Length + ")";
        else if (value is StringBuilder sb)
            return sb + " (Length: " + sb.Length + ")";
        else if (value is int i)
            return i + "";
        else if (value is DateTime dt)
            return dt + "";
        else if (value is DateTimeOffset dto)
            return dto + "";
        else if (value is TimeSpan ts)
            return ts + "";
        else if (value is bool b)
            return b + "";
        else if (value is double d)
            return d + "";
        else if (value is float f)
            return f + "";
        else if (value is char c)
            return c + "";
        else if (value is Enum en)
            return en.ToText() + " (enum value: " + en.ToValue() + ")";
        else if (value is long i64)
            return i64 + "";
        else if (value is short i16)
            return i16 + "";
        else if (value is bool?)
            return (value as bool?).Value + "";
        else if (value is int?)
            return (value as int?).Value + "";
        else if (value is double?)
            return (value as double?).Value + "";
        else if (value is short?)
            return (value as short?).Value + "";
        else if (value is long?)
            return (value as long?).Value + "";

        return null;
    }

    static void Append(StringBuilder sb, Type type, object value, int level)
    {
        sb.Append(GetTabs(level) + type?.Name + ": " + value);
    }

    static string GetTabs(int level)
    {
        if (level == 0) return "";
        var tabs = "";
        for (int i = 0; i < level; i++)
            tabs += "\t";
        return tabs;
    }

    static bool IsNativeType(Type type)
    {
        return type == SystemType.StringType
            || type == SystemType.CharType
            || type == SystemType.IntType
            || type == SystemType.BoolType
            || type == SystemType.DateTimeType
            || type == SystemType.TimeSpanType
            || type == SystemType.DateTimeOffsetType
            || type == SystemType.DoubleType
            || type == typeof(short)
            || type == typeof(long)
            || type == typeof(decimal)
            || IsNullableType(type);
    }

    static bool IsListType(object o)
    {
        return o is IEnumerable && o is not string;
    }

    static bool IsClassType(Type classType)
    {
        return classType.IsClass &&
        !classType.IsEnum &&
        !classType.IsArray &&
        !IsNativeType(classType) &&
        classType != typeof(StringBuilder) &&
        !IsNullableType(classType);
    }

    static bool IsNullableType(Type type)
    {
        return type == SystemType.DateTimeTypeNullable ||
            type == SystemType.IntTypeNullable ||
            type == SystemType.DateTimeOffsetTypeNullable ||
            type == SystemType.TimeSpanType ||
            type == SystemType.BoolTypeNullable ||
            type == SystemType.DoubleTypeNullable;
    }

    static void WriteToFileWithDateTime(StringBuilder logString)
    {
        logString.Append("\n");
        SafeWrite(logString.ToString());
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
                if (Directory.Exists(@"C:\Temp"))
                    File.AppendAllText(@"C:\Temp\syslib-log" + DateTime.Now.Millisecond + ".txt", "Error writing dump file: " + ex.Message + "\n");
                else if (Directory.Exists(@"C:\Logs"))
                    File.AppendAllText(@"C:\Logs\syslib-log" + DateTime.Now.Millisecond + ".txt", "Error writing dump file: " + ex.Message + "\n");
                else if (Directory.Exists(@"C:\"))
                    File.AppendAllText(@"C:\syslib-log" + DateTime.Now.Millisecond + ".txt", "Error writing dump file: " + ex.Message + "\n");
                else if (Directory.Exists(Environment.CurrentDirectory))
                    File.AppendAllText(Environment.CurrentDirectory + @"\syslib-log" + DateTime.Now.Millisecond + ".txt", "Error writing dump file: " + ex.Message + "\n");
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