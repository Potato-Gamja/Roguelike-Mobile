using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using Random = UnityEngine.Random;
using MagicArsenal;

public class Weapon : MonoBehaviour
{
    GameManager gameManager;
    MonManager monManager;
    [SerializeField]
    MagicBeamStatic magicBeamStatic;
    GameObject player;
    PlayerScript playerScript;

    public Scanner scanner;
    public GameObject laserBeam;
    [SerializeField]
    GameObject orb;
    [SerializeField]
    Weapon laserEndPoint;

    public Animator swordAni;

    LevelUpScript levelUpScript;
    public string type;

    Rigidbody2D rb;
    public int hitCount;
    public float hitTime = 0.1f;
    
    public float time = 0;

   // [Header("Missile Weapon")]

    [Header("Sword Weapon")]
    BoxCollider2D boxCollider;
    float durTime;

    [Header("Laser Weapon")]
    CircleCollider2D circleCollider;
    MagicBeamStatic laserLength;
    [SerializeField]
    Weapon laserWeapon;
    Scanner laserScanner;
    public bool isHit = false;
    [SerializeField]
    float rotSpeed = 999999;
    public Vector3 targetPos;
    Vector3 dir;
    public bool isCheack = true;
    public GameObject laser;
    public GameObject target;
   

    [Header("Blast Weapon")]
    public List<GameObject> blastTarget;
    public BoxCollider2D boxCollider_blast;
    ParticleSystem particleSystem_;
    float playTime = 0f;
    bool isPlay;
    bool isCount;

    [Header("Floor Weapon")]
    public List<GameObject> floorTarget;
    public bool isSlow = false;
    public bool isDecrease = false;
    public GameObject circle;

    void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        levelUpScript = GameObject.FindWithTag("LevelManager").GetComponent<LevelUpScript>();
        monManager = GameObject.FindWithTag("GameManager").GetComponent<MonManager>();
        player = GameObject.FindWithTag("Player");
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        rb = GetComponent<Rigidbody2D>();

        if (type == "missile")
        {
            circleCollider = GetComponent<CircleCollider2D>();
            hitCount = levelUpScript.missileData.baseCount;
        }
        else if (type == "sword")
        {
            swordAni = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();
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
        if (gameManager.isOver)
            return;

        switch (type)
        {
            case "missile":
                if (transform.position.x > 20f || transform.position.x < -20f || transform.position.y > 20f || transform.position.y < -20f)
                {
                    hitCount = levelUpScript.missileData.count;
                    gameObject.SetActive(false);
                }
                transform.Translate(Vector3.up * levelUpScript.missileData.speed * Time.deltaTime);
                break;

            case "sword":
                if (time >= levelUpScript.swordData.duration)
                {
                    playerScript.isSword = true;
                    playerScript.attackTime_Sword = 0f;
                    time = 0f;
                    swordAni.SetBool("isSword", false);
                    playerScript.soundManager.StopSwordSound();
                }

                time += Time.deltaTime;

                transform.Rotate(Vector3.back * levelUpScript.swordData.speed * Time.deltaTime);
                transform.position = player.transform.position;
                break;

            case "laser":
                if (levelUpScript.isLaser)
                {
                    LaserTrans();
                }
                break;

            case "laserEnd":
                if (levelUpScript.isLaser && playerScript.isLaser)
                {
                    if (target == null || target.activeSelf == false)
                    {
                        target = laserScanner.targetObj;
                        laserLength.beamLength = 0;
                        isCheack = true;
                    }
                    circleCollider.enabled = true;
                    time += Time.deltaTime;

                    if (hitCount < 1)
                    {
                        circleCollider.enabled = false;
                        playerScript.attackTime_Laser = 0f;
                        playerScript.isLaser = false;
                        hitCount = levelUpScript.laserData.count;
                        laserLength.beamLength = 0;
                        laserScanner.nearestTarget = null;
                        target = null;
                        playerScript.soundManager.StopLaserSound();
                        laserBeam.SetActive(false);
                    }
                }
                break;

            case "blast":
                time += Time.deltaTime;

                if (isPlay)
                    playTime += Time.deltaTime;

                if (playTime < 0.5f && playTime >= 0.45f)
                {
                    boxCollider_blast.enabled = true;
                }
                if (playTime >= 0.5f)
                {
                    boxCollider_blast.enabled = false;
                    isPlay = false;
                    playTime = 0f;
                }

                if (time >= levelUpScript.blastData.attackDelay)
                {
                    float ranX_1 = player.transform.position.x - 4.5f;
                    float ranX_2 = player.transform.position.x + 4.5f;

                    float ranY_1 = player.transform.position.y - 6f;
                    float ranY_2 = player.transform.position.y + 6f;

                    if (ranX_1 < playerScript.minPos.x)
                        ranX_1 = playerScript.minPos.x;

                    if (ranX_2 > playerScript.maxPos.x)
                        ranX_2 = playerScript.maxPos.x;

                    if (ranY_1 < playerScript.minPos.y)
                        ranY_1 = playerScript.minPos.y;

                    if (ranY_2 > playerScript.maxPos.y)
                        ranY_2 = playerScript.maxPos.y;

                    Vector2 ran = new Vector2(math.floor(Random.Range(ranX_1, ranX_2) * 10) * 0.1f,
                                              math.floor(Random.Range(ranY_1, ranY_2) * 10) * 0.1f);

                    transform.position = ran;
                    isPlay = true;
                    particleSystem_.Play();
                    playerScript.soundManager.PlayBlastSound();
                    time = 0f;
                }
                break;

            case "floor":
                time += Time.deltaTime;

                break;
        }
    }

    public void Retarget()
    {
            target = laserScanner.targetObj;
    }

    public void LaserTrans()
    {
        if (playerScript.isLaser && laserBeam.activeSelf)
        {
            transform.position = orb.transform.position;
            if (!scanner.nearestTarget)
                return;

            if (laserEndPoint.target && laserEndPoint.isHit)
                targetPos = laserEndPoint.target.transform.position;
            else if (laserEndPoint.target && !laserEndPoint.isHit)
                targetPos = scanner.nearestTarget.position;

            dir = targetPos - orb.transform.position;
            dir = dir.normalized;
            float dis;
            dis = Vector2.Distance(targetPos, orb.transform.position);
            laserLength.beamLength = dis;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
        }
        
    }

    public void FloorDebuff()
    {
        for (int i = 0; i < floorTarget.Count; i++)
        {
            MonsterScript targetMon = floorTarget[i].GetComponent<MonsterScript>();
            targetMon.speed = targetMon.speed_Defalt - targetMon.speed_Defalt * 0.01f * levelUpScript.floorData.slow;

            targetMon.defense = targetMon.defense_Defalt - levelUpScript.floorData.decrease;
        }

    }

    public void SwordDis()
    {
        levelUpScript.sword[levelUpScript.swordCount].SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
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