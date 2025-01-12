
using Eloi.IID;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IIDUI2DMono_TextLog : MonoBehaviour
{

    [TextArea(0,10)]
    public string m_loggerText = "";
    public int m_maxLines = 10;
    public bool m_appendToTop = false;

    public List<string> m_linesReceived = new List<string>();
    public UnityEvent<string> m_onLogChanged;


    public void Push(STRUCT_ReceivedIID received) { 
    
        long difference = (long)received.receivedTimestampUTCNTP - (long)received.date;
        string line = $"IID[{received.index}] = {received.integer} @ {received.date} @ {received.receivedTimestampUTCNTP} (D{difference})";
        if (m_appendToTop )
            m_linesReceived.Insert(0, line);
        else
            m_linesReceived.Add(line);

        if (m_linesReceived.Count > m_maxLines)
            if (m_appendToTop)
                m_linesReceived.RemoveAt(m_linesReceived.Count - 1);
            else
                m_linesReceived.RemoveAt(0);

        m_loggerText = string.Join("\n", m_linesReceived.ToArray());
        m_onLogChanged.Invoke(m_loggerText);
    }

    public void Push(byte[] iidBytes) {

        IIDUtility.GetIIDFrom(iidBytes,out bool isValide, out int index, out int integer, out ulong date);
        
        if (!isValide)
            return;
        string line = $"IID[{index}] = {integer} @ {date}";
        if (m_appendToTop )
            m_linesReceived.Insert(0, line);
        else
            m_linesReceived.Add(line);

        if (m_linesReceived.Count > m_maxLines)
            if (m_appendToTop)
                m_linesReceived.RemoveAt(m_linesReceived.Count - 1);
            else
                m_linesReceived.RemoveAt(0);

        m_loggerText = string.Join("\n", m_linesReceived.ToArray());
        m_onLogChanged.Invoke(m_loggerText);
        
    }
}
