using UnityEngine;
using UnityEngine.UI;

public class LevelUpScript : MonoBehaviour
{
    public GameManager gameManager;
    public Joystick joystick;
    public PlayerScript playerScript;
    public PlayerData playerData;
    public GameObject[] sword;
    public GameObject fade;
    public Button pasue;
    
    [Header("Weapon")]                                //무기와 무기 능력 선택 카드
    public GameObject weaponPanel;
    public GameObject[] tempWeapons;
    public GameObject[] weapons;
    public GameObject missileWeapon;
    public GameObject swordWeapon;
    public GameObject laserWeapon;
    public GameObject blastWeapon;
    public GameObject floorWeapon;
    public GameObject[] missileWeapons;
    public GameObject[] swordWeapons;
    public GameObject[] laserWeapons;
    public GameObject[] blastWeapons;
    public GameObject[] floorWeapons;

    public GameObject[] unlockWeapons;
    
    [Header("Data")]                                  //무기 데이터
    public WeaponData missileData;
    public WeaponData swordData;
    public WeaponData laserData;
    public WeaponData blastData;
    public WeaponData floorData;

    public GameObject laserOrb;
    public GameObject laser;
    public GameObject[] blast;
    public GameObject floor;
    public Weapon floorW;
    public Sprite starSprite;                         //무기 능력 레벨 업 시 변경할 별 스프라이트

    GameObject mStarPanel_0;                          //별 판넬
    GameObject mStarPanel_1;
    GameObject mStarPanel_2;
    GameObject mStarPanel_3;
    GameObject mStarPanel_4;

    Image[] mStar_0 = new Image[5];                   //빈 별 이미지
    Image[] mStar_1 = new Image[5];                   //이후 아래는 위와 동일일
    Image[] mStar_2 = new Image[5];
    Image[] mStar_3 = new Image[5];
    Image[] mStar_4 = new Image[5];

    GameObject sStarPanel_0;
    GameObject sStarPanel_1;
    GameObject sStarPanel_2;
    GameObject sStarPanel_3;
    GameObject sStarPanel_4;

    Image[] sStar_0 = new Image[4];
    Image[] sStar_1 = new Image[5];
    Image[] sStar_2 = new Image[5];
    Image[] sStar_3 = new Image[5];
    Image[] sStar_4 = new Image[1];

    GameObject lStarPanel_0;
    GameObject lStarPanel_1;
    GameObject lStarPanel_2;
    GameObject lStarPanel_3;

    Image[] lStar_0 = new Image[5];
    Image[] lStar_1 = new Image[5];
    Image[] lStar_2 = new Image[5];
    Image[] lStar_3 = new Image[5];

    GameObject bStarPanel_0;
    GameObject bStarPanel_1;
    GameObject bStarPanel_2;
    GameObject bStarPanel_3;
    GameObject bStarPanel_4;

    Image[] bStar_0 = new Image[5];
    Image[] bStar_1 = new Image[5];
    Image[] bStar_2 = new Image[5];
    Image[] bStar_3 = new Image[5];
    Image[] bStar_4 = new Image[5];

    GameObject fStarPanel_0;
    GameObject fStarPanel_1;
    GameObject fStarPanel_2;
    GameObject fStarPanel_3;

    Image[] fStar_0 = new Image[5];
    Image[] fStar_1 = new Image[5];
    Image[] fStar_2 = new Image[5];
    Image[] fStar_3 = new Image[5];

    public int swordCount = 0;                        //마법검의 개수
    public int blastCount = 0;                        //블래스트의 개수
                                        
    public bool isSword = false;                      //무기 해금 여부
    public bool isLaser = false;
    public bool isBlast = false;
    public bool isFloor = false;
    
    int missile_Lv_0 = 0;                             //무기 능력 별 레벨
    int missile_Lv_1 = 0;
    int missile_Lv_2 = 0;
    int missile_Lv_3 = 0;
    int missile_Lv_4 = 0;

    int sword_Lv_0 = 0;
    int sword_Lv_1 = 0;
    int sword_Lv_2 = 0;
    int sword_Lv_3 = 0;
    int sword_Lv_4 = 0;

    int laser_Lv_0 = 0;
    int laser_Lv_1 = 0;
    int laser_Lv_2 = 0;
    int laser_Lv_3 = 0;

    int blast_Lv_0 = 0;
    int blast_Lv_1 = 0;
    int blast_Lv_2 = 0;
    int blast_Lv_3 = 0;
    int blast_Lv_4 = 0;

