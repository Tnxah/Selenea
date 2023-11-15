using TMPro;
using UnityEngine;

public class DeviceService : MonoBehaviour
{
    [SerializeField]
    private bool isCharging;
    [SerializeField]
    private bool hasNetworkConnection;
    [SerializeField]
    private float brightness;
    [SerializeField]
    private float volume;

    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject audioManager;
    private AndroidJavaObject currentActivity;

    private void Awake()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        audioManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "audio");
    }
    private void FixedUpdate()
    {
        
    }

    public bool IsCharging()
    {
        isCharging = SystemInfo.batteryStatus == BatteryStatus.Charging;
        return isCharging;
    }

    public bool HasNetworkConnection()
    {
        hasNetworkConnection = Application.internetReachability != NetworkReachability.NotReachable;
        return hasNetworkConnection;
    }

    public float GetBrightnessLevel()
    {
        brightness = Screen.brightness;
        return brightness;
    }

    public float GetVolumeLevel()
    {
        var currentVolume = audioManager.Call<int>("getStreamVolume", 3);
        var maxVolume = audioManager.Call<int>("getStreamMaxVolume", 3);

        volume = (float)currentVolume / maxVolume * 100f;

        return volume;
    }
}
