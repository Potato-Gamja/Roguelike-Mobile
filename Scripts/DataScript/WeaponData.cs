using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptble Object/WeaponData")]
//에셋메뉴를 만들어 무기 데이터를 관리

public class WeaponData : ScriptableObject
{
    public float baseSpeed;                  //무기의 속도 (미사일, 마법검)
    public float speed;

    public float baseDamage;                 //무기의 데미지 (모든 무기)
    public float damage;

    public float baseRange;                  //무기의 범위 (블래스트, 장판)
    public float range;

    public float baseAttackDelay;            //무기의 공격 딜레이 (미사일, 마법검, 레이저, 블래스트)
    public float attackDelay;

    public bool baseIsKnock;                 //무기의 넉백 여부 (미사일, 마법검, 블래스트)
    public bool isKnock;
    
    public float baseKnockback;              //무기의 넉백 크기 (미사일, 마법검, 블래스트)
    public float knockback;

    public float baseDuration;               //무기의 지속시간 (마법검)
    public float duration;

    public float baseCritical;               //무기의 치명타 확률 (미사일, 블래스트)
    public float critical;

    public float baseCriticalDamage;         //무기의 치명타 데미지 (미사일, 블래스트)
    public float criticalDamage;

    public int baseCount;                    //무기의 공격 횟수 (레이저)
    public int count;

    public float baseSlow;                   //무기의 슬로우 (장판)
    public float slow;
    
    public float baseDecrease;               //무기의 방어력 감소 (장판)
    public float decrease;

}
