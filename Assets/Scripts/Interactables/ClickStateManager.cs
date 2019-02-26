using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClickState
{
    Idle = 0,
    DragBoxSelect= 1,
    UIClick =2,
}
public class ClickStateManager : MonoBehaviour
{
    private static ClickStateManager instance;
    public  static ClickStateManager GetInstance {  get { return instance; } }

    public ClickState currentState;

    public void Awake()
    {
        instance = this;
    }
}
