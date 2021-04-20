using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBase : MonoBehaviour, IDamageable
{

    // FX
    [SerializeField] private AudioSource hit;
    [SerializeField] public Transform EnemyGFX;
    [SerializeField] public bool handlesRotation = false;

    // health and damage
    [SerializeField] public int maxHealth;
    [HideInInspector] public int health;
    [SerializeField] private int damage;

    // movement and pathfinding
    [SerializeField] public bool idle = false;
    private Rigidbody2D rb;

    // misc
    [SerializeField] public LayerMask trigger;
    private Transform target;
    [SerializeField] public float checkArea;
    [HideInInspector] public bool nearPlayer;
    [HideInInspector] public bool shouldTrack;
    [HideInInspector] public bool invincible = false;

    // Start is called before the first frame update
    void Start()
    {

        gameObject.GetComponent<AIPath>().canMove = false;

        GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
        
        target = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        
        health = maxHealth; 
        
    }

    private void Update()
    {
        

        #region Logic
        nearPlayer = Physics2D.OverlapCircle(transform.position, checkArea, trigger);

        if ((nearPlayer == true || health != maxHealth)) 
        {
            shouldTrack = true;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        #endregion

        #region Visuals
        if (target.position.x > transform.position.x && !handlesRotation)
        {
            EnemyGFX.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (target.position.x < transform.position.x && !handlesRotation)
        {
            EnemyGFX.GetComponent<SpriteRenderer>().flipX = false;
        }
        #endregion

       
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (shouldTrack && !idle)
            gameObject.GetComponent<AIPath>().canMove = true;
        else
            gameObject.GetComponent<AIPath>().canMove = false;      
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
        if (!invincible)
        {
            hit.Play();
            health -= amount;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkArea);
    }

}
