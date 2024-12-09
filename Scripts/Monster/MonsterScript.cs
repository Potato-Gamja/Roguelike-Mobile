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

    float damageDelay = 0.8f;                                     //몬스터의 공격 쿨타임
    float damageTime = 0;                                         //공격 타이머

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
            var main = damageParticle[i].main;                                                         //데미지 파티클의 main 할당
            main.startLifetime = 1.2f;                                                                 //라이프타임 값 변경
            main.startSize = 0.105f;                                                                   //시작크기 값 변경
            main.startSpeed = 0.8f;                                                                    //시작속도 값 변경
            main.duration = 0.8f;                                                                      //유지시간 값 변경
            main.maxParticles = 1;                                                                     //최대 파티클의 수 변경

            for (int j = 0; j < 10; j++)
            {
                particle[i].particle_[j] = Instantiate(damageParticle[i]);                             //데미지 파티클 오브젝트 생성
                particle[i].particle_[j].transform.parent = damageObj.transform;                       //오브젝트의 부모 변경
                particle[i].particle_[j].transform.position = damageParticle[i].transform.position;    //오브젝트의 위치 변경
            }
        }

        ResetStat();                                                                                   //몬스터의 스탯 설정
        speed_Defalt = speed;                                                                          //몬스터의 기본 이동속도 값 대입입
        defense_Defalt = defense;                                                                      //몬스터의 기본 방어력 값 대입
    }

    void Update()
    {
        if (gameManager.isOver)                                                             //게임오버 시 리턴
            return;

        damageTime += Time.deltaTime;                                                       //공격 쿨타임 비교 시간 값 증가

        if (hp <= 0 && !animator.GetBool("Die"))                                            //체력이 0이하와 애니메이터의 파라미터 Die가 거짓일 경우 실행
        {
            gameManager.AddExp();                                                           //플레이어 경험치 증가
            gameManager.killCount++;                                                        //처치한 몬스터 수 증가
            monManager.monCount++;                                                          //몬스터 카운트 증가
            isLive = false;                                                                 //몬스터의 생존 여부
            isSlow = false;                                                                 //몬스터의 이동속도 감소 여부
            isDecrease = false;                                                             //몬스터의 방어력 감소 여부
            blast.blastTarget.Remove(gameObject);                                           //블래스트의 타겟에서 해당 몬스터 제거
            col.enabled = false;                                                            //콜라이더 비활성화
            playerScript.soundManager.PlayMonDeadSound();                                   //몬스터 처치 사운드 재생
            animator.SetBool("Die", true);                                                  //몬스터 처치 애니메이션 실행이 되게 파라미터 Die를 참으로
            StartCoroutine(MonActive());                                                    //몬스터의 비활성화 함수
        }
        
        if (!isLive)                                                                        //플레이어가 살아있지않을 경우 리턴
            return;
            
        if (!isContact && playerScript.hp > 0)                                              //플레이어와 접촉해있지 않고 플레이어가 살아있을 경우 실행
        {
            if (target.position.x <= transform.position.x)                                  //몬스터가 플레이어의 방향을 바라보게 스프라이트의 플립X을 껐다 켰다
                spriter.flipX = true;
            else if (target.position.x > transform.position.x)
                spriter.flipX = false;
        }
        
        if (isHit)                                                                          //몬스터가 피격 시 리턴 -> 맞으면서 움직이지않게, 경직 상태
            return;
            
        TargetTracking();                                                                   //몬스터가 플레이어를 쫓아가는 함수
    }

    IEnumerator MonActive()  
    {
        yield return wait_mon;                                                              //몬스터의 사망 애니메이션 재생 시간

        spriter.color = new Color(255, 255, 255);                                           //몬스터의 컬러값 원래대로
        gameObject.SetActive(false);                                                        //몬스터 비활성화

    }

    void LateUpdate()                                                                       //플레이어와 몬스터의 높이 차이에 따라 소팅레이어 값 변경경
    {
        if (playerScript.hp >  0)
        {
            if (target.transform.position.y + 0.2f > transform.position.y)                  //플레이어가 더 높이 있을 경우
            {
                spriter.sortingOrder = 103;                                                 //플레이어보다 앞으로 보이게 값 변경
            }
            else if (target.transform.position.y  - 0.2f < transform.position.y)            //플레이어가 더 낮게 있을 경우
            {
                spriter.sortingOrder = 100;                                                 //플레이어보다 뒤로 보이게 값 변경
            }
            else
            {
                spriter.sortingOrder = 101;                                                 //그게 아니라면 원래대로
            }
        }
    }

    public void ResetStat()                                                                                           //몬스터의 스탯 설정
    {
        hp = monsterData.hp + (gameManager.time / 5);                                                                 //몬스터 데이터의 체력과 시간에 흐름에 따른 추가 체력 합연산                                            
        defense = monsterData.defense + levelUpScript.monDefense;                                                     //몬스터 데이터의 방어력과 추가 방어력 합연산
        damage = monsterData.damage;                                                                                  //몬스터 데이터의 데미지 대입
        offense = monsterData.offense + levelUpScript.monOffense;                                                     //몬스터 데이터의 공격력과 추가 공격력 합연산
        speed = math.floor(Random.Range(monsterData.speed - 0.1f,                                                     //몬스터 이동속도 랜덤 -+ 0.1값과 추가 이동속도의 합연산 값을 소수점 한자리 버리기
                            monsterData.speed + 0.1f) * 10f) / 10f + (monsterData.speed * levelUpScript.monSpeed);
        knockbackDefense = monsterData.knockbackDefense + levelUpScript.monKnockbackDefense;                          //몬스터 데이터의 넉백저항과 추가 넉백저항 합연산
        knockbackTime = monsterData.knockbackTime;                                                                    //몬스터 데이터의 넉백 쿨타임

        isLive = true;                                                                                                //몬스터 생존 여부
        isHit = false;                                                                                                //피격 여부
        col.enabled = true;                                                                                           //콜라이더 활성화

        animator.SetBool("Die", false);                                                                               //애니메이터의 파라미터 Die을 거짓으로로
    }

    void TargetTracking()                                                                   //몬스터의 Rigidbody을 이용한 이동
    {
        Vector2 dirVec = target.position - rigid.position;                                  //몬스터와 플레이어의 거리
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;                       //방향 벡터에 이동속도와 델타타임을 곱연산

        rigid.MovePosition(rigid.position + nextVec);                                       //몬스터의 목적지를 향해 이동 
    }

    public void Knock_Missile()                                                           //미사일에 대한 넉백
    {
        isHit = true;                                                                     //피격 여부
        float knockbackPower = levelUpScript.missileData.knockback - knockbackDefense;    //미사일 넉백과 몬스터의 넉백저향을 계산 뒤 대입
        
        if (knockbackPower < 0)                                                           //넉백저항이 커서 뒤가 아니라 앞으로 밀려오는 현상 막기
            knockbackPower = 0;

        if (isHit)
            StartCoroutine(MissileKnockBack(knockbackPower));                             //미사일 넉백 코루틴 실행
    }

    public void Knock_Blast(GameObject blast)                                             //블래스트 넉백
    {
        isHit = true;                                                                     //피격 여부
        float knockbackPower = levelUpScript.blastData.knockback - knockbackDefense;      //블래스트 넉백과 몬스터의 넉백저항을 계산 뒤 대입
        
        if (knockbackPower < 0)                                                           //넉백저항이 커서 뒤가 아니라 앞으로 밀려오는 현상 막기
            knockbackPower = 0;

        if (isHit)
            StartCoroutine(BlastKnockBack(knockbackPower, blast));                        //블래스트 넉백 코루틴 실행
    }
    
    IEnumerator MissileKnockBack(float power)
    {
        yield return wait_1;                                                              //지연
        Vector3 playerPos = target.transform.position;                                    //플레이어 위치 대입
        Vector3 dirVec = transform.position - playerPos;                                  //몬스터와 플레이어의 거리
        rigid.AddForce(dirVec.normalized * power, ForceMode2D.Impulse);                   //방향 백터에 넉백 정도를 곱하여 순간적인 힘을 가함
        yield return wait_2;                                                              //넉백 되는 시간 동안 지연
        rigid.velocity = Vector2.zero;                                                    //넉백을 없애기 위한 멈추기
        isHit = false;                                                                    //피격 여부
    }

    IEnumerator BlastKnockBack(float power, GameObject blast)                             //이후 아래는 위와 동일
    {
        yield return wait_1;
        Vector3 blastPos = blast.transform.position;
        Vector3 dirVec = transform.position - blastPos;
        rigid.AddForce(dirVec.normalized * power, ForceMode2D.Impulse);
        yield return wait_2;
        rigid.velocity = Vector2.zero;
        isHit = false;
    }
    
    private void OnTriggerStay2D(Collider2D collision)                                           //트리거 콜라이더가 접촉 중
    {
        if (collision.CompareTag("Player") && playerScript.hp > 0)                               //접촉한 콜라이더의 태그가 플레이어일 시 & 플레이어의 체력이 0 초과일 경우
        {
            isContact = true;                                                                    //플레이어와의 접촉 여부

            if (damageTime > damageDelay)                                                        //공격 가능한 지 체크
            {
                float totalDamage;                                                               //총 데미지 변수
                totalDamage = (offense + damage) - playerScript.defense;                         //몬스터의 공격력과 데미지을 합하고 플레이어의 방어력과 계산하여 데미지의 총합 대입

                if (totalDamage >= 1)                                                            //데미지가 1이상일 경우
                {
                    playerScript.hp -= totalDamage;                                              //데미지만큼 플레이어의 체력 감소
                    playerScript.animator.SetTrigger("Hit");                                     //플레이어의 피격 애니메이터 트리거 작동

                    if (playerScript.hp <= 0)                                                    //플레이어의 체력이 0보다 아내로 내려가지 않게 하기
                        playerScript.hp = 0;

                    playerScript.SetData();                                                      //감소된 체력을 적용하기 위한 플레이어의 데이터 설정 함수 실행
                    gameManager.hpBar.fillAmount = playerScript.hp / playerScript.maxHp;         //감소된 체력을 플레이어의 체력바에 적용

                }
                else                                                                             //데미지가 0이하 일 경우
                { 
                    playerScript.hp -= 1;                                                        //플레이어의 체력 1감소
                    playerScript.animator.SetTrigger("Hit");                                     //플레이어의 피격 애니메이터 트리거 작동

                    if (playerScript.hp <= 0)                                                    //플레이어의 체력이 0보다 아내로 내려가지 않게 하기
                        playerScript.hp = 0;

                    playerScript.SetData();                                                      //감소된 체력을 적용하기 위한 플레이어의 데이터 설정 함수 실행
                    gameManager.hpBar.fillAmount = playerScript.hp / playerScript.maxHp;         //감소된 체력을 플레이어의 체력바에 적용

                }

                damageTime = 0;                                                                  //공격 타이머 0으로 초기화
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)                            //트리거 콜라이더 접촉 중지 시
    {
        if (collision.CompareTag("Player") && playerScript.hp > 0)                //플레이어와 접촉, 플레이어의 체력이 0 초과 시 실행
            isContact = false;                                                    //플레이어와의 접촉 여부

    }

    private void OnTriggerEnter2D(Collider2D collision)                                                            //트리거 콜라이더 접촉 시
    {
        if (collision.CompareTag("Sword"))                                                                         //접촉한 콜라이더의 태그가 마법검일 경우우
        {

            isHit = true;                                                                                          //피격 여부
            float totalDamage;                                                                                     //마법검의 총 데미지
            bool isCritical = false;                                                                               //치명타 여부
            float swordDamage = Random.Range(levelUpScript.swordData.damage - 3,                                   //마법검의 데미지 -+3의 랜덤값
                                             levelUpScript.swordData.damage + 3);

            if (!levelUpScript.playerData.isTrueDamage)                                                            //고정피해 off 시
                totalDamage = math.floor((playerScript.offense / 100 * swordDamage - defense) * 10f) / 10f;        //플레이어의 공격력과 마법검의 데미지, 몬스터의 방어력을 계산한 뒤 소수점 한자리 버리기
            else                                                                                                   //고정피해 on 시
                totalDamage = math.floor((playerScript.offense / 100 * swordDamage) * 10f) / 10f;                  //고정피해이므로 플레이어의 공격력과 마법검의 데미지 계산 뒤 소수점 한자리 버리기

            if (totalDamage < 1f)                                                                                  //마법검의 총 데미지가 1 미만일 경우
                totalDamage = 1f;                                                                                  //데미지 1로

            totalDamage = math.floor(totalDamage);                                                                 //소수점을 버린 값을 총 데미지에 대입
            DamageText(totalDamage, isCritical);                                                                   //데미지 파티클을 표시하는 함수
            animator.SetTrigger("Hit");                                                                            //몬스터의 피격 트리거 작동
            hp -= totalDamage;                                                                                     //총 데미지만큼 몬스터의 체력 감소

            float knockbackPower = levelUpScript.swordData.knockback - knockbackDefense;                           //마법검의 넉백과 몬스터의 넉백저항을 계산한 뒤 대입

            if (knockbackPower < 0)                                                                                //넉백저항이 커서 뒤가 아니라 앞으로 밀려오는 현상 막기
                knockbackPower = 0;

            if (!levelUpScript.swordData.isKnock)                                                                  //만약 마법검의 넉백이 제거되었을 경우에는 0으로
                knockbackPower = 0;

            if (isHit)
                StartCoroutine(SwordKnockBack(knockbackPower));                                                    //마법검 넉백 코루틴 실행행
        }

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
    
    public void DamageText(float totaldamage, bool isCritical)           //데미지 파티클 함수
    {
        int units;                                                       //일의 자리수
        int tens;                                                        //십의 자리수
        int hunbreds;                                                    //백의 자리수
        int thousands;                                                   //천의 자리수
        int damage = (int)totaldamage;                                   //총 데미지를 int형으로 대입

        if (!levelUpScript.playerData.isTrueDamage)                      //고정데미지 여부 확인
        {
            if (isCritical)                                              //치명타 피해 시
            {
                for (int i = 0; i < damageParticle.Length; i++)
                {
                    for (int j = 0; j < damageParticle.Length; j++)
                    {
                        var main = particle[i].particle_[j].main;
                        main.startLifetime = 1.2f;
                        main.startSize = 0.105f;                         //사이즈 변경
                        main.startSpeed = 0.8f;
                        main.startColor = new Color(200, 5, 0);          //노란색으로 색상 변경
                    }
                }
            }
            else                                                         //일반 피해 시
            {
                for (int i = 0; i < damageParticle.Length; i++)
                {
                    for (int j = 0; j < damageParticle.Length; j++)
                    {
                        var main = particle[i].particle_[j].main;
                        main.startLifetime = 1.2f;
                        main.startSize = 0.1f;
                        main.startSpeed = 0.8f;
                        main.startColor = new Color(255, 255, 255);     //흰색으로 변경
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
        if (damage < 10)                                          //데미지가 10 미만일 경우
        {
            units = damage;                                                                                           //일의 자리의 데미지 인덱스 값

            particle[0].particle_[pCount].textureSheetAnimation.SetSprite(0, gameManager.damageSprite[units]);        //파티클 시스템의 텍스쳐시트애니메이션의 스프라이트를 일의 자리 데미지 인덱스 값의 스프라이트로 설정
            particle[0].particle_[pCount].transform.position = new Vector3(damageObj.transform.position.x,            //파티클의 위치 조정
                                                   damageObj.transform.position.y, damageObj.transform.position.z);
            particle[0].particle_[pCount].Emit(1);                                                                    //파티클 입자를 1개 방출

            pCount++;                                                                                                 //데미지 파티클의 인덱스 값 증가
            if (pCount >= damageParticle.Length)                                                                      //데미지 파티클의 인덱스 값이 오버 시 0으로 초기화
                pCount = 0;
        }
        else if (10 <= damage && damage < 100)                    //데미지가 10 이상 100 미만일 경우우
        {
            units = damage % 10;                                  //데미지의 일의 자리를 구하기 위해 나머지 연산자 사용
            tens = damage / 10;                                   //데미지의 십의 자리를 구하기 위해 10으로 나누기 사용

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
        else if (100 <= damage && damage < 1000)                  //데미지가 100 이상 1000 미만일 경우
        {
            units = damage % 10;                                  //데미지의 일의 자리를 구하기 위해 나머지 연산자 사용
            tens = (damage / 10) % 10;                            //데미지의 십의 자리를 구하기 위해 나누기와 나머지 연산자 사용
            hunbreds = damage / 100;                              //데미지의 백의 자리를 구하기 위해 나누기 연산자 사용용

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
    }

}
