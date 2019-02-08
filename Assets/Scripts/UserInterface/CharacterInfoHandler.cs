using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnitStats;
using Utilities;

public class CharacterInfoHandler : MonoBehaviour
{
    public Image hpBar;
    public CharacterStatsSystem unitStats;
    public Text charName;
    public Text title;
    public bool isUpdating = false;
    
    public void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.UPDATE_UNIT_HEALTH, UpdateHealthBar);
    }
    public void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.UPDATE_UNIT_HEALTH, UpdateHealthBar);
    }
    public void Initialize(CharacterStatsSystem stats)
    {
        unitStats = stats;
        charName.text = stats.name;
        title.text = stats.title;
        UpdateHealthBar();
    }
    public void Update()
    {
        
    }
    public void UpdateHealthBar(Parameters p = null)
    {
        hpBar.fillAmount = GetHpFill();
    }

    public float GetHpFill()
    {
        float fill = 0;

        if (unitStats == null)
        {
            return 0;
        }
        float  tmp = unitStats.health_C / unitStats.health_M;
        fill = tmp;
        
        return fill;
    }

    public void ClearHandler()
    {
        hpBar.fillAmount = 1;
        unitStats = null;
    }
}
