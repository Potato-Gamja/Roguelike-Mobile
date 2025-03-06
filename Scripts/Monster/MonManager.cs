using System.Collections;
using UnityEngine;

public class MonManager : MonoBehaviour
{
    GameManager gameManager;

    public SceneScript sceneScript;
    
    [SerializeField]
    int monNum = 0;                                      //몬스터 웨이브의 식별 넘버

    public bool monEvent_0 = false;                      //몬스터 스폰 방식
    public bool monEvent_1 = false;                      //몬스터 스폰 방식
    public bool baseSpawn = true;

    public Vector2 minVec;                               //경고와 몬스터의 스폰 최솟값
    public Vector2 maxVec;                               //경고와 몬스터의 스폰 최댓값

    float posX1;                                         //경고와 몬스터의 X 좌푯값
    float posY1;                                         //경고와 몬스터의 Y 좌푯값

    float warnRan = 0.1f;                                //경고 오브젝트 쿨타임

   

    public GameObject monGroup_1;                        //몬스터 오브젝트의 부모로 할 오브젝트들
    public GameObject monGroup_2;
    public GameObject monGroup_3;
    public GameObject monGroup_4;

    public GameObject[] mons_1;                          //웨이브 별 몬스터의 종류
    public GameObject[] mons_2;
    public GameObject[] mons_3;
    public GameObject[] mons_4;

    public GameObject[] monPrefabs_1;                    //몬스터 오브젝트 풀링에 사용될 배열
    public GameObject[] monPrefabs_2;
    public GameObject[] monPrefabs_3;
    public GameObject[] monPrefabs_4;

    public MonsterScript[] monScripts_1;                 //풀링된 몬스터 오브젝트의 스크립트를 담을 배열
    public MonsterScript[] monScripts_2;
    public MonsterScript[] monScripts_3;
    public MonsterScript[] monScripts_4;

    public int monPool = 150;                            //풀링할 몬스터의 수
    public int monCount = 0;                             //처치한 몬스터의 수 카운트
    public int mCount = 0;                               //몬스터 배열의 인덱스
    
    public GameObject warns;                             //경고 오브젝트
    public GameObject warnGroup;                         //경고 오브젝트의 부모로 할 오브젝트
    public GameObject[] warnPrefabs;                     //경고 오브젝트 풀링에 사용될 배열
    public WarnScript[] warnScript;                      //풀링된 경고 오브젝트의 스크립트를 담을 배열
      
    public float warnTime = 1.2f;                        //경고 애니메이션 시간
    public int warnPool = 20;                            //풀링할 경고의 수
    public int wCount = 0;                               //경고 배열의 인덱스
    
    public int minCount;                                 //몬스터 이벤트의 스폰되는 몬스터 수 최솟값
    public int maxCount;                                 //몬스터 이벤트의 스폰되는 몬스터 수 최댓값

    public float minCool;                                //몬스터 이벤트의 경고 오브젝트 스폰시간의 최솟값
    public float maxCool;                                //몬스터 이벤트의 경고 오브젝트 스폰시간의 최댓값

    bool check_0 = false;                               //몬스터 스폰 관련 값 설정에 사용되는 불
    bool check_1 = false;
    bool check_2 = false;
    bool check_3 = false;
    
    float time = 0;                                      //기본 몬스터 스폰에 사용되는 시간
    float spawnTime = 0.1f;                              //기본 몬스터 스폰이 실행되는 시간
    
    float time_ = 0;                                     //몬스터 이벤트에 사용되는 시간
    float eventTime = 15f;                               //몬스터 이벤트가 실행되는 시간

    

