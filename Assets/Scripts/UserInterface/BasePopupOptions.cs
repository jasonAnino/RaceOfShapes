using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using InteractableScripts.Behavior;

namespace UserInterface
{
    [RequireComponent(typeof(Button))]
    public class BasePopupOptions : MonoBehaviour
    {
       public TextMeshProUGUI textHolder;
        public ActionType currentAction;

        public void SetTextHolder(ActionType newOption)
        {
            textHolder.text = newOption.ToString();
            currentAction = newOption;
        }
    }
}
