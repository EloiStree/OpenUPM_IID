namespace Eloi.IID { 


using System;
using System.Collections.Generic;
using System.Threading;

public class IntegerTimeQueueHolder
{
    public class WaitingShortcut
    {
        public byte[] HoldBytes { get; }
        public long LocalTimeCreated { get; }
        public long LocalTimeToExecute { get; }

        public WaitingShortcut(byte[] holdBytes, long timeInMilliseconds, int delayInMilliseconds)
        {
            HoldBytes = holdBytes;
            LocalTimeCreated = timeInMilliseconds;
            LocalTimeToExecute = timeInMilliseconds + delayInMilliseconds;
        }

        public bool IsReady(long currentTime)
        {
            return currentTime >= LocalTimeToExecute;
        }
    }

    public class QueueOfShortcuts
    {
        private List<WaitingShortcut> list = new List<WaitingShortcut>();

        public void AppendAt0(WaitingShortcut shortcut)
        {
            list.Insert(0, shortcut);
        }

        public bool HasWaitingBytes()
        {
            return list.Count > 0;
        }

        public List<WaitingShortcut> CheckForBytesToExtract(long currentTime)
        {
            List<WaitingShortcut> result = new List<WaitingShortcut>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].IsReady(currentTime))
                {
                    result.Add(list[i]);
                    list.RemoveAt(i);
                }
            }
            return result;
        }

        public void Clear()
        {
            list.Clear();
        }
    }

    public class BytesActionDelegate
    {
        private Action<byte[]> byteHandler;

        public BytesActionDelegate(Action<byte[]> byteHandler)
        {
            this.byteHandler = byteHandler;
        }

        public void OutOfQueue(byte[] bytesToPush)
        {
            Console.WriteLine("Bytes Out Of Queue: " + BitConverter.ToString(bytesToPush));
            byteHandler(bytesToPush);
        }
    }

    private QueueOfShortcuts inQueueBytes = new QueueOfShortcuts();
    private long currentTime;
    private BytesActionDelegate handleAction;

    public IntegerTimeQueueHolder(BytesActionDelegate handleAction, int checkTimeInMilliseconds)
    {
        this.handleAction = handleAction;
        StartLoopInThread(checkTimeInMilliseconds);
    }

    public void PushBytesToQueueAtLocalTime(byte[] holdBytes, long timeInMilliseconds, int delayInMilliseconds)
    {
        inQueueBytes.AppendAt0(new WaitingShortcut(holdBytes, timeInMilliseconds, delayInMilliseconds));
        Console.WriteLine("Pushed Bytes To Queue: " + BitConverter.ToString(holdBytes) + " " + inQueueBytes.HasWaitingBytes());
    }

    public void PushBytesToQueue(byte[] holdBytes, int delayInMilliseconds)
    {
        PushBytesToQueueAtLocalTime(holdBytes, GetTimeInMilliseconds(), delayInMilliseconds);
    }

    private void StartLoopInThread(int timeInWaitingMilliseconds)
    {
        Console.WriteLine("Start Loop In Thread");
        Thread t = new Thread(() => LoopForThreadWithTime(timeInWaitingMilliseconds));
        t.Start();
        Console.WriteLine("Thread Started");
    }

    private void LoopForThreadWithTime(int timeInWaitingMilliseconds)
    {
        Console.WriteLine("Loop For Thread With Time");
        double waitingTimeSeconds = timeInWaitingMilliseconds / 1000.0;
        while (true)
        {
            CheckTheQueueForShortcuts();
            Thread.Sleep((int)(waitingTimeSeconds * 1000));
        }
    }

    public void ClearQueue()
    {
        inQueueBytes.Clear();
    }

    private void CheckTheQueueForShortcuts()
    {
        if (!inQueueBytes.HasWaitingBytes())
            return;

        currentTime = GetTimeInMilliseconds();
        foreach (var shortcut in inQueueBytes.CheckForBytesToExtract(currentTime))
        {
            byte[] bytesStore = shortcut.HoldBytes;
            Console.WriteLine("Extracted Bytes From Queue: " + BitConverter.ToString(bytesStore) + " " + inQueueBytes.HasWaitingBytes());
            handleAction.OutOfQueue(bytesStore);
        }
    }

    private static long GetTimeInMilliseconds()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
}
