using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("Player Stat")]
    public int playerHp = 10;
    public int playerDefense = 0;
    public int playerDamage = 10;
    public float playerSpeed = 10f;
    public float playerCritical = 5f;
    public float attackDelay = 1.0f;

    [Header("Monster Stat")]
    public int monHp = 5;
    public int monDefense = 0;
    public int monDamage = 2;
    public float monSpeed = 2.0f;

}
