using UnityEngine;
using UnityEngine.Events;

namespace Eloi.IID
{
    public class IIDMono_SendBytesIID : MonoBehaviour {

        public string m_ntpServer = "be.pool.ntp.org";
        public string m_ipv4="127.0.0.1";
        public int m_port=3615;
        public bool m_useQueueThread;
        public SendBytesIID m_sender;

        public UnityEvent<byte[]> m_onBytesPushed;

        public void PushBytesToListener(byte[] data)
        {
            if (m_onBytesPushed != null)
                 m_onBytesPushed.Invoke(data);
        }
        void OnEnable()
        {
            m_sender = new SendBytesIID(PushBytesToListener, m_ntpServer, m_useQueueThread);
        }
        void OnDisable()
        {
            m_sender = null;
        }

        public void PushBytes(byte[] data)
        {
            m_sender.PushBytes(data);
        }
        public void PushText(string text)
        {
            m_sender.PushText(text);
        }
        public int m_defaultIndex = 0;

        [ContextMenu("PushRandomIntegerFrom0To100")]
        public void PushRandomIntegerFrom0To100()
        {
          PushBytes(IIDUtility.I(Random.Range(0, 100)));
        }
        [ContextMenu("PushRandomIntegerFromMinToMax")]

        public void PushRandomIntegerFromMinToMax()
        {
            PushBytes(IIDUtility.I(Random.Range(int.MinValue, int.MaxValue)));
        }
        [ContextMenu("PushRandomIntegerFrom0ToMax")]

        public void PushRandomIntegerFrom0ToMax()
        {
            PushBytes(IIDUtility.I(Random.Range(0, int.MaxValue)));
        }

        public void PushInteger(int integer)
        {
            PushBytes(IIDUtility.I(integer));
        }

        public void PushIntegerWithIndex(int integer)
        {
            PushBytes(IIDUtility.II(m_defaultIndex, integer));
        }

        public void PushIndexInteger(int index, int integer)
        {
            PushBytes(IIDUtility.II(index, integer));
        }


        [ContextMenu("PushRandomIntegerFrom0To100WithIndex")]
        public void PushRandomIntegerFrom0To100WithIndex()
        {
            PushBytes(IIDUtility.II(m_defaultIndex, Random.Range(0, 100)));
        }
        [ContextMenu("PushRandomIntegerFromMinToMaxWithIndex")]
        public void PushRandomIntegerFromMinToMaxWithIndex()
        {
            PushBytes(IIDUtility.II(m_defaultIndex, Random.Range(int.MinValue, int.MaxValue)));
        }
        [ContextMenu("PushRandomIntegerFrom0ToMaxWithIndex")]
        public void PushRandomIntegerFrom0ToMaxWithIndex()
        {
            PushBytes(IIDUtility.II(m_defaultIndex, Random.Range(0, int.MaxValue)));
        }

    }
}