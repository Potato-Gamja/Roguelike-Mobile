using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    int activePointerId_ = -1;
    
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (activePointerId_ == -1)
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
            activePointerId_ = -1;
            base.OnPointerUp(eventData);
        }
    }
}
