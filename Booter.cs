using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace OSBase.Compiler.ConsoleOSLauncher;
public interface IBooter
{
    public LoadPreparer LoadPreparer { get; }
    public void StartUp();
    public void Shutdown();
    public void Close();
    public void Process();
}
public class Emulator : IBooter
{
    public LoadPreparer LoadPreparer { get; set; }
    public Assembly OSAssembly;
    public ConsoleOS consoleOSInstance;
    public OSInstaller OSInstallerInstance;
    public Type OSIntallerType;
    public Task OSTask;
    public MethodInfo OSMainMethod;


    public void Close()
    {
        OSTask.Dispose();
        Environment.Exit(0);
    }

    public void Shutdown()
    {
        OSTask.Dispose();
        Environment.Exit(0);
    }

    public void StartUp()
    {
        try
        {
            LoadPreparer = JsonConvert.DeserializeObject<LoadPreparer>(File.ReadAllText("LoadPreparer.json"));
            File.Delete("LoadPreparer.json");
            System.Console.WriteLine("Load " + LoadPreparer.compiledOS.AssemblyFile + " file...");
            OSAssembly = Assembly.LoadFrom(LoadPreparer.compiledOS.AssemblyFile);
            Type[] types = OSAssembly.GetTypes();
            List<Type> ConsoleOS = (from p in types where p.IsSubclassOf(typeof(ConsoleOS)) select p).ToList();
            List<Type> OSInstaller = (from p in types where p.IsSubclassOf(typeof(OSInstaller)) select p).ToList();
            System.Console.WriteLine($"Find Main method in {ConsoleOS.First().FullName}");
            OSMainMethod = ConsoleOS.First().GetMethod("Main", BindingFlags.Static | BindingFlags.Public);
            System.Console.WriteLine($"Main method is {OSMainMethod.Name}");
            consoleOSInstance = (ConsoleOS)Activator.CreateInstance(ConsoleOS.First());
            System.Console.WriteLine($"Get the OS installer from {OSInstaller.First().FullName}");
            OSInstallerInstance = (OSInstaller)Activator.CreateInstance(OSInstaller.First());
            OSIntallerType = OSInstaller.First();
        }
        catch (Exception e)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write($"An unhandled exception was thrown on startup: ");
            System.Console.ResetColor();
            System.Console.Write(e + "\r\n");
            throw new Exception("Startup error");
        }
    }
    public async void Process()
    {
        System.Console.WriteLine("---Starting the OS---");
        try
        {
            OSIntallerType.GetMethod("FastInstall").Invoke(OSInstallerInstance, null);
            OSMainMethod.Invoke(null, new object[] { });
            //while(!OSTask.IsCompleted) await Task.Yield();
            //if (OSTask.IsFaulted) throw OSTask.Exception;
        }
        catch (Exception e)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write($"An unhandled exception was thrown while the OS was running: ");
            System.Console.ResetColor();
            System.Console.Write(e + "\r\n");
            throw new Exception("Processing error");
        }
    }
}