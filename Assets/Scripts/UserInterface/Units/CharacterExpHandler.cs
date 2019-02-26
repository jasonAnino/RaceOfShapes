using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitsScripts;
using UnitsScripts.Behaviour;
public class CharacterExpHandler : MonoBehaviour
{
    public Image xpBar;

    public void UpdateExperience(float currentExperience, float maxExperience)
    {
        if (xpBar == null)
            return;

        xpBar.fillAmount = currentExperience / maxExperience;
    }
}
