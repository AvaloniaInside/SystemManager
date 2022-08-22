using System.Diagnostics;

namespace AvaloniaInside.SystemManager;

internal class Bash
{
    public static void Execute(string arguments, out string error, out string output)
    {
        Execute(SystemConstants.Sh, arguments, out error, out output);
    }

    public static void Execute(string file, string arguments, out string error, out string output)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = file,
                Arguments = $"-c \"{arguments}\"",
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