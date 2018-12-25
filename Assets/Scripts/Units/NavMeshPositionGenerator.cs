using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using UnitsScripts.Behaviour;
/// <summary>
///  Responsibility is to generate  vectors to properly give each units a proper position 
///  right after the player click
/// </summary>
public class NavMeshPositionGenerator : MonoBehaviour
{
    private static NavMeshPositionGenerator instance;
    public static NavMeshPositionGenerator GetInstance
    {
        get { return instance; }
    }

    public void Awake()
    {
        instance = this;
    }

    public List<Vector3> ObtainPositions(int count, Vector3 clickPosition, List<UnitBaseBehaviourComponent> units, float positionSpacing = 2.0f)
    {
        List<Vector3> newPositions = new List<Vector3>();
        // if there are more than 1 unit
        if(units.Count > 1)
        {
            // Check if clickedPosition is pathable by trying it on the leading unit.
            for (int i = 0; i < units.Count; i++)
            {
                // Check each unit if the clicked position is pathable
                if (CheckVectorIfPathable(units[i], clickPosition))
                {
                    if(newPositions.Contains(clickPosition))
                    {
                        newPositions.Add(GenerateCandidatePosition(clickPosition, positionSpacing, units[i]));
                    }
                    else
                    {
                        newPositions.Add(clickPosition);
                    }
                }   
                else
                {
                    Vector3 tmp = GenerateCandidatePosition(clickPosition, positionSpacing, units[i], false);
                    if(!newPositions.Contains(tmp))
                    {
                        newPositions.Add(tmp);
                    }
                    else
                    {
                        newPositions.Add(GenerateCandidatePosition(tmp, positionSpacing + 0.5f, units[i]));
                    }
                }
            }
            return newPositions;
        }
        else
        {
            // if its pathable and units is only one, no need to generate multiple paths.
            if (CheckVectorIfPathable(units[0], clickPosition))
            {
                newPositions.Add(clickPosition);
            }
            else
            {
                // Create a new Click Position based on the click Position.
                newPositions.Add(GenerateCandidatePosition(clickPosition, positionSpacing, units[0], false));

            }
            return newPositions;
        }
    }
    public Vector3 GenerateCandidatePosition(Vector3 basePosition, float spacing, UnitBaseBehaviourComponent unit, bool pathable = true)
    {
        Vector3 finalPosition;
        if(pathable)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spacing;
            randomDirection += basePosition;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, spacing, 1);
            finalPosition = hit.position;

            // Check Final Position is Pathable
            if (!CheckVectorIfPathable(unit, finalPosition))
            {
                // if not, find the nearest position that is pathable.
                finalPosition = GenerateCandidatePosition(finalPosition, spacing+0.75f, unit);
            }
        }
        else
        {
            NavMeshHit hit = new NavMeshHit();
            Vector3 storePotentialClosestPosition = new Vector3();
            if(!NavMesh.SamplePosition(basePosition, out hit, 3.25f, NavMesh.AllAreas))
            {
                storePotentialClosestPosition = hit.position;
            }
            else
            {
                NavMeshPath nav = new NavMeshPath();
                unit.gameObject.GetComponent<NavMeshAgent>().CalculatePath(basePosition, nav);
                
                if(nav.corners.Length  > 0)
                {
                    storePotentialClosestPosition = ObtainPathLastPoint(nav, basePosition);
                }
                else
                {
                    Vector3 tmp = hit.position;
                    if (CheckVectorIfPathable(unit, tmp))
                    {
                        storePotentialClosestPosition = tmp;
                    }
                    else
                    {
                        // This is the New Clicked Position
                        NavMesh.FindClosestEdge(tmp, out hit, NavMesh.AllAreas);
                        // Implement here the RNG system of clicked nowhere else.
                        unit.gameObject.GetComponent<NavMeshAgent>().CalculatePath(hit.position, nav);
                        storePotentialClosestPosition = ObtainPathLastPoint(nav, hit.position);
                    }
                }
            }
                finalPosition = storePotentialClosestPosition;

        }

        return finalPosition;
      
    }

    private Vector3 ObtainPathLastPoint(NavMeshPath nav, Vector3 basePosition)
    {
        if(nav.corners.Length <= 0)
        {
            Debug.LogError("Navigation Error : NavMeshPath has no corners, are you sure you generated it properly?");
            return Vector3.zero;
        }
        Vector3 lastPoint = new Vector3();
       
        float nearestPointDistance = 10000.0f;
        for (int i = 0; i < nav.corners.Length; i++)
        {
            float tmpDistance = Vector3.Distance(basePosition, nav.corners[i]);
            if (tmpDistance < nearestPointDistance)
            {
                nearestPointDistance = tmpDistance;
                lastPoint = nav.corners[i];
            }
        }
        return lastPoint;
    }
    public bool CheckVectorIfPathable(UnitBaseBehaviourComponent unit, Vector3 candidatePos)
    {
        NavMeshPath nav = new NavMeshPath();

        unit.gameObject.GetComponent<NavMeshAgent>().CalculatePath(candidatePos, nav);

        return (nav.status == NavMeshPathStatus.PathComplete);
    }
}
