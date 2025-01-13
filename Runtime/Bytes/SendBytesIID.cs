
using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Eloi.IID
{

    [System.Serializable]
public class SendBytesIID
{
        [SerializeField] private int m_ntpOffsetLocalToServerInMilliseconds;
        [SerializeField] private int m_ntpOffsetLocalToServerInMillisecondsManualAdjustment;
        [SerializeField] Action<byte[]> m_bytesPusherHandler;
         private IntegerTimeQueueHolder m_queueThread;

    public SendBytesIID(Action<byte[]> handler, int ntpOffset=0, bool useQueueThread = false)
    {

         this.m_bytesPusherHandler = handler;
         this.m_ntpOffsetLocalToServerInMilliseconds = ntpOffset;

         if (useQueueThread)
         {
                IntegerTimeQueueHolder.BytesActionDelegate d =
                    new IntegerTimeQueueHolder.BytesActionDelegate(handler);
             this.m_queueThread = new IntegerTimeQueueHolder(d, 1);
         }
    }

    public int GetNtpOffset()
    {
        return this.m_ntpOffsetLocalToServerInMilliseconds + m_ntpOffsetLocalToServerInMillisecondsManualAdjustment;
    }

    public void PushIntegerAsShortcut(string text)
    {
        byte[] bytes = IIDUtility.TextShortcutToBytes(text);
        if (bytes != null)
        {
                this.PushBytes(bytes);
        }
    }

    public void PushBytes(byte[] bytes)
    {
      if (this.m_bytesPusherHandler != null)       
        this.m_bytesPusherHandler.Invoke(bytes);
            
    }

    public void PushText(string text)
    {
        PushBytes(Encoding.UTF8.GetBytes(text));
    }

    public void PushInteger(int value)
    {
        PushBytes(IIDUtility.IntegerToBytes(value));
    }

    public void PushIndexInteger(int index, int value)
    {
        PushBytes(IIDUtility.IndexIntegerToBytes(index, value));
    }

        public void PushIndexIntegerDate(int index, int value, long dateTimeStampNtpUtc)
        {
            PushBytes(IIDUtility.IndexIntegerDateToBytes(index, value, (ulong)dateTimeStampNtpUtc));
        }
        public void PushIndexIntegerDate(int index, int value, ulong dateTimeStampNtpUtc)
        {
            PushBytes(IIDUtility.IndexIntegerDateToBytes(index, value,dateTimeStampNtpUtc));
        }

        public void PushRandomInteger(int index, int fromValue, int toValue)
    {
        System.Random random = new System.Random();
        int value = random.Next(fromValue, toValue);
        PushIndexInteger(index, value);
    }

    public void PushRandomInteger100(int index)
    {
        PushRandomInteger(index, 0, 100);
    }

    public void PushRandomIntegerIntMax(int index)
    {
        PushRandomInteger(index, int.MinValue, int.MaxValue);
    }

    

        public void SetNtpOffsetTick(int ntpOffsetLocalToServer)
        {
            this.m_ntpOffsetLocalToServerInMilliseconds = ntpOffsetLocalToServer;
        }

       


        public void PushIndexIntegerDateLocalNow(int index, int value)
        {
            IIDUtility. GetTimestampLocalMillisecondsUtc(out long timestampLocalMilliseconds);
            PushIndexIntegerDate(index, value, timestampLocalMilliseconds);
        }
        public void GetTimestampMillisecondsUtcNtp(out long timestampMilliseconds)
        {
            IIDUtility.GetTimestampLocalMillisecondsUtc(out long timestampLocalMilliseconds);
            timestampMilliseconds = timestampLocalMilliseconds 
            + this.m_ntpOffsetLocalToServerInMilliseconds
            + this.m_ntpOffsetLocalToServerInMillisecondsManualAdjustment;

        }
        public void PushIndexIntegerDateNtpNow(int index, int value)
        {
                  GetTimestampMillisecondsUtcNtp(out long timestampLocalMilliseconds);
                PushIndexIntegerDate(index, value, timestampLocalMilliseconds);
        }

    public void PushIndexIntegerDateNtpInMilliseconds(int index, int value, int milliseconds)
        {
            GetTimestampMillisecondsUtcNtp(out long timestampLocalMilliseconds);
            PushIndexIntegerDate(index, value, timestampLocalMilliseconds + milliseconds);
    }

        public void PushIndexIntegerDateNtpInSeconds(int index, int value, int seconds)
        {
        PushIndexIntegerDateNtpInMilliseconds(index, value, seconds * 1000);
        }
     
        public bool IsUsingQueueThread()
        {
            return this.m_queueThread != null;
        }

        public void PushIntegerInQueue(int value, int delayInMilliseconds)
        {
            this.m_queueThread.PushBytesToQueue(IIDUtility.IntegerToBytes(value), delayInMilliseconds);
        }

        public void PushIndexIntegerInQueue(int index, int value, int delayInMilliseconds)
        {
            this.m_queueThread.PushBytesToQueue(IIDUtility.IndexIntegerToBytes(index, value), delayInMilliseconds);
        }

        public void ClearQueue()
        {
            this.m_queueThread.ClearQueue();
        }

    }
}