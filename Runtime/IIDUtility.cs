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

        public static void BytesToIndexDate(byte[] bytes, out int index, out ulong date)
        {
            index = BitConverter.ToInt32(bytes, 0);
            date = BitConverter.ToUInt64(bytes, 4);
        }

        public static void BytesToIndexIntegerDate(byte[] bytes, out int index, out int value, out ulong date)
        {
            index = BitConverter.ToInt32(bytes, 0);
            value = BitConverter.ToInt32(bytes, 4);
            date = BitConverter.ToUInt64(bytes, 8);
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

    public static byte[] IndexIntegerDateToBytes(int index, int value, ulong dateTimeStampNtpUtc)
    {
        byte[] indexBytes = BitConverter.GetBytes(index);
        byte[] valueBytes = BitConverter.GetBytes(value);
        byte[] dateBytes = BitConverter.GetBytes(dateTimeStampNtpUtc);
        return indexBytes.Concat(valueBytes).Concat(dateBytes).ToArray();
    }

        public static byte[] IndexIntegerNowRelayMillisecondsToBytesLocal(int index, int value, int delayInMilliseconds)
        {

            IIDUtility.GetTimestampLocalMillisecondsUtc(out ulong adjustedTimeMilliseconds);
            adjustedTimeMilliseconds += (ulong)delayInMilliseconds;
            return IndexIntegerDateToBytes(index, value, adjustedTimeMilliseconds);
        }
        public static byte[] IndexIntegerNowRelayMillisecondsToBytesNTP(int index, int value, int delayInMilliseconds)
        {

            IIDUtility.GetTimestampLocalMillisecondsUtcGlobalNTP(out ulong adjustedTimeMilliseconds);
            adjustedTimeMilliseconds += (ulong)delayInMilliseconds;
            return IndexIntegerDateToBytes(index, value, adjustedTimeMilliseconds);
        }

        public static void GetIIDFrom(byte[] iidBytes, out bool isValide, out int index, out int integer, out ulong date)
        {
            int length = iidBytes.Length;
            isValide = false;
            index = 0;
            integer = 0;
            date = 0;
            if (length == 4)
            {
                integer = BytesToInt(iidBytes);
                isValide = true;
            }
            else if (length == 8)
            {
                BytesToIndexInteger(iidBytes, out index, out integer);
                isValide = true;
            }
            else if (length == 12)
            {
                BytesToIndexDate(iidBytes, out index, out date);
                isValide = true;
            }
            else if (length == 16)
            {
                BytesToIndexIntegerDate(iidBytes, out index, out integer, out date);
                isValide = true;
            }

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
                return IndexIntegerNowRelayMillisecondsToBytesNTP(index, integer, delay);
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
                    return IndexIntegerNowRelayMillisecondsToBytesNTP(index, integer, delay);
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

        public static byte[] IID(int index, int integerValue, ulong date)
        {
            return IndexIntegerDateToBytes(index, integerValue, date);
        }

        public static byte[] IID(int index, int integerValue, long date)
        {
            return IndexIntegerDateToBytes(index, integerValue, (ulong)date);
        }

        public static byte[] IID_MS(int index, int integerValue, int milliseconds)
        {
            GetTimestampLocalMillisecondsUtcGlobalNTP(out ulong timestampLocalMillisecondsUtcNTP);
            timestampLocalMillisecondsUtcNTP += (ulong)milliseconds;
            return IndexIntegerDateToBytes(index, integerValue, timestampLocalMillisecondsUtcNTP);
        }
        public static void GetTimestampLocalMillisecondsUtcGlobalNTP(out long timestampLocalMillisecondsUtcNTP)
        {
            timestampLocalMillisecondsUtcNTP = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() +GetDefaultGlobalNtpOffsetInMilliseconds();
        }
        public static void GetTimestampLocalMillisecondsUtc(out long timestampLocalMillisecondsUtc)
        {
            timestampLocalMillisecondsUtc = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
        public static void GetTimestampLocalMillisecondsUtcGlobalNTP(out ulong timestampLocalMillisecondsUtcNTP)
        {
            GetTimestampLocalMillisecondsUtcGlobalNTP(out long timestampLocalMillisecondsUtc);
            timestampLocalMillisecondsUtcNTP = (ulong)timestampLocalMillisecondsUtc;
          }
        public static void GetTimestampLocalMillisecondsUtc(out ulong timestampLocalMillisecondsUtc)
        {
            GetTimestampLocalMillisecondsUtc(out long timestampLocalMillisecondsUtcLong);
            timestampLocalMillisecondsUtc = (ulong)timestampLocalMillisecondsUtcLong;
          }
        public void GetTimestampLocalMillisecondsNTP(int offsetMilliseconds, int offsetManualAdjustement, out long timestampLocalMilliseconds)
        {
            GetTimestampLocalMillisecondsUtc(out timestampLocalMilliseconds);
            timestampLocalMilliseconds += offsetMilliseconds;
            timestampLocalMilliseconds += offsetManualAdjustement;

        }
        public void GetTimestampLocalMillisecondsNTP(int offsetMilliseconds, out long timestampLocalMilliseconds)
        {
            GetTimestampLocalMillisecondsUtc(out timestampLocalMilliseconds);
            timestampLocalMilliseconds += offsetMilliseconds;
        }

    }
}