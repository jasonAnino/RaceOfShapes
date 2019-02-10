using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitsScripts.Behaviour;
using WorldObjectScripts.Behavior;

public enum PooledObjectName
{
    VisualTexts = 0,
}
/// <summary>
/// Instead of pushing forward in instantiating, use this manager to create stuff prior to the game start
/// </summary>
public class GameObjectPoolManager : MonoBehaviour
{
    private static GameObjectPoolManager instance;
    public static GameObjectPoolManager GetInstance
    {
        get { return instance; }
    }

    public void Awake()
    {
        instance = this;
    }

    public GameObject[] pooledObjects;
 
    public List<List<GameObject>> generatedPooledObjects = new List<List<GameObject>>();

    public void GenerateSpecificPoolObject(PooledObjectName pooledObject, GameObject thisGameObject, int count)
    {
              
        switch (pooledObject)
        {
            case PooledObjectName.VisualTexts:
                CreateVisualTexts(count, thisGameObject);
                break;
        }
    }

    private void CreateVisualTexts(int count, GameObject owner)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject tmp = GameObject.Instantiate(pooledObjects[0], owner.transform.position, Quaternion.identity);
            InGameVisualText text = tmp.GetComponent<InGameVisualText>();
            text.Hide();
        }
    }
}
