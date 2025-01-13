using UnityEngine;
using UnityEngine.Events;

public class IIDMono_SplitReceivedIID : MonoBehaviour
{
    public UnityEvent<int> m_onInteger;
    public UnityEvent<int, int> m_onIndexInteger;
    public UnityEvent<int, int, ulong> m_onIndexIntegerDate;
    public UnityEvent<int, int, ulong, ulong> m_onIndexIntegerDateReceived;

    public void PushIn(STRUCT_ReceivedIID iid)
    {
        m_onInteger.Invoke(iid.integer);
        m_onIndexInteger.Invoke(iid.index, iid.integer);
        m_onIndexIntegerDate.Invoke(iid.index, iid.integer, iid.date);
        m_onIndexIntegerDateReceived.Invoke(iid.index, iid.integer, iid.date, iid.receivedTimestampUTCNTP);
    }

}
