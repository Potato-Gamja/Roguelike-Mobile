using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    public GameObject missile;                //미사일 게임오브젝트
    public GameObject[] missilePrefab;        //미사일 풀링 배열
    public GameObject missileGroup;           //미사일 부모 오브젝트

    public int weaponPool;                    //미사일 풀링 수

    void Awake()
    {
        missilePrefab = new GameObject[weaponPool];

        for (int i = 0; i < missilePrefab.Length; i++)
        {
            GameObject gameObject = Instantiate(missile);            //미사일 오브젝트 생성
            gameObject.transform.parent = missileGroup.transform;    //미사일 오브젝트의 부모 변경
            missilePrefab[i] = gameObject;                           //미사일 풀링 배열에 게임오브젝트 대입
            gameObject.SetActive(false);                             //미사일 비활성화
        }

    }

}
