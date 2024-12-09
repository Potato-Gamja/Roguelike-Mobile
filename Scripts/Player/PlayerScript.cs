using MagicArsenal;
using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public LevelUpScript levelUpScript;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    Scanner scanner;
    [SerializeField]
    WeaponScript weaponScript
    public SoundManager soundManager;

    private Rigidbody2D rb;
    public Animator animator;
    [SerializeField]
    SpriteRenderer spriter;
    Transform trans;
    [SerializeField]
    Transform staff;

    public GameObject hpBar;
    public GameObject hpBarPos;

    public GameObject sword;
    public Weapon[] swords;
    public GameObject laser;
    
    public GameObject laserBeam;
    public GameObject laserOrb;

    public Weapon laserWeapon;
    public Weapon laserEnd;

    public GameObject floor;
    MagicRotation floorRot;

    public int playerType;

    public float maxHp;
    public float hp;
    public int hp_;
    public float defense;
    public float offense;
    public float baseSpeed;
    public float speed;
    public float critical;
    public float baseAttackDelay;
    public float attackDelay;
    public float exp;
    public Vector3 scale;

    int weaponCount = 0;
    float attackTime_Missile = 0;
    public float attackTime_Laser = 0;
    public float attackTime_Sword = 0;
    public float swordScale = 0;

    [SerializeField]
    CircleCollider2D[] missileCol;
    [SerializeField]
    Weapon[] missileWeapon;

    public bool isSword = true;
    public bool isLaser = false;
    bool isMove = false;
    bool isLive = true;

    public FloatingJoystick joy;
    public Vector2 minPos;
    public Vector2 maxPos;

    public Vector3 vec;

    void Awake()
    {
        SetData();

        hp = levelUpScript.playerData.hp;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        floorRot = GetComponent<MagicRotation>();

    }

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            missileCol[i] = weaponScript.missilePrefab[i].GetComponent<CircleCollider2D>();
            missileWeapon[i] = weaponScript.missilePrefab[i].GetComponent<Weapon>();
        }
    }

    public void SetData()
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
        if (Time.timeScale == 0f || gameManager.isOver)
            return;

        attackTime_Missile += Time.deltaTime;

        if (levelUpScript.isLaser)
        {
            attackTime_Laser += Time.deltaTime;
        }
        if (levelUpScript.isSword)
        {
            attackTime_Sword += Time.deltaTime;
        }

        Move();

        if (attackTime_Missile >= (levelUpScript.missileData.attackDelay / 100) * attackDelay)
        {
            Attack_Missile();
        }
        if (attackTime_Sword >= (levelUpScript.swordData.attackDelay / 100 * attackDelay) && isSword)
        {
            if (isSword)
                Attack_Sword();
            isSword = false;

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
