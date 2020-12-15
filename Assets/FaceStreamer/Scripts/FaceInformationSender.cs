using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using System.Linq;
using System;

public class FaceInformationSender : MonoBehaviour
{
    public FaceInformationExtractor faceExtractor;

    public string targetIp = "127.0.0.1";
    public int targetPort = 6019;

    public InputField ip;
    public InputField port;

    public void NewIP(string _ip)
    {
        if (!ValidateIPv4(_ip))
            return;

        targetIp = _ip;
        PlayerPrefs.SetString("OSC-IP", targetIp);

        Connect();
    }
    public void NewPort(string _port)
    {
        try
        {
            targetPort = Int32.Parse(_port);
        }
        catch (Exception)
        {
            return;
        }

        PlayerPrefs.SetInt("OSC-PORT", targetPort);

        Connect();
    }

    public void Connect()
    {
        if (!UIManager.instance.oscEnabled)
            return;

        if (OSCMaster.HasClient("FaceSender"))
        {
            OSCMaster.RemoveClient("FaceSender");
        }

        OSCMaster.CreateClient("FaceSender", targetIp, targetPort);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("OSC-IP"))
        {
            targetIp = PlayerPrefs.GetString("OSC-IP");
            ip.text = targetIp;
        }
        if (PlayerPrefs.HasKey("OSC-PORT"))
        {
            targetPort = PlayerPrefs.GetInt("OSC-PORT");
            port.text = targetPort.ToString();
        }

        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.instance.oscEnabled || targetIp == "127.0.0.1")
            return;

        var msg = new OSCMessage("/facePosition");
        msg.Append(faceExtractor.facePosition.x);
        msg.Append(faceExtractor.facePosition.y);
        msg.Append(faceExtractor.facePosition.z);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);

        msg = new OSCMessage("/faceDirection");
        msg.Append(faceExtractor.faceDirection.x);
        msg.Append(faceExtractor.faceDirection.y);
        msg.Append(faceExtractor.faceDirection.z);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);

        msg = new OSCMessage("/noiseTipPosition");
        msg.Append(faceExtractor.noiseTip.position.x);
        msg.Append(faceExtractor.noiseTip.position.y);
        msg.Append(faceExtractor.noiseTip.position.z);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);

        msg = new OSCMessage("/leftEyePosition");
        msg.Append(faceExtractor.leftEye.position.x);
        msg.Append(faceExtractor.leftEye.position.y);
        msg.Append(faceExtractor.leftEye.position.z);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);

        msg = new OSCMessage("/rightEyePosition");
        msg.Append(faceExtractor.rightEye.position.x);
        msg.Append(faceExtractor.rightEye.position.y);
        msg.Append(faceExtractor.rightEye.position.z);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);

        msg = new OSCMessage("/mouthPosition");
        msg.Append(faceExtractor.mouth.position.x);
        msg.Append(faceExtractor.mouth.position.y);
        msg.Append(faceExtractor.mouth.position.z);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);

        msg = new OSCMessage("/mouthHeight");
        msg.Append(faceExtractor.mouthHeight);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);

        msg = new OSCMessage("/mouthWidth");
        msg.Append(faceExtractor.mouthWidth);

        OSCMaster.SendMessageUsingClient("FaceSender", msg);


    }

    public bool ValidateIPv4(string ipString)
    {
        if (String.IsNullOrWhiteSpace(ipString))
        {
            return false;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        byte tempForParsing;

        return splitValues.All(r => byte.TryParse(r, out tempForParsing));
    }
}