    int floor_Lv_0 = 0;
    int floor_Lv_1 = 0;
    int floor_Lv_2 = 0;
    int floor_Lv_3 = 0;

    bool isWeapon = false;
    bool isStat = false;

    int i;
    int ii;
    int n;

    //임시로 무기 능력을 저장하는 오브젝트트
    GameObject tempW_0;
    GameObject tempW_1;
    GameObject tempW_2;

    public GameObject weaponGroup;   //무기 능력 오브젝트 그룹
    public Button reroll;            //리롤버튼
    public Image rerollImage;        //리롤버튼 이미지

    public MonManager monManager;    //몬스터 매니저저
    public float monHp;
    public float monOffense;
    public float monDefense;
    public float monSpeed;
    public float monKnockbackDefense;

    float time = 0;

    private void Awake()
    {    
        //로컬라이제이션을 적용시킨 뒤에 오브젝트 비활성화
        weaponGroup.SetActive(false);

        ResetStat();         //스탯 재설정
        SetStatWeapon();     //무기 능력 설정
        SetStar();           //무기 능력 레벨 별 표시 설정
    }

    //스탯 재설정
    void ResetStat()
    {
        //미사일 데이터
        missileData.speed = missileData.baseSpeed;
        missileData.damage = missileData.baseDamage;
        missileData.attackDelay = missileData.baseAttackDelay;
        missileData.critical = missileData.baseCritical;
        missileData.count = missileData.baseCount;
        //마법검 데이터
        swordData.speed = swordData.baseSpeed;
        swordData.damage = swordData.baseDamage;
        swordData.attackDelay = swordData.baseAttackDelay;
        swordData.isKnock = swordData.baseIsKnock;
        swordData.knockback = swordData.baseKnockback;
        swordData.duration = swordData.baseDuration;
        //레이저 데이터
        laserData.speed = laserData.baseSpeed;
        laserData.damage = laserData.baseDamage;
        laserData.attackDelay = laserData.baseAttackDelay;
        laserData.baseDuration = laserData.duration;
        laserData.count = laserData.baseCount;
        //블래스트 데이터
        blastData.range = blastData.baseRange;
        blastData.speed = blastData.baseSpeed;
        blastData.damage = blastData.baseDamage;
        blastData.attackDelay = blastData.baseAttackDelay;
        blastData.critical = blastData.baseCritical;
        blastData.criticalDamage = blastData.baseCriticalDamage;
        blastData.count = blastData.baseCount;
        //장판 데이터
        floorData.range = floorData.baseRange;
        floorData.damage = floorData.baseDamage;
        floorData.attackDelay = floorData.baseAttackDelay;
        floorData.slow = floorData.baseSlow;
        floorData.decrease = floorData.baseDecrease;
        //플레이어 데이터터
        playerData.maxHp = playerData.baseHp;
        playerData.hp = playerData.baseHp;
        playerData.defense = playerData.baseDefense;
        playerData.offense = playerData.baseOffense;
        playerData.speed = playerData.baseSpeed;
        playerData.attackDelay = playerData.baseAttackDelay;
        playerData.isTrueDamage = playerData.baseIsTrueDamage;
        playerData.critical = playerData.baseCritical;
    }

    //무기 별로 자식 오브젝트에 있는 능력을 각 무기의 배열에 넣는 작업
    void SetStatWeapon()
    {
        //미사일 능력 설정
        for (int i = 0; i < missileWeapon.transform.childCount; i++)
        {
            missileWeapons[i] = missileWeapon.transform.GetChild(i).gameObject;
        }
        //마법검 능력 설정
        for (int i = 0; i < swordWeapon.transform.childCount; i++)
        {
            swordWeapons[i] = swordWeapon.transform.GetChild(i).gameObject;
        }
        //레이저 능력 설정일
            tempWeapons[i] = unlockWeapons[0];
            i++;
        }

        if (isLaser)
        {
            if (laser_Lv_0 < 5)
            {
                tempWeapons[i] = laserWeapons[0];
                i++;
            }
            if (laser_Lv_1 < 5)
            {
                tempWeapons[i] = laserWeapons[1];
                i++;
            }
            if (laser_Lv_2 < 5)
            {
                tempWeapons[i] = laserWeapons[2];
                i++;
            }
            if (laser_Lv_3 < 5)
            {
                tempWeapons[i] = laserWeapons[3];
                i++;
            }
        }
        else if (!isLaser)
        {
            tempWeapons[i] = unlockWeapons[1];
            i++;
        }

