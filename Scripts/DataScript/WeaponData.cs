using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptble Object/WeaponData")]

public class WeaponData : ScriptableObject
{
    public float baseSpeed;
    public float speed;

    public float baseDamage;
    public float damage;

    public float baseRange;
    public float range;

    public float baseAttackDelay;
    public float attackDelay;

    public bool baseIsKnock;
    public bool isKnock;
    public float baseKnockback;
    public float knockback;

    public float baseDuration;
    public float duration;

    public float baseCritical;
    public float critical;

    public float baseCriticalDamage;
    public float criticalDamage;

    public int baseCount;
    public int count;

    public float baseSlow;
    public float slow;
    public float baseDecrease;
    public float decrease;

}