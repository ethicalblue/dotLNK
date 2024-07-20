namespace dotLNKLibrary;

public sealed partial class WindowsShortcut
{
    public sealed class ShellLinkHeader
    {
        public sealed class HeaderSize
        {
            public static string ValueAsText => Data.Take(Size).Reverse().ToArray().AsHex();
            public static bool IsValid => Enumerable.SequenceEqual(Value, new byte[] { 0x4C, 0x00, 0x00, 0x00 }.Reverse());
#pragma warning disable IDE0051 // Remove unused private members
            private static int Offset => 0x00000000;
#pragma warning restore IDE0051 // Remove unused private members
            private static int Size => sizeof(int);
            private static byte[] Value => Data.Take(Size).Reverse().ToArray();
        }

        public sealed class LinkCLSID
        {
            public static string ValueAsText => new Guid(Value).ToString();
            public static bool IsValid => string.Equals(ValueAsText.ToLower(), "00021401-0000-0000-c000-000000000046".ToLower());
            private static int Offset => 0x00000004;
            private static int Size => 0x00000010;
            private static byte[] Value => Data.Take(new Range(Offset, Offset + Size)).ToArray();
        }

        public sealed class LinkFlags
        {
            public static string ValuesAsText()
            {
                string text = Value.Reverse().ToArray().AsHex() + " [";

                foreach (Flags item in Enum.GetValues(typeof(Flags)))
                {
                    if (IsLinkFlagSet(item))
                    {
                        text += Enum.GetName(item) + ", ";
                    }
                }

                return text.TrimEnd(", ".ToCharArray()) + "]";
            }
            private static int Offset => 0x00000014;
            private static int Size => sizeof(int);
            private static byte[] Value => Data.Take(new Range(Offset, Offset + Size)).ToArray();
            internal enum Flags
            {
                HasLinkTargetIDList = 0,
                HasLinkInfo = 1,
                HasName = 2,
                HasRelativePath = 3,
                HasWorkingDir = 4,
                HasArguments = 5,
                HasIconLocation = 6,
                IsUnicode = 7,
                ForceNoLinkInfo = 8,
                HasExpString = 9,
                RunInSeparateProcess = 10,
                Unused1 = 11,
                HasDarwinID = 12,
                RunAsUser = 13,
                HasExpIcon = 14,
                NoPidlAlias = 15,
                Unused2 = 16,
                RunWithShimLayer = 17,
                ForceNoLinkTrack = 18,
                EnableTargetMetadata = 19,
                DisableLinkPathTracking = 20,
                DisableKnownFolderTracking = 21,
                DisableKnownFolderAlias = 22,
                AllowLinkToLink = 23,
                UnaliasOnSave = 24,
                PreferEnvironmentPath = 25,
                HasKeepLocalIDListForUNCTargetName = 26
            }
            internal static bool IsLinkFlagSet(Flags flag) => (Value.AsInt() & (1 << (int)flag)) != 0;
        }

        public sealed class FileAttributesFlags
        {
            public static string ValuesAsText()
            {
                string text = Value.Reverse().ToArray().AsHex() + " [";

                foreach (Flags item in Enum.GetValues(typeof(Flags)))
                {
                    if (IsLinkFlagSet(item))
                    {
                        text += Enum.GetName(item) + ", ";
                    }
                }

                return text.TrimEnd(", ".ToCharArray()) + "]";
            }
            private static int Offset => 0x00000018;
            private static int Size => sizeof(int);
            private static byte[] Value => Data.Take(new Range(Offset, Offset + Size)).ToArray();
            internal enum Flags
            {
                FILE_ATTRIBUTE_READONLY = 0,
                FILE_ATTRIBUTE_HIDDEN = 1,
                FILE_ATTRIBUTE_SYSTEM = 2,
                Reserved1 = 3,
                FILE_ATTRIBUTE_DIRECTORY = 4,
                FILE_ATTRIBUTE_ARCHIVE = 5,
                Reserved2 = 6,
                FILE_ATTRIBUTE_NORMAL = 7,
                FILE_ATTRIBUTE_TEMPORARY = 8,
                FILE_ATTRIBUTE_SPARSE_FILE = 9,
                FILE_ATTRIBUTE_REPARSE_POINT = 10,
                FILE_ATTRIBUTE_COMPRESSED = 11,
                FILE_ATTRIBUTE_OFFLINE = 12,
                FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 13,
                FILE_ATTRIBUTE_ENCRYPTED = 14
            }

            internal static bool IsLinkFlagSet(Flags flag) => (Value.AsInt() & (1 << (int)flag)) != 0;
        }

