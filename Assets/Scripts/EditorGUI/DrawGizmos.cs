using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float sRad = 1.15f;
        Gizmos.DrawSphere(transform.position, sRad);
    }
}