        if (isBlast)
        {
            if (blast_Lv_0 < 5)
            {
                tempWeapons[i] = blastWeapons[0];
                i++;
            }
            if (blast_Lv_1 < 5)
            {
                tempWeapons[i] = blastWeapons[1];
                i++;
            }
            if (blast_Lv_2 < 5)
            {
                tempWeapons[i] = blastWeapons[2];
                i++;
            }
            if (blast_Lv_3 < 5)
            {
                tempWeapons[i] = blastWeapons[3];
                i++;
            }
            if (blast_Lv_4 < 5)
            {
                tempWeapons[i] = blastWeapons[4];
                i++;
            }
        }
        else if (!isBlast)
        {
            tempWeapons[i] = unlockWeapons[2];
            i++;
        }

        if (isFloor)
        {
            if (floor_Lv_0 < 5)
            {
                tempWeapons[i] = floorWeapons[0];
                i++;
            }
            if (floor_Lv_1 < 5)
            {
                tempWeapons[i] = floorWeapons[1];
                i++;
            }
            if (floor_Lv_2 < 5)
            {
                tempWeapons[i] = floorWeapons[2];
                i++;
            }
            if (floor_Lv_3 < 5)
            {
                tempWeapons[i] = floorWeapons[3];
                i++;
            }
        }
        else if (!isFloor)
        {
            tempWeapons[i] = unlockWeapons[3];
            i++;
        }
       
        weapons = new GameObject[3];                                     //무기 능력 선택에 쓸 배열
        n = Random.Range(0, i);                                          //배열 인덱스로 쓰일 n 값을 랜덤으로 0~i 사이 값으로 설정
        
        weapons[0] = tempWeapons[n];                                     //배열에 무작위 무기 능력을 넣기
        tempW_0 = weapons[0].transform.parent.gameObject;                //무작위 무기 능력의 부모를 임시로 tempW_0에 넣는다 -> 나중에 부모를 변경한 뒤에 다시 돌려놓기 위함
        weapons[0].transform.SetParent(weaponPanel.transform);           //해당 무기 능력의 부모 변경
        weapons[0].transform.localPosition = new Vector3(-720, 0, 0);    //무기 능력 선택 카드의 위치 조정
        weapons[0].SetActive(true);                                      //무기 능력 카드 상태 활성화

        n = Random.Range(0, i);
        weapons[1] = tempWeapons[n];
        while (weapons[0] == weapons[1])                                 //첫번째와 두번째 능력이 같을 경우 다른 능력이 선택될 때까지 반복문 실행
        {
            n = Random.Range(0, i);
            weapons[1] = tempWeapons[n];
        }
        tempW_1 = weapons[1].transform.parent.gameObject;                //이후 아래는 위와 동일
        weapons[1].transform.SetParent(weaponPanel.transform);
        weapons[1].transform.localPosition = new Vector3(0, 0, 0);
        weapons[1].SetActive(true);

