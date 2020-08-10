using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public GameUtils.UnityEventObj onShipHit;
    public GameUtils.UnityEventObj onBulletHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Rise event if collide with ship
    private void OnTriggerEnter(Collider other)
    {
        ShipController sc = other.gameObject.GetComponent<ShipController>();
        if(null == sc)
        {
            sc = other.gameObject.GetComponentInChildren<ShipController>();
        }
        if (null != sc) onShipHit.Invoke(gameObject);

        BulletController bc = other.gameObject.GetComponent<BulletController>();
        if (null != bc) onBulletHit.Invoke(gameObject);
    }
}
