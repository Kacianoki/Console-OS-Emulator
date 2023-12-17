using System.IO.Compression;
using System.Reflection;
using System.Diagnostics;
namespace OSBase.Compiler;
public class CompiledOS
{
    public ConsoleOSInfo ConsoleOS;
    public string AssemblyFile;
    public AssemblyInfo[] DependentAssemblies;
    public string[] DependentFiles;
    public Version CompilerVersion;
}
public class OS
{
    public ConsoleOS consoleOS;
    public OSInstaller installer;
    public Assembly Assembly;
    public OS(ConsoleOS consoleOS, Assembly assembly, OSInstaller installer)
    {
        this.consoleOS = consoleOS;
        Assembly = assembly;
        this.installer = installer;
    }
}
public class AssemblyInfo
{
    public string Name;
    public string FullName;
    public Version Version;
    public AssemblyInfo(string name, string fullName, Version version)
    {
        Name = name;
        FullName = fullName;
        Version = version;
    }
}