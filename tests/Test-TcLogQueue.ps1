param([Parameter(Mandatory = $true)][string]$AgIOPath)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms
Add-Type -ReferencedAssemblies System.Windows.Forms -TypeDefinition @'
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
public static class TcLogQueueTest
{
    public static string Run(string path)
    {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
            string dependency = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), new AssemblyName(args.Name).Name + ".dll");
            return System.IO.File.Exists(dependency) ? Assembly.LoadFrom(dependency) : null;
        };
        var assembly = Assembly.LoadFrom(path);
        var type = assembly.GetType("AgIO.FormISOBUS", true);
        var flags = BindingFlags.NonPublic | BindingFlags.Instance;
        var append = type.GetMethod("AppendLog", flags);
        var flush = type.GetMethod("FlushPendingLog", flags);
        var length = type.GetField("pendingLogLength", flags);
        var omitted = type.GetField("omittedLogLines", flags);
        using (var form = (Form)Activator.CreateInstance(type))
        {
            form.ShowInTaskbar = false;
            form.Opacity = 0;
            var handle = form.Handle;
            var task = Task.Run(() => Parallel.For(0, 20000, i =>
                append.Invoke(form, new object[] { "TC_TEST_" + i + new string('x', 160) })));
            // Deliberately do not pump UI messages while both output readers run.
            if (!task.Wait(5000)) throw new Exception("Output reader waits for UI thread");
            if ((int)length.GetValue(form) > 100000) throw new Exception("Queue is unbounded");
            if ((int)omitted.GetValue(form) == 0) throw new Exception("Overflow test did not trigger");
            append.Invoke(form, new object[] { new string('z', 1000000) });
            if ((int)length.GetValue(form) > 100000) throw new Exception("Long line bypassed bound");
            form.Show();
            for (int i = 0; i < 20; i++) flush.Invoke(form, null);
            if ((int)length.GetValue(form) != 0) throw new Exception("Queue did not drain");
            var box = (TextBox)type.GetField("textBoxRcv", flags).GetValue(form);
            if (box.TextLength == 0 || box.TextLength > 100000) throw new Exception("UI retention failed");
            for (int i = 0; i < 1000; i++) append.Invoke(form, new object[] { "TC_FINAL_" + i });
            for (int i = 0; i < 20; i++) flush.Invoke(form, null);
            if (!box.Text.Contains("TC_FINAL_999")) throw new Exception("Recent line lost");
            form.Hide();
        }
        return "PASS: 20000 concurrent lines without UI pumping; bounded queue; long line; UI drain; latest line; dispose";
    }
}
'@
[TcLogQueueTest]::Run((Resolve-Path -LiteralPath $AgIOPath).Path)
