using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHead : MonoBehaviour
{

    [SerializeField] private Transform Player;
    [SerializeField] private float dashSpeed;
    private Transform EnemyGFX;
    private Rigidbody2D rb;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyGFX = GetComponent<EnemyBase>().EnemyGFX;    
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyBase>().shouldTrack) 
        {
            // add dashing
            #region Logic
            Vector2 lookDir = Player.position - transform.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            #endregion

            #region Visual
            if (((angle > 95 && angle < 175) || (angle < -95 && angle > -175))) // left
            {
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), -1);

            }
            else if (((angle < 85 && angle > 5) || (angle > -85 && angle < -5))) // right
            {
            
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), 1);
                

            }
            #endregion
        }
    }
}
