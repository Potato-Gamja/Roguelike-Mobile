using UnityEngine;

public class WarnScript : MonoBehaviour
{
    MonManager monManager;

    public float time;        //타이머
    public float warnTime;    //경고 시간
    
    float posX = 0;           //좌표 X
    float posY = 0;           //좌표 Y

    private void Awake()
    {
        monManager = GameObject.Find("GameManager").GetComponent<MonManager>();
        
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= warnTime)                                                          //경고 시간에 달성 시 실행
        {
            monManager.EnableMons(posX, posY);                                         //몬스터 생성 함수 실행
            gameObject.SetActive(false);                                               //경고 오브젝트 비활성화
        }
    }

    public void Set(float pos1, float pos2, float warntime)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-20f, 20f));        //경고 오브젝트의 회전값을 랜덤으로 하여 동일한 모양의 경고만 나오는 것을 방지
        posX = pos1;                                                                   //좌표 X값
        posY = pos2;                                                                   //좌표 Y값
        warnTime = warntime;                                                           //경고 시간
        time = 0;                                                                      //타이머 0으로 초기화

        if (monManager.mCount >= monManager.monPool)                                   //경고와 몬스터의 인덱스 값이 초과 시 0으로 초기화
            monManager.mCount = 0;
        if (monManager.wCount >= monManager.warnPool)
            monManager.wCount = 0;
    }
}
