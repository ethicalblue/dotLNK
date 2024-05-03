using System.Text;

namespace dotLNKLibrary;

public sealed partial class WindowsShortcut
{
    public sealed class StringData
    {
        public static List<(string, string)> Items()
        {
            if (ShellLinkHeader.HeaderSize.IsValid == false)
                return [];

            List<(string, string)> dataList = [];

            int index = LinkInfo.Offset + (int)LinkInfo.LinkInfoSize.ValueAsUInt;

            int lastIndex = index;

            if(ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasName))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    List<byte> bytes = Data.Take(new Range(lastIndex, lastIndex + (size * sizeof(char)))).ToList();

                    dataList.Add(new("StringData.NameString", Encoding.Unicode.GetString(bytes.ToArray())));

                    lastIndex += size * sizeof(char);
                }
            }
            else
            {
                dataList.Add(new("StringData.NameString", string.Empty));
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasRelativePath))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    List<byte> bytes = Data.Take(new Range(lastIndex, lastIndex + (size * sizeof(char)))).ToList();

                    dataList.Add(new("StringData.RelativePath", Encoding.Unicode.GetString(bytes.ToArray())));

                    lastIndex += size * sizeof(char);
                }
            }
            else
            {
                dataList.Add(new("StringData.RelativePath", string.Empty));
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasWorkingDir))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    List<byte> bytes = Data.Take(new Range(lastIndex, lastIndex + (size * sizeof(char)))).ToList();

                    dataList.Add(new("StringData.WorkingDir", Encoding.Unicode.GetString(bytes.ToArray())));

                    lastIndex += size * sizeof(char);
                }
            }
            else
            {
                dataList.Add(new("StringData.WorkingDir", string.Empty));
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasArguments))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    List<byte> bytes = Data.Take(new Range(lastIndex, lastIndex + (size * sizeof(char)))).ToList();

                    dataList.Add(new("StringData.CommandLineArguments", Encoding.Unicode.GetString(bytes.ToArray())));

                    lastIndex += size * sizeof(char);
                }
            }
            else
            {
                dataList.Add(new("StringData.CommandLineArguments", string.Empty));
            }

            if (ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasIconLocation))
            {
                int size = Data.Take(new Range(lastIndex, lastIndex + sizeof(short))).ToArray().AsShort();

                lastIndex += sizeof(short);

                if (size > 0)
                {
                    List<byte> bytes = Data.Take(new Range(lastIndex, lastIndex + (size * sizeof(char)))).ToList();

                    dataList.Add(new("StringData.IconLocation", Encoding.Unicode.GetString(bytes.ToArray())));

                    _ = size * sizeof(char);
                }
            }
            else
            {
                dataList.Add(new("StringData.IconLocation", string.Empty));
            }

            return dataList;
        }
    }
}