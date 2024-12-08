using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

using Random = UnityEngine.Random;

public class LevelUpScript : MonoBehaviour
{
    public GameManager gameManager;
    public Joystick joystick;
    [SerializeField]
    Button pasue;
    public PlayerScript playerScript;
    public PlayerData playerData;
    public GameObject[] sword;
    public GameObject fade;

    [Header("Stat")]
    public GameObject statPanel;
    public GameObject[] tempStats;
    public GameObject[] stats;

    public GameObject defaultStat;
    public GameObject buffStat;
    public GameObject debuffStat;
    public GameObject[] defaultStats;
    public GameObject[] buffStats;
    public GameObject[] debuffStats;

    [Header("Weapon")]
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


    [Header("Data")]

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
    public Sprite starSprite;

    GameObject mStarPanel_0;
    GameObject mStarPanel_1;
    GameObject mStarPanel_2;
    GameObject mStarPanel_3;
    GameObject mStarPanel_4;

    Image[] mStar_0 = new Image[5];
    Image[] mStar_1 = new Image[5];
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

    public int swordCount = 0;
    public int blastCount = 0;

    public bool isSword = false;
    public bool isLaser = false;
    public bool isBlast = false;
    public bool isFloor = false;

    int missile_Lv_0 = 0;
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

    GameObject tempS_0;
    GameObject tempS_1;
    GameObject tempS_2;
    GameObject tempS_3;
    GameObject tempS_4;
    GameObject tempS_5;

    GameObject tempW_0;
    GameObject tempW_1;
    GameObject tempW_2;

    [SerializeField]
    GameObject weaponGroup;
    [SerializeField]
    Button reroll;
    [SerializeField]
    Image rerollImage;

    public MonManager monManager;
    public float monHp;
    public float monOffense;
    public float monDefense;
    public float monSpeed;
    public float monKnockbackDefense;

    float time = 0;

    private void Awake()
    {
        weaponGroup.SetActive(false);

        ResetStat();
        SetStatWeapon();
        SetStar();
    }

    void ResetStat()
    {
        missileData.speed = missileData.baseSpeed;
        missileData.damage = missileData.baseDamage;
        missileData.attackDelay = missileData.baseAttackDelay;
        missileData.critical = missileData.baseCritical;
        missileData.count = missileData.baseCount;

        swordData.speed = swordData.baseSpeed;
        swordData.damage = swordData.baseDamage;
        swordData.attackDelay = swordData.baseAttackDelay;
        swordData.isKnock = swordData.baseIsKnock;
        swordData.knockback = swordData.baseKnockback;
        swordData.duration = swordData.baseDuration;

        laserData.speed = laserData.baseSpeed;
        laserData.damage = laserData.baseDamage;
        laserData.attackDelay = laserData.baseAttackDelay;
        laserData.baseDuration = laserData.duration;
        laserData.count = laserData.baseCount;

        blastData.range = blastData.baseRange;
        blastData.speed = blastData.baseSpeed;
        blastData.damage = blastData.baseDamage;
        blastData.attackDelay = blastData.baseAttackDelay;
        blastData.critical = blastData.baseCritical;
        blastData.criticalDamage = blastData.baseCriticalDamage;
        blastData.count = blastData.baseCount;

        floorData.range = floorData.baseRange;
        floorData.damage = floorData.baseDamage;
        floorData.attackDelay = floorData.baseAttackDelay;
        floorData.slow = floorData.baseSlow;
        floorData.decrease = floorData.baseDecrease;

        playerData.maxHp = playerData.baseHp;
        playerData.hp = playerData.baseHp;
        playerData.defense = playerData.baseDefense;
        playerData.offense = playerData.baseOffense;
        playerData.speed = playerData.baseSpeed;
        playerData.attackDelay = playerData.baseAttackDelay;
        playerData.isTrueDamage = playerData.baseIsTrueDamage;
        playerData.critical = playerData.baseCritical;
    }

