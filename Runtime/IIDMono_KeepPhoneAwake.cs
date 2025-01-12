using UnityEngine;

public class IIDMono_KeepPhoneAwake : MonoBehaviour
{
    void OnEnable()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
