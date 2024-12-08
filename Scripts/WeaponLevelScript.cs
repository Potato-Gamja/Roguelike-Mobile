using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLevelScript : MonoBehaviour
{
    public LevelUpScript levelUpScript;

    GameObject starPanel;
    Image[] star;

    private void Awake()
    {
        for (int i = 0; i < starPanel.transform.childCount; i++)
        {
            star[i] = starPanel.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
    }
}
