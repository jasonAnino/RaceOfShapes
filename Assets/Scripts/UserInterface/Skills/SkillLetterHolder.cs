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

    public void SetCurrentLetter(int idx)
    {
        bgImage.sprite = lettersVocals[idx];
        fillImage.sprite = lettersVocals[idx];
    }
    public void CompleteLetter()
    {
        letterCalled = true;
        curDuration = maxDuration;
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
}
