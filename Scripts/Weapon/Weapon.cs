using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using Random = UnityEngine.Random;
using MagicArsenal;

public class Weapon : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]
    MagicBeamStatic magicBeamStatic;   //에셋의 스크립트, 레이저 길이를 조절
    GameObject player;
    PlayerScript playerScript;
    LevelUpScript levelUpScript;
    
    public Scanner scanner;            //스캐너
    
    public Animator swordAni;          //마법검 애니메이터

    public string type;                //무기 종류를 구분하는 문자열

    public int hitCount;               //타격 회수
    public float time = 0;             //무기 타이머

    [Header("Laser Weapon")]           //레이저 무기
    public GameObject laserBeam;       //레이저 빔
    [SerializeField]
    GameObject orb;                    //레이저 오브
    [SerializeField]
    Weapon laserEndPoint;              //레이저 타격 지점
    CircleCollider2D circleCollider;   //레이저 콜라이더
    MagicBeamStatic laserLength;       //레이저 길이
    [SerializeField]    
    Weapon laserWeapon;                //레이저 스크립트
    Scanner laserScanner;              //레이저 스캐너
    public bool isHit = false;         //레이저 타격 여부
    [SerializeField]
    float rotSpeed = 999999;           //레이저의 회전속도
    public Vector3 targetPos;          //타겟의 위치
    Vector3 dir;                       //타겟과의 거리
    public GameObject laser;           //레이저 게임오브젝트
    public GameObject target;          //레이저의 타겟 게임오브젝트트
    public float hitTime = 0.1f;       //레이저 타격 딜레이

    [Header("Blast Weapon")]                 //블래스트 무기
    public List<GameObject> blastTarget;     //게임오브젝트형 리스트의 블래스트 타겟
    public BoxCollider2D boxCollider_blast;  //블래스트의 박스 콜라이더
    ParticleSystem particleSystem_;          //블래스트의 파티클 시스템
    float playTime = 0f;                     //블래스트의 실행 시간
    bool isPlay;                             //블래스트의 실행 여부

    [Header("Floor Weapon")]                 //장판 무기
    public List<GameObject> floorTarget;     //게임오브젝트형 리스트의 장판 타겟
    public bool isSlow = false;              //장판의 이동속도 감소 여부
    public bool isDecrease = false;          //장판의 방어력 감소 여부
    public GameObject circle;                //장판 무기의 배경 오브젝트

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        levelUpScript = GameObject.FindWithTag("LevelManager").GetComponent<LevelUpScript>();
        player = GameObject.FindWithTag("Player");
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        //문자열 비교를 이용
        if (type == "missile")
        {
            circleCollider = GetComponent<CircleCollider2D>();
            hitCount = levelUpScript.missileData.baseCount;
        }
        else if (type == "sword")
        {
            swordAni = GetComponent<Animator>();
        }
        else if(type == "laser")
        {
            laserScanner = laser.GetComponent<Scanner>();
            laserLength = laser.GetComponent<MagicBeamStatic>();
        }
        else if (type == "laserEnd")
        {
            laserScanner = laser.GetComponent<Scanner>();
            laserLength = laser.GetComponent<MagicBeamStatic>();

            circleCollider = GetComponent<CircleCollider2D>();
            hitCount = levelUpScript.laserData.baseCount;
        }
        else if (type == "blast")
        {
            particleSystem_ = GetComponent<ParticleSystem>();
        }
        else if (type == "floor")
        {

        }

    }

    void Update()
    {
        if (gameManager.isOver)                                            //게임오버 시 리턴
            return;

        switch (type)                                                      //타입 비교로 무기마다 다르게 실행
        {
            case "missile":                                                //미사일
                if (transform.position.x > 20f ||                          //맵을 벗어났을 경우
                    transform.position.x < -20f || 
                    transform.position.y > 20f || 
                    transform.position.y < -20f)
                {
                    hitCount = levelUpScript.missileData.count;            //타격 회수 초기화
                    gameObject.SetActive(false);                           //미사일 비활성화
                }
                transform.Translate(Vector3.up *                           //미사일 이동
                                    levelUpScript.missileData.speed * 
                                    Time.deltaTime);    
                break;

            case "sword":                                                  //마법검
                if (time >= levelUpScript.swordData.duration)              //마법검 지속시간 오버
                {
                    playerScript.isSword = true;                           //마법검 준비 완료
                    playerScript.attackTime_Sword = 0f;                    //마법검 쿨타임 타이머 초기화
                    time = 0f;                                             //지속시간 타이머 초기화
                    swordAni.SetBool("isSword", false);                    //비활성화 애니메이션 재생
                    playerScript.soundManager.StopSwordSound();            //마법검 사운드 중지
                }

                time += Time.deltaTime;                                    //타이머 시간 증가

                transform.Rotate(Vector3.back *                            //마법검 회전
                                 levelUpScript.swordData.speed * Time.deltaTime);
                transform.position = player.transform.position;            //플레이어 추격
                break;

            case "laser":                                                          //레이저
                if (levelUpScript.isLaser)
                {
                    if (playerScript.isLaser && laserBeam.activeSelf)              //레이저빔 활성화 시
                    {
                        transform.position = orb.transform.position;               //레이저 오브의 위치 추격
                        if (!scanner.nearestTarget)                                //스캐너의 타겟이 없을 경우 리턴
                            return;

                        if (laserEndPoint.target && laserEndPoint.isHit)           //레이저가 공격 중일 시 타겟 유지
                            targetPos = laserEndPoint.target.transform.position;
                        else if (laserEndPoint.target && !laserEndPoint.isHit)     //레이저가 공격 중이 아닐 시 타겟 변경
                            targetPos = scanner.nearestTarget.position;

                        dir = targetPos - orb.transform.position;                  //타겟과 오브의 거리 계산
                        dir = dir.normalized;                                      //방향 벡터
                        float dis;
                        dis = Vector2.Distance(targetPos, orb.transform.position); //타겟과 오브의 거리 비교
                        laserLength.beamLength = dis;                              //레이저 빔의 길이 설정
                        Quaternion lookRot = Quaternion.LookRotation(dir);         //레이저 방향 설정
                        transform.rotation = Quaternion.Slerp(transform.rotation,  //레이저 회전
                                                              lookRot, '
                                                              Time.deltaTime * rotSpeed);  
                    }
                }
                break;

            case "laserEnd":                                                       //레이저 타격 오브젝트
                if (levelUpScript.isLaser && playerScript.isLaser)
                {
                    if (target == null || target.activeSelf == false)              //타겟이 없거나 타겟이 비활성화 상태일 경우
                    {
                        target = laserScanner.targetObj;                           //타겟 재설정
                        laserLength.beamLength = 0;                                //레이저 빔의 길이 0으로 설정
                    }
                    circleCollider.enabled = true;                                 //타격 콜라이더 활성화
                    time += Time.deltaTime;                                        //타이머 증가

                    if (hitCount < 1)                                              //타격 회수 0
                    {
                        circleCollider.enabled = false;                            //콜라이더 비활성화
                        playerScript.attackTime_Laser = 0f;                        //레이저 타이머 초기화
                        playerScript.isLaser = false;                              //공격 여부
                        hitCount = levelUpScript.laserData.count;                  //레이저 타격회수 재설정
                        laserLength.beamLength = 0;                                //레이저 빔의 길이 0으로 설정
                        laserScanner.nearestTarget = null;                         //스캐너의 타겟 초기화
                        target = null;                                             //타겟 초기화
                        playerScript.soundManager.StopLaserSound();                //레이저 사운드 중지
                        laserBeam.SetActive(false);                                //레이저 빔 비활성화
                    }
                }
                break;

            case "blast":                                                    //블래스트
                time += Time.deltaTime;                                      //타이머 증가

                if (isPlay)                                                  //블래스트 공격 중
                    playTime += Time.deltaTime;

                if (playTime < 0.5f && playTime >= 0.45f)                    //콜라이더 활성화 시간
                {
                    boxCollider_blast.enabled = true;                        //콜라이더 활성화
                }
                if (playTime >= 0.5f)
                {
                    boxCollider_blast.enabled = false;                       //콜라이더 비활성화
                    isPlay = false;                                          //블래스트 실행 여부 비활성화
                    playTime = 0f;                                           //블래스트 실행 시간 초기화
                }

                if (time >= levelUpScript.blastData.attackDelay)             //쿨타임 완료
                {
                    float ranX_1 = player.transform.position.x - 4.5f;       //플레이어 기준으로 랜덤 좌푯값
                    float ranX_2 = player.transform.position.x + 4.5f;

                    float ranY_1 = player.transform.position.y - 6f;
                    float ranY_2 = player.transform.position.y + 6f;

                    if (ranX_1 < playerScript.minPos.x)                      //플레이어의 이동범위 밖일 경우 범위 조정
                        ranX_1 = playerScript.minPos.x;

                    if (ranX_2 > playerScript.maxPos.x)
                        ranX_2 = playerScript.maxPos.x;

                    if (ranY_1 < playerScript.minPos.y)
                        ranY_1 = playerScript.minPos.y;

                    if (ranY_2 > playerScript.maxPos.y)
                        ranY_2 = playerScript.maxPos.y;

                    Vector2 ran = new Vector2(math.floor(Random.Range(ranX_1, ranX_2) * 10) * 0.1f,    //랜덤 좌푯값 소수점 한자리 버리기
                                              math.floor(Random.Range(ranY_1, ranY_2) * 10) * 0.1f);

                    transform.position = ran;                                //블래스트 위치 이동
                    isPlay = true;                                           //블래스트 실행 여부 활성화
                    particleSystem_.Play();                                  //파티클 시스템 실행
                    playerScript.soundManager.PlayBlastSound();              //블래스트 사운드 재생
                    time = 0f;                                               //타이머 초기화화
                }
                break;

            case "floor":                                //장판
                time += Time.deltaTime;                  //타이머 증가

                break;
        }
    }

    public void Retarget()                    //레이저의 타겟 재설정
    {
            target = laserScanner.targetObj;
    }


    public void FloorDebuff()                                                                                             //장판 디버프
    {
        for (int i = 0; i < floorTarget.Count; i++)
        {
            MonsterScript targetMon = floorTarget[i].GetComponent<MonsterScript>();                                       //장판 타겟 리스트에 있는 몬스터 스크립트를 가져오기
            targetMon.speed = targetMon.speed_Defalt - targetMon.speed_Defalt * 0.01f * levelUpScript.floorData.slow;     //몬스터 이동속도 감소

            targetMon.defense = targetMon.defense_Defalt - levelUpScript.floorData.decrease;                              //몬스터 방어력 감소
        }

    }

    public void SwordDis()                                                            //마법검 비활성화
    {
        levelUpScript.sword[levelUpScript.swordCount].SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)                                       //
    {
        if (collision.CompareTag("Monster") && type == "missile" && hitCount > 0)
        {
            if (target == null)
            {
                target = collision.gameObject;
            }

            if (target != null)
            {
                hitCount--;

                MonsterScript targetMon = target.GetComponent<MonsterScript>();

                float totalDamage;
                bool isCritical = false;

                float critical = math.floor(Random.Range(0f, 100f));
                float missileDamage = Random.Range(levelUpScript.missileData.damage - 2f, levelUpScript.missileData.damage + 3f);

                if (!levelUpScript.playerData.isTrueDamage)
                    totalDamage = math.floor((playerScript.offense * 0.01f * missileDamage - targetMon.defense));
                else
                    totalDamage = math.floor((playerScript.offense * 0.01f * missileDamage));

                if (critical <= playerScript.critical + levelUpScript.missileData.critical)
                {
                    totalDamage = math.floor(totalDamage * 1.8f);
                    isCritical = true;
                }
                else
                {
                    isCritical = false;
                }

                if (totalDamage < 1f)
                    totalDamage = 1f;

                totalDamage = math.floor(totalDamage);

                targetMon.DamageText(totalDamage, isCritical);
                targetMon.animator.SetTrigger("Hit");
                targetMon.hp = targetMon.hp - totalDamage;
                targetMon.Knock_Missile();

                if (hitCount < 1)
                {
                    circleCollider.enabled = false;
                    target = null;
                    gameObject.SetActive(false);
                }
                target = null;

            }
        }

        if (collision.CompareTag("Monster") && type == "blast")
        {
            MonsterScript targetMon = collision.gameObject.GetComponent<MonsterScript>();

            isHit = true;
            float totalDamage;
            bool isCritical = false;

            float critical = math.floor(Random.Range(0f, 100f));
            float blastDamage = Random.Range(levelUpScript.blastData.damage - 2f, levelUpScript.blastData.damage + 3f);

            if (!levelUpScript.playerData.isTrueDamage)
                totalDamage = math.floor((playerScript.offense / 100 * blastDamage - targetMon.defense));
            else
                totalDamage = math.floor((playerScript.offense / 100 * blastDamage));

            if (critical <= playerScript.critical + levelUpScript.blastData.critical)
            {
                totalDamage = math.floor(totalDamage * (1.8f + levelUpScript.blastData.criticalDamage));
                isCritical = true;
            }
            else
            {
                isCritical = false;
            }
            if (totalDamage < 1f)
                totalDamage = 1f;

            totalDamage = math.floor(totalDamage);

            targetMon.animator.SetTrigger("Hit");

            targetMon.DamageText(totalDamage, isCritical);
            targetMon.Knock_Blast(gameObject);
            targetMon.hp -= totalDamage;

        }

        if (collision.CompareTag("Monster") && type == "floor")
        {
            floorTarget.Add(collision.gameObject);

        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && type == "laserEnd" && hitCount > 0)
        {
            if (target != null)
            {
                isHit= true;
                if (time > hitTime)
                {
                    time = 0;

                    MonsterScript targetMon = target.GetComponent<MonsterScript>();

                    float totalDamage;
                    bool isCritical = false;

                    float critical = math.floor(Random.Range(0f, 100f) * 10f) / 10f;
                    float laserDamage = Random.Range(levelUpScript.laserData.damage, levelUpScript.laserData.damage + 2f);

                    if (!levelUpScript.playerData.isTrueDamage)
                        totalDamage = math.floor((playerScript.offense / 100 * laserDamage - targetMon.defense) * 10f) / 10f;
                    else
                        totalDamage = math.floor((playerScript.offense / 100 * laserDamage) * 10f) / 10f;

                    if (critical <= playerScript.critical + levelUpScript.laserData.critical)
                    {
                        totalDamage = math.floor(totalDamage * 1.8f * 10f) / 10f;
                        isCritical = true;
                    }
                    else
                    {
                        isCritical = false;
                    }

                    if (totalDamage < 1f)
                        totalDamage = 1f;

                    totalDamage = math.floor(totalDamage);
                    targetMon.animator.SetTrigger("Hit");
                    targetMon.DamageText(totalDamage, isCritical);
                    targetMon.hp -= totalDamage;
                    hitCount--;
                }

            }
        }
        if (collision.CompareTag("Monster") && type == "floor")
        {
            for (int i = 0; i < floorTarget.Count; i++)
            {
                MonsterScript targetMon = floorTarget[i].GetComponent<MonsterScript>();

                if (!targetMon.isSlow)
                {
                    targetMon.speed = targetMon.speed_Defalt - (targetMon.speed_Defalt / 100 * levelUpScript.floorData.slow);
                    targetMon.isSlow = true;
                }
                if (!targetMon.isDecrease)
                {
                    targetMon.defense = targetMon.defense_Defalt - levelUpScript.floorData.decrease;
                    targetMon.isDecrease = true;
                }
            }
            if (time >= levelUpScript.floorData.attackDelay)
            {
                time = 0f;
                for(int i = floorTarget.Count - 1; i > 0; i--)
                {   
                    MonsterScript targetMon = floorTarget[i].GetComponent<MonsterScript>();

                    float totalDamage;
                    bool isCritical = false;

                    float floorDamage = Random.Range(levelUpScript.floorData.damage, levelUpScript.floorData.damage + 2f);

                    totalDamage = math.floor((playerScript.offense / 100 * floorDamage - targetMon.defense) * 10f) / 10f;

                    if (totalDamage < 1f)
                        totalDamage = 1f;

                    totalDamage = math.floor(totalDamage);
                    targetMon.animator.SetTrigger("Hit");
                    targetMon.DamageText(totalDamage, isCritical);
                    targetMon.hp -= totalDamage;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && type == "laserEnd")
        {

        }
        if (collision.CompareTag("Monster") && type == "floor")
        {
            MonsterScript targetMon = collision.GetComponent<MonsterScript>();

            if (targetMon.isSlow)
            {
                targetMon.speed = targetMon.speed_Defalt;
                targetMon.isSlow = false;
            }
            if (targetMon.isDecrease)
            {
                targetMon.defense = targetMon.defense_Defalt;
                targetMon.isDecrease = false;
            }
            floorTarget.Remove(collision.gameObject);
        }
    }
}
