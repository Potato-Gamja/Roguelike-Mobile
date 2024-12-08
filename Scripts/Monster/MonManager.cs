using System.Collections;
using UnityEngine;

public class MonManager : MonoBehaviour
{
    public static MonManager instance;
    GameManager gameManager;

    public SceneScript sceneScript;

    public int mCount = 0;
    public float time = 0;

    [SerializeField]
    int monNum = 0;

    public bool monEvent_0 = false;
    public bool monEvent_1 = false;
    public bool baseSpawn = true;

    public Vector2 minVec;
    public Vector2 maxVec;

    float posX1;
    float posY1;
    float posX2;
    float posY2;

    float warnRan = 0.1f;

    public GameObject warnGroup;
    public WarnScript[] warnScript;

    public GameObject monGroup_1;
    public GameObject monGroup_2;
    public GameObject monGroup_3;
    public GameObject monGroup_4;

    public GameObject[] mons_1;
    public GameObject[] mons_2;
    public GameObject[] mons_3;
    public GameObject[] mons_4;

    public GameObject[] monPrefabs_1;
    public GameObject[] monPrefabs_2;
    public GameObject[] monPrefabs_3;
    public GameObject[] monPrefabs_4;

    public MonsterScript[] monScripts_1;
    public MonsterScript[] monScripts_2;
    public MonsterScript[] monScripts_3;
    public MonsterScript[] monScripts_4;

    public int monPool = 50;
    public int monCount = 0;

    public GameObject warns;
    public GameObject[] warnPrefabs;
    public float warnTime = 1.2f;
    public int warnPool = 10;
    public int wCount = 0;
    public int minCount;
    public int maxCount;

    public float minCool;
    public float maxCool;

    bool cheack_0 = false;
    bool cheack_1 = false;
    bool cheack_2 = false;
    bool cheack_3 = false;

    float time_ = 0;
    float eventTime = 15f;

    float spawnTime = 0.1f;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();

        warnPrefabs = new GameObject[warnPool];
        for (int i = 0; i < warnPrefabs.Length; i++)
        {
            GameObject warnObject = Instantiate(warns);
            warnObject.transform.parent = warnGroup.transform;
            warnPrefabs[i] = warnObject;
            warnScript[i] = warnPrefabs[i].GetComponent<WarnScript>();
            warnObject.SetActive(false);
        }

        monPrefabs_1 = new GameObject[monPool];
        for (int i = 0; i < monPrefabs_1.Length; i++)
        {
            GameObject monObject1 = Instantiate(mons_1[Random.Range(0, mons_1.Length)]);
            monObject1.transform.parent = monGroup_1.transform;
            monPrefabs_1[i] = monObject1;
            monScripts_1[i] = monPrefabs_1[i].GetComponent<MonsterScript>();
            monObject1.SetActive(false);
        }

        monPrefabs_2 = new GameObject[monPool];
        for (int i = 0; i < monPrefabs_2.Length; i++)
        {
            GameObject monObject2 = Instantiate(mons_2[Random.Range(0, mons_2.Length)]);
            monObject2.transform.parent = monGroup_2.transform;
            monPrefabs_2[i] = monObject2;
            monScripts_2[i] = monPrefabs_2[i].GetComponent<MonsterScript>();
            monObject2.SetActive(false);
        }

        monPrefabs_3 = new GameObject[monPool];
        for (int i = 0; i < monPrefabs_3.Length; i++)
        {
            GameObject monObject3 = Instantiate(mons_3[Random.Range(0, mons_3.Length)]);
            monObject3.transform.parent = monGroup_3.transform;
            monPrefabs_3[i] = monObject3;
            monScripts_3[i] = monPrefabs_3[i].GetComponent<MonsterScript>();
            monObject3.SetActive(false);
        }

