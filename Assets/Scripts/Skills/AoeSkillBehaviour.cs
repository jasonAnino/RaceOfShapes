using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnitStats;
using UnitsScripts.Behaviour;
using ComboSystem;
using Utilities.MousePointer;
namespace SkillBehaviour
{
    public class AoeSkillBehaviour : BaseSkillBehaviour
    {
        public AoeProjectileComponent projectile;
        public GameObject areaOfEffect;
        public GameObject onCollisionFx;
        public Vector3 targetPosition; // On Player Left Click
        public bool startAiming = false;

        RaycastHit hit;
        public override void Update()
        {
            if (startAiming)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    transform.position = hit.point;
                }
            }
        }

        public void StartSkillCasting()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                targetPosition = hit.point;
            }
            transform.position = targetPosition;
            owner.ReceiveOrder(UnitOrder.GenerateIdleOrder(), true);
            owner.MakeUnitLookAt(targetPosition);
            startAiming = false;
            CursorManager.GetInstance.CursorChangeTemporary(CursorType.NORMAL);
        }
    }
}
