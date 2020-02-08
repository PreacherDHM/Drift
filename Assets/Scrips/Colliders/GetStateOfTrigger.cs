using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStateOfTrigger : MonoBehaviour
{
    public bool stateOftrigger;
    public int collisionLayer = 1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == collisionLayer)
        {
            stateOftrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == collisionLayer)
        {
            stateOftrigger = false;
        }
    }
}
