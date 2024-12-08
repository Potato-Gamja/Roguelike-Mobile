using UnityEngine;
using UnityEngine.UI;

public class LevelUpScript : MonoBehaviour
{
    public GameManager gameManager;                   //스크립트, 오브젝트
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

    public GameObject weaponGroup;                   //무기 능력 오브젝트 그룹
    public Button reroll;                            //리롤버튼
    public Image rerollImage;                        //리롤버튼 이미지

    public MonManager monManager; 
    public float monHp;
    public float monOffense;
    public float monDefense;
    public float monSpeed;
    public float monKnockbackDefense;

    float time = 0;

    private void Awake()
    {    
        weaponGroup.SetActive(false);                 //로컬라이제이션을 적용시킨 뒤에 오브젝트 비활성화

        ResetStat();                                  //스탯 재설정
        SetStatWeapon();                              //무기 능력 설정
        SetStar();                                    //무기 능력 레벨 별 표시 설정
    }

    //스탯 재설정
    void ResetStat()
    {
        missileData.speed = missileData.baseSpeed;             //미사일 데이터
        missileData.damage = missileData.baseDamage;
        missileData.attackDelay = missileData.baseAttackDelay;
        missileData.critical = missileData.baseCritical;
        missileData.count = missileData.baseCount;
        
        swordData.speed = swordData.baseSpeed;                 //마법검 데이터
        swordData.damage = swordData.baseDamage;
        swordData.attackDelay = swordData.baseAttackDelay;
        swordData.isKnock = swordData.baseIsKnock;
        swordData.knockback = swordData.baseKnockback;
        swordData.duration = swordData.baseDuration;
       
        laserData.speed = laserData.baseSpeed;                 //레이저 데이터
        laserData.damage = laserData.baseDamage;
        laserData.attackDelay = laserData.baseAttackDelay;
        laserData.baseDuration = laserData.duration;
        laserData.count = laserData.baseCount;
        
        blastData.range = blastData.baseRange;                 //블래스트 데이터
        blastData.speed = blastData.baseSpeed;
        blastData.damage = blastData.baseDamage;
        blastData.attackDelay = blastData.baseAttackDelay;
        blastData.critical = blastData.baseCritical;
        blastData.criticalDamage = blastData.baseCriticalDamage;
        blastData.count = blastData.baseCount;
       
        floorData.range = floorData.baseRange;                 //장판 데이터
        floorData.damage = floorData.baseDamage;
        floorData.attackDelay = floorData.baseAttackDelay;
        floorData.slow = floorData.baseSlow;
        floorData.decrease = floorData.baseDecrease;
       
        playerData.maxHp = playerData.baseHp;                  //플레이어 데이터
        playerData.hp = playerData.baseHp;
        playerData.defense = playerData.baseDefense;
        playerData.offense = playerData.baseOffense;
        playerData.speed = playerData.baseSpeed;
        playerData.attackDelay = playerData.baseAttackDelay;
        playerData.isTrueDamage = playerData.baseIsTrueDamage;
        playerData.critical = playerData.baseCritical;
    }
    
    void SetStatWeapon()                                                                  //무기 별로 자식 오브젝트에 있는 능력을 각 무기의 배열에 넣기기
    {

        for (int i = 0; i < missileWeapon.transform.childCount; i++)                      //미사일 무기 그룹의 자식을 미사일 무기 배열에 순차적으로 추가
        {
            missileWeapons[i] = missileWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < swordWeapon.transform.childCount; i++)                        //마법검 무기 그룹의 자식을 미사일 무기 배열에 순차적으로 추가
        {
            swordWeapons[i] = swordWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < laserWeapon.transform.childCount; i++)                        //레이저 무기 그룹의 자식을 미사일 무기 배열에 순차적으로 추가
        {
            laserWeapons[i] = laserWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < blastWeapon.transform.childCount; i++)                        //블래스트 무기 그룹의 자식을 미사일 무기 배열에 순차적으로 추가
        {
            blastWeapons[i] = blastWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < floorWeapon.transform.childCount; i++)                        //장판 무기 그룹의 자식을 미사일 무기 배열에 순차적으로 추가
        {
            floorWeapons[i] = floorWeapon.transform.GetChild(i).gameObject;
        }
    }
    
