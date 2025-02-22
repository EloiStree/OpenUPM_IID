using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Allows to make guest none auth game where the index is should by the client.
/// </summary>
public class IIDMono_ByteIndexOverrdideIndexRelay : MonoBehaviour
{

    public UnityEvent<byte[]> m_onRelayBytes;

    public int m_randomIndex;
    public string m_playerPrefsIndexName="PlayerRandomIndex";
    public byte[] m_lastPush;

    private void Awake()
    {
        m_randomIndex = Random.Range(0, int.MinValue);
    }
    private void OnEnable()
    {
        m_randomIndex= PlayerPrefs.GetInt(m_playerPrefsIndexName, m_randomIndex);
        PlayerPrefs.SetInt(m_playerPrefsIndexName, m_randomIndex);
    }

    [ContextMenu("Set new random index")]
    public void OverrideIndexRandomly() {

        m_randomIndex = Random.Range(0, int.MinValue);
        PlayerPrefs.SetInt(m_playerPrefsIndexName, m_randomIndex);
    }

    public void PushBytesIn(byte [] bytesArray)
    {
        if (bytesArray != null) {

            if (bytesArray.Length == 8) {

                bytesArray[0] = (byte)(m_randomIndex & 0xFF);
                bytesArray[1] = (byte)((m_randomIndex >> 8) & 0xFF);
                bytesArray[2] = (byte)((m_randomIndex >> 16) & 0xFF);
                bytesArray[3] = (byte)((m_randomIndex >> 24) & 0xFF);
            }

            if (bytesArray.Length == 16)
            {
                //Set four frist bytes to index as little endian
                bytesArray[0] = (byte)(m_randomIndex & 0xFF);
                bytesArray[1] = (byte)((m_randomIndex >> 8) & 0xFF);
                bytesArray[2] = (byte)((m_randomIndex >> 16) & 0xFF);
                bytesArray[3] = (byte)((m_randomIndex >> 24) & 0xFF);
            }
        }
        m_lastPush= bytesArray;
        m_onRelayBytes.Invoke(bytesArray);
    }
}
