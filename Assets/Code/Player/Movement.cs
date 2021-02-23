using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour, IDamageable
{


    // movement
    private Rigidbody2D rb;
    private Vector2 movementVector;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator anim;
    private bool isMoving = false;
    private bool stopMoving = false;

    // Dashing
    private bool dashing = false;
    //[SerializeField] private float dashForce = 10;
    private Vector2 mouseLoc = new Vector2();
    //[SerializeField] private float dashMultiplier = 2;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float timeBtwDash = 0.5f;
    private bool movementOverAngle = true;
    private float currentTimeBtwDash;

    // Death/Health 
    [SerializeField] private Vector2 CheckSize;
    private bool touchingCollisionEdge = false;
    private bool touchingWalk = true;
    [SerializeField] private Transform feetpos;
    [SerializeField] private LayerMask walk;
    [SerializeField] private LayerMask edge;
    [SerializeField] private int maxHealth;
    [HideInInspector] public int health;
    [SerializeField] public bool dead = false;

    // Visuals
    [SerializeField] private GameObject shadows;

    // Misc
    [SerializeField] private Spellbook book;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTimeBtwDash = timeBtwDash;
    }

    private void Update()
    {
        if (movementOverAngle)
        {
            anim.SetInteger("DirectionX", (int)movementVector.x);
            anim.SetInteger("DirectionY", (int)movementVector.y);
            if (movementVector.x < 0) 
            {
                book.playerSpr.flipX = true;
            } else if (movementVector.x > 0) 
            {
                book.playerSpr.flipX = false;
            }

        } 


        
        if (Input.GetButtonDown("Fire1") && book.gameObject.activeSelf == true)
        {
            movementOverAngle = false;
            book.AngleCheck(right, left, top, bottom);
            Invoke("SetMovementOverAngle", 0.5f);
        }



        if (touchingCollisionEdge && !touchingWalk && !dashing) 
        {
            stopMoving = true;
            shadows.SetActive(false);
            Invoke("Die", 0.5f);
        }

        touchingCollisionEdge = Physics2D.OverlapBox(feetpos.position, CheckSize, 0, edge);
        touchingWalk = Physics2D.OverlapBox(feetpos.position, CheckSize, 0, walk);

        isMoving = movementVector != Vector2.zero;
        

        anim.SetBool("Running", isMoving);
        

        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (dashing)
            book.enabled = false;



        if (currentTimeBtwDash <= 0)
        {
            if (!dashing && Input.GetKeyDown(KeyCode.Space))
            {
                //dashing = true;
                Invoke("EndDash", dashTime);
                mouseLoc = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2)).normalized;
                currentTimeBtwDash = timeBtwDash;
            }
        } else 
        {
            currentTimeBtwDash -= Time.deltaTime;
        }
        

        

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
       
    }

    void FixedUpdate()
    {
        // TODO: Dashing, 
        if (!dashing && !stopMoving)
        {
            shadows.SetActive(true);
            anim.enabled = true;
            rb.MovePosition(rb.position + ((movementVector * speed) * Time.deltaTime));
            book.enabled = true;
        }
        /*
        if (dashing)
        {
            //shadows.SetActive(false);
            anim.SetBool("Running", false);
            anim.enabled = false;
            float step = (dashMultiplier * Time.deltaTime);
            gameObject.layer = 13;
            transform.position = Vector3.MoveTowards(transform.position, mouseLoc * dashForce, step);
        }
        */
    }

    void Die() 
    {
       
        dead = true;
        rb.gravityScale = 125;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Camera.main.GetComponent<FollowPlayerScript>().enabled = false;
        GetComponent<SortingGroup>().sortingOrder = -6;
        Invoke("ReloadScene", 0.4f);
    }

    void ReloadScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetMovementOverAngle() 
    {
        movementOverAngle = true;
    }

    void EndDash() 
    {
        dashing = false;
        gameObject.layer = 9;
    }

    void top() 
    {
        book.bookSpr.sortingOrder = 0;
        anim.SetInteger("DirectionY", 1);
        anim.SetInteger("DirectionX", 0);
    }
    void bottom() {
       
        book.bookSpr.sortingOrder = 2;
        anim.SetInteger("DirectionY", -1);
        anim.SetInteger("DirectionX", 0);
    }
    void right() {
        book.bookSpr.sortingOrder = 2;
        anim.SetInteger("DirectionX", 1);
        anim.SetInteger("DirectionY", 0);
        book.playerSpr.flipX = false;
    }
    void left() {
        book.bookSpr.sortingOrder = 2;
        anim.SetInteger("DirectionX", -1);
        anim.SetInteger("DirectionY", 0);
        book.playerSpr.flipX = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetpos.position, CheckSize);

    }

    public void Damage(int amount) 
    {
        health -= amount;
    }

}