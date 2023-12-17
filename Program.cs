using Newtonsoft.Json;
using OSBase.Compiler.ConsoleOSLauncher;

public class Program
{
    public static void Main(string[] args)
    {
        Emulator booter = new Emulator();
        try
        {


            booter.StartUp();
            System.Console.WriteLine("---OS Startup---");
            booter.Process();
        }
        catch(Exception ex)
        {
            File.WriteAllText("CrashLog.log", ex.ToString());
        }
        Console.WriteLine("Press Enter to close emulator.");
        Console.ReadLine();
    }
}