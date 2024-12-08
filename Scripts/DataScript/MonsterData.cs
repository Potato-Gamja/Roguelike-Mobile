using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MonData", menuName = "Scriptble Object/MonData")]

public class MonsterData : ScriptableObject
{
    public float hp;
    public float waveHp;
    public float defense;
    public int damage;
    public float offense;
    public float speed;
    public float knockbackDefense;
    public float knockbackTime;

}
