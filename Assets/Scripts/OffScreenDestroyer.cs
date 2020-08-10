using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trash collector, rise events for off screen objects
public class OffScreenDestroyer : MonoBehaviour
{
    [SerializeField]
    private BoxCollider bounds;
    // Start is called before the first frame update
    void Start()
    {
        bounds = GetComponent<BoxCollider>();
        Debug.Assert(null != bounds, "No collider at OffScreenDestroyer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        OffScreenReceiver ofr = other.gameObject.GetComponent<OffScreenReceiver>();
        if (null != ofr) ofr.OnScreenExit();
    }
}
