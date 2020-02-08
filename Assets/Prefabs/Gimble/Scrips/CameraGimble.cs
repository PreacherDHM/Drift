using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGimble : MonoBehaviour
{

    public Transform Player;
    public float trackingSpeed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        //Player = GameObject.Find("Player-1").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, Player.position, trackingSpeed * Vector2.Distance(transform.position,Player.position));
    }
}