        public sealed class CreationTime
        {
            public static DateTime DateTimeValue() { try { return DateTime.FromFileTime(Misc.AsLong(ValueLow, ValueHigh)); } catch (Exception) { return DateTime.MinValue; } }
            public static bool IsZero => Misc.AsLong(ValueLow, ValueHigh) == 0x0000000000000000;
            private static int Offset => 0x0000001C;
            private static int Size => sizeof(int) + sizeof(int);
            private static byte[] ValueLow => [.. Data.Take(new Range(Offset, Offset + sizeof(int)))];
            private static byte[] ValueHigh => [.. Data.Take(new Range(Offset + sizeof(int), Offset + sizeof(long)))];
        }

        public sealed class AccessTime
        {
            public static DateTime DateTimeValue() { try { return DateTime.FromFileTime(Misc.AsLong(ValueLow, ValueHigh)); } catch (Exception) { return DateTime.MinValue; } }
            public static bool IsZero => Misc.AsLong(ValueLow, ValueHigh) == 0x0000000000000000;
            private static int Offset => 0x00000024;
            private static int Size => sizeof(int) + sizeof(int);
            private static byte[] ValueLow => [.. Data.Take(new Range(Offset, Offset + sizeof(int)))];
            private static byte[] ValueHigh => [.. Data.Take(new Range(Offset + sizeof(int), Offset + sizeof(long)))];
        }

        public sealed class WriteTime
        {
            public static DateTime DateTimeValue() { try { return DateTime.FromFileTime(Misc.AsLong(ValueLow, ValueHigh)); } catch (Exception) { return DateTime.MinValue; } }
            public static bool IsZero => Misc.AsLong(ValueLow, ValueHigh) == 0x0000000000000000;
            private static int Offset => 0x0000002C;
            private static int Size => sizeof(int) + sizeof(int);
            private static byte[] ValueLow => [.. Data.Take(new Range(Offset, Offset + sizeof(int)))];
            private static byte[] ValueHigh => [.. Data.Take(new Range(Offset + sizeof(int), Offset + sizeof(long)))];
        }

        public sealed class FileSize
        {
            public static string ValueAsText => Value.Reverse().ToArray().AsHex() + $" ({Value.AsUInt()} bytes)";
            public static string Warning => "FileSize will be truncated if greater than 4 294 967 295 (0xFFFFFFFF).";
            private static int Offset => 0x00000034;
            private static int Size => sizeof(int);
            private static byte[] Value => [.. Data.Take(new Range(Offset, Offset + Size))];
        }

        public sealed class IconIndex
        {
            public static int ValueAsInt => Value.AsInt();
            private static int Offset => 0x00000038;
            private static int Size => sizeof(int);
            private static byte[] Value => [.. Data.Take(new Range(Offset, Offset + Size))];
        }

