using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// TabComponent Streamlines the behaviour of all potential Tabs.
/// </summary>
[RequireComponent(typeof(Animation))]
public class TabComponent : MonoBehaviour
{
    public string tabName;
    public Animation myAnim;
    public KeyCode key;
    private Hotkey myHotkey;
    public bool isOpen = false;

    public virtual void Start()
    {
        CreateMyHotKey();

        if(myAnim == null)
        {
            myAnim = this.GetComponent<Animation>();
        }
    }
    public virtual void Update()
    {
        if (Input.GetKeyDown(key))
            SwitchTab();
    }
    public virtual void CreateMyHotKey()
    {
        myHotkey = new Hotkey();
        myHotkey.key = key;
        myHotkey.thisComponent = this;
        myHotkey.tabName = this.gameObject.name;

        if(HotkeyManager.GetInstance != null)
        {
            HotkeyManager.GetInstance.AddHotkey(myHotkey);
        }
    }

    public virtual void SwitchTab()
    {
        if (isOpen)
            CloseTab();
        else
            OpenTab();
    }
    public virtual void OpenTab()
    {
        myAnim.Play(tabName + "_Open");
        isOpen = true;
    }
    public virtual void CloseTab()
    {
        myAnim.Play(tabName + "_Close");
        isOpen = false;
    }
}
