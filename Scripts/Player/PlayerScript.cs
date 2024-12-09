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
    Transform staff;

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
    float attackTime_Laser = 0;         //레이저 공격 타이머
    float attackTime_Sword = 0;         //마법검 공격 타이머

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
        if (attackTime_Laser >= (levelUpScript.laserData.attackDelay / 100) * attackDelay && !isLaser)
        {
            laserWeapon.hitCount = levelUpScript.laserData.count;
            isLaser = true;
            //laserWeapon.LaserTrans();
            laserEnd.Retarget();
            laserBeam.SetActive(true);
            soundManager.PlayLaserSound();
        }
    }

    void Move()
    {
        float x = joy.Horizontal;
        float y = joy.Vertical;

        vec = new Vector3(x, y, 0);

        if (x < 0)
        {
            animator.SetBool("Run", true);
            trans.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
            hpBar.transform.localScale = new Vector3(-0.02f, 0.026f, 0.02f);
            laserOrb.transform.position = new Vector3(transform.position.x - 0.75f, transform.position.y + 0.75f, transform.position.z);
            laserOrb.transform.localScale = new Vector3(-0.8f, 0.8f, 1);
            floor.transform.localScale = new Vector3(-levelUpScript.floorData.range, levelUpScript.floorData.range, levelUpScript.floorData.range);
            
        }
        else if (x > 0)
        {
            animator.SetBool("Run", true);
            trans.localScale = new Vector3(scale.x, scale.y, scale.z);
            hpBar.transform.localScale = new Vector3(0.02f, 0.026f, 0.02f);
            laserOrb.transform.position = new Vector3(transform.position.x - 0.75f, transform.position.y + 0.75f, transform.position.z);
            laserOrb.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            floor.transform.localScale = new Vector3(levelUpScript.floorData.range, levelUpScript.floorData.range, levelUpScript.floorData.range);
            
        }
        else if (x == 0 || y == 0)
        {
            animator.SetBool("Run", false);
        }

        transform.position += (baseSpeed / 100 * speed) * Time.deltaTime * vec;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x),
                                         Mathf.Clamp(transform.position.y, minPos.y, maxPos.y),
                                         0);
    }

    void Attack_Missile()
    {
        if (!scanner.nearestTarget)
            return;

        Vector3 targetPos = scanner.nearestTarget.position;
        Vector3 dir = targetPos - staff.transform.position;
        dir = dir.normalized;

        if (weaponScript.missilePrefab[weaponCount].activeSelf == false)
        {
            weaponScript.missilePrefab[weaponCount].transform.position = staff.transform.position;
            weaponScript.missilePrefab[weaponCount].SetActive(true);
            soundManager.PlayMissileSound();
            weaponScript.missilePrefab[weaponCount].transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            missileCol[weaponCount].enabled = true;
            missileWeapon[weaponCount].hitCount = levelUpScript.missileData.count;
            
            attackTime_Missile = 0;

            weaponCount++;

            if (weaponCount >= weaponScript.weaponPool)
                weaponCount = 0;
        }
    }

    public void Attack_Sword()
    {
        if (isSword)
        {
            levelUpScript.sword[levelUpScript.swordCount].SetActive(true);
            swords[levelUpScript.swordCount].swordAni.SetBool("isSword", true);
            levelUpScript.sword[levelUpScript.swordCount].transform.position = gameObject.transform.position;
            
            soundManager.PlaySwordSound();
        }
    }
}