        monPrefabs_4 = new GameObject[monPool];
        for (int i = 0; i < monPrefabs_4.Length; i++)
        {
            GameObject monObject4 = Instantiate(mons_4[Random.Range(0, mons_4.Length)]);
            monObject4.transform.parent = monGroup_4.transform;
            monPrefabs_4[i] = monObject4;
            monScripts_4[i] = monPrefabs_4[i].GetComponent<MonsterScript>();
            monObject4.SetActive(false);
        }

    }

    void Update()
    {
        if (gameManager.isOver)
            return;

        time += Time.deltaTime;
        time_ += Time.deltaTime;

        if (monCount < 300 && !monEvent_1 && !cheack_0)
        {
            cheack_0 = true;
            eventTime = 3.0f;
            warnTime = 1.2f;
            minCool = 0.8f;
            maxCool = 1f;
            minCount = 3;
            maxCount = 5;
        }
        else if (monCount >= 300 && monCount < 700 && !monEvent_1 && !cheack_1)
        {
            cheack_1 = true;
            eventTime = 5.0f;
            monNum = 1;
            warnTime = 1.1f;
            minCool = 0.7f;
            maxCool = 0.9f;
            minCount = 4;
            maxCount = 6;

        }
        else if (monCount >= 700 && monCount < 1000 && !monEvent_1 && !cheack_2)
        {
            cheack_2 = true;
            eventTime = 5.0f;
            monNum = 2;
            warnTime = 1.0f;
            minCool = 0.6f;
            maxCool = 0.8f;
            minCount = 5;
            maxCount = 8;
        }
        else if (monCount >= 1000 && !monEvent_1 && !cheack_3)
        {
            cheack_3 = true;
            eventTime = 5.0f;
            monNum = 3;
            warnTime = 0.9f;
            minCool = 0.6f;
            maxCool = 0.75f;
            minCount = 6;
            maxCount = 10;
        }

        if (time >= spawnTime)
        {
            time = 0;
            spawnTime = Random.Range(minCool, maxCool);
            MonEvent_0();
        }

        if (time_ >= eventTime)
        {
            time_ = 0;
            minCool = 0.2f;
            maxCool = 0.4f;
            monEvent_1 = true;

            StartCoroutine(MonEvent_1());
        }

    }

    //public void EnableWarns(bool isSpawn)
    //{
    //    if (!isSpawn)
    //    {
    //        monEvent_0 = false;
    //        return;
    //    }

    //    if (wCount >= warnPool)
    //        wCount = 0;
    //    var warnSpawn = warnPrefabs[wCount];

    //    if (isSpawn && !monEvent_0)
    //    {
    //        baseSpawn = true;
    //        if (warnPrefabs[wCount].activeSelf == false)
    //        {
    //            time = 0;

    //            posX1 = Random.Range(minVec.x, maxVec.x);
    //            posY1 = Random.Range(minVec.y, maxVec.y);

    //            warnSpawn.transform.position = new Vector3(posX1, posY1, 0);
    //            warnPrefabs[wCount].SetActive(true);
    //            warnScript[wCount].Set1(posX1, posY1, warnTime);
    //            baseSpawn = false;
    //        }
    //    }
    //    else if(isSpawn && monEvent_0)
    //    {
    //        baseSpawn = false;
    //        return;
    //    }
    //    if (mCount >= monPool)
    //        mCount = 0;
    //    if (wCount >= warnPool)
    //        wCount = 0;
    //}

    public void MonEvent_0()
    {

        var warnSpawn = warnPrefabs[wCount];
        posX2 = Random.Range(minVec.x, maxVec.x);
        posY2 = Random.Range(minVec.y, maxVec.y);

        warnSpawn.transform.position = new Vector3(posX2, posY2, 0);

        warnPrefabs[wCount].SetActive(true);
        warnScript[wCount].Set(posX2, posY2, warnTime);
        wCount++;

        if (mCount >= monPool)
            mCount = 0;
        if (wCount >= warnPool)
            wCount = 0;

        monEvent_0 = false;

    }

    IEnumerator MonEvent_1()
    {
        int ran = Random.Range(minCount, maxCount);

        for (int i = 0; i < ran; i++)
        {
            var warnSpawn = warnPrefabs[wCount];

            posX2 = Random.Range(minVec.x, maxVec.x);
            posY2 = Random.Range(minVec.y, maxVec.y);

            warnSpawn.transform.position = new Vector3(posX2, posY2, 0);

            warnPrefabs[wCount].SetActive(true);
            warnScript[wCount].Set(posX2, posY2, warnTime);
            wCount++;

            if (mCount >= monPool)
                mCount = 0;
            if (wCount >= warnPool)
                wCount = 0;

            warnRan = Random.Range(minCool, maxCool);
            yield return warnRan;
        }

        if (monEvent_1)
        {
            if (monCount < 300)
            {
                minCool = 0.8f;
                maxCool = 1f;
                minCount = 4;
                maxCount = 6;
                warnTime = 1.2f;
            }
            else if (monCount >= 300)
            {
                minCool = 0.7f;
                maxCool = 0.9f;
                minCount = 4;
                maxCount = 8;
                warnTime = 1.1f;

            }
            else if (monCount >= 700)
            {
                minCool = 0.6f;
                maxCool = 0.8f;
                minCount = 6;
                maxCount = 10;
                warnTime = 1.0f;
            }
            else if (monCount >= 1000)
            {
                minCool = 0.6f;
                maxCool = 0.75f;
                minCount = 7;
                maxCount = 10;
                warnTime = 1.0f;
            }

            monEvent_1 = false;
        }

    }

    public void EnableMons(float posX, float posY)
    {
        switch (monNum)
        {
            case 0:
                var monSpawn_1 = monPrefabs_1[mCount];
                monSpawn_1.transform.position = new Vector3(posX, posY, 0);

                monScripts_1[mCount].ResetStat();
                monPrefabs_1[mCount].SetActive(true);
                mCount++;
                break;

            case 1:
                var monSpawn_2 = monPrefabs_2[mCount];
                monSpawn_2.transform.position = new Vector3(posX, posY, 0);

                monScripts_2[mCount].ResetStat();
                monPrefabs_2[mCount].SetActive(true);
                mCount++;
                break;

            case 2:
                var monSpawn_3 = monPrefabs_3[mCount];
                monSpawn_3.transform.position = new Vector3(posX, posY, 0);

                monScripts_3[mCount].ResetStat();
                monPrefabs_3[mCount].SetActive(true);
                mCount++;
                break;

            case 3:
                var monSpawn_4 = monPrefabs_4[mCount];
                monSpawn_4.transform.position = new Vector3(posX, posY, 0);

                monScripts_4[mCount].ResetStat();
                monPrefabs_4[mCount].SetActive(true);
                mCount++;
                break;
        }
        if (mCount >= monPool)
            mCount = 0;
        if (wCount >= warnPool)
            wCount = 0;
    }

}
