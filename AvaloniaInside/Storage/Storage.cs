namespace AvaloniaInside;

public class Storage
{
    public static string Test()
    {
        var maxFreeSpaceDrive = string.Empty;
        long maxFreeSpace = 0;
        try
        {
            var allDrives = DriveInfo.GetDrives()
                .Where(d => d.IsReady && d.DriveType == DriveType.Fixed);
            //// Get the max size drive
            foreach (var drive in allDrives)
                try
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                        if (drive.TotalFreeSpace > maxFreeSpace)
                        {
                            maxFreeSpace = drive.TotalFreeSpace;
                            maxFreeSpaceDrive = drive.Name;
                        }
                }
                catch (IOException)
                {
                }
        }
        catch (Exception)
        {
        }

        return maxFreeSpaceDrive;
    }
}