namespace Eloi.IID
{ 
using System.Security.Cryptography.X509Certificates;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

    public class ListenBytesIID
    {
        private int ntpOffsetInMilliseconds;
        private int manualAdjustmentSourceToLocalNtpOffsetInMilliseconds;
        private int integerToSyncNtp;

        public Action<int> OnReceiveInteger { get; set; }
        public Action<int, int> OnReceiveIndexInteger { get; set; }
        public Action<int, int, int> OnReceiveIndexIntegerDate { get; set; }
        public Action<int, int> OnReceivedIntegerDate { get; set; }

       

        public ListenBytesIID(string ivp4, int port, int ntpOffsetInMilliseconds = 0, int integerToSyncNtp = 1259)
        {

            this.ntpOffsetInMilliseconds = ntpOffsetInMilliseconds;
            this.integerToSyncNtp = integerToSyncNtp;


            OnReceiveInteger = DebugReceivedInteger;
            OnReceiveIndexInteger = DebugReceivedIndexInteger;
            OnReceiveIndexIntegerDate = DebugReceivedIndexIntegerDate;
            OnReceivedIntegerDate = DebugReceivedIntegerDate;

          
        }

        private void DebugReceivedInteger(int value)
        {
            Console.WriteLine($"Received Integer: {value}");
        }

        private void DebugReceivedIndexInteger(int index, int value)
        {
            Console.WriteLine($"Received Index Integer: {index} {value}");
        }

        private void DebugReceivedIntegerDate(int value, int date)
        {
            long time = GetNtpTimeInMillisecondsWithManualAdjustment();
            Console.WriteLine($"Received Integer Date: {value} {date} vs {time} dif {time - date}");
        }

        private void DebugReceivedIndexIntegerDate(int index, int value, int date)
        {
            long time = GetNtpTimeInMillisecondsWithManualAdjustment();
            Console.WriteLine($"Received Index Integer Date: {index} {value} {date} vs {time} dif {time - date}");
        }

        private long GetNtpTimeInMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + ntpOffsetInMilliseconds;
        }

        private long GetNtpTimeInMillisecondsWithManualAdjustment()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + ntpOffsetInMilliseconds - manualAdjustmentSourceToLocalNtpOffsetInMilliseconds;
        }

        private void NotifyInteger(int value)
        {
            OnReceiveInteger?.Invoke(value);
        }

        private void NotifyIndexInteger(int index, int value)
        {
            OnReceiveIndexInteger?.Invoke(index, value);
        }

        private void NotifyIndexIntegerDate(int index, int value, int date)
        {
            OnReceiveIndexIntegerDate?.Invoke(index, value, date);
        }

        private void NotifyIntegerDate(int value, int date)
        {
            OnReceivedIntegerDate?.Invoke(value, date);
        }

        private bool IsIntegerSyncNtpRequest(int value)
        {
            return integerToSyncNtp != 0 && value == integerToSyncNtp;
        }

        private void RequestToSyncNtp(int millisecondsSource, long millisecondsLocal)
        {
            int diffSourceToLocal = (int)(millisecondsLocal - millisecondsSource);
            manualAdjustmentSourceToLocalNtpOffsetInMilliseconds = diffSourceToLocal;
        }

        private void ParseBytesReceived(byte[] data)
        {


            if (data == null) return;

            int size = data.Length;
            if (size == 4)
            {
                int value = BitConverter.ToInt32(data, 0);
                NotifyInteger(value);
            }
            else if (size == 8)
            {
                int index = BitConverter.ToInt32(data, 0);
                int value = BitConverter.ToInt32(data, 4);
                NotifyIndexInteger(index, value);
            }
            else if (size == 12)
            {
                int value = BitConverter.ToInt32(data, 0);
                int date = BitConverter.ToInt32(data, 4);
                if (IsIntegerSyncNtpRequest(value))
                {
                    RequestToSyncNtp(date, GetNtpTimeInMilliseconds());
                }
                NotifyIntegerDate(value, date);
            }
            else if (size == 16)
            {
                int index = BitConverter.ToInt32(data, 0);
                int value = BitConverter.ToInt32(data, 4);
                int date = BitConverter.ToInt32(data, 8);
                if (IsIntegerSyncNtpRequest(value))
                {
                    RequestToSyncNtp(date, GetNtpTimeInMilliseconds());
                }
                NotifyIndexIntegerDate(index, value, date);
            }

        }
    }
}