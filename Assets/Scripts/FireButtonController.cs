using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pressed;

    public bool Pressed
    {
        get
        {
            return pressed;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        pressed = Input.GetButton("Fire1");
#endif
    }
}
