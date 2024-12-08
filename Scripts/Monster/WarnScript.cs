using UnityEngine;

public class WarnScript : MonoBehaviour
{
    MonManager monManager;

    GameObject[] monPrefabs_1;
    GameObject[] monPrefabs_2;
    GameObject[] monPrefabs_3;
    GameObject[] monPrefabs_4;

    Transform trans;

    //int mCount;
    public float time;
    int count;
    int ran;
    bool type = false;

    float posX = 0;
    float posY = 0;

    public float warnTime;

    private void Awake()
    {
        monManager = GameObject.Find("GameManager").GetComponent<MonManager>();
        
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= warnTime)
        {
            monManager.EnableMons(posX, posY);
            gameObject.SetActive(false);
        }
    }

    public void Set(float pos1, float pos2, float warntime)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f));
        posX = pos1;
        posY = pos2;
        warnTime = warntime;
        time = 0;
        type = true;

        if (monManager.mCount >= monManager.monPool)
            monManager.mCount = 0;
        if (monManager.wCount >= monManager.warnPool)
            monManager.wCount = 0;
    }
}
