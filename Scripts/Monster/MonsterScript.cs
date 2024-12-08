using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

[System.Serializable]
public class DamageParticle
{
    public ParticleSystem[] particle_;
}

public class MonsterScript : MonoBehaviour
{
    WaitForFixedUpdate wait_1 = new WaitForFixedUpdate();
    WaitForSeconds wait_2 = new WaitForSeconds(0.1f);
    WaitForSeconds wait_mon = new WaitForSeconds(0.4f);


    public MonsterData monsterData;
    public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    LevelUpScript levelUpScript;
    GameManager gameManager;
    PlayerScript playerScript;
    MonManager monManager;

    public Animator animator;

    public IObjectPool<GameObject> Pool { get; set; }

    float damageDelay = 0.8f;
    float damageTime = 0;

    bool isContact = false;

    public float hp;
    public float damage;
    public float defense;
    public float offense;
    public float speed;
    public float knockbackDefense;
    public float knockbackTime;

    bool isLive = true;
    bool isHit = false;

    public bool isSlow = false;
    public bool isDecrease = false;

    Rigidbody2D rigid;
    CircleCollider2D col;
    SpriteRenderer spriter;
    public Rigidbody2D target;

    [SerializeField]
    ParticleSystem[] damageParticle;
    [SerializeField]
    GameObject damageObj;
    public DamageParticle[] particle;
    [SerializeField]
    Weapon laserEnd;
    [SerializeField]
    Weapon floor;
    [SerializeField]
    Weapon blast;
    [SerializeField]
    GameObject blastObj;
    public float speed_Defalt;
    public float defense_Defalt;

    int pCount_0 = 0;
    int pCount_00 = 0;
    int pCount_000 = 0;
    int pCount_0000 = 0;

    int pCount = 0;

    void Awake()
    {
        levelUpScript = GameObject.FindWithTag("LevelManager").GetComponent<LevelUpScript>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        monManager = GameObject.FindWithTag("GameManager").GetComponent<MonManager>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponentInChildren<SpriteRenderer>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        target = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        
        for (int i = 0; i < damageParticle.Length; i++)
        {
            var main = damageParticle[i].main;
            main.startLifetime = 1.2f;
            main.startSize = 0.105f;
            main.startSpeed = 0.8f;
            main.duration = 0.8f;
            main.maxParticles = 1;

            for (int j = 0; j < 10; j++)
            {
                particle[i].particle_[j] = Instantiate(damageParticle[i]);
                particle[i].particle_[j].transform.parent = damageObj.transform;
                particle[i].particle_[j].transform.position = damageParticle[i].transform.position;
            }
        }

        ResetStat();
        speed_Defalt = speed;
        defense_Defalt = defense;
    }

    void Update()
    {
        if (gameManager.isOver)
            return;

        damageTime += Time.deltaTime;

        if (hp <= 0 && !animator.GetBool("Die"))
        {
            gameManager.AddExp();
            gameManager.killCount++;
            monManager.monCount++;
            isLive = false;
            isSlow = false;
            isDecrease = false;
            blast.blastTarget.Remove(gameObject);
            col.enabled = false;
            playerScript.soundManager.PlayMonDeadSound();
            animator.SetBool("Die", true);
            StartCoroutine(MonActive());
        }

        if (!isLive)
            return;

        if (!isContact && playerScript.hp > 0)
        {
            if (target.position.x <= transform.position.x)
                spriter.flipX = true;
            else if (target.position.x > transform.position.x)
                spriter.flipX = false;
        }
        
        if (!isLive || isHit)
            return;
        TargetTracking();
    }

    IEnumerator MonActive()
    {
        yield return wait_mon;

        spriter.color = new Color(255, 255, 255);
        gameObject.SetActive(false);

    }

    void LateUpdate()
    {
        if (playerScript.hp >  0)
        {
            if (target.transform.position.y + 0.2f > transform.position.y)
            {
                spriter.sortingOrder = 103;
            }
            else if (target.transform.position.y  - 0.2f < transform.position.y)
            {
                spriter.sortingOrder = 100;
            }
            else
            {
                spriter.sortingOrder = 101;
            }
        }
    }

