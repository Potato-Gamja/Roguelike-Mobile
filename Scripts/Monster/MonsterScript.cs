using System.Collections;
using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;                                //Mathematics와 UnityEngine의 모호한 참조로 인해 UnityEngine에 있는 랜덤을 사용 

[System.Serializable]                                             //데이터 직렬화
public class DamageParticle                                       //파티클 시스템 이차원배열을 위한 클래스
{
    public ParticleSystem[] particle_;                            //이차원 배열
}

public class MonsterScript : MonoBehaviour
{
    WaitForFixedUpdate wait_1 = new WaitForFixedUpdate();         //코루틴의 yield return new 최적화를 위해 미리 캐싱하여 사용
    WaitForSeconds wait_2 = new WaitForSeconds(0.1f);
    WaitForSeconds wait_mon = new WaitForSeconds(0.4f);

    public MonsterData monsterData;
    LevelUpScript levelUpScript;
    GameManager gameManager;
    PlayerScript playerScript;
    MonManager monManager;

    public Animator animator;                                     //몬스터의 애니메이터

    float damageDelay = 0.8f;                                     //몬스터가 플레이어에게 피해를 입히는 쿨타임
    float damageTime = 0;                                         //시간

    bool isContact = false;                                       //몬스터와 플레이어가 접촉해있는지에 대한 여부

    public float hp;                                              //체력
    public float damage;                                          //데미지
    public float defense;                                         //방어력
    public float offense;                                         //공격력
    public float speed;                                           //스피드
    public float knockbackDefense;                                //넉백저항력
    public float knockbackTime;                                   //넉백쿨타임

    bool isLive = true;                                           //몬스터의 생존 여부
    bool isHit = false;                                           //몬스터의 피격 여부

    public bool isSlow = false;                                   //몬스터의 이동속도 감소 여부
    public bool isDecrease = false;                               //몬스터의 방어력 감소 여부

    Rigidbody2D rigid;
    CircleCollider2D col;
    SpriteRenderer spriter;
    public Rigidbody2D target;                                    //몬스터의 타겟 = 플레이어

    [SerializeField]
    ParticleSystem[] damageParticle;                              //데미지 파티클 시스템 배열
    [SerializeField]
    GameObject damageObj;                                         //데미지 파티클 게임오브젝트
    public DamageParticle[] particle;                             //데미지 파티클 이차원배열
    [SerializeField]
    Weapon laserEnd;                                              //무기 레이저의 끝 스크립트
    [SerializeField]
    Weapon floor;                                                 //무기 장판 스크립트
    [SerializeField]
    Weapon blast;                                                 //무기 블래스트 스크립트
    [SerializeField]   
    GameObject blastObj;                                          //무기 블래스트 오브젝트
    
    public float speed_Defalt;                                    //몬스터의 기본 이동속도 - 장판 범위에서 벗어났을 경우 원래대로 돌려놓기 위한 값
    public float defense_Defalt;                                  //몬스터의 기본 방어력 - 장판 범위에서 벗어났을 경우 원래대로 돌려놓기 위한 값
    
    int pCount = 0;                                               //파티클 시스템 오브젝트의 인덱스 값

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
        
        for (int i = 0; i < damageParticle.Length; i++)                                                //데미지 파티클 시스템 풀링과 파티클 시스템의 값 변경
        {
            var main = damageParticle[i].main;                                                         //데미지 파티클
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

            particle[0].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[units]);
            particle[1].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[tens]);
            particle[2].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[hunbreds]);
            particle[3].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[thousands]);

            particle[0].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x + 0.21f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[1].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x + 0.07f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[2].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x - 0.07f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[3].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x - 0.21f,
                                                   damageObj.transform.position.y, damageObj.transform.position.z);

            particle[0].particle_[pCount].Emit(1);
            particle[1].particle_[pCount].Emit(1);
            particle[2].particle_[pCount].Emit(1);
            particle[3].particle_[pCount].Emit(1);

            pCount++;
            if (pCount >= damageParticle.Length)
                pCount = 0;
        }
    }

}
