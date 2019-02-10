using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TextColour
{
    RED = 0,
    GREEN = 1,
    ORANGE = 2,
};
public class InGameVisualText : BasePoolComponent
{
    public TextMeshPro counterText;
    public List<Color> colorsAvailable;
    public bool startShowing = false;
    public float showDuration = 0.5f;
    public float curDuration = 0.0f;
    public UnitPoolHolder parent;
    public bool moveUp = false;
    public float moveSpeed = 10f;
    public float defSpeed = 10f;
    public void Initialize(UnitPoolHolder newParent)
    {
        parent = newParent;
        Hide();
    }

    public void Update()
    {
        if(startShowing)
        {
            if(curDuration > showDuration)
            {
                startShowing = false;
                curDuration = 0;
                Hide();
            }
            curDuration += Time.deltaTime;
            if(moveUp)
            {
                transform.localPosition = new Vector3(0, transform.localPosition.y +(moveSpeed * Time.deltaTime), transform.localPosition.z);
            }
            float zPos= (parent.parentVar.rotation.eulerAngles.y) * -1;
            transform.localRotation = Quaternion.Euler(-90, 180, zPos);
            counterText.gameObject.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
    public void ShowUp(float count, TextColour counterColor = TextColour.RED, float duration = 5.0f)
    {
        InitialShowUp(duration);
        moveUp = true;
        counterText.color = colorsAvailable[(int)counterColor];
        counterText.text = count.ToString();
    }
    public void ShowUp(string text, TextColour counterColor = TextColour.RED, float duration = 5.0f)
    {
        InitialShowUp(duration);
        counterText.color = colorsAvailable[(int)counterColor];
        counterText.text = text;
    }

    private void InitialShowUp(float duration)
    {
        showDuration = duration;
        startShowing = true;
        counterText.gameObject.SetActive(true);
    }
    public void Hide()
    {
        parent.RemoveThisFromCurVisibleVisuals(this);
        transform.localPosition = Vector3.zero; 
        counterText.gameObject.SetActive(false);
    }
}