    void SetStatWeapon()
    {

        for (int i = 0; i < defaultStat.transform.childCount; i++)
        {
            defaultStats[i] = defaultStat.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < buffStat.transform.childCount; i++)
        {
            buffStats[i] = buffStat.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < debuffStat.transform.childCount; i++)
        {
            debuffStats[i] = debuffStat.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < missileWeapon.transform.childCount; i++)
        {
            missileWeapons[i] = missileWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < swordWeapon.transform.childCount; i++)
        {
            swordWeapons[i] = swordWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < laserWeapon.transform.childCount; i++)
        {
            laserWeapons[i] = laserWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < blastWeapon.transform.childCount; i++)
        {
            blastWeapons[i] = blastWeapon.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < floorWeapon.transform.childCount; i++)
        {
            floorWeapons[i] = floorWeapon.transform.GetChild(i).gameObject;
        }


    }
    void SetStar()
    {
        //Image star = starPanel.transform.GetChild(missile_Lv_0).GetComponent<Image>();

        mStarPanel_0 = missileWeapons[0].transform.GetChild(4).gameObject;
        mStarPanel_1 = missileWeapons[1].transform.GetChild(4).gameObject;
        mStarPanel_2 = missileWeapons[2].transform.GetChild(4).gameObject;
        mStarPanel_3 = missileWeapons[3].transform.GetChild(4).gameObject;
        mStarPanel_4 = missileWeapons[4].transform.GetChild(4).gameObject;

        for (int i = 0; i < 5; i++)
        {
            mStar_0[i] = mStarPanel_0.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
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
    public void StatsArr()
    {
        ii = 0;
        for (int j = 0; j < defaultStats.Length; j++)
        {
            tempStats[j] = defaultStats[j];
            ii++;
        }
        for (int j = 0; j < buffStats.Length; j++)
        {
            tempStats[j + defaultStats.Length] = buffStats[j];
            ii++;
        }
        stats = new GameObject[4];

        n = Random.Range(0, defaultStats.Length);
        stats[0] = tempStats[n];
        tempS_0 = stats[0].transform.parent.gameObject;
        stats[0].transform.SetParent(statPanel.transform);
        stats[0].transform.localPosition = new Vector3(-680, 0, 0);

        n = Random.Range(0, defaultStats.Length);
        stats[1] = tempStats[n];
        while (stats[0] == stats[1])
        {
            n = Random.Range(0, defaultStats.Length);
            stats[1] = tempStats[n];
        }
        tempS_1 = stats[1].transform.parent.gameObject;
        stats[1].transform.SetParent(statPanel.transform);
        stats[1].transform.localPosition = new Vector3(0, 0, 0);

        n = Random.Range(defaultStats.Length, defaultStats.Length + buffStats.Length);
        stats[2] = tempStats[n];
        while (stats[0] == stats[2] || stats[1] == stats[2])
        {
            n = Random.Range(defaultStats.Length, defaultStats.Length + buffStats.Length);
            stats[2] = tempStats[n];
        }
        tempS_2 = stats[2].transform.parent.gameObject;
        stats[2].transform.SetParent(statPanel.transform);
        stats[2].transform.localPosition = new Vector3(680, 0, 0);
    }

    public void WeaponsArr()
    {
        i = 0;

        if (missile_Lv_0 < 5)
        {
            tempWeapons[i] = missileWeapons[0];
            i++;
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

        if (isSword)
        {
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

        weapons = new GameObject[3];

        n = Random.Range(0, i);
        weapons[0] = tempWeapons[n];
        tempW_0 = weapons[0].transform.parent.gameObject;
        weapons[0].transform.SetParent(weaponPanel.transform);
        weapons[0].transform.localPosition = new Vector3(-720, 0, 0);
        weapons[0].SetActive(true);

        n = Random.Range(0, i);
        weapons[1] = tempWeapons[n];
        while (weapons[0] == weapons[1])
        {
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

    public void RelocationWeapons()
    {
        weapons[0].transform.SetParent(tempW_0.transform);
        weapons[1].transform.SetParent(tempW_1.transform);
        weapons[2].transform.SetParent(tempW_2.transform);
    }

    public void RelocationStats()
    {
        stats[0].transform.SetParent(tempS_0.transform);
        stats[1].transform.SetParent(tempS_1.transform);
        stats[2].transform.SetParent(tempS_2.transform);
        //stats[3].transform.SetParent(tempS_3.transform);
        //stats[4].transform.SetParent(tempS_4.transform);
    }

    public void ReArr_Weapons()
    {
        weapons[0].transform.SetParent(tempW_0.transform);
        weapons[1].transform.SetParent(tempW_1.transform);
        weapons[2].transform.SetParent(tempW_2.transform);

        WeaponsArr();
    }

    public void ReArr_Weapons_Reroll()
    {
        weapons[0].transform.SetParent(tempW_0.transform);
        weapons[1].transform.SetParent(tempW_1.transform);
        weapons[2].transform.SetParent(tempW_2.transform);

        reroll.interactable = false;
        rerollImage.color = new Color32(52, 255, 0, 60);
        WeaponsArr();
    }

    public void ReArr_Stats()
    {
        stats[0].transform.SetParent(tempS_0.transform);
        stats[1].transform.SetParent(tempS_1.transform);
        stats[2].transform.SetParent(tempS_2.transform);
        //stats[3].transform.SetParent(tempS_3.transform);
        //stats[4].transform.SetParent(tempS_4.transform);

        StatsArr();
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

    public void StatSelectEvent_0()
    {
        isStat = true;
        statPanel.SetActive(true);
        Time.timeScale = 0.0f;
        gameManager.joystick.SetActive(false);
        StatsArr();
    }

    public void StatSelectEvent_1()
    {
        if (!isWeapon)
        {
            gameManager.joystick.SetActive(true);
            TimeSlowUp();
        }
        joystick.handle.anchoredPosition = Vector2.zero;
        joystick.input = Vector2.zero;
        statPanel.SetActive(false);
        isStat = false;
    }

    public void StatSelectEvent_1_1()
    {
        isStat = false;
        if (isWeapon)
        {
            WeaponSelectEvent_0();
        }
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

    //Missile
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

    //Sword
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

    //Laser
    public void LaserEvent_0()
    {
        laserData.count -= 2;
        laserData.damage += (laserData.baseDamage / 100) * 60;
        laserData.attackDelay -= (laserData.baseAttackDelay / 100) * 6;
        lStar_0[laser_Lv_0].sprite = starSprite;
        laser_Lv_0++;
    }
    public void LaserEvent_1()
    {
        laserData.count += 6;
        laserData.damage += (laserData.baseDamage / 100) * 20;
        laserData.attackDelay += (laserData.baseAttackDelay / 100) * 4;
        lStar_1[laser_Lv_1].sprite = starSprite;
        laser_Lv_1++;
    }
    public void LaserEvent_2()
    {
        laserData.count -= 1;
        laserData.attackDelay -= (laserData.baseAttackDelay / 100) * 8;
        lStar_2[laser_Lv_2].sprite = starSprite;
        laser_Lv_2++;
    }
    public void LaserEvent_3()
    {
        laserData.count += 3;
        laserData.attackDelay += (laserData.baseAttackDelay / 100) * 2;
        lStar_3[laser_Lv_3].sprite = starSprite;
        laser_Lv_3++;
    }

    //Blast
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