    void SetStar()                                                                                   //무기 능력 레벨을 표시하는 별 세팅
    {
        mStarPanel_0 = missileWeapons[0].transform.GetChild(4).gameObject;                           //미사일 무기 자식오브젝트 중 판넬 대입
        mStarPanel_1 = missileWeapons[1].transform.GetChild(4).gameObject;
        mStarPanel_2 = missileWeapons[2].transform.GetChild(4).gameObject;
        mStarPanel_3 = missileWeapons[3].transform.GetChild(4).gameObject;
        mStarPanel_4 = missileWeapons[4].transform.GetChild(4).gameObject;

        for (int i = 0; i < 5; i++)
        {
            mStar_0[i] = mStarPanel_0.transform.GetChild(i).gameObject.GetComponent<Image>();        //판넬의 자식오브젝트인 별 이미지 컴포넌트를 대입
        }                                                                                            //이후 아래는 위와 동일
        for (int i = 0; i < 5; i++)
        {
            mStar_1[i] = mStarPanel_1.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            mStar_2[i] = mStarPanel_2.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            mStar_3[i] = mStarPanel_3.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            mStar_4[i] = mStarPanel_4.transform.GetChild(i).gameObject.GetComponent<Image>();
        }

        sStarPanel_0 = swordWeapons[0].transform.GetChild(4).gameObject;
        sStarPanel_1 = swordWeapons[1].transform.GetChild(4).gameObject;
        sStarPanel_2 = swordWeapons[2].transform.GetChild(4).gameObject;
        sStarPanel_3 = swordWeapons[3].transform.GetChild(4).gameObject;
        sStarPanel_4 = swordWeapons[4].transform.GetChild(4).gameObject;

        for (int i = 0; i < 4; i++)
        {
            sStar_0[i] = sStarPanel_0.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            sStar_1[i] = sStarPanel_1.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            sStar_2[i] = sStarPanel_2.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            sStar_3[i] = sStarPanel_3.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 1; i++)
        {
            sStar_4[i] = sStarPanel_4.transform.GetChild(i).gameObject.GetComponent<Image>();
        }

        lStarPanel_0 = laserWeapons[0].transform.GetChild(4).gameObject;
        lStarPanel_1 = laserWeapons[1].transform.GetChild(4).gameObject;
        lStarPanel_2 = laserWeapons[2].transform.GetChild(4).gameObject;
        lStarPanel_3 = laserWeapons[3].transform.GetChild(4).gameObject;

        for (int i = 0; i < 5; i++)
        {
            lStar_0[i] = lStarPanel_0.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            lStar_1[i] = lStarPanel_1.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            lStar_2[i] = lStarPanel_2.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            lStar_3[i] = lStarPanel_3.transform.GetChild(i).gameObject.GetComponent<Image>();
        }

        bStarPanel_0 = blastWeapons[0].transform.GetChild(4).gameObject;
        bStarPanel_1 = blastWeapons[1].transform.GetChild(4).gameObject;
        bStarPanel_2 = blastWeapons[2].transform.GetChild(4).gameObject;
        bStarPanel_3 = blastWeapons[3].transform.GetChild(4).gameObject;
        bStarPanel_4 = blastWeapons[4].transform.GetChild(4).gameObject;

        for (int i = 0; i < 5; i++)
        {
            bStar_0[i] = bStarPanel_0.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            bStar_1[i] = bStarPanel_1.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            bStar_2[i] = bStarPanel_2.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            bStar_3[i] = bStarPanel_3.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            bStar_4[i] = bStarPanel_4.transform.GetChild(i).gameObject.GetComponent<Image>();
        }

        fStarPanel_0 = floorWeapons[0].transform.GetChild(4).gameObject;
        fStarPanel_1 = floorWeapons[1].transform.GetChild(4).gameObject;
        fStarPanel_2 = floorWeapons[2].transform.GetChild(4).gameObject;
        fStarPanel_3 = floorWeapons[3].transform.GetChild(4).gameObject;

        for (int i = 0; i < 5; i++)
        {
            fStar_0[i] = fStarPanel_0.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            fStar_1[i] = fStarPanel_1.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            fStar_2[i] = fStarPanel_2.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        for (int i = 0; i < 5; i++)
        {
            fStar_3[i] = fStarPanel_3.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
    }

    public void WeaponsArr()                                            
    {
        i = 0;                                                                      //무작위 무기 배열의 최대 인덱스 값으로 쓰일 i에 0을 대입

        if (missile_Lv_0 < 5)                                                       //무기 능력 별로 최대 레벨 달성 시 배열에 추가하지 않기 위한 조건문
        {
            tempWeapons[i] = missileWeapons[0];
            i++;                                                                    //인덱스 값 i 증가 1
        }
        if (missile_Lv_1 < 5)
        {
            tempWeapons[i] = missileWeapons[1];
            i++;
        }
        if (missile_Lv_2 < 5)
        {
            tempWeapons[i] = missileWeapons[2];
            i++;
        }
        if (missile_Lv_3 < 5)
        {
            tempWeapons[i] = missileWeapons[3];
            i++;
        }
        if (missile_Lv_4 < 5)
        {
            tempWeapons[i] = missileWeapons[4];
            i++;
        }

        if (isSword)                                                               //무기를 잠금해제 했는지 확인하는 조건문
        {                                                                          //이후 아래는 위와 동일
            if (sword_Lv_0 < 4)
            {
                tempWeapons[i] = swordWeapons[0];
                i++;
            }
            if (sword_Lv_1 < 5)
            {
                tempWeapons[i] = swordWeapons[1];
                i++;
            }
            if (sword_Lv_2 < 5 && sword_Lv_4 == 0)
            {
                tempWeapons[i] = swordWeapons[2];
                i++;
            }
            if (sword_Lv_3 < 5)
            {
                tempWeapons[i] = swordWeapons[3];
                i++;
            }
            if (sword_Lv_4 < 1 && sword_Lv_2 == 0)
            {
                tempWeapons[i] = swordWeapons[4];
                i++;
            }
        }
        else if (!isSword)
        {
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

        weapons = new GameObject[3];                                                //무기 능력 선택에 쓸 배열 크기 할당당

        n = Random.Range(0, i);                                                     //배열 인덱스로 쓰일 n 값을 랜덤으로 0~i 사이 값으로 설정
        weapons[0] = tempWeapons[n];                                                //배열에 무작위 무기 능력을 넣기
        tempW_0 = weapons[0].transform.parent.gameObject;                           //무작위 무기 능력의 부모를 임시로 tempW_0에 넣는다 -> 나중에 부모를 변경한 뒤에 다시 돌려놓기 위함
        weapons[0].transform.SetParent(weaponPanel.transform);                      //해당 무기 능력의 부모 변경
        weapons[0].transform.localPosition = new Vector3(-720, 0, 0);               //무기 능력 선택 카드의 위치 조정
        weapons[0].SetActive(true);                                                 //무기 능력 카드 상태 활성화

        n = Random.Range(0, i);
        weapons[1] = tempWeapons[n];
        while (weapons[0] == weapons[1])                                            //첫번째와 두번째 능력이 같을 경우 다른 능력이 선택될 때까지 반복문 실행
        {                                                                           //이후 아래는 위와 동일
            n = Random.Range(0, i);
            weapons[1] = tempWeapons[n];
        }
        tempW_1 = weapons[1].transform.parent.gameObject;
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

    public void RelocationWeapons()                                                 //부모가 변경된 무기 능력를 기존의 부모로 다시 변경, 능력 선택 시 실행
    {
        weapons[0].transform.SetParent(tempW_0.transform);
        weapons[1].transform.SetParent(tempW_1.transform);
        weapons[2].transform.SetParent(tempW_2.transform);
    }
    
    public void ReArr_Weapons_Reroll()                                             //부모가 변경된 무기 능력를 기존의 부모로 다시 변경, 리롤 버튼을 눌렀을 시 실행
    {
        weapons[0].transform.SetParent(tempW_0.transform);                         //임시로 넣어둔 부모를 다시 불러와 무기 능력의 부모를 원래대로 돌려놓기기
        weapons[1].transform.SetParent(tempW_1.transform);
        weapons[2].transform.SetParent(tempW_2.transform);

        reroll.interactable = false;                                               //리롤 버튼 기능 비활성화
        rerollImage.color = new Color32(52, 255, 0, 60);                           //리롤 비활성화 표시를 위한 버튼 이미지의 컬러 값 변경
        WeaponsArr();
    }

    public void UnlockWeapons_0()                              //마법검 잠금해제
    {
        isSword = true;                                        //마법검 활성화 여부
        sword[0].SetActive(true);                              //마법검 활성화
        playerScript.soundManager.PlaySwordSound();            //마법검 사운드 재생
    }
    public void UnlockWeapons_1()                              //레이저 잠금해제
    {
        isLaser = true;                                        //레이저 활성화 여부
        laserOrb.SetActive(true);                              //레이저오브 활성화
        laser.SetActive(true);                                 //레이저 활성화
    }
    public void UnlockWeapons_2()                              //블래스트 잠금해제
    {
        isBlast = true;                                        //블래스트 활성화 여부
        blast[0].SetActive(true);                              //블래스트 활성화
    }
    public void UnlockWeapons_3()                              //장판 잠금해제
    {
        isFloor = true;                                        //장판 활성화 여부
            floor.SetActive(true);                             //장판 활성화
    }

    public void WeaponSelectEvent_0()                              //무기 능력 선택 이벤트
    {
        isWeapon = true;                                           //무기 능력 선택 이벤트 활성화 여부
        pasue.interactable = false;                                //일시정지 버튼 기능 비활성화
        Time.timeScale = 0.0f;                                     //타임스케일을 0으로 하여 정지상태
        gameManager.joystick.SetActive(false);                     //조이스틱 비활성화
        WeaponsArr();                                              //무작위 무기 능력 정하기
        fade.SetActive(true);                                      //페이드 활성화
        weaponPanel.SetActive(true);                               //무기 능력 선택 판넬 활성화
        reroll.interactable = true;                                //리롤 버튼 기능 활성화
        rerollImage.color = new Color32(52, 255, 0, 255);          //리롤 버튼 이미지의 컬러 값 기존대로 변경
    }

    public void TimeSlowUp()                                       //무기 능력 선택 이벤트 후 천천히 정지 상태 풀리기
    {
        if (Time.timeScale <= 1)                                   //타임스케일이 1과 같거나 작을 때 실행
        {
            if (fade.activeSelf == false)                          //페이드 비활성화
                fade.SetActive(true);

            pasue.interactable = true;                             //일시정지 버튼 기능 활성화

            time += 0.15f;                                         //가속적으로 일시정지가 해제하기 위한 연산
            Time.timeScale += time;                                //타임스케일의 값을 타임만큼 더하여 시간을 흐르게 하기

            if (Time.timeScale >= 1)                               //타임스케일의 값이 1과 같거나 1보다 클 경우 실행
            {
                fade.SetActive(false);                             //페이드 비활성화
                Time.timeScale = 1.0f;                             //정상적인 게임속도를 위해 타임스케일에 1 대입
                time = 0;                                          //타임 값 0으로 초기화
                weaponPanel.SetActive(false);                      //무기 능력 선택 판넬 비활성화
                return;                                            //리턴
            }
            else
                Invoke("TimeSlowUp", 0.06f);                       //인보크로 타임슬로우업을 0.06초의 딜레이를 가지고 실행
        }
    }

    public void MissileEvent_0()                                              //미사일 능력
    {
        missileData.damage += (missileData.baseDamage / 100) * 30;            //미사일 데미지 증가
        mStar_0[missile_Lv_0].sprite = starSprite;                            //미사일 능력의 별 이미지 변경
        missile_Lv_0++;                                                       //미사일 레벨 증가
    }
    public void MissileEvent_1()
    {
        missileData.critical += 8f;                                           //미사일 치명타 확률 증가
        missileData.speed -= (missileData.baseSpeed / 100) * 6;               //미사일 속도 감소
        mStar_1[missile_Lv_1].sprite = starSprite;                            
        missile_Lv_1++;
    }
    public void MissileEvent_2()
    {
        missileData.damage -= (missileData.baseDamage / 100) * 8;             //미사일 데미지 감소
        missileData.count += 1;                                               //미사일 관통 수치 증가
        mStar_2[missile_Lv_2].sprite = starSprite;
        missile_Lv_2++;
    }
    public void MissileEvent_3()
    {
        missileData.speed += (missileData.baseSpeed / 100) * 10;              //미사일 속도 증가
        missileData.attackDelay -= (missileData.baseAttackDelay / 100) * 8;   //미사일 쿨타임 감소
        mStar_3[missile_Lv_3].sprite = starSprite;
        missile_Lv_3++;
    }
    public void MissileEvent_4()
    {
        missileData.damage += (missileData.baseDamage / 100) * 50;            //미사일 데미지 증가
        missileData.attackDelay += (missileData.baseAttackDelay / 100) * 14;  //미사일 쿨타임 증가
        mStar_4[missile_Lv_4].sprite = starSprite;
        missile_Lv_4++;
    }

    public void SwordEvent_0()                                                //마법검 능력
    {
        sword[swordCount].SetActive(false);                                   //이전 레벨의 마법검 비활성화
        swordData.damage -= (swordData.baseDamage / 100) * 10;                //마법검 데미지 감소
        swordCount++;                                                         //마법검 배열의 인덱스 값 증가
        sword[swordCount].SetActive(true);                                    //인덱스 값의 마법검 활성화
        playerScript.soundManager.PlaySwordSound();                           //마법검 사운드 재생
        sStar_0[sword_Lv_0].sprite = starSprite;                              //마법검 능력의 별 이미지 변경
        sword_Lv_0++;                                                         //마법검 레벨 증가
    }
    public void SwordEvent_1()
    {
        swordData.speed += (swordData.baseSpeed / 100) * 12;                  //마법검 회전속도 증가
        swordData.duration += (swordData.baseDuration / 100) * 8;             //마법검 지속시간 증가
        sStar_1[sword_Lv_1].sprite = starSprite;
        sword_Lv_1++;
    }
    public void SwordEvent_2()
    {
        swordData.damage -= (swordData.baseDamage / 100) * 8;                 //마법검 데미지 감소
        swordData.knockback += (swordData.baseKnockback / 100) * 6;           //마법검 넉백 증가
        sStar_2[sword_Lv_2].sprite = starSprite;
        sword_Lv_2++;
    }
    public void SwordEvent_3()
    {
        swordData.damage += (swordData.baseDamage / 100) * 30;                //마법검의 데미지 증가
        swordData.speed -= (swordData.baseSpeed / 100) * 8;                   //마법검의 회전전속도 감소
        sStar_3[sword_Lv_3].sprite = starSprite;
        sword_Lv_3++;
    }
    public void SwordEvent_4()
    {
        swordData.damage += (swordData.baseDamage / 100) * 160;               //마법검의 데미지 증가
        swordData.isKnock = false;                                            //마법검의 넉백 여부 비활성화
        sStar_4[sword_Lv_4].sprite = starSprite;
        sword_Lv_4++;
    }

    public void LaserEvent_0()                                                //레이저 능력
    {
        laserData.count -= 2;                                                 //레이저 타격 회수 감소
        laserData.damage += (laserData.baseDamage / 100) * 60;                //레이저 데미지 증가
        laserData.attackDelay -= (laserData.baseAttackDelay / 100) * 6;       //레이저 쿨타임 감소
        lStar_0[laser_Lv_0].sprite = starSprite;                              //레이저 능력의 별 이미지 변경
        laser_Lv_0++;                                                         //해당 레이저 레벨 증가
    }
    public void LaserEvent_1()
    {
        laserData.count += 6;                                                 //레이저 타격 회수 증가
        laserData.damage += (laserData.baseDamage / 100) * 20;                //레이저 데미지 증가
        laserData.attackDelay += (laserData.baseAttackDelay / 100) * 4;       //레이저 쿨타임 증가
        lStar_1[laser_Lv_1].sprite = starSprite;
        laser_Lv_1++;
    }
    public void LaserEvent_2()
    {
        laserData.count -= 1;                                                 //레이저 타격 회수 감소
        laserData.attackDelay -= (laserData.baseAttackDelay / 100) * 8;       //레이저 쿨타임 감소
        lStar_2[laser_Lv_2].sprite = starSprite;
        laser_Lv_2++;
    }
    public void LaserEvent_3()
    {
        laserData.count += 3;                                                 //레이저 타격 회수 증가
        laserData.attackDelay += (laserData.baseAttackDelay / 100) * 2;       //레이저 쿨타임 증가
        lStar_3[laser_Lv_3].sprite = starSprite;
        laser_Lv_3++;
    }

    public void BlastEvent_0()                                                //블래스트 능력
    {
        blastData.damage -= blastData.baseDamage * 0.1f;                      //블래스트 데미지 감소
        blastCount++;                                                         //블래스트의 배열 인덱스 값 증가
        blast[blastCount].SetActive(true);                                    //인덱스 값의 블래스트 활성화
        bStar_0[blast_Lv_0].sprite = starSprite;                              //블래스트 능력의 별 이미지 변경
        blast_Lv_0++;                                                         //블래스트 레벨 증가
    }
    public void BlastEvent_1()
    {
        blastData.damage += blastData.baseDamage * 0.2f;                      //블래스트 데미지 증가
        blastData.range += blastData.baseRange * 0.05f;                       //블래스트 범위 증가
        for (int i = 0; i < blast.Length; i++)                                //모든 블래스트의 스케일값 조정
        {
            blast[i].transform.localScale = new Vector3(blastData.range, blastData.range, blastData.range);
        }
        bStar_1[blast_Lv_1].sprite = starSprite;
        blast_Lv_1++;
    }
    public void BlastEvent_2()
    {
        blastData.attackDelay -= blastData.baseAttackDelay * 0.05f;           //블래스트의 쿨타임 감소
        bStar_2[blast_Lv_2].sprite = starSprite;
        blast_Lv_2++;
    }
    public void BlastEvent_3()
    {
        blastData.damage += blastData.baseDamage * 0.3f;                      //블래스트의 데미지 증가
        blastData.critical += 6f;                                             //블래스트의 치명타 확률 증가
        bStar_3[blast_Lv_3].sprite = starSprite;
        blast_Lv_3++;
    }
    public void BlastEvent_4()
    {
        blastData.criticalDamage += 0.2f;                                     //블래스트의 치명타 데미지 증가
        bStar_4[blast_Lv_4].sprite = starSprite;
        blast_Lv_4++;
    }

    public void FloorEvent_0()                                                //장판 능력
    {
        floorData.damage += (floorData.baseDamage / 100) * 20;                //장판의 데미지 증가
        fStar_0[floor_Lv_0].sprite = starSprite;                              //장판 능력의 별 이미지 변경
        floor_Lv_0++;                                                         //장판 레벨 증가
    }
    public void FloorEvent_1()
    {
        floorData.range += floorData.baseRange / 12;                          //장판의 범위 증가
        floor.transform.localScale = new Vector3(floorData.range, floorData.range, floorData.range);        //장판의 스케일값 조정
        fStar_1[floor_Lv_1].sprite = starSprite;
        floor_Lv_1++;
    }
    public void FloorEvent_2()
    {
        floorData.slow += 10;                                                 //장판의 이동속도 감소량 증가
        floorW.isSlow = true;                                                 //장판의 이동속도 감소 여부
        floorW.FloorDebuff();                                                 //장판 내에 있는 적에게 이동속도 감소 효과 부여
        fStar_2[floor_Lv_2].sprite = starSprite;
        floor_Lv_2++;
    }
    public void FloorEvent_3()
    {
        floorData.decrease += 5;                                              //장판의 방어력 감소량 증가
        floorW.isDecrease = true;                                             //장판의 방어력 감소 여부
        floorW.FloorDebuff();                                                 //장판 내에 있는 적에게 방어력 감소 효과 부여
        fStar_3[floor_Lv_3].sprite = starSprite;
        floor_Lv_3++;
    }

}
