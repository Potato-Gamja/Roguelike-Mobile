using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    public GameObject missile;
    public GameObject[] missilePrefab;
    public GameObject missileGroup;

    public int weaponPool;

    void Awake()
    {
        missile = gameManager.weapon[gameManager.weaponType];

        missilePrefab = new GameObject[weaponPool];

        for (int i = 0; i < missilePrefab.Length; i++)
        {
            GameObject gameObject = Instantiate(missile);
            gameObject.transform.parent = missileGroup.transform;
            missilePrefab[i] = gameObject;
            gameObject.SetActive(false);
        }

    }

}