    void Awake()
    {
        gameManager = GetComponent<GameManager>();

        warnPrefabs = new GameObject[warnPool];                                                    //경고 오브젝트 풀링의 배열 크기 할당
        for (int i = 0; i < warnPrefabs.Length; i++)                                               //배열의 크기만큼 경고 오브젝트 풀링 실행
        {
            GameObject warnObject = Instantiate(warns);                                            //경고 오브젝트 생성
            warnObject.transform.parent = warnGroup.transform;                                     //풀링된 오브젝트의 부모를 경고 그룹 오브젝트로 변경
            warnPrefabs[i] = warnObject;                                                           //생성한 경고 오브젝트를 배열에 넣기
            warnScript[i] = warnPrefabs[i].GetComponent<WarnScript>();                             //경고 오브젝트의 스크립트를 스크립트 배열에 넣기 
            warnObject.SetActive(false);                                                           //생성한 경고 오브젝트 비활성화
        }

        monPrefabs_1 = new GameObject[monPool];                                                    //몬스터 오브젝트 풀링의 배열 크기 할당
        for (int i = 0; i < monPrefabs_1.Length; i++)                                              //배열의 크기만큼 몬스터 오브젝트 풀링 실행
        {
            GameObject monObject1 = Instantiate(mons_1[Random.Range(0, mons_1.Length)]);           //랜덤한 몬스터의 종류로 몬스터 오브젝트 생성
            monObject1.transform.parent = monGroup_1.transform;                                    //풀링된 오브젝트의 부모를 경고 그룹 오브젝트로 변경
            monPrefabs_1[i] = monObject1;                                                          //생성한 몬스터 오브젝트를 배열에 넣기
            monScripts_1[i] = monPrefabs_1[i].GetComponent<MonsterScript>();                       //몬스터 오브젝트의 스크립트를 배열에 넣기 
            monObject1.SetActive(false);                                                           //생성한 몬스터 오브젝트 비활성화
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
        if (gameManager.isOver)                                                         //게임오버 시 실행이 되지않게 하기 위한 조건문
            return;

        time += Time.deltaTime;
        time_ += Time.deltaTime;

        SetSpawn();                                                                     //몬스터 스폰 관련 값 설정

        if (time >= spawnTime)                                                          //기본 몬스터 스폰
        {
            time = 0;                                                                   //시간 0으로 초기화
            spawnTime = Random.Range(minCool, maxCool);                                 //스폰 시간 랜덤
            MonEvent_0();                                                               //몬스터 스폰 이벤트 실행
        }

        if (time_ >= eventTime)                                                         //몬스터 이벤트 스폰
        {
            time_ = 0;                                                                  //시간 0으로 초기화
            minCool = 0.2f;                                                             //최소 스폰 시간
            maxCool = 0.4f;                                                             //최대 스폰 시간
            monEvent_1 = true;                                                          //이벤트 스폰 여부

            StartCoroutine(MonEvent_1());                                               //이벤트 스폰은 코루틴으로 실행
        }

    }

    void SetSpawn()
    {
        if (monCount < 300 && !monEvent_1 && !check_0)                                 //처치한 몬스터의 수에 따른 경고&몬스터 스폰 관련 값 설정
        {
            check_0 = true;                                                            //조건문이 한번만 실행되게 check_0 값을 변경
            eventTime = 3.0f;                                                          //몬스터 이벤트 스폰 시간
            warnTime = 1.2f;                                                           //경고 실행 시간
            minCool = 0.8f;                                                            //최소 스폰 시간
            maxCool = 1f;                                                              //최대 스폰 시간
            minCount = 3;                                                              //몬스터 이벤트의 최소 스폰 수
            maxCount = 5;                                                              //몬스터 이벤트의 최대 스폰 수
        }
        else if (monCount >= 300 && monCount < 700 && !monEvent_1 && !check_1)        
        {
            check_1 = true;
            eventTime = 5.0f;
            monNum = 1;
            warnTime = 1.1f;
            minCool = 0.7f;
            maxCool = 0.9f;
            minCount = 4;
            maxCount = 6;

        }
        else if (monCount >= 700 && monCount < 1000 && !monEvent_1 && !check_2)
        {
            check_2 = true;
            eventTime = 5.0f;
            monNum = 2;
            warnTime = 1.0f;
            minCool = 0.6f;
            maxCool = 0.8f;
            minCount = 5;
            maxCount = 8;
        }
        else if (monCount >= 1000 && !monEvent_1 && !check_3)
        {
            check_3 = true;
            eventTime = 5.0f;
            monNum = 3;
            warnTime = 0.9f;
            minCool = 0.6f;
            maxCool = 0.75f;
            minCount = 6;
            maxCount = 10;
        }
    }

    public void MonEvent_0()                                                            //기본 몬스터 스폰
    {
        var warnSpawn = warnPrefabs[wCount];                                            //경고 오브젝트 할당
        posX1 = Random.Range(minVec.x, maxVec.x);                                       //경고 오브젝트가 나타날 X 랜덤 좌푯값
        posY1 = Random.Range(minVec.y, maxVec.y);                                       //경고 오브젝트가 나타날 Y 랜덤 좌푯값

        warnSpawn.transform.position = new Vector3(posX1, posY1, 0);                    //경고 오브젝트의 포지션 설정

        warnPrefabs[wCount].SetActive(true);                                            //경고 오브젝트 활성화
        warnScript[wCount].Set(posX1, posY1, warnTime);                                 //경고 스크립트의 함수 실행
        wCount++;                                                                       //경고 배열의 인덱스 값 증가

        if (mCount >= monPool)                                                          //경고와 몬스터의 인덱스 값이 오버될 경우 0으로 초기화
            mCount = 0;
        if (wCount >= warnPool)
            wCount = 0;

        monEvent_0 = false;                                                             //기본 몬스터 스폰 여부

    }

    IEnumerator MonEvent_1()                                                            //몬스터 이벤트 스폰
    {
        int ran = Random.Range(minCount, maxCount);                                     //몬스터의 랜덤 스폰 수

        for (int i = 0; i < ran; i++)                                                   //랜덤 스폰 수만큼 반복
        {
            var warnSpawn = warnPrefabs[wCount];                                        //경고 오브젝트 할당

            posX1 = Random.Range(minVec.x, maxVec.x);                                   //경고 오브젝트가 나타날 X 랜덤 좌푯값
            posY1 = Random.Range(minVec.y, maxVec.y);                                   //경고 오브젝트가 나타날 Y 랜덤 좌푯값

            warnSpawn.transform.position = new Vector3(posX1, posY1, 0);                //경고 오브젝트의 포지션 설정

            warnPrefabs[wCount].SetActive(true);                                        //경고 오브젝트 활성화
            warnScript[wCount].Set(posX1, posY1, warnTime);                             //경고 스크립트의 함수 실행
            wCount++;                                                                   //경고 배열의 인덱스 값 증가

            if (mCount >= monPool)                                                      
                mCount = 0;
            if (wCount >= warnPool)
                wCount = 0;

            warnRan = Random.Range(minCool, maxCool);                                   //경고 오브젝트 활성화의 랜덤 쿨타임
            yield return warnRan;
        }

        if (monEvent_1)                                                                 //이벤트 종료 후 값를 원래 값으로 돌려놓기
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

    public void EnableMons(float posX, float posY)                                      //경고 스크립트에서 호출되는 몬스터 활성화 함수, posX와 posY는 경고 오브젝트의 위치
    {
        switch (monNum)                                                                 //몬스터 웨이브 식별
        {
            case 0:                                                                     //웨이브
                var monSpawn_1 = monPrefabs_1[mCount];                                  //몬스터 할당
                monSpawn_1.transform.position = new Vector3(posX, posY, 0);             //몬스터 위치를 경고 위치로 변경

                monScripts_1[mCount].ResetStat();                                       //몬스터의 스탯 설정
                monPrefabs_1[mCount].SetActive(true);                                   //몬스터 활성화
                mCount++;                                                               //몬스터의 인덱스 값 증가
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
