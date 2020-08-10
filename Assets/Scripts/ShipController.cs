using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main player controller
public class ShipController : MonoBehaviour
{
    private Rigidbody shipRB;

    [SerializeField]
    private float shipMaxSpeed;

    //Screen bounds for ship
    [SerializeField]
    private Bounds screenBounds;

    //Virtual joystick controller
    [SerializeField]
    private JoystickController joystickController;

    //Ship animator
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //load settings
        shipMaxSpeed = GameController.Instance.CurrentGameSettings.shipMaxSpeed;

        //listen events
        GameController.Instance.OnGameStateChange.AddListener(OnGameStateChange);

        //set rigidbody ref
        shipRB = GetComponent<Rigidbody>();
        Debug.Assert(null != shipRB, "No ship rigidbody");

        //calculate bounds
        SetScreenBounds();

        joystickController = FindObjectOfType<JoystickController>();

        OnGameStateChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.GameState == GameController.GameStates.Play) UpdateControls();
    }

    private void UpdateControls()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        Vector3 mv = new Vector3(Input.GetAxis("Horizontal")*shipMaxSpeed, 0f, Input.GetAxis("Vertical")*shipMaxSpeed); //keyboard for editoer and standalone
#endif        
        if (!joystickController.idle)
        {
            mv = new Vector3(0f, 0f, 1f);
            mv = Quaternion.Euler(0f, -joystickController.angle, 0f) * mv;
            mv = joystickController.speed * shipMaxSpeed * mv;
        }

        mv = Vector3.ClampMagnitude(mv, shipMaxSpeed);
        Vector3 newpos = transform.position + mv * Time.deltaTime;
        newpos.x = Mathf.Clamp(newpos.x, screenBounds.min.x, screenBounds.max.x);
        newpos.z = Mathf.Clamp(newpos.z, screenBounds.min.z, screenBounds.max.z);

        //animation params
        animator.SetFloat("CurrentStrafe", (transform.position-newpos).normalized.x);
        

        shipRB.MovePosition(newpos);

    }

    //Main game state listener
    public void OnGameStateChange()
    {
        if(GameController.Instance.GameState == GameController.GameStates.Play)
        {
            SetChildsActive(true);
        }
        else
        {
            SetChildsActive(false);
        }
    }

    //For activate/desactivate all child objects
    private void SetChildsActive(bool active)
    {
        Transform trans;
        int c = transform.childCount;
        for(int i = 0; i<c; i++)
        {
            trans = transform.GetChild(i);
            trans.gameObject.SetActive(active);
        }
    }

    //calculate screen bounds
    private void SetScreenBounds()
    {
        Vector3 a = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 b = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        Bounds cbb = cc.bounds;      
        screenBounds = new Bounds();
        screenBounds.SetMinMax(a, b);
        screenBounds.size -= cbb.size;
    }
}