    public void ResetStat()
    {
        hp = monsterData.hp + (gameManager.time / 5);
        hp += hp * levelUpScript.monHp;
        defense = monsterData.defense + levelUpScript.monDefense;
        damage = monsterData.damage;
        offense = monsterData.offense + levelUpScript.monOffense;
        speed = math.floor(Random.Range(monsterData.speed - 0.1f, monsterData.speed + 0.1f) * 10f) / 10f + (monsterData.speed * levelUpScript.monSpeed);
        knockbackDefense = monsterData.knockbackDefense + levelUpScript.monKnockbackDefense;
        knockbackTime = monsterData.knockbackTime;

        if (offense <= 0)
            offense = 0;

        isLive = true;
        isHit = false;
        col.enabled = true;

        animator.SetBool("Die", false);
    }

    void TargetTracking()
    {
        if (monsterData.name == "Spirit")
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    public void Knock_Missile()
    {
        isHit = true;
        float knockbackPower = levelUpScript.missileData.knockback - knockbackDefense;
        if (knockbackPower < 0)
            knockbackPower = 0;

        if (isHit)
            StartCoroutine(MissileKnockBack(knockbackPower));
    }

    public void Knock_Blast(GameObject blast)
    {
        isHit = true;
        float knockbackPower = levelUpScript.blastData.knockback - knockbackDefense;
        if (knockbackPower < 0)
            knockbackPower = 0;

        if (isHit)
            StartCoroutine(BlastKnockBack(knockbackPower, blast));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerScript.hp > 0)
        {
            isContact = true;

            if (damageTime > damageDelay)
            {
                float totalDamage;
                totalDamage = (offense + damage) - playerScript.defense;

                if (totalDamage >= 1)
                {
                    playerScript.hp -= totalDamage;
                    playerScript.animator.SetTrigger("Hit");

                    if (playerScript.hp <= 0)
                        playerScript.hp = 0;

                    playerScript.SetData();
                    gameManager.hpBar.fillAmount = playerScript.hp / playerScript.maxHp;

                }
                else
                {
                    playerScript.hp -= 1;
                    playerScript.animator.SetTrigger("Hit");

                    if (playerScript.hp <= 0)
                        playerScript.hp = 0;

                    playerScript.SetData();
                    gameManager.hpBar.fillAmount = playerScript.hp / playerScript.maxHp;

                }

                damageTime = 0;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerScript.hp > 0)
            isContact = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {

            isHit = true;
            float totalDamage;
            bool isCritical = false;
            float swordDamage = Random.Range(levelUpScript.swordData.damage - 3, levelUpScript.swordData.damage + 3);

            if (!levelUpScript.playerData.isTrueDamage)
                totalDamage = math.floor((playerScript.offense / 100 * swordDamage - defense) * 10f) / 10f;
            else
                totalDamage = math.floor((playerScript.offense / 100 * swordDamage) * 10f) / 10f;

            if (totalDamage < 1f)
                totalDamage = 1f;

            totalDamage = math.floor(totalDamage);
            DamageText(totalDamage, isCritical);
            animator.SetTrigger("Hit");
            hp -= totalDamage;

            float knockbackPower = levelUpScript.swordData.knockback - knockbackDefense;

            if (knockbackPower < 0)
                knockbackPower = 0;

            if (!levelUpScript.swordData.isKnock)
                knockbackPower = 0;

            if (isHit)
                StartCoroutine(SwordKnockBack(knockbackPower));
        }

    }

    IEnumerator MissileKnockBack(float power)
    {
        yield return wait_1;
        Vector3 playerPos = target.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * power, ForceMode2D.Impulse);
        yield return wait_2;
        rigid.velocity = Vector2.zero;
        isHit = false;
    }

    IEnumerator BlastKnockBack(float power, GameObject blast)
    {
        yield return wait_1;
        Vector3 blastPos = blast.transform.position;
        Vector3 dirVec = transform.position - blastPos;
        rigid.AddForce(dirVec.normalized * power, ForceMode2D.Impulse);
        yield return wait_2;
        rigid.velocity = Vector2.zero;
        isHit = false;
    }

