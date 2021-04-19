using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingHead : MonoBehaviour
{

    [SerializeField] private Transform Player;
    [SerializeField] private float dashForce;
    private Transform EnemyGFX;
    private Rigidbody2D rb;
    private float angle;
    [SerializeField] private float timeToDash;
    private float currentTimeToDash;
    private bool dashing = false;
    private Vector2 lookDir;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyGFX = GetComponent<EnemyBase>().EnemyGFX;    
        currentTimeToDash = timeToDash;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyBase>().shouldTrack) 
        {
            

            // add dashing
            #region Logic
            if (!dashing) {
                lookDir = Player.position - transform.position;
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            #endregion
            
            if (currentTimeToDash <= 0 && !dashing) 
            {
                dashing = true;
                Invoke("EndDash", 0.5f);
               
            } else if (!dashing) 
            {
                currentTimeToDash -= Time.deltaTime;
            }
            
            #region Visual
            if (!dashing) {
           
                if (((angle > 95 && angle < 175) || (angle < -95 && angle > -175))) // left
                {
                    transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), -1);

                }
                else if (((angle < 85 && angle > 5) || (angle > -85 && angle < -5))) // right
                {
                
                    transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), 1);
                    

                }
            }
            #endregion
        }
    }

    void EndDash() 
    {
        dashing = false;
        rb.velocity = Vector2.zero;
        currentTimeToDash = timeToDash;
    }

    private void FixedUpdate()
    {
        if (dashing) 
        {
            rb.velocity = lookDir.normalized * dashForce;
            //rb.AddForce(lookDir.normalized * dashForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EndDash();
    }
}
