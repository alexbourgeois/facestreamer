using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject settings;

    public Toggle oscToggle;
    public Toggle tcpToggle;

    public Text versionText;

    public bool oscEnabled = false;
    public bool tcpEnabled = false;

    public void ToggleSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }

    private void Awake()
    {
        instance = this;

        oscToggle.SetIsOnWithoutNotify(!oscToggle.isOn);
        if (PlayerPrefs.HasKey("OSC-Enable"))
        {
            oscEnabled = PlayerPrefs.GetInt("OSC-Enable") == 1;
            Debug.LogError("UIMANAGER OSC : " + oscEnabled);
        }
        oscToggle.isOn = oscEnabled;

        tcpToggle.SetIsOnWithoutNotify(!tcpToggle.isOn);
        if (PlayerPrefs.HasKey("TCP-Enable"))
        {
            tcpEnabled = PlayerPrefs.GetInt("TCP-Enable") == 1;
        }
        tcpToggle.isOn = tcpEnabled;

        versionText.text = Application.version;
    }

    public void OSCToggle(bool state)
    {
        PlayerPrefs.SetInt("OSC-Enable", state ? 1 : 0);
        oscEnabled = state;
        Debug.LogError("SET UIMANAGER OSC : " + oscEnabled);
    }

    public void TCPToggle(bool state)
    {
        PlayerPrefs.SetInt("TCP-Enable", state ? 1 : 0);
        tcpEnabled = state;
    }
}
