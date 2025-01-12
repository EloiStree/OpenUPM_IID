using System;
using System.Linq;

namespace Eloi.IID { 

public  class IIDUtility
{
    

    public static string DefaultNtpServer = "be.pool.ntp.org";
    public static int DefaultGlobalNtpOffsetInMilliseconds = 0;

    public static bool IsTextIpv4(string serverName)
    {
        var pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
        return System.Text.RegularExpressions.Regex.IsMatch(serverName, pattern);
    }

    public static string GetIpv4(string serverName)
    {
        if (IsTextIpv4(serverName))
        {
            return serverName;
        }
        var hostEntry = System.Net.Dns.GetHostEntry(serverName);
        return hostEntry.AddressList[0].ToString();
    }

    public static int GetDefaultGlobalNtpOffsetInMilliseconds()
    {
        return DefaultGlobalNtpOffsetInMilliseconds;
    }

    public static int BytesToInt(byte[] bytes)
    {
        return BitConverter.ToInt32(bytes, 0);
    }

        public static void BytesToIndexInteger(byte[] bytes, out int index, out int value)
        {
            index = BitConverter.ToInt32(bytes, 0);
            value = BitConverter.ToInt32(bytes, 4);
        }

        public static void BytesToIndexDate(byte[] bytes, out int index, out long date)
        {
            index = BitConverter.ToInt32(bytes, 0);
            date = BitConverter.ToInt64(bytes, 4);
        }

        public static void BytesToIndexIntegerDate(byte[] bytes, out int index, out int value, out long date)
        {
            index = BitConverter.ToInt32(bytes, 0);
            value = BitConverter.ToInt32(bytes, 4);
            date = BitConverter.ToInt64(bytes, 8);
        }

    public static byte[] IntegerToBytes(int value)
    {
        return BitConverter.GetBytes(value);
    }

    public static byte[] IndexIntegerToBytes(int index, int value)
    {
        byte[] indexBytes = BitConverter.GetBytes(index);
        byte[] valueBytes = BitConverter.GetBytes(value);
        return indexBytes.Concat(valueBytes).ToArray();
    }

    public static byte[] IndexIntegerDateToBytes(int index, int value, long date)
    {
        byte[] indexBytes = BitConverter.GetBytes(index);
        byte[] valueBytes = BitConverter.GetBytes(value);
        byte[] dateBytes = BitConverter.GetBytes(date);
        return indexBytes.Concat(valueBytes).Concat(dateBytes).ToArray();
    }

    public static byte[] IndexIntegerNowRelayMillisecondsToBytes(int index, int value, int delayInMilliseconds)
    {
        long currentTimeMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long adjustedTimeMilliseconds = currentTimeMilliseconds + delayInMilliseconds + DefaultGlobalNtpOffsetInMilliseconds;
        return IndexIntegerDateToBytes(index, value, adjustedTimeMilliseconds);
    }

    public static byte[] TextShortcutToBytes(string text)
    {
        try
        {
            if (text.StartsWith("i:"))
            {
                int integer = int.Parse(text.Split(':')[1]);
                return IntegerToBytes(integer);
            }
            else if (text.StartsWith("ii:"))
            {
                var parts = text.Split(':')[1].Split(',');
                int index = int.Parse(parts[0]);
                int integer = int.Parse(parts[1]);
                return IndexIntegerToBytes(index, integer);
            }
            else if (text.StartsWith("iid:"))
            {
                var parts = text.Split(':')[1].Split(',');
                int index = int.Parse(parts[0]);
                int integer = int.Parse(parts[1]);
                int delay = int.Parse(parts[2]);
                return IndexIntegerNowRelayMillisecondsToBytes(index, integer, delay);
            }
            else
            {
                var tokens = text.Replace(",", " ").Split(' ').Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
                int size = tokens.Length;
                if (size == 1)
                {
                    int integer = int.Parse(text);
                    return IntegerToBytes(integer);
                }
                else if (size == 2)
                {
                    int index = int.Parse(tokens[0]);
                    int integer = int.Parse(tokens[1]);
                    return IndexIntegerToBytes(index, integer);
                }
                else if (size == 3)
                {
                    int index = int.Parse(tokens[0]);
                    int integer = int.Parse(tokens[1]);
                    int delay = int.Parse(tokens[2]);
                    return IndexIntegerNowRelayMillisecondsToBytes(index, integer, delay);
                }
                else
                {
                    int integer = int.Parse(text);
                    return IntegerToBytes(integer);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
        return null;
    }

    public static int GetRandomInteger(int fromValue, int toValue)
    {
        Random random = new Random();
        return random.Next(fromValue, toValue);
    }

    public static int GetRandomInteger100()
    {
        return GetRandomInteger(0, 100);
    }

    public static int GetRandomIntegerIntMax()
    {
        return GetRandomInteger(int.MinValue, int.MaxValue);
    }

    public static int GetRandomIntegerIntMaxPositive()
    {
        return GetRandomInteger(0, int.MaxValue);
    }

    public static byte[] I(int integerValue)
    {
        return IntegerToBytes(integerValue);
    }

    public static byte[] II(int index, int integerValue)
    {
        return IndexIntegerToBytes(index, integerValue);
    }

    public static byte[] IID(int index, int integerValue, long date)
    {
        return IndexIntegerDateToBytes(index, integerValue, date);
    }

    public static byte[] IID_MS(int index, int integerValue, int milliseconds)
    {
        return IndexIntegerDateToBytes(index, integerValue, milliseconds);
    }
}
}