using System.Text;

namespace dotLNKLibrary;

public sealed partial class WindowsShortcut
{
    public sealed class LinkInfo
    {
        public static int Offset => LinkTargetIDList.Exists ? LinkTargetIDList.Offset + LinkTargetIDList.Size : 0x0000004C;
        public static bool Exists => ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasLinkInfo);
        public sealed class LinkInfoSize
        {
            private static int Size => sizeof(int);
            private static int Offset => LinkInfo.Offset;
            public static uint ValueAsUInt => Exists ? Data.Take(new Range(Offset, Offset + Size)).ToArray().AsUInt() : 0x00000000;
        }
        public sealed class LinkInfoHeaderSize
        {
            private static int Size => sizeof(int);
            private static int Offset => LinkInfo.Offset + sizeof(int);
            public static uint ValueAsUInt => Exists ? Data.Take(new Range(Offset, Offset + Size)).ToArray().AsUInt() : 0x00000000;
            public static string Information()
            {
                if (ValueAsUInt == 0x0000001C)
                {
                    return "Offsets to the optional fields are not specified. (LocalBasePathOffsetUnicode and CommonPathSuffixOffsetUnicode)";
                }
                else if (ValueAsUInt >= 0x00000024)
                {
                    return "Offsets to the optional fields are specified. (LocalBasePathOffsetUnicode and CommonPathSuffixOffsetUnicode)";
                }
                else
                {
                    return "...";
                }
            }
        }
        public sealed class LinkInfoFlags
        {
            public static string ValuesAsText()
            {
                string text = Data.Take(new Range(Offset, Offset + Size)).Reverse().ToArray().AsHex() + " [";

                foreach (Flags item in Enum.GetValues(typeof(Flags)))
                {
                    if (IsLinkFlagSet(item))
                    {
                        text += Enum.GetName(item) + ", ";
                    }
                }

                return text.TrimEnd(", ".ToCharArray()) + "]";
            }
            private static int Offset => LinkInfo.Offset + (2 * sizeof(int));
            private static int Size => sizeof(int);
            private static uint Value => Data.Take(new Range(Offset, Offset + Size)).ToArray().AsUInt();
            internal enum Flags
            {
                VolumeIDAndLocalBasePath = 0,
                CommonNetworkRelativeLinkAndPathSuffix = 1
            }

