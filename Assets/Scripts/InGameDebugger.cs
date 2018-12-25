using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameDebugger : MonoBehaviour {

    private static InGameDebugger instance;
    public static InGameDebugger GetInstance
    { get { return instance;  }  }

    public Text notif;

    public bool showNotifMesg = false;
    public float timer = 0.0f;
    public float mesgDuration = 5.0f;

    public void Awake()
    {
        instance = this;
    }
    public void Update()
    {
        if(showNotifMesg)
        {
            timer += Time.deltaTime;
            if(timer > mesgDuration)
            {
                notif.enabled = false;
            }
        }
    }

    public void NotifyInGame(string mesg)
    {
        notif.text = mesg;
        showNotifMesg = true;
    }
}
