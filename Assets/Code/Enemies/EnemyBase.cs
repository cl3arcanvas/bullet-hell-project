using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBase : MonoBehaviour, IDamageable
{

    [SerializeField] private bool idle = false;
    [SerializeField] private float speed = 200;
    [SerializeField] public int maxHealth;
    [SerializeField] private AudioSource hit;
    [SerializeField] public LayerMask player;
    [SerializeField] private Transform target;
    [SerializeField] private float nextWayPointDistance = 3;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    [SerializeField] private Transform EnemyGFX;
    [SerializeField] public float checkArea;
    public bool nearPlayer;
    public bool shouldTrack;
    [HideInInspector] public int health;
    [SerializeField] private int damage;
    [SerializeField] private bool ControlledByScript = true;

    // Start is called before the first frame update
    void Start()
    {

        gameObject.GetComponent<AIPath>().canMove = false;

        GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;

        if (idle)
        {
            speed = 0;
        }

        target = GameObject.FindGameObjectWithTag("Player").transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        

        health = maxHealth; 

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
        if (ControlledByScript)
        {
            if (path == null)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;

            }
            else
            {
                reachedEndOfPath = false;
            }

            if (reachedEndOfPath)
            {

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

            if (force.x >= 0.01f)
            {
                EnemyGFX.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (force.x <= -0.01f)
            {
                EnemyGFX.GetComponent<SpriteRenderer>().flipX = true;
            }


        } else if (!ControlledByScript && shouldTrack && !idle) 
        {
            gameObject.GetComponent<AIPath>().canMove = true;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(damage);
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
