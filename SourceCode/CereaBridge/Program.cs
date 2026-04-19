using System;

namespace CereaBridge
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var cfg = BridgeConfig.Load();
            using (var bridge = new BridgeService(cfg))
            {
                bridge.Start();
                Console.WriteLine("CereaBridge running. Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
