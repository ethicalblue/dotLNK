using dotLNKLibrary;
using static dotLNKLibrary.WindowsShortcut;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(@"/---------------------------------------------------------\");
        Console.WriteLine(@"| dotLNK_Console is Windows Shortcut (.lnk) Analysis Tool |");
        Console.WriteLine(@"| by ethical.blue Magazine // Cybersecurity clarified.    |");
        Console.WriteLine(@"\---------------------------------------------------------/");
        Console.WriteLine();

        string filePath;
        bool pause = false;

        if (args.Length <= 0)
        {
            Console.Write("FileName: ");
            filePath = Console.ReadLine() ?? string.Empty;
            pause = true;
        }
        else if (args.Length == 1)
        {
            filePath = args.ElementAtOrDefault(0) ?? string.Empty;
        }
        else
        {
            filePath = string.Empty;
            Console.WriteLine("Usage: dotLNK_Console <LnkFileNameToAnalyze>");
            Console.ReadLine();
            return;
        }

        Console.WriteLine();

        if (string.IsNullOrWhiteSpace(filePath))
            return;

        if (File.Exists(filePath) == false)
        {
            Console.WriteLine("File not found.");
            if(pause)
                Console.ReadLine();
            return;
        }

        Data = File.ReadAllBytes(filePath);

        Console.WriteLine("--== ShellLinkHeader ==--");
        Console.WriteLine("HeaderSize: " + ShellLinkHeader.HeaderSize.ValueAsText);
        Console.WriteLine("HeaderSize.IsValid: " + ShellLinkHeader.HeaderSize.IsValid);
        Console.WriteLine("LinkCLSID: " + ShellLinkHeader.LinkCLSID.ValueAsText);
        Console.WriteLine("LinkCLSID.IsValid: " + ShellLinkHeader.LinkCLSID.IsValid);
        Console.WriteLine("LinkFlags: " + ShellLinkHeader.LinkFlags.ValuesAsText());
        Console.WriteLine("FileAttributesFlags: " + ShellLinkHeader.FileAttributesFlags.ValuesAsText());
        Console.WriteLine("CreationTime.DateTime: " + ShellLinkHeader.CreationTime.DateTimeValue().ToUniversalTime());
        Console.WriteLine("CreationTime.IsZero: " + ShellLinkHeader.CreationTime.IsZero.ToString());
        Console.WriteLine("AccessTime.DateTime: " + ShellLinkHeader.AccessTime.DateTimeValue().ToUniversalTime());
        Console.WriteLine("AccessTime.IsZero: " + ShellLinkHeader.AccessTime.IsZero.ToString());
        Console.WriteLine("WriteTime.DateTime: " + ShellLinkHeader.WriteTime.DateTimeValue().ToUniversalTime());
        Console.WriteLine("WriteTime.IsZero: " + ShellLinkHeader.WriteTime.IsZero.ToString());
        Console.WriteLine("FileSize.ValueAsText: " + ShellLinkHeader.FileSize.ValueAsText);
        Console.WriteLine("FileSize.Warning: " + ShellLinkHeader.FileSize.Warning);
        Console.WriteLine("IconIndex: " + ShellLinkHeader.IconIndex.ValueAsInt.ToString());
        Console.WriteLine("ShowCommand: " + ShellLinkHeader.ShowCommand.ValueAsText());
        Console.WriteLine("HotKey.LowByte: " + ShellLinkHeader.HotKey.LowByteAsText());
        Console.WriteLine("HotKey.HighByte: " + ShellLinkHeader.HotKey.HighByteAsText());
        Console.WriteLine("Reserved1: " + ShellLinkHeader.Reserved1.ValueAsText);
        Console.WriteLine("Reserved2: " + ShellLinkHeader.Reserved2.ValueAsText);
        Console.WriteLine("Reserved3: " + ShellLinkHeader.Reserved3.ValueAsText);
        Console.WriteLine();

        Console.WriteLine("--== LinkTargetIDList ==--");
        Console.WriteLine("Exists: " + LinkTargetIDList.Exists.ToString());
        Console.WriteLine("Size: " + LinkTargetIDList.Size.ToString());
        foreach (var item in LinkTargetIDList.IDList.Items())
        {
            if (string.IsNullOrEmpty(item))
                Console.WriteLine();
            else
                Console.WriteLine(item);
        }
        Console.WriteLine();

        Console.WriteLine("--== LinkInfo ==--");
        Console.WriteLine("Exists: " + LinkInfo.Exists.ToString());
        Console.WriteLine("Offset: " + LinkInfo.Offset.AsHex());
        Console.WriteLine("LinkInfoSize: " + LinkInfo.LinkInfoSize.ValueAsUInt.ToString());
        Console.WriteLine("LinkInfoHeaderSize: " + LinkInfo.LinkInfoHeaderSize.ValueAsUInt.AsHex());
        Console.WriteLine("LinkInfoFlags: " + LinkInfo.LinkInfoFlags.ValuesAsText());
        Console.WriteLine("LinkInfoHeaderSize.Information: " + LinkInfo.LinkInfoHeaderSize.Information());
        Console.WriteLine();

        Console.WriteLine("VolumeID.DriveType: " + LinkInfo.VolumeID.DriveType());
        Console.WriteLine("VolumeID.DriveSerialNumber: " + LinkInfo.VolumeID.DriveSerialNumber());
        Console.WriteLine("VolumeID.VolumeLabel: " + LinkInfo.VolumeID.VolumeLabel());
        Console.WriteLine("VolumeID.VolumeLabelUnicode: " + LinkInfo.VolumeID.VolumeLabelUnicode());
        Console.WriteLine("LocalBasePath: " + LinkInfo.LocalBasePath.Value());
        Console.WriteLine("LocalBasePathUnicode: " + LinkInfo.LocalBasePathUnicode.Value());
        Console.WriteLine();

        Console.WriteLine("CommonNetworkRelativeLink.Exists: " + LinkInfo.CommonNetworkRelativeLink.Exists.ToString());
        Console.WriteLine("CommonNetworkRelativeLinkSize: " + LinkInfo.CommonNetworkRelativeLink.CommonNetworkRelativeLinkSize.ValueAsUInt.ToString());
        Console.WriteLine("CommonNetworkRelativeLinkFlags: " + LinkInfo.CommonNetworkRelativeLink.CommonNetworkRelativeLinkFlags.ValuesAsText());
        Console.WriteLine("NetworkProviderType: " + LinkInfo.CommonNetworkRelativeLink.NetworkProviderType());
        Console.WriteLine("NetNameOffset: " + LinkInfo.CommonNetworkRelativeLink.NetNameOffset.ToString());
        Console.WriteLine("DeviceNameOffset: " + LinkInfo.CommonNetworkRelativeLink.DeviceNameOffset.ToString());
        Console.WriteLine("NetNameUnicodeOffset: " + LinkInfo.CommonNetworkRelativeLink.NetNameOffsetUnicode.ToString());
        Console.WriteLine("DeviceNameUnicodeOffset: " + LinkInfo.CommonNetworkRelativeLink.DeviceNameOffsetUnicode.ToString());
        Console.WriteLine("NetName: " + LinkInfo.CommonNetworkRelativeLink.NetName());
        Console.WriteLine("NetNameUnicode: " + LinkInfo.CommonNetworkRelativeLink.NetNameUnicode());
        Console.WriteLine("DeviceName: " + LinkInfo.CommonNetworkRelativeLink.DeviceName());
        Console.WriteLine("DeviceNameUnicode: " + LinkInfo.CommonNetworkRelativeLink.DeviceNameUnicode());
        Console.WriteLine();

        Console.WriteLine("--== StringData ==--");
        foreach (var item in StringData.Items())
        {
            if (string.IsNullOrEmpty(item.Item1))
                Console.WriteLine();
            else
                Console.WriteLine($"{item.Item1}: {item.Item2}");
        }
        Console.WriteLine();

        Console.WriteLine("--== ExtraData ==--");
        foreach (var item in ExtraData.ExtractEssentialData())
        {
            if (string.IsNullOrEmpty(item.name))
                Console.WriteLine();
            else
                Console.WriteLine($"{item.name}: {item.value}");
        }
        Console.WriteLine();

        Console.WriteLine("--== ExtraData.TextStrings ==--");
        foreach (var item in ExtraData.TextStrings())
        {
            if (string.IsNullOrEmpty(item))
                Console.WriteLine();
            else
                Console.WriteLine(item);
        }

        if (pause)
            Console.ReadLine();
    }
}