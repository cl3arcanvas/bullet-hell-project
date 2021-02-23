using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBase : MonoBehaviour, IDamageable
{

    [SerializeField] private bool idle = false;
    [SerializeField] private float speed = 200;
    [SerializeField] private int health;
    [SerializeField] private AudioSource hit;

    [SerializeField] private Transform target;
    [SerializeField] private float nextWayPointDistance = 3;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    [SerializeField] private Transform EnemyGFX;
    [SerializeField] private float checkArea;
    private bool nearPlayer;
    private bool shouldTrack;
    [SerializeField] private LayerMask player;
    private int maxHealth;

    // Start is called before the first frame update
    void Start()
    {

        if (idle)
        {
            speed = 0;
        }


        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        maxHealth = health; 

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    private void UpdatePath() 
    {


        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    private void Update()
    {

        nearPlayer = Physics2D.OverlapCircle(transform.position, checkArea, player);



        if (nearPlayer == true || health != maxHealth) 
        {
            shouldTrack = true;
                
        }




        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

     

        if (path == null ) 
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) 
        {
            reachedEndOfPath = true;
            return;

        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (shouldTrack)
            rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWayPointDistance) 
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f) 
        {
            EnemyGFX.GetComponent<SpriteRenderer>().flipX = false;
        } else if (rb.velocity.x <= -0.01f) 
        {
            EnemyGFX.GetComponent<SpriteRenderer>().flipX = true;        
        }

    }

    public void Damage(int amount) 
    {
        hit.Play();
        health -= amount;
    }

    private void OnPathComplete(Path p) 
    {
        if (!p.error) 
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkArea);
    }

}
