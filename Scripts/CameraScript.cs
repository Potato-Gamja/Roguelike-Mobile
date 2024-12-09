using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;                 //플레이어의 트랜스폼

    public float speed = 0.1f;               //카메라의 이동속도

    Vector3 camPos;                          //카메라의 위치

    public Vector2 minCamPos;                //카메라의 최소 이동 범위
    public Vector2 maxCamPos;                //카메라의 최대 이동 범위

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 pos = Vector3.Lerp(transform.position, player.position, speed);                         //부드러운 이동을 위한 선형보간

        transform.position = new Vector3(Mathf.Clamp(pos.x, minCamPos.x, maxCamPos.x) + camPos.x,       //클램프를 이용하여 최소, 최대 범위를 설정하여 값이 넘지 않도록 하기
                                         Mathf.Clamp(pos.y, minCamPos.y, maxCamPos.y) + camPos.y,
                                         -10f + camPos.y);
    }

}
