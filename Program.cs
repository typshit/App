class Program
{
    static async Task Main(string[] args)
    {
        bool IsVm = VMDetection.IsVirtualMachine();
        Console.WriteLine(IsVm);
        await AppFunctions.GeoLocateAndDetectVPN();
    }
}
