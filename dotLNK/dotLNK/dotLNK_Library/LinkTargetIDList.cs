namespace dotLNKLibrary;

public sealed partial class WindowsShortcut
{
    public sealed class LinkTargetIDList
    {
        public static bool Exists => ShellLinkHeader.LinkFlags.IsLinkFlagSet(ShellLinkHeader.LinkFlags.Flags.HasLinkTargetIDList);
        public static int Offset => 0x0000004C;
        public static int Size => Exists ? IDListSize.Value + sizeof(short) : 0x00000000;
        public sealed class IDListSize
        {
            internal static int Value => Exists ? Data.Take(new Range(Offset, Offset + Size)).ToArray().AsUShort() : 0x00000000;
            private static int Offset => 0x0000004C;
            private static int Size => Exists ? sizeof(short) : 0;
        }
        public sealed class IDList
        {
            public static List<string> Items()
            {
                List<string> itemIDList = [];

                if (Exists == false)
                    return [];

                int index = 0x0000004E;

                int itemIDSize = Data.Take(new Range(index, index + sizeof(short))).ToArray().AsShort();

                byte[] itemData = [];

                if (itemIDSize == 0x14)
                {
                    string clsid = new Guid([.. Data.Take(new Range(index, index + itemIDSize)).Skip(sizeof(int))]).ToString().ToLower();

                    var clsidName = CLSID.Items.FirstOrDefault(i => i.Value.Contains(clsid)).Key;

                    itemIDList.Add($"CLSID: {clsid} ({clsidName})");

                    index += itemIDSize;
                }
                
                while (index < IDListSize.Value)
                {
                    itemIDSize = Data.Take(new Range(index, index + sizeof(short))).ToArray().AsShort();
                    itemData = Data.Take(new Range(index, index + itemIDSize)).ToArray();

                    itemIDList.Add($"Raw Data ({itemIDSize} bytes)" + Environment.NewLine + itemData.AsHexDump());
                    itemIDList.Add("ASCII: " + itemData.AsTextDump());

                    index += itemIDSize;
                }
                return itemIDList;
            }
        }
    }
}