    IEnumerator SwordKnockBack(float power)
    {
        yield return wait_1;
        Vector3 playerPos = target.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * power, ForceMode2D.Impulse);
        yield return wait_2;
        rigid.velocity = Vector2.zero;
        isHit = false;
    }

    public void DamageText(float totaldamage, bool isCritical)
    {
        int units;
        int tens;
        int hunbreds;
        int thousands;
        int damage = (int)totaldamage;

        if (!levelUpScript.playerData.isTrueDamage)
        {
            if (isCritical)
            {
                for (int i = 0; i < damageParticle.Length; i++)
                {
                    for (int j = 0; j < damageParticle.Length; j++)
                    {
                        var main = particle[i].particle_[j].main;
                        main.startLifetime = 1.2f;
                        main.startSize = 0.105f;
                        main.startSpeed = 0.8f;
                        main.startColor = new Color(200, 5, 0);
                    }
                }
            }
            else
            {
                for (int i = 0; i < damageParticle.Length; i++)
                {
                    for (int j = 0; j < damageParticle.Length; j++)
                    {
                        var main = particle[i].particle_[j].main;
                        main.startLifetime = 1.2f;
                        main.startSize = 0.1f;
                        main.startSpeed = 0.8f;
                        main.startColor = new Color(255, 255, 255);
                    }
                }

            }
        }
        else if (levelUpScript.playerData.isTrueDamage)
        {
            if (isCritical)
            {
                for (int i = 0; i < damageParticle.Length; i++)
                {
                    for (int j = 0; j < damageParticle.Length; j++)
                    {
                        var main = particle[i].particle_[j].main;
                        main.startLifetime = 1.2f;
                        main.startSize = 0.105f;
                        main.startSpeed = 0.8f;
                        main.startColor = new Color(70, 70, 70);
                    }
                }
            }
            else
            {
                for (int i = 0; i < damageParticle.Length; i++)
                {
                    for (int j = 0; j < damageParticle.Length; j++)
                    {
                        var main = particle[i].particle_[j].main;
                        main.startLifetime = 1.2f;
                        main.startSize = 0.1f;
                        main.startSpeed = 0.8f;
                        main.startColor = new Color(150, 150, 150);
                    }
                }
            }
        }
        if (damage < 10)
        {
            units = damage % 10;

            particle[0].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[units]);
            particle[0].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[0].particle_[pCount].Emit(1);

            pCount++;
            if (pCount >= damageParticle.Length)
                pCount = 0;
        }
        else if (10 <= damage && damage < 100)
        {
            units = damage % 10;
            tens = damage / 10;

            particle[0].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[units]);
            particle[1].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[tens]);

            particle[0].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x + 0.07f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[1].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x - 0.07f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);

            particle[0].particle_[pCount].Emit(1);
            particle[1].particle_[pCount].Emit(1);

            pCount++;
            if (pCount >= damageParticle.Length)
                pCount = 0;
        }
        else if (100 <= damage && damage < 1000)
        {
            units = damage % 10;
            tens = (damage / 10) % 10;
            hunbreds = damage / 100;

            particle[0].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[units]);
            particle[1].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[tens]);
            particle[2].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[hunbreds]);

            particle[0].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x + 0.14f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[1].particle_[pCount].transform.position = new Vector3(transform.position.x,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[2].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x - 0.14f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);

            particle[0].particle_[pCount].Emit(1);
            particle[1].particle_[pCount].Emit(1);
            particle[2].particle_[pCount].Emit(1);

            pCount++;
            if (pCount >= damageParticle.Length)
                pCount = 0;
        }
        else if (1000 <= damage)
        {
            units = damage % 10;
            tens = (damage / 10) % 10;
            hunbreds = (damage / 100) % 10;
            thousands = damage / 1000;

            particle[0].particle_[pCount_0000].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[units]);
            particle[1].particle_[pCount_0000].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[tens]);
            particle[2].particle_[pCount_0000].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[hunbreds]);
            particle[3].particle_[pCount_0000].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[thousands]);

            particle[0].particle_[pCount_0000].transform.position = new Vector3(damageObj.transform.position.x + 0.21f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[1].particle_[pCount_0000].transform.position = new Vector3(damageObj.transform.position.x + 0.07f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[2].particle_[pCount_0000].transform.position = new Vector3(damageObj.transform.position.x - 0.07f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[3].particle_[pCount_0000].transform.position = new Vector3(damageObj.transform.position.x - 0.21f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);

            particle[0].particle_[pCount_0000].Emit(1);
            particle[1].particle_[pCount_0000].Emit(1);
            particle[2].particle_[pCount_0000].Emit(1);
            particle[3].particle_[pCount_0000].Emit(1);

            pCount_0000++;
            if (pCount_0000 >= damageParticle.Length)
                pCount_0000 = 0;
        }
    }

}
