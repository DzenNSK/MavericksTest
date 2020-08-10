using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public float angle { get; private set; } //angle of stick direction
    public float speed { get; private set; } //stick shift
    public bool idle { get; private set; } // idle state (zero shift)

    [SerializeField]
    private float maxOffset;
    private Image jsContainer;
    private Image joystick;

    void Start()
    {
        //Images for pad and stick
        jsContainer = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();

        idle = true;
        GameController.Instance.OnGameStateChange.AddListener(OnGameStateChange);
    }

    //Joystick drag resolver
    public void OnDrag(PointerEventData ped)
    {
        Vector2 pos = Vector2.zero;
        idle = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (jsContainer.rectTransform,
                ped.position,
                null,
                out pos);

        pos.x = (pos.x / (jsContainer.rectTransform.rect.width / 2));
        pos.y = (pos.y / (jsContainer.rectTransform.rect.height / 2));

        pos = Vector2.ClampMagnitude(pos, maxOffset);
        speed = pos.magnitude / maxOffset;
        angle = Vector2.SignedAngle(Vector2.up, pos);

        joystick.rectTransform.anchoredPosition = new Vector3(pos.x * (jsContainer.rectTransform.rect.width / 2), pos.y * (jsContainer.rectTransform.rect.height / 2));

    }

    public void OnPointerDown(PointerEventData ped)
    {

        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        angle = 0;
        speed = 0;
        idle = true;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
    }

    //main game state listener
    private void OnGameStateChange()
    {
        if (GameController.Instance.GameState == GameController.GameStates.Play)
        {
            joystick.rectTransform.anchoredPosition = Vector3.zero;
            angle = 0;
            speed = 0;
            idle = true;
        }
    }
}