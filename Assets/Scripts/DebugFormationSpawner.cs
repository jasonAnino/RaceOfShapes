using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Purpose is to check if the position clicked is navigable
///  Only Responsibility is to spawn his children to specific positions to visually tell the positions
///  of the generated Vectors
/// </summary>
public class DebugFormationSpawner : MonoBehaviour
{
    public GameObject vectorBall;


    public List<GameObject> currentBalls = new List<GameObject>();
    public void PlaceVectorBalls(int ballCount, List<Vector3> vectorPositions)
    {
        if(currentBalls.Count > 0)
        {
            for (int i = 0; i < currentBalls.Count; i++)
            {
                Destroy(currentBalls[i]);
            }
        }

        currentBalls.Clear();
        for (int i = 0; i < vectorPositions.Count; i++)
        {
            GameObject tmp = Instantiate(vectorBall, vectorPositions[i], Quaternion.identity, null);
            currentBalls.Add(tmp);
        }
    }
}
