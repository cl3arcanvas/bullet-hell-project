using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHead : MonoBehaviour
{

    [SerializeField] private Transform Player;
    [SerializeField] private float dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyBase>().shouldTrack) 
        {
            // add dashing
        }
    }
}
