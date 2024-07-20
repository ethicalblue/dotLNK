using dotLNK_Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotLNKLibrary;

public sealed partial class WindowsShortcut
{
    public sealed class ExtraData
    {
        private static int lastIndex = 0x00000000;
        private static uint blockSize = 0x00000000;
        private static uint blockSignature = 0x00000000;
        private static void ReadSignature()
        {
            blockSize = Data.Take(new Range(lastIndex, lastIndex + sizeof(uint))).ToArray().AsUInt();
            blockSignature = Data.Take(new Range(lastIndex + sizeof(uint), lastIndex + (2 *sizeof(uint)))).ToArray().AsUInt();
        }

        public static List<string> TextStrings()
        {
            if (ShellLinkHeader.HeaderSize.IsValid == false)
                return [];

            List<string> dataList = [];

            SkipStringData();

            List<byte> buffer = [];

            for (int i = lastIndex; i < Data.Length; i++)
            {
                if (Data.ElementAt(i) == 0x00 && Data.ElementAt(i - 1) == 0x00)
                {
                    string filtered = buffer.AsFilteredTextDump();

                    if (string.IsNullOrWhiteSpace(filtered) == false && filtered.Length > 1)
                    {
                        dataList.Add(filtered);
                    }
                    buffer.Clear();
                }
                else
                {
                    buffer.Add(Data.ElementAt(i));
                }
            }

            return dataList;
        }