        public sealed class ShowCommand
        {
            public static uint ValueAsUInt => Value.AsUInt();
            public static string ValueAsText()
            {
                if(ValueAsUInt == 0x00000001)
                {
                    return "SW_SHOWNORMAL (0x00000001)";
                }
                if (ValueAsUInt == 0x00000003)
                {
                    return "SW_SHOWMAXIMIZED (0x00000003)";
                }
                if (ValueAsUInt == 0x00000007)
                {
                    return "SW_SHOWMINNOACTIVE (0x00000007)";
                }

                return "SW_SHOWNORMAL";
            }
            private static int Offset => 0x0000003C;
            private static int Size => sizeof(int);
            private static byte[] Value => [.. Data.Take(new Range(Offset, Offset + Size))];
        }
        public sealed class HotKey
        {
            public static string LowByteAsText() => Value[1] switch
            {
                0x00 => $"0x00",
                0x30 => $"0 (0x{Value[1]:X2})",
                0x31 => $"1 (0x{Value[1]:X2})",
                0x32 => $"2 (0x{Value[1]:X2})",
                0x33 => $"3 (0x{Value[1]:X2})",
                0x34 => $"4 (0x{Value[1]:X2})",
                0x35 => $"5 (0x{Value[1]:X2})",
                0x36 => $"6 (0x{Value[1]:X2})",
                0x37 => $"7 (0x{Value[1]:X2})",
                0x38 => $"8 (0x{Value[1]:X2})",
                0x39 => $"9 (0x{Value[1]:X2})",
                0x41 => $"A (0x{Value[1]:X2})",
                0x42 => $"B (0x{Value[1]:X2})",
                0x43 => $"C (0x{Value[1]:X2})",
                0x44 => $"D (0x{Value[1]:X2})",
                0x45 => $"E (0x{Value[1]:X2})",
                0x46 => $"F (0x{Value[1]:X2})",
                0x47 => $"G (0x{Value[1]:X2})",
                0x48 => $"H (0x{Value[1]:X2})",
                0x49 => $"I (0x{Value[1]:X2})",
                0x4A => $"J (0x{Value[1]:X2})",
                0x4B => $"K (0x{Value[1]:X2})",
                0x4C => $"L (0x{Value[1]:X2})",
                0x4D => $"M (0x{Value[1]:X2})",
                0x4E => $"N (0x{Value[1]:X2})",
                0x4F => $"O (0x{Value[1]:X2})",
                0x50 => $"P (0x{Value[1]:X2})",
                0x51 => $"Q (0x{Value[1]:X2})",
                0x52 => $"R (0x{Value[1]:X2})",
                0x53 => $"S (0x{Value[1]:X2})",
                0x54 => $"T (0x{Value[1]:X2})",
                0x55 => $"U (0x{Value[1]:X2})",
                0x56 => $"V (0x{Value[1]:X2})",
                0x57 => $"W (0x{Value[1]:X2})",
                0x58 => $"X (0x{Value[1]:X2})",
                0x59 => $"Y (0x{Value[1]:X2})",
                0x5A => $"Z (0x{Value[1]:X2})",
                0x70 => $"F1 (0x{Value[1]:X2})",
                0x71 => $"F2 (0x{Value[1]:X2})",
                0x72 => $"F3 (0x{Value[1]:X2})",
                0x73 => $"F4 (0x{Value[1]:X2})",
                0x74 => $"F5 (0x{Value[1]:X2})",
                0x75 => $"F6 (0x{Value[1]:X2})",
                0x76 => $"F7 (0x{Value[1]:X2})",
                0x77 => $"F8 (0x{Value[1]:X2})",
                0x78 => $"F9 (0x{Value[1]:X2})",
                0x79 => $"F10 (0x{Value[1]:X2})",
                0x7A => $"F11 (0x{Value[1]:X2})",
                0x7B => $"F12 (0x{Value[1]:X2})",
                0x7C => $"F13 (0x{Value[1]:X2})",
                0x7D => $"F14 (0x{Value[1]:X2})",
                0x7E => $"F15 (0x{Value[1]:X2})",
                0x7F => $"F16 (0x{Value[1]:X2})",
                0x80 => $"F17 (0x{Value[1]:X2})",
                0x81 => $"F18 (0x{Value[1]:X2})",
                0x82 => $"F19 (0x{Value[1]:X2})",
                0x83 => $"F20 (0x{Value[1]:X2})",
                0x84 => $"F21 (0x{Value[1]:X2})",
                0x85 => $"F22 (0x{Value[1]:X2})",
                0x86 => $"F23 (0x{Value[1]:X2})",
                0x87 => $"F24 (0x{Value[1]:X2})",
                0x90 => $"NUM LOCK (0x{Value[1]:X2})",
                0x91 => $"SCROLL LOCK (0x{Value[1]:X2})",
                _ => $"0x{Value[1]:X2}"
            };
            public static string HighByteAsText() => Value[0] switch
            {
                0x00 => "0x00",
                0x01 => $"SHIFT (0x{Value[0]:X2})",
                0x02 => $"CTRL (0x{Value[0]:X2})",
                0x04 => $"ALT (0x{Value[0]:X2})",
                _ => $"0x{Value[0]:X2}"
            };
            private static int Offset => 0x00000040;
            private static int Size => 2;
            private static byte[] Value => [.. Data.Take(new Range(Offset, Offset + Size)).Reverse()];
        }

        public sealed class Reserved1
        {
            public static string ValueAsText => Value.Reverse().ToArray().AsHex();
            private static int Offset => 0x00000042;
            private static int Size => sizeof(short);
            private static byte[] Value => [.. Data.Take(new Range(Offset, Offset + Size))];

        }
        public sealed class Reserved2
        {
            public static string ValueAsText => Value.Reverse().ToArray().AsHex();
            private static int Offset => 0x00000044;
            private static int Size => sizeof(int);
            private static byte[] Value => [.. Data.Take(new Range(Offset, Offset + Size))];

        }
        public sealed class Reserved3
        {
            public static string ValueAsText => Value.Reverse().ToArray().AsHex();
            private static int Offset => 0x00000048;
            private static int Size => sizeof(int);
            private static byte[] Value => [.. Data.Take(new Range(Offset, Offset + Size))];
        }
    }
}