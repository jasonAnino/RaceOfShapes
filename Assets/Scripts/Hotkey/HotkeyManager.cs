using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Utilities;

[Serializable]
public class Hotkey
{
    public string tabName;
    public TabComponent thisComponent;
    public KeyCode key;
}
public class HotkeyManager : MonoBehaviour
{
    private static HotkeyManager instance;
    public static HotkeyManager GetInstance
    {
        get { return instance; }
    }

    public void Awake()
    {
        instance = this;
    }
    public List<Hotkey> hotkey = new List<Hotkey>();
    

    public void AddHotkey(Hotkey thiskey)
    {
        if(hotkey.Contains(thiskey))
        {
            return;
        }

        hotkey.Add(thiskey);
    }
}
