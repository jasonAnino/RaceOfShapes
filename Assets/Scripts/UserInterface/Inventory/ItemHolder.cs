using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemHolder : MonoBehaviour
{
    public Image imageHolder;
    public bool followMouse;
    // Update is called once per frame
    void Update()
    {
        if(followMouse)
        imageHolder.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

    }
    public void CopyImageToholder(Sprite thisSprite)
    {
        imageHolder.sprite = thisSprite;
    }

    public void StartFollowMouse()
    {
        //Debug.Log("Start Following Mouse!");
        followMouse = true;
        imageHolder.color = new Color(1,1,1,1);
    }

    public void StopFollowMouse()
    {
        followMouse = false;
        imageHolder.color = new Color(1, 1, 1, 0);
    }
}