        n = Random.Range(0, i);
        weapons[2] = tempWeapons[n];
        while (weapons[0] == weapons[2] || weapons[1] == weapons[2])
        {
            n = Random.Range(0, i);
            weapons[2] = tempWeapons[n];
        }
        tempW_2 = weapons[2].transform.parent.gameObject;
        weapons[2].transform.SetParent(weaponPanel.transform);
        weapons[2].transform.localPosition = new Vector3(720, 0, 0);
        weapons[2].SetActive(true);

    }

    //부모가 변경된 무기 능력를 기존의 부모로 다시 변경, 능력 선택 시 실행
    public void RelocationWeapons()
    {
        weapons[0].transform.SetParent(tempW_0.transform);
        weapons[1].transform.SetParent(tempW_1.transform);
        weapons[2].transform.SetParent(tempW_2.transform);
    }
    
    //부모가 변경된 무기 능력를 기존의 부모로 다시 변경, 능력 선택 시 실행
    public void ReArr_Weapons_Reroll()
    {
        weapons[0].transform.SetParent(tempW_0.transform);
        weapons[1].transform.SetParent(tempW_1.transform);
        weapons[2].transform.SetParent(tempW_2.transform);

        reroll.interactable = false;
        rerollImage.color = new Color32(52, 255, 0, 60);
        WeaponsArr();
    }

    public void UnlockWeapons_0()
    {
        isSword = true;
        sword[0].SetActive(true);
        playerScript.soundManager.PlaySwordSound();
    }
    public void UnlockWeapons_1()
    {
        isLaser = true;
        laserOrb.SetActive(true);
        laser.SetActive(true);
    }
    public void UnlockWeapons_2()
    {
        isBlast = true;
        blast[0].SetActive(true);
    }
    public void UnlockWeapons_3()
    {
        isFloor = true;
        floor.SetActive(true);
    }

    public void WeaponSelectEvent_0()
    {
        isWeapon = true;
        if (isStat)
        {
            return;
        }
        pasue.interactable = false;
        Time.timeScale = 0.0f;
        gameManager.joystick.SetActive(false);
        WeaponsArr();
        fade.SetActive(true);
        weaponPanel.SetActive(true);
        reroll.interactable = true;
        rerollImage.color = new Color32(52, 255, 0, 255);
    }

    public void WeaponSelectEvent_1()
    {
        if (!isStat)
        {
            gameManager.joystick.SetActive(true);
            TimeSlowUp();
        }
        joystick.handle.anchoredPosition = Vector2.zero;
        joystick.input = Vector2.zero;
        weaponPanel.SetActive(false);
        isWeapon = false;

    }

    public void TimeSlowUp()
    {
        if (Time.timeScale <= 1)
        {
            if (fade.activeSelf == false)
                fade.SetActive(true);

            pasue.interactable = true;

            time += 0.15f;
            Time.timeScale += time;

            if (Time.timeScale >= 1)
            {
                fade.SetActive(false);
                Time.timeScale = 1.0f;
                time = 0;
                statPanel.SetActive(false);
                weaponPanel.SetActive(false);
                return;
            }
            else
                Invoke("TimeSlowUp", 0.06f);
        }
    }

    //미사일 능력력
    public void MissileEvent_0()
    {
        missileData.damage += (missileData.baseDamage / 100) * 30;
        mStar_0[missile_Lv_0].sprite = starSprite;
        playerScript.SetData();
        missile_Lv_0++;
    }
    public void MissileEvent_1()
    {
        missileData.critical += 8f;
        missileData.speed -= (missileData.baseSpeed / 100) * 6;
        mStar_1[missile_Lv_1].sprite = starSprite;
        missile_Lv_1++;
    }
    public void MissileEvent_2()
    {
        missileData.damage -= (missileData.baseDamage / 100) * 8;
        missileData.count += 1;
        mStar_2[missile_Lv_2].sprite = starSprite;
        missile_Lv_2++;
    }
    public void MissileEvent_3()
    {
        missileData.speed += (missileData.baseSpeed / 100) * 10;
        missileData.attackDelay -= (missileData.baseAttackDelay / 100) * 8;
        mStar_3[missile_Lv_3].sprite = starSprite;
        missile_Lv_3++;
    }
    public void MissileEvent_4()
    {
        missileData.damage += (missileData.baseDamage / 100) * 50;
        missileData.attackDelay += (missileData.baseAttackDelay / 100) * 14;
        mStar_4[missile_Lv_4].sprite = starSprite;
        missile_Lv_4++;
    }

    //마법검 능력
    public void SwordEvent_0()
    {
        sword[swordCount].SetActive(false);
        swordData.damage -= (swordData.baseDamage / 100) * 10;
        swordCount++;
        sword[swordCount].SetActive(true);
        playerScript.soundManager.PlaySwordSound();
        sStar_0[sword_Lv_0].sprite = starSprite;
        sword_Lv_0++;
    }
    public void SwordEvent_1()
    {
        swordData.speed += (swordData.baseSpeed / 100) * 12;
        swordData.duration += (swordData.baseDuration / 100) * 8;
        sStar_1[sword_Lv_1].sprite = starSprite;
        sword_Lv_1++;
    }
    public void SwordEvent_2()
    {
        swordData.damage -= (swordData.baseDamage / 100) * 8;
        swordData.knockback += (swordData.baseKnockback / 100) * 6;
        sStar_2[sword_Lv_2].sprite = starSprite;
        sword_Lv_2++;
    }
    public void SwordEvent_3()
    {
        swordData.damage += (swordData.baseDamage / 100) * 30;
        swordData.speed -= (swordData.baseSpeed / 100) * 8;
        sStar_3[sword_Lv_3].sprite = starSprite;
        sword_Lv_3++;
    }
    public void SwordEvent_4()
    {
        swordData.damage += (swordData.baseDamage / 100) * 160;
        swordData.isKnock = false;
        sStar_4[sword_Lv_4].sprite = starSprite;
        sword_Lv_4++;
    }

    //레이저 능력
    public void LaserEvent_0()
    {
        laserData.count -= 2;                                                //레이저 타격 회수 감소
        laserData.damage += (laserData.baseDamage / 100) * 60;               //레이저 데미지 증가
        laserData.attackDelay -= (laserData.baseAttackDelay / 100) * 6;      //레이저 쿨타임 감소
        lStar_0[laser_Lv_0].sprite = starSprite;                             //레이저 능력의 별 이미지 변경
        laser_Lv_0++;                                                        //해당 레이저 레벨 증가
    }
    public void LaserEvent_1()
    {
        laserData.count += 6;                                                //레이저 타격 회수 증가
        laserData.damage += (laserData.baseDamage / 100) * 20;               //레이저 데미지 증가
        laserData.attackDelay += (laserData.baseAttackDelay / 100) * 4;      //레이저 쿨타임 증가
        lStar_1[laser_Lv_1].sprite = starSprite;
        laser_Lv_1++;
    }
    public void LaserEvent_2()
    {
        laserData.count -= 1;                                                //레이저 타격 회수 감소
        laserData.attackDelay -= (laserData.baseAttackDelay / 100) * 8;      //레이저 쿨타임 감소
        lStar_2[laser_Lv_2].sprite = starSprite;
        laser_Lv_2++;
    }
    public void LaserEvent_3()
    {
        laserData.count += 3;                                                //레이저 타격 회수 증가
        laserData.attackDelay += (laserData.baseAttackDelay / 100) * 2;      //레이저 쿨타임 증가
        lStar_3[laser_Lv_3].sprite = starSprite;
        laser_Lv_3++;
    }

    //블래스트 능력
    public void BlastEvent_0()
    {
        blastData.damage -= blastData.baseDamage * 0.1f;
        blastCount++;
        blast[blastCount].SetActive(true);
        bStar_0[blast_Lv_0].sprite = starSprite;
        blast_Lv_0++;
    }
    public void BlastEvent_1()
    {
        blastData.damage += blastData.baseDamage * 0.2f;
        blastData.range += blastData.baseRange * 0.05f;
        for (int i = 0; i < blast.Length; i++)
        {
            blast[i].transform.localScale = new Vector3(blastData.range, blastData.range, blastData.range);
        }
        //blastData.attackDelay -= (blastData.baseAttackDelay / 100) * 8;
        bStar_1[blast_Lv_1].sprite = starSprite;
        blast_Lv_1++;
    }
    public void BlastEvent_2()
    {
        //blastData.damage += (blastData.baseDamage / 100) * 5;
        blastData.attackDelay -= blastData.baseAttackDelay * 0.05f;
        bStar_2[blast_Lv_2].sprite = starSprite;
        blast_Lv_2++;
    }
    public void BlastEvent_3()
    {
        blastData.damage += blastData.baseDamage * 0.3f;
        for (int i = 0; i < blast.Length; i++)
        {
            blast[i].transform.localScale = new Vector3(blastData.range, blastData.range, blastData.range);
        }
        blastData.critical += 6f;
        bStar_3[blast_Lv_3].sprite = starSprite;
        blast_Lv_3++;
    }
    public void BlastEvent_4()
    {
        blastData.criticalDamage += 0.2f;
        bStar_4[blast_Lv_4].sprite = starSprite;
        blast_Lv_4++;
    }

    //Floor
    public void FloorEvent_0()
    {
        floorData.damage += (floorData.baseDamage / 100) * 20;
        fStar_0[floor_Lv_0].sprite = starSprite;
        floor_Lv_0++;
    }
    public void FloorEvent_1()
    {
        floorData.range += floorData.baseRange / 12;
        floor.transform.localScale = new Vector3(floorData.range, floorData.range, floorData.range);
        fStar_1[floor_Lv_1].sprite = starSprite;
        floor_Lv_1++;
    }
    public void FloorEvent_2()
    {
        floorData.slow += 10;
        floorW.isSlow = true;
        floorW.FloorDebuff();
        fStar_2[floor_Lv_2].sprite = starSprite;
        floor_Lv_2++;
    }
    public void FloorEvent_3()
    {
        floorData.decrease += 5;
        floorW.isDecrease = true;
        floorW.FloorDebuff();
        fStar_3[floor_Lv_3].sprite = starSprite;
        floor_Lv_3++;
    }

}
