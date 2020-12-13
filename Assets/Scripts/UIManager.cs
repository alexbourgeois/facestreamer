using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject settings;

    public void ToggleSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }
}
