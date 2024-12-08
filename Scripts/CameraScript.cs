using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;

    public float speed = 0.1f;

    Vector3 camPos;

    public Vector2 minCamPos;
    public Vector2 maxCamPos;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 pos = Vector3.Lerp(transform.position, player.position, speed);

        transform.position = new Vector3(Mathf.Clamp(pos.x, minCamPos.x, maxCamPos.x) + camPos.x,
                                         Mathf.Clamp(pos.y, minCamPos.y, maxCamPos.y) + camPos.y,
                                         -10f + camPos.y);
    }

    public void SetResolution()
    {
        int setWidth = 1080;
        int setHeight = 1920;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
