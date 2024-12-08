using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick        //Joystick Pack 에셋의 조이스틱 베이스
{
    int activePointerId_ = -1;                  //중복터치를 방지하기 위한 포인터 아이디
    
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (activePointerId_ == -1)             //중복터치를 방지하기 위한 조건문
        {
            activePointerId_ = eventData.pointerId;
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == activePointerId_)
        {
            background.gameObject.SetActive(false);
            activePointerId_ = -1;                //터치가 가능하게 포인터 아이디 값을 변경
            base.OnPointerUp(eventData);
        }
    }
}
