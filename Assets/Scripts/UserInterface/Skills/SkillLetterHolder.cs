using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillLetterHolder : MonoBehaviour
{
    public Image bgImage;
    public Image fillImage;
    public TextMeshProUGUI letterText;
    public List<Sprite> lettersVocals; //  W, A , S, D
    public float maxDuration;
    public float curDuration;

    public bool startDuration = false;
    public bool letterCalled = false;

    public void Update()
    {
        if(startDuration)
        {
            curDuration += Time.deltaTime;
            if (curDuration >= maxDuration)
            {
                letterCalled = false;
                startDuration = false;
            }
            UpdateFillAmount();
        }
    }

    public void SetCurrentLetter(int idx, bool isCompleted = false, bool fillShouldMove = false)
    {
        bgImage.sprite = lettersVocals[idx];
        fillImage.sprite = lettersVocals[idx];
        switch (idx)
        {
            case 0:
                letterText.text = "W";
                break;
            case 1:
                letterText.text = "A";
                break;
            case 2:
                letterText.text = "S";
                break;
            case 3:
                letterText.text = "D";
                break;
        }
        if(isCompleted)
        {
            CompleteLetter();
        }
        else if(fillShouldMove)
        {
            startDuration = true;
        }
    }
    public void CompleteLetter()
    {
        letterCalled = true;
        curDuration = maxDuration;
        fillImage.color = new Color(0, 0.3955245f, 1, 1);
        startDuration = false;
        UpdateFillAmount();
    }

    public void SetDuration(float max)
    {
        maxDuration = max;
    }

    public void UpdateFillAmount()
    {
        fillImage.fillAmount = curDuration / maxDuration;
    }

    public void ResetLetter()
    {
        Debug.Log("Resetting Letter : " + this.gameObject.name);
        fillImage.color = new Color(1, 1, 0, 1);
        curDuration = 0;
        fillImage.fillAmount = 0;
        this.gameObject.SetActive(false);
    }
}