            internal static bool IsLinkFlagSet(Flags flag) => (Value & (1 << (int)flag)) != 0;
        }
        public sealed class VolumeID
        {
            public static string DriveType()
            {
                if (Exists == false)
                    return string.Empty;

                var drive = Data.Take(new Range(Offset, Offset + sizeof(int))).ToArray().AsUInt();

                if (drive == 0x00000000)
                    return "DRIVE_UNKNOWN (0x00000000)";
                else if (drive == 0x00000001)
                    return "DRIVE_NO_ROOT_DIR (0x00000001)";
                else if (drive == 0x00000002)
                    return "DRIVE_REMOVABLE (0x00000002)";
                else if (drive == 0x00000003)
                    return "DRIVE_FIXED (0x00000003)";
                else if (drive == 0x00000004)
                    return "DRIVE_REMOTE (0x00000004)";
                else if (drive == 0x00000005)
                    return "DRIVE_CDROM (0x00000005)";
                else if (drive == 0x00000006)
                    return "DRIVE_RAMDISK (0x00000006)";
                else
                    return $"0x{drive:X8}";

            }
            public static string DriveSerialNumber()
            {
                if (Exists == false)
                    return string.Empty;

                var bytes = Data.Take(new Range(Offset + sizeof(int), Offset + (2 * sizeof(int)))).Reverse().ToArray();

                string dump = string.Empty;

                for (int i = 0; i < bytes.Length; i++)
                {
                    dump += $"{bytes[i]:x2}";
                    if (i == (bytes.Length - 1) / 2)
                        dump += "-";
                }

                return dump;
            }
            public static string VolumeLabel()
            {
                if (Exists == false)
                    return string.Empty;

                int rel = Data.Take(new Range(Offset + (2 * sizeof(int)), Offset + (3 * sizeof(int)))).ToArray().AsInt();

                if (rel == 0x00000014)
                {
                    return string.Empty;
                }
                else
                {
                    var size = Data.Take(new Range(Offset - sizeof(int), Offset)).ToArray().AsInt();

                    var bytes = Data.Take(new Range(Offset + rel - sizeof(int), Offset + size - sizeof(int))).ToArray();

                    return Encoding.ASCII.GetString(bytes);
                }
            }
            public static string VolumeLabelUnicode()
            {
                if (Exists == false)
                    return string.Empty;

                int rel = Data.Take(new Range(Offset + (2 * sizeof(int)), Offset + (3 * sizeof(int)))).ToArray().AsInt();

                if (rel == 0x00000014)
                {
                    rel = Data.Take(new Range(Offset + (3 * sizeof(int)), Offset + (4 * sizeof(int)))).ToArray().AsInt();

                    var size = Data.Take(new Range(Offset - sizeof(int), Offset)).ToArray().AsInt();

                    var bytes = Data.Take(new Range(Offset + rel - sizeof(int), Offset + size - sizeof(int))).ToArray();

                    return Encoding.Unicode.GetString(bytes);
                }
                else
                {
                    return string.Empty;
                }
            }
            private static bool Exists => LinkInfoFlags.IsLinkFlagSet(LinkInfoFlags.Flags.VolumeIDAndLocalBasePath);
            internal static int Size => Exists ? Data.Take(new Range(Offset - sizeof(int), Offset)).ToArray().AsInt() : 0x00000000;
            private static int Offset => LinkInfoHeaderSize.ValueAsUInt >= 0x00000024 ? LinkInfo.Offset + (10 * sizeof(uint)) : LinkInfo.Offset + (8 * sizeof(uint));
        }
        public sealed class LocalBasePath
        {
            public static string Value()
            {
                if(Exists == false)
                {
                    return string.Empty;
                }
                else
                {
                    int rel = 0;

                    var offset = Offset + VolumeID.Size + (8 * sizeof(uint));

                    foreach (var item in Data.Skip(offset - sizeof(int)))
                    {
                        if (item == 0x00)
                            break;
                        else
                            rel++;
                    }

                    var bytes = Data.Take(new Range(offset - sizeof(int), offset - sizeof(int) + rel)).ToArray();

                    return Encoding.ASCII.GetString(bytes);
                }
            }
            public static bool Exists => LinkInfoFlags.IsLinkFlagSet(LinkInfoFlags.Flags.VolumeIDAndLocalBasePath);
        }
        public sealed class LocalBasePathUnicode
        {
            public static string Value()
            {
                if (Exists == false)
                {
                    return string.Empty;
                }
                else
                {
                    int rel = 0;

                    var offset = Offset + VolumeID.Size + (10 * sizeof(uint));

                    foreach (var item in Data.Skip(offset - sizeof(int)))
                    {
                        if (item == 0x00)
                            break;
                        else
                            rel++;
                    }

                    var bytes = Data.Take(new Range(offset - sizeof(int), offset - sizeof(int) + rel)).ToArray();

                    return Encoding.Unicode.GetString(bytes);
                }
            }
            public static bool Exists => LinkInfoFlags.IsLinkFlagSet(LinkInfoFlags.Flags.VolumeIDAndLocalBasePath) && LinkInfoHeaderSize.ValueAsUInt >= 0x00000024;
        }
        public sealed class CommonNetworkRelativeLink
        {
            public static bool Exists => LinkInfoFlags.IsLinkFlagSet(LinkInfoFlags.Flags.CommonNetworkRelativeLinkAndPathSuffix);
            private static int Offset => LinkInfo.Offset + Data.Take(new Range(LinkInfo.Offset + (5 * sizeof(int)), LinkInfo.Offset + (6 * sizeof(int)))).ToArray().AsInt();

            public sealed class CommonNetworkRelativeLinkSize
            {
                private static int Size => sizeof(int);
                private static int Offset => CommonNetworkRelativeLink.Offset;
                public static uint ValueAsUInt => Exists ? Data.Take(new Range(Offset, Offset + Size)).ToArray().AsUInt() : 0x00000000;
            }

