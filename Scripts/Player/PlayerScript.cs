using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public LevelUpScript levelUpScript;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    Scanner scanner;
    [SerializeField]
    WeaponScript weaponScript;
    public SoundManager soundManager;

    public Animator animator;
    [SerializeField]
    SpriteRenderer spriter;
    Transform trans;
    [SerializeField]
    Transform staff;                    //미사일의 공격 위치

    public GameObject hpBar;            //체력바
    public GameObject hpBarPos;         //체력바 위치

    public GameObject sword;            //마법검 오브젝트
    public Weapon[] swords;             //마법검 스크립트 배열
    
    public GameObject laser;            //레이저 오브젝트
    public GameObject laserBeam;        //레이저의 빔
    public GameObject laserOrb;         //레이저의 오브

    public Weapon laserWeapon;          //레이저 무기 스크립트
    public Weapon laserEnd;             //레이저 끝 부위 스크립트

    public GameObject floor;            //장판 오브젝트

    public float maxHp;                 //최대 체력
    public float hp;                    //현재 체력
    public float defense;               //방어력
    public float offense;               //공격력
    public float baseSpeed;             //기본 이동속도
    public float speed;                 //이동속도
    public float critical;              //치명타 확률
    public float baseAttackDelay;       //기본 공격속도
    public float attackDelay;           //공격속도
    public float exp;                   //경험치
    public Vector3 scale;               //크기

    int weaponCount = 0;                //미사일 인덱스 값
    
    float attackTime_Missile = 0;       //미사일 공격 타이머
    public float attackTime_Laser = 0;         //레이저 공격 타이머
    public float attackTime_Sword = 0;         //마법검 공격 타이머

    [SerializeField]
    CircleCollider2D[] missileCol;      //미사일의 콜라이더
    [SerializeField]
    Weapon[] missileWeapon;             //미사일의 무기 스크립트

    public bool isSword = true;        //마법검 활성화 여부
    public bool isLaser = false;        //레이저 활성화 여부

    public FloatingJoystick joy;        //조이스틱 스크립트
    
    public Vector2 minPos;              //플레이어의 최소 이동 범위
    public Vector2 maxPos;              //플레이어의 최대 이동 범위

    public Vector3 vec;                 //플레이어의 이동 벡터터

    void Awake()
    {
        SetData();                      //데이터 설정정

        hp = levelUpScript.playerData.hp;
        animator = GetComponent<Animator>();
        trans = GetComponent<Transform>();

    }

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            missileCol[i] = weaponScript.missilePrefab[i].GetComponent<CircleCollider2D>();
            missileWeapon[i] = weaponScript.missilePrefab[i].GetComponent<Weapon>();
        }
    }

    public void SetData()                                        //플레이어 데이터의 값을 각 스탯에 대입
    {
        maxHp = levelUpScript.playerData.maxHp;
        hp = hp;
        defense = levelUpScript.playerData.defense;
        offense = levelUpScript.playerData.offense;
        baseSpeed = levelUpScript.playerData.speed;
        critical = levelUpScript.playerData.critical;
        attackDelay = levelUpScript.playerData.attackDelay;
        exp = levelUpScript.playerData.exp;
        scale = levelUpScript.playerData.scale;
    }


    void Update()
    {
        if (Time.timeScale == 0f || gameManager.isOver)        //일시정지 상태거나 게임오버 상태일 경우 리턴
            return;

        attackTime_Missile += Time.deltaTime;

        if (levelUpScript.isLaser)                             //레이저가 활성화 되어있을 시 실행
        {
            attackTime_Laser += Time.deltaTime;
        }
        if (levelUpScript.isSword)                             //마법검이 활성화 되어있을 시 타이머
        {
            attackTime_Sword += Time.deltaTime;
        }

        Move();                                                //플레이어 이동 함수 실행

        if (attackTime_Missile >= (levelUpScript.missileData.attackDelay / 100) * attackDelay)            //미사일 쿨타임과 타이머 비교
        {
            Attack_Missile();
        }
        if (attackTime_Sword >= (levelUpScript.swordData.attackDelay / 100 * attackDelay) && isSword)     //마법검 쿨타임과 타이머 비교
        {
            if (isSword)
                Attack_Sword();                                                                           //마법검 공격 실행
            isSword = false;                                                                              //마법검 공격 여부

        }
        if (attackTime_Laser >= (levelUpScript.laserData.attackDelay / 100) * attackDelay && !isLaser)    //레이저 쿨타임과 타이머 비교
        {
            laserWeapon.hitCount = levelUpScript.laserData.count;                                         //레이저의 최대 타격 회수 대입
            isLaser = true;                                                                               //레이저 공격 여부
            laserEnd.Retarget();                                                                          //레이저의 타겟 설정
            laserBeam.SetActive(true);                                                                    //레이저빔 활성화
            soundManager.PlayLaserSound();                                                                //레이저 사운드 재생
        }
    }

    void Move()                                //플레이어 이동
    {
        float x = joy.Horizontal;              //조이스틱의 수평 값 대입
        float y = joy.Vertical;                //조이스틱의 수직 값 대입

        vec = new Vector3(x, y, 0);            //입력값 x, y 대입

        if (x < 0)                                                                    //왼쪽으로 이동 시
        {
            animator.SetBool("Run", true);                                            //달리기 애니메이션 실행
            trans.localScale = new Vector3(scale.x * -1, scale.y, scale.z);           //플레이어가 왼쪽을 보게 스케일 값 변경
            hpBar.transform.localScale = new Vector3(-0.02f, 0.026f, 0.02f);          //체력바 스케일 값 변경
            laserOrb.transform.position = new Vector3(transform.position.x - 0.75f,   //레이저 오브의 위치와 스케일 값 변경
                                                      transform.position.y + 0.75f,
                                                      transform.position.z);
            laserOrb.transform.localScale = new Vector3(-0.8f, 0.8f, 1);
            floor.transform.localScale = new Vector3(-levelUpScript.floorData.range,  //장판의 스케일 값 조정
                                                     levelUpScript.floorData.range,
                                                     levelUpScript.floorData.range);
            
        }
        else if (x > 0)                                                               //오른쪽으로 이동 시
        {
            animator.SetBool("Run", true); 
            trans.localScale = new Vector3(scale.x, scale.y, scale.z);  
            hpBar.transform.localScale = new Vector3(0.02f, 0.026f, 0.02f);
            laserOrb.transform.position = new Vector3(transform.position.x - 0.75f,
                                                      transform.position.y + 0.75f,
                                                      transform.position.z);
            laserOrb.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            floor.transform.localScale = new Vector3(levelUpScript.floorData.range,
                                                     levelUpScript.floorData.range,
                                                     levelUpScript.floorData.range);
            
        }
        else if (x == 0 || y == 0)                                                   //입력값이 없을 경우
        {
            animator.SetBool("Run", false);                                          //달리기 애니메이션 중지, idle 애니메이션으로
        }

        transform.position += (baseSpeed / 100 * speed) * Time.deltaTime * vec;                    //조이스틱의 입력값에 속도를 곱한 만큼 이동
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),    //클램프를 사용하여 최소, 최대 이동범위 설정
                                         Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
                                         0);
    }

    void Attack_Missile()                                                                                             //미사일 공격
    {
        if (!scanner.nearestTarget)                                                                                   //스캐너에 타겟이 없을 경우 리턴
            return;

        Vector3 targetPos = scanner.nearestTarget.position;                                                           //타겟 위치
        Vector3 dir = targetPos - staff.transform.position;                                                           //타겍과 공격 위치인 지팡이와의 거리 비교
        dir = dir.normalized;                                                                                         //방향 벡터터

        if (weaponScript.missilePrefab[weaponCount].activeSelf == false)                                              //인덱스 값의 미사일이 비활성화일 경우 실행
        {
            weaponScript.missilePrefab[weaponCount].transform.position = staff.transform.position;                    //미사일을 지팡이의 공격 위치로 이동
            weaponScript.missilePrefab[weaponCount].SetActive(true);                                                  //미사일 활성화
            soundManager.PlayMissileSound();                                                                          //미사일 사운드 재생
            weaponScript.missilePrefab[weaponCount].transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);  //타겟의 방향쪽으로 미사일 회전
            missileCol[weaponCount].enabled = true;                                                                   //미사일 콜라이더 활성화
            missileWeapon[weaponCount].hitCount = levelUpScript.missileData.count;                                    //미사일의 타격 수
            
            attackTime_Missile = 0;                                                                                   //미사일의 타이머 0으로 초기화

            weaponCount++;                                                                                            //인덱스 값 증가

            if (weaponCount >= weaponScript.weaponPool)                                                               //인덱스 값 초과 시 0으로 초기화
                weaponCount = 0;
        }
    }

    public void Attack_Sword()                                                                                        //마법검 공격
    {
        if (isSword)
        {
            levelUpScript.sword[levelUpScript.swordCount].SetActive(true);                                            //마법검 활성화
            swords[levelUpScript.swordCount].swordAni.SetBool("isSword", true);                                       //마법검 애니메이션 실행
            
            soundManager.PlaySwordSound();                                                                            //마법검 사운드 재생
        }
    }
}
