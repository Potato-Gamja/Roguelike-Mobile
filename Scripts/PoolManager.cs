using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    public static MonManager instance;

    public float createTime = 1f;
    public int monPool = 100;
    int mCount = 0;
    float time = 0;

    [SerializeField]
    Vector2 minVec;
    [SerializeField]
    Vector2 maxVec;

    float posX;
    float posY;

    public GameObject mons;
    public GameObject[] monPrefabs;

    public float warnTime = 1.2f;
    public int warnPool = 10;
    public int wCount = 0;
    public GameObject warns;
    public GameObject[] warnPrefabs;

}
