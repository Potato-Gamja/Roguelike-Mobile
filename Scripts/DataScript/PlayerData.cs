using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptble Object/PlayerData")]

public class PlayerData : ScriptableObject
{
    public float baseHp;
    public float maxHp;
    public float hp;

    public float baseDefense;
    public float defense;

    public float baseOffense;
    public float offense;

    public float baseSpeed;
    public float speed;

    public float baseAttackDelay;
    public float attackDelay;

    public bool baseIsTrueDamage;
    public bool isTrueDamage;

    public float baseCritical;
    public float critical;

    public float exp;
    public float[] exps;
    public Vector3 scale;
}
