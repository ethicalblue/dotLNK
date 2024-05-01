namespace dotLNKLibrary;

public sealed partial class WindowsShortcut
{
    public sealed class ExtraData
    {
        public static List<string> ExtractEssentialData()
        {
            List<string> dataList = [];

            #region Skip StringData

            int lastIndex = LinkInfo.Offset + (int)LinkInfo.LinkInfoSize.ValueAsUInt;

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

            #endregion

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
    }
}