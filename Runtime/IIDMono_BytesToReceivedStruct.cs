using UnityEngine;
using UnityEngine.Events;


public struct STRUCT_ReceivedIID { 

    public int index;
    public int integer;
    public ulong date;
    public ulong receivedTimestampUTCNTP;
}

public class IIDMono_BytesToReceivedStruct : MonoBehaviour
{
    public int m_offsetMillisecondsNTP= 0;
    public UnityEvent<STRUCT_ReceivedIID> m_onReceived;


    public void SetNtpOffsetMilliseconds(long offsetMillisecondsNTP)
    {
        this.m_offsetMillisecondsNTP = (int) offsetMillisecondsNTP;
    }

    public void PushIn(byte[] bytes) { 
    

        STRUCT_ReceivedIID iid = new STRUCT_ReceivedIID();
        Eloi.IID.IIDUtility.GetIIDFrom(bytes,
            out bool isValide,
            out iid.index, 
            out iid.integer,
            out iid.date
            );
        if (!isValide)
            return;

        Eloi.IID.IIDUtility.GetTimestampLocalMillisecondsUtc(out long timestamp);
        timestamp += m_offsetMillisecondsNTP;
        iid.receivedTimestampUTCNTP = (ulong)timestamp;
        m_onReceived.Invoke(iid);
        
    }
    
}
