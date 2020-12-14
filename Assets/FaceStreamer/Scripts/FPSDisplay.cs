using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public bool showFPS = true;
    public Color textColor = Color.white;
    public int offset = 10;
    public int fontSize = 15;
    public float fps;
    public Text debugText;

    private GUIStyle _style;

    private void Start()
    {
        _style = new GUIStyle();
    }

    public void ToggleFPSDisplay()
    {
        showFPS = !showFPS;
    }
    void Update()
    {
        //    QualitySettings.vSyncCount = 0;
        //  Application.targetFrameRate = 60;
        fps = (1.0f / Time.smoothDeltaTime);

        if (debugText)
            debugText.text = fps.ToString("F2");

        if (Input.GetKeyDown("f"))
            ToggleFPSDisplay();
    }
    void OnGUI()
    {
        if (showFPS) {
            _style.fontSize = fontSize;
            _style.normal.textColor = textColor;
            
            GUI.Label(new Rect(offset, offset, 100, 100), fps.ToString("F1"), _style);
        }
    }
}