        public static List<(string name, string value)> ExtractEssentialData()
        {
            if (ShellLinkHeader.HeaderSize.IsValid == false)
                return [];

            List<(string name, string value)> dataList = [];

            SkipStringData();

            for(int i = 0; i < 11; i++)
            {
                ReadSignature();

                if (blockSignature == (uint)ExtraDataSignature.ConsoleDataBlock)
                {
                    dataList.Add(new("ExtraData.ConsoleDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }

                if (blockSignature == (uint)ExtraDataSignature.ConsoleFEDataBlock)
                {
                    dataList.Add(new("ExtraData.ConsoleFEDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }

                if (blockSignature == (uint)ExtraDataSignature.DarwinDataBlock)
                {
                    dataList.Add(new("ExtraData.DarwinDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }

                if (blockSignature == (uint)ExtraDataSignature.EnvironmentVariableDataBlock)
                {
                    dataList.Add(new("ExtraData.EnvironmentVariableDataBlock.Exists", "True"));
                    lastIndex += 2 * sizeof(uint);

                    string TargetAnsi = CodePagesEncodingProvider.Instance.GetEncoding((int)CodePage.WINDOWS_1250)?
                        .GetString(Data.Take(new Range(lastIndex, lastIndex + 260)).ToArray()) ?? string.Empty;
                    lastIndex += 260;
                    dataList.Add(new("TargetAnsi", TargetAnsi));

                    string TargetUnicode = Encoding.Unicode.GetString(Data.Take(new Range(lastIndex, lastIndex + 520)).ToArray());
                    lastIndex += 520;
                    dataList.Add(new("TargetUnicode", TargetUnicode));

                    dataList.Add(new(string.Empty, string.Empty));
                }

                if (blockSignature == (uint)ExtraDataSignature.IconEnvironmentDataBlock)
                {
                    dataList.Add(new("ExtraData.IconEnvironmentDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }

                if (blockSignature == (uint)ExtraDataSignature.KnownFolderDataBlock)
                {
                    dataList.Add(new("ExtraData.KnownFolderDataBlock.Exists", "True"));
                    lastIndex += 2 * sizeof(uint);

                    var guid = Data.Take(new Range(lastIndex, lastIndex + 16)).ToArray();
                    lastIndex += 16;

                    string KnownFolderID = new Guid(guid).ToString().ToLower();
                    dataList.Add(new("KnownFolderID", KnownFolderID));
                    string Folder = KnownFolders.Items.FirstOrDefault(x => x.guid.ToLower().Contains(KnownFolderID, StringComparison.CurrentCultureIgnoreCase)).name;
                    dataList.Add(new("KnownFolderName (resolved)", Folder));
                    string Details = KnownFolders.Items.FirstOrDefault(x => x.guid.Contains(KnownFolderID, StringComparison.CurrentCultureIgnoreCase)).details;
                    dataList.Add(new("KnownFolderPath (resolved)", Details));

                    dataList.Add(new(string.Empty, string.Empty));
                }

                if (blockSignature == (uint)ExtraDataSignature.PropertyStoreDataBlock)
                {
                    dataList.Add(new("ExtraData.PropertyStoreDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }

                if (blockSignature == (uint)ExtraDataSignature.ShimDataBlock)
                {
                    dataList.Add(new("ExtraData.ShimDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }

                if (blockSignature == (uint)ExtraDataSignature.SpecialFolderDataBlock)
                {
                    dataList.Add(new("ExtraData.SpecialFolderDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }

                if (blockSignature == (uint)ExtraDataSignature.TrackerDataBlock)
                {
                    dataList.Add(new("ExtraData.TrackerDataBlock.Exists", "True"));
                    lastIndex += 2 * sizeof(uint);

                    uint length = Data.Take(new Range(lastIndex, lastIndex + sizeof(uint))).ToArray().AsUInt();
                    lastIndex += sizeof(uint);
                    dataList.Add(new("Length", length.AsHex()));

                    uint version = Data.Take(new Range(lastIndex, lastIndex + sizeof(uint))).ToArray().AsUInt();
                    lastIndex += sizeof(uint);
                    dataList.Add(new("Version", version.AsHex()));

                    string MachineID = CodePagesEncodingProvider.Instance.GetEncoding((int)CodePage.WINDOWS_1250)?
                        .GetString(Data.Take(new Range(lastIndex, lastIndex + 16)).ToArray()) ?? string.Empty;
                    lastIndex += 16;
                    dataList.Add(new("MachineID", MachineID));

                    var guid0 = Data.Take(new Range(lastIndex, lastIndex + 16)).ToArray();
                    lastIndex += 16;
                    var guid1 = Data.Take(new Range(lastIndex, lastIndex + 16)).ToArray();
                    lastIndex += 16;
                    var guid2 = Data.Take(new Range(lastIndex, lastIndex + 16)).ToArray();
                    lastIndex += 16;
                    var guid3 = Data.Take(new Range(lastIndex, lastIndex + 16)).ToArray();
                    lastIndex += 16;

                    string Droid = new Guid(guid0).ToString().ToLower();
                    dataList.Add(new("Droid", Droid));

                    string Droid_2 = new Guid(guid1).ToString().ToLower();
                    dataList.Add(new("Droid_2", Droid_2));

                    string DroidBirth = new Guid(guid2).ToString().ToLower();
                    dataList.Add(new("DroidBirth", DroidBirth));

                    string DroidBirth_2 = new Guid(guid3).ToString().ToLower();
                    dataList.Add(new("DroidBirth_2", DroidBirth_2));

                    string MACAddress = string.Join(":", Enumerable.Range(0, 6).Select(i => Droid_2.Split('-').LastOrDefault()?.Substring(i * 2, 2)));
                    dataList.Add(new("MAC Address", MACAddress));

                    string MACVendor = MAC_Vendors.Items.FirstOrDefault(a => MACAddress.StartsWith(a.prefix, StringComparison.CurrentCultureIgnoreCase)).vendor;
                    dataList.Add(new("MAC Vendor", MACVendor));

                    dataList.Add(new(string.Empty, string.Empty));
                }

                if (blockSignature == (uint)ExtraDataSignature.VistaAndAboveIDListDataBlock)
                {
                    dataList.Add(new("ExtraData.VistaAndAboveIDListDataBlock.Exists", "True"));
                    dataList.Add(new("(unimplemented)", "-"));
                    dataList.Add(new(string.Empty, string.Empty));
                    lastIndex += (int)blockSize;
                }
            }

            return dataList;
        }
        
        private static void SkipStringData()
        {
            lastIndex = LinkInfo.Offset + (int)LinkInfo.LinkInfoSize.ValueAsUInt;

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasName))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    lastIndex += size * sizeof(char);
                }
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasRelativePath))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    lastIndex += size * sizeof(char);
                }
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasWorkingDir))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    lastIndex += size * sizeof(char);
                }
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasArguments))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    lastIndex += size * sizeof(char);
                }
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasIconLocation))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    lastIndex += size * sizeof(char);
                }
            }
        }
    }

    public enum ExtraDataSize : uint
    {
        ConsoleDataBlock = 0x000000CC,
        ConsoleFEDataBlock = 0x0000000C,
        DarwinDataBlock = 0x00000314,
        EnvironmentVariableDataBlock = 0x00000314,
        IconEnvironmentDataBlock = 0x00000314,
        KnownFolderDataBlock = 0x0000001C,
        PropertyStoreDataBlock = 0x0000000C,
        ShimDataBlock = 0x00000088,
        SpecialFolderDataBlock = 0x00000010,
        TrackerDataBlock = 0x00000060,
        VistaAndAboveIDListDataBlock = 0x0000000A
    }

    public enum ExtraDataSignature : uint
    {
        ConsoleDataBlock = 0xA0000002,
        ConsoleFEDataBlock = 0xA0000004,
        DarwinDataBlock = 0xA0000006,
        EnvironmentVariableDataBlock = 0xA0000001,
        IconEnvironmentDataBlock = 0xA0000007,
        KnownFolderDataBlock = 0xA000000B,
        PropertyStoreDataBlock = 0xA0000009,
        ShimDataBlock = 0xA0000008,
        SpecialFolderDataBlock = 0xA0000005,
        TrackerDataBlock = 0xA0000003,
        VistaAndAboveIDListDataBlock = 0xA000000C
    }

    public static List<(uint size, uint signature, string name)> ExtraDataSizesAndSignatures =
    [
        new(0x000000CC, 0xA0000002, "ConsoleDataBlock"),
        new(0x0000000C, 0xA0000004, "ConsoleFEDataBlock"),
        new(0x00000314, 0xA0000006, "DarwinDataBlock"),
        new(0x00000314, 0xA0000001, "EnvironmentVariableDataBlock"),
        new(0x00000314, 0xA0000007, "IconEnvironmentDataBlock"),
        new(0x0000001C, 0xA000000B, "KnownFolderDataBlock"),
        new(0x0000000C, 0xA0000009, "PropertyStoreDataBlock"),
        new(0x00000088, 0xA0000008, "ShimDataBlock"),
        new(0x00000010, 0xA0000005, "SpecialFolderDataBlock"),
        new(0x00000060, 0xA0000003, "TrackerDataBlock"),
        new(0x0000000A, 0xA000000C, "VistaAndAboveIDListDataBlock")
    ];
}