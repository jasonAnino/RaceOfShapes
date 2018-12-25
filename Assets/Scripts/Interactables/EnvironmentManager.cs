using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InteractableScripts.Behavior;
using WorldObjectScripts.Behavior;

public class EnvironmentManager : MonoBehaviour
{
    private static EnvironmentManager instance;
    public static EnvironmentManager GetInstance
    {
        get { return instance;  }
    }

    public List<GameObject> interactablePrefabs = new List<GameObject>();


    public void Awake()
    {
        instance = this;
    }

    
}