            public sealed class CommonNetworkRelativeLinkFlags
            {
                public static string ValuesAsText()
                {
                    string text = Data.Take(new Range(Offset, Offset + Size)).Reverse().ToArray().AsHex() + " [";

                    foreach (Flags item in Enum.GetValues(typeof(Flags)))
                    {
                        if (IsLinkFlagSet(item))
                        {
                            text += Enum.GetName(item) + ", ";
                        }
                    }

                    return text.TrimEnd(", ".ToCharArray()) + "]";
                }
                private static int Offset => CommonNetworkRelativeLink.Offset + sizeof(int);
                private static int Size => sizeof(int);
                private static uint Value => Data.Take(new Range(Offset, Offset + Size)).ToArray().AsUInt();
                internal enum Flags
                {
                    ValidDevice = 0,
                    ValidNetType = 1
                }

                internal static bool IsLinkFlagSet(Flags flag) => (Value & (1 << (int)flag)) != 0;
            }
            public static int NetNameOffset => Data.Take(new Range(Offset + (2 * sizeof(int)), Offset + (3 * sizeof(int)))).ToArray().AsInt();
            public static int DeviceNameOffset => Data.Take(new Range(Offset + (3 * sizeof(int)), Offset + (4 * sizeof(int)))).ToArray().AsInt();
            public static (string, string) NetworkProviderType()
            {
                var provider = Data.Take(new Range(Offset + (4 * sizeof(int)), Offset + (5 * sizeof(int)))).ToArray().AsUInt();

                if (provider == 0x001A0000)
                    return new("WNNC_NET_AVID", $"0x{provider:X8}");
                else if (provider == 0x00020000)
                    return new("WNNC_NET_LANMAN", $"0x{provider:X8}");
                else if (provider == 0x001B0000)
                    return new("WNNC_NET_DOCUSPACE", $"0x{provider:X8}");
                else if (provider == 0x001C0000)
                    return new("WNNC_NET_MANGOSOFT", $"0x{provider:X8}");
                else if (provider == 0x001D0000)
                    return new("WNNC_NET_SERNET", $"0x{provider:X8}");
                else if (provider == 0x001E0000)
                    return new("WNNC_NET_RIVERFRONT1", $"0x{provider:X8}");
                else if (provider == 0x001F0000)
                    return new("WNNC_NET_RIVERFRONT2", $"0x{provider:X8}");
                else if (provider == 0x00200000)
                    return new("WNNC_NET_DECORB", $"0x{provider:X8}");
                else if (provider == 0x00210000)
                    return new("WNNC_NET_PROTSTOR", $"0x{provider:X8}");
                else if (provider == 0x00220000)
                    return new("WNNC_NET_FJ_REDIR", $"0x{provider:X8}");
                else if (provider == 0x00230000)
                    return new("WNNC_NET_DISTINCT", $"0x{provider:X8}");
                else if (provider == 0x00240000)
                    return new("WNNC_NET_TWINS", $"0x{provider:X8}");
                else if (provider == 0x00250000)
                    return new("WNNC_NET_RDR2SAMPLE", $"0x{provider:X8}");
                else if (provider == 0x00260000)
                    return new("WNNC_NET_CSC", $"0x{provider:X8}");
                else if (provider == 0x00270000)
                    return new("WNNC_NET_3IN1", $"0x{provider:X8}");
                else if (provider == 0x00290000)
                    return new("WNNC_NET_EXTENDNET", $"0x{provider:X8}");
                else if (provider == 0x002A0000)
                    return new("WNNC_NET_STAC", $"0x{provider:X8}");
                else if (provider == 0x002B0000)
                    return new("WNNC_NET_FOXBAT", $"0x{provider:X8}");
                else if (provider == 0x002C0000)
                    return new("WNNC_NET_YAHOO", $"0x{provider:X8}");
                else if (provider == 0x002D0000)
                    return new("WNNC_NET_EXIFS", $"0x{provider:X8}");
                else if (provider == 0x002E0000)
                    return new("WNNC_NET_DAV", $"0x{provider:X8}");
                else if (provider == 0x002F0000)
                    return new("WNNC_NET_KNOWARE", $"0x{provider:X8}");
                else if (provider == 0x00300000)
                    return new("WNNC_NET_OBJECT_DIRE", $"0x{provider:X8}");
                else if (provider == 0x00310000)
                    return new("WNNC_NET_MASFAX", $"0x{provider:X8}");
                else if (provider == 0x00320000)
                    return new("WNNC_NET_HOB_NFS", $"0x{provider:X8}");
                else if (provider == 0x00330000)
                    return new("WNNC_NET_SHIVA", $"0x{provider:X8}");
                else if (provider == 0x00340000)
                    return new("WNNC_NET_IBMAL", $"0x{provider:X8}");
                else if (provider == 0x00350000)
                    return new("WNNC_NET_LOCK", $"0x{provider:X8}");
                else if (provider == 0x00360000)
                    return new("WNNC_NET_TERMSRV", $"0x{provider:X8}");
                else if (provider == 0x00370000)
                    return new("WNNC_NET_SRT", $"0x{provider:X8}");
                else if (provider == 0x00380000)
                    return new("WNNC_NET_QUINCY", $"0x{provider:X8}");
                else if (provider == 0x00390000)
                    return new("WNNC_NET_OPENAFS", $"0x{provider:X8}");
                else if (provider == 0x003A0000)
                    return new("WNNC_NET_AVID1", $"0x{provider:X8}");
                else if (provider == 0x003B0000)
                    return new("WNNC_NET_DFS", $"0x{provider:X8}");
                else if (provider == 0x003C0000)
                    return new("WNNC_NET_KWNP", $"0x{provider:X8}");
                else if (provider == 0x003D0000)
                    return new("WNNC_NET_ZENWORKS", $"0x{provider:X8}");
                else if (provider == 0x003E0000)
                    return new("WNNC_NET_DRIVEONWEB", $"0x{provider:X8}");
                else if (provider == 0x003F0000)
                    return new("WNNC_NET_VMWARE", $"0x{provider:X8}");
                else if (provider == 0x00400000)
                    return new("WNNC_NET_RSFX", $"0x{provider:X8}");
                else if (provider == 0x00410000)
                    return new("WNNC_NET_MFILES", $"0x{provider:X8}");
                else if (provider == 0x00420000)
                    return new("WNNC_NET_MS_NFS", $"0x{provider:X8}");
                else if (provider == 0x00430000)
                    return new("WNNC_NET_GOOGLE", $"0x{provider:X8}");
                else
                    return new("?", $"0x{provider:X8}");
            }
            public static int NetNameOffsetUnicode => NetNameOffset > 0x00000014 ? Data.Take(new Range(Offset + (5 * sizeof(int)), Offset + (6 * sizeof(int)))).ToArray().AsInt() : 0x00000000;
            public static int DeviceNameOffsetUnicode => NetNameOffset > 0x00000014 ? Data.Take(new Range(Offset + (6 * sizeof(int)), Offset + (7 * sizeof(int)))).ToArray().AsInt() : 0x00000000;
            public static string NetName()
            {
                if (NetNameOffset == 0x00000000)
                    return string.Empty;

                int rel = 0;

                var offset = Offset + NetNameOffset;

                foreach (var item in Data.Skip(offset))
                {
                    if (item == 0x00)
                        break;
                    else
                        rel++;
                }

                var bytes = Data.Take(new Range(Offset + NetNameOffset, Offset + NetNameOffset + rel)).ToArray();

                return Encoding.ASCII.GetString(bytes);
            }
            public static string NetNameUnicode()
            {
                if (NetNameOffsetUnicode == 0x00000000)
                    return string.Empty;

                int rel = 0;

                var offset = Offset + NetNameOffsetUnicode;

                foreach (var item in Data.Skip(offset))
                {
                    if (item == 0x00)
                        break;
                    else
                        rel++;
                }

                var bytes = Data.Take(new Range(offset, offset + rel)).ToArray();

                return Encoding.Unicode.GetString(bytes);
            }
            public static string DeviceName()
            {
                if (DeviceNameOffset == 0x00000000)
                    return string.Empty;

                int rel = 0;

                var offset = Offset + DeviceNameOffset;

                foreach (var item in Data.Skip(offset))
                {
                    if (item == 0x00)
                        break;
                    else
                        rel++;
                }

                var bytes = Data.Take(new Range(offset, offset + rel)).ToArray();

                return Encoding.ASCII.GetString(bytes);
            }
            public static string DeviceNameUnicode()
            {
                if (DeviceNameOffsetUnicode == 0x00000000)
                    return string.Empty;

                int rel = 0;

                var offset = Offset + DeviceNameOffsetUnicode;

                foreach (var item in Data.Skip(offset))
                {
                    if (item == 0x00)
                        break;
                    else
                        rel++;
                }

                var bytes = Data.Take(new Range(offset, offset + rel)).ToArray();

                return Encoding.ASCII.GetString(bytes);
            }
        }
    }
}