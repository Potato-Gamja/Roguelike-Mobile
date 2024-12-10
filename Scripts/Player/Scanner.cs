using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;            //스캔 범위
    public LayerMask targetLayer;      //타겟의 레이어
    public RaycastHit2D[] targets;     //오브젝트의 정보를 가져오기 위한 레이캐스트
    public Transform nearestTarget;    //타겟의 트랜스폼

    public Weapon laserEnd;            //레이저 무기의 스크립트
    public GameObject targetObj;       //타겟의 게임오브젝트, 레이저 무기 타겟에 사용

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);     //원형모양의 스캔 범위 내에 있는 타겟 레이어를 스캔
        GetNearest();
    }

    void GetNearest()
    {
        Transform result = null;                       
        GameObject obj = null;                        
        float diff = 100;                           

        foreach (RaycastHit2D target in targets)                       //레이캐스트 타켓 배열에 접근
        {
            Vector3 myPos = transform.position;                        //스캐너의 위치를 대입
            Vector3 targetPos = target.transform.position;             //타겟의 위치를 대입
            float curDiff = Vector3.Distance(myPos, targetPos);        //타겟과의 거리 계산

            if (curDiff < diff)                                        //타겟과의 거리가 100보다 작을 경우
            {
                diff = curDiff;                                        //타겟과의 거리 계산 값 대입
                result = target.transform;                             //타겟의 트랜스폼 대입
                obj = result.gameObject;                               //트랜스폼의 게임오브젝트를 대입
            }
        }

        nearestTarget = result;                                        //타겟의 트랜스폼 대입
        targetObj = obj;                                               //타겟의 게임오브젝트 저장, 레이저 무기 타겟에 사용되는 게임오브젝트
    }
}
