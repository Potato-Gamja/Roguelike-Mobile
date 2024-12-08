using UnityEngine;
using Unity.VisualScripting;
[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptble Object/SkillData")]

public class LevelUpData : ScriptableObject
{
    [Header("Weapon Type")]
    public string weapon;
    public bool istrue;


    [Header("Weapon Stat Level")]
    public int[] weaponLevel;

    [Header("Weapon stat")]
    public float damage;
    public float speed;
    public float critical;
    public int pass;
    public float coltime;
    public float range;
    public float duration;
    public float criticalDamage;
    public float slow;

}