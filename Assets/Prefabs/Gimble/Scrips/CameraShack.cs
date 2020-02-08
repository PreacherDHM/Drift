using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShack : MonoBehaviour
{
    // Start is called before the first frame update
    Transform gimbleHead;

    public bool Shack = false;

    public float shackAmount = 1;
    public float shackDurashion = 1;
    public float decreaseFactor = 1;

    private void Awake()
    {
        gimbleHead = GameObject.Find("Gimble-Head").transform;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shackDurashion > 0 && Shack)
        {
            transform.position = gimbleHead.position + Random.insideUnitSphere * shackAmount;

            shackDurashion -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shackDurashion = 0;
            transform.position = gimbleHead.position;
        }
        
    }
}
