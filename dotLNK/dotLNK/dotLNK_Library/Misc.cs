using System.Text.RegularExpressions;
using System.Text;

namespace dotLNKLibrary;

public static class Misc
{
    public static long AsLong(byte[] low, byte[] high)
    {
        return (long)high.AsUInt() << sizeof(int) * 0x08 | low.AsUInt();
    }

    public static unsafe double Entropy(byte[] data)
    {
        int* a = stackalloc int[0x100];
        int* b = a + 0x100;

        for (int i = data.Length; --i >= 0;)
            a[data[i]]++;

        double h = 0.00;
        double c = data.Length;
        while (--b >= a)
            if (*b > 0)
                h += *b * Math.Log(*b / c, 2.00);

        return -h / c;
    }
}

public static class Extensions
{
    public static string AsHexDump(this byte[] bytes)
    {
        string dump = string.Empty;

        foreach (var item in bytes)
        {
            dump += $"{item:X2} ";
        }

        return dump;
    }

    public static string AsHtmlHexDump(this byte[] bytes)
    {
        string dump = string.Empty;

        foreach (var item in bytes)
        {
            if (item == 0x00)
            {
                dump += $"<span style=\"color: #aaaaaa;\">{item:X2}</span> ";
            }
            else if (item > 0x7F)
            {
                dump += $"<span style=\"color: #00{item:X2}FF;\">{item:X2}</span> ";
            }
            else
            {
                dump += $"<span style=\"color: #0097ff;\">{item:X2}</span> ";
            }
        }

        return dump;
    }

    public static string AsTextDump(this byte[] bytes)
    {
        string dump = string.Empty;

        foreach (char item in bytes)
        {
            if (char.IsLetterOrDigit(item) || char.IsPunctuation(item) || char.IsSymbol(item))
            {
                dump += item;
            }
            else
            {
                dump += $" ";
            }
        }

        return dump;
    }

    public static string AsFilteredTextDump(this byte[] bytes)
    {
        string dump = Encoding.Unicode.GetString(bytes);

        return Regex.Replace(dump, $"[^{(char)0x20}-~]+", string.Empty);
    }
    public static string AsFilteredTextDump(this List<byte> bytes)
    {
        string dump = Encoding.Unicode.GetString(bytes.ToArray());

        return Regex.Replace(dump, $"[^{(char)0x20}-~]+", string.Empty);
    }

    public static string AsHex(this byte[] bytes)
    {
        return "0x" + Convert.ToHexString(bytes);
    }

    public static string AsHexNo0x(this byte[] bytes)
    {
        return Convert.ToHexString(bytes);
    }
    public static string AsHex(this short num)
    {
        byte[] bytes = [.. BitConverter.GetBytes(num).Reverse()];

        return "0x" + Convert.ToHexString(bytes);
    }
    public static string AsHex(this ushort num)
    {
        byte[] bytes = [.. BitConverter.GetBytes(num).Reverse()];

        return "0x" + Convert.ToHexString(bytes);
    }
    public static string AsHex(this int num)
    {
        byte[] bytes = [.. BitConverter.GetBytes(num).Reverse()];

        return "0x" + Convert.ToHexString(bytes);
    }
    public static string AsHex(this uint num)
    {
        byte[] bytes = [.. BitConverter.GetBytes(num).Reverse()];

        return "0x" + Convert.ToHexString(bytes);
    }
    public static short AsShort(this byte[] bytes)
    {
        if (bytes == null || bytes.Length < 2)
            return 0;

        return BitConverter.ToInt16(bytes);
    }
    public static ushort AsUShort(this byte[] bytes)
    {
        if (bytes == null || bytes.Length < 2)
            return 0;

        return BitConverter.ToUInt16(bytes);
    }
    public static int AsInt(this byte[] bytes)
    {
        if (bytes == null || bytes.Length < 4)
            return 0;

        return BitConverter.ToInt32(bytes);
    }
    public static uint AsUInt(this byte[] bytes)
    {
        if (bytes == null || bytes.Length < 4)
            return 0;

        return BitConverter.ToUInt32(bytes);
    }
    public static long AsLong(this byte[] bytes)
    {
        if (bytes == null || bytes.Length < 8)
            return 0;

        return BitConverter.ToInt64(bytes);
    }
    public static ulong AsULong(this byte[] bytes)
    {
        if (bytes == null || bytes.Length < 8)
            return 0;

        return BitConverter.ToUInt64(bytes);
    }
    public static byte[] AsBytes(this int num)
    {
        return [.. BitConverter.GetBytes(num)];
    }
    public static byte[] AsBytes(this uint num)
    {
        return [.. BitConverter.GetBytes(num)];
    }
}