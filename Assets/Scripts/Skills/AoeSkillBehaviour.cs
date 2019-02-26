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
        public bool moveCasting = true;
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
        public void SetUnitsToReceive(List<UnitBaseBehaviourComponent> units)
        {
            foreach(UnitBaseBehaviourComponent item in units)
            {
                if(!affectedUnits.Contains(item))
                {
                    affectedUnits.Add(item);
                    item.ReceiveBuff(skillEffect);
                }
            }
        }
        public override void StartSkillCasting()
        {
            base.StartSkillCasting();
            projectile.gameObject.SetActive(true);
            projectile.StartMoving(targetPosition);
        }

        public void ProjectileTouchDown()
        {
            if (!moveCasting)
            {
                owner.canMove = true;
            }
            if (projectile != null)
            {
                projectile.gameObject.SetActive(false);
            }
            if(onCollisionFx != null)
            {
                onCollisionFx.transform.position = projectile.transform.position;
                onCollisionFx.gameObject.SetActive(true);
            }
            if(areaOfEffect != null)
            {
                areaOfEffect.gameObject.SetActive(false);
            }
        }
    }
}
