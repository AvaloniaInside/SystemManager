using System.Diagnostics;

namespace AvaloniaInside;

class Bash
{
    public static void Execute(string file, string arguments, out string error, out string output)
    {
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = file,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };
        process.Start();

        error = process.StandardError.ReadToEnd();
        output = process.StandardOutput.ReadToEnd();
    }
}