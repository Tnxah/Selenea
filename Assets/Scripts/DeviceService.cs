using System;
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

    public static event Action<bool> onChargingChangedCallBack;
    public static event Action<bool> onNetworkChangedCallBack;
    public static event Action<float> onBrigtnessChangedCallBack;
    public static event Action<float> onVolumeChangedCallBack;

    private void Awake()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        audioManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "audio");
    }
    private void FixedUpdate()
    {
        ManageChanges();
    }

    private void ManageChanges()
    {
        if (isCharging != IsCharging())
        {
            onChargingChangedCallBack?.Invoke(isCharging);  
        }

        if (hasNetworkConnection != HasNetworkConnection())
        {
            onNetworkChangedCallBack?.Invoke(hasNetworkConnection);
        }

        if (brightness != GetBrightnessLevel())
        {
            onBrigtnessChangedCallBack?.Invoke(brightness);
        }
        
        if (volume != GetVolumeLevel())
        {
            onVolumeChangedCallBack?.Invoke(volume);
        }
    }

    private bool IsCharging()
    {
        isCharging = SystemInfo.batteryStatus == BatteryStatus.Charging;
        return isCharging;
    }

    private bool HasNetworkConnection()
    {
        hasNetworkConnection = Application.internetReachability != NetworkReachability.NotReachable;
        return hasNetworkConnection;
    }

    private float GetBrightnessLevel()
    {
        brightness = Screen.brightness;
        return brightness;
    }

    private float GetVolumeLevel()
    {
        var currentVolume = audioManager.Call<int>("getStreamVolume", 3);
        var maxVolume = audioManager.Call<int>("getStreamMaxVolume", 3);

        volume = (float)currentVolume / maxVolume * 100f;

        return volume;
    }
}
