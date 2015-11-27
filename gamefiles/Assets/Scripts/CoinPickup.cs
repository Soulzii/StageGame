using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {

    public float Value;
    public float rX;
    public float rY;
    public float rZ;

    //Script voor het afvangen van collision met een muntje en het laten roteren van het muntje
    void OnTriggerEnter(Collider collider)
    {

        if(collider.tag =="Player")
        {
            GameHandler.Instance.PickUp(this.Value);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(rX, rY, rZ);
    }

}
