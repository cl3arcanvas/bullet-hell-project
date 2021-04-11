using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{

    // movement
    private Rigidbody2D rb;
    private Vector2 movementVector;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator anim;
    private bool isMoving = false;
    public bool stopMoving = false;

    // Dashing
    private bool movementOverAngle = true;

    // Death/Health 
    [SerializeField] private Vector2 CheckSize;
    private bool touchingCollisionEdge = false;
    private bool touchingWalk = true;
    [SerializeField] private Transform feetpos;
    [SerializeField] private LayerMask walk;
    [SerializeField] private LayerMask edge;
    [SerializeField] private int maxHealth;
    [HideInInspector] public int health;
    [HideInInspector] public bool dead = false;
    private bool falling;
    [SerializeField] private float invinceTime;
    private float currentInvincTime;
    [SerializeField] private Image deathScreen;

    // Visuals
    [SerializeField] private GameObject shadows;

    // Misc
    [SerializeField] private Spellbook book;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fillHearts;
    [SerializeField] private Sprite notFilledHearts;
    [SerializeField] private Image[] manaContainer;
    [SerializeField] private Sprite fillMana;
    [SerializeField] private Sprite notFilledMana;
    [SerializeField] private AudioSource hitSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        currentInvincTime = 0;
    }

    private void Update()
    {

        touchingCollisionEdge = Physics2D.OverlapBox(feetpos.position, CheckSize, 0, edge);
        touchingWalk = Physics2D.OverlapBox(feetpos.position, CheckSize, 0, walk);
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = movementVector != Vector2.zero;
        anim.SetBool("Running", isMoving);

        #region Controlling
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
        #endregion

        #region Death and Health
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fillHearts;
            }
            else
            {
                hearts[i].sprite = notFilledHearts;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        for (int i = 0; i < manaContainer.Length; i++)
        {
            if (i < book.mana)
            {
                manaContainer[i].sprite = fillMana;
            }
            else
            {
                manaContainer[i].sprite = notFilledMana;
            }

            if (i < book.maxMana)
            {
                manaContainer[i].enabled = true;
            }
            else
            {
                manaContainer[i].enabled = false;
            }
        }

        if (currentInvincTime > 0) 
        {
            currentInvincTime -= Time.deltaTime;
        }

        if (touchingCollisionEdge && !touchingWalk) 
        {
            stopMoving = true;
            shadows.SetActive(false);
            falling = true;
            Invoke("Die", 0.5f);
        }

        if (health <= 0)
        {
            falling = false;

            Die();
        }
#endregion
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
       
    }

    void FixedUpdate()
    {
        // TODO: Dashing, 
        if (!stopMoving)
        {
            shadows.SetActive(true);
            anim.enabled = true;
            rb.MovePosition(rb.position + ((movementVector * speed) * Time.deltaTime));
            book.enabled = true;
        }
    }

    void Die()
    {
        deathScreen.gameObject.SetActive(true);
        dead = true;
        stopMoving = true;
        GetComponent<SortingGroup>().sortingOrder = -4;
        anim.enabled = false;
        if (falling)
        {
            rb.gravityScale = 125;
            Invoke("ReloadScene", 0.4f);
            GetComponent<SortingGroup>().sortingOrder = -6;
        }
        else
        {
            Invoke("ReloadScene", 1f);
        }

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Camera.main.GetComponent<FollowPlayerScript>().enabled = false;

    }

    #region Utils
    void ReloadScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetMovementOverAngle() 
    {
        movementOverAngle = true;
    }
    #endregion
   
    #region Extras
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetpos.position, CheckSize);

    }
    #endregion

    public void Damage(int amount)
    {

        if (currentInvincTime <= 0)
        {
            hitSound.Play();
            health -= amount;
            currentInvincTime = invinceTime;
        }
    }

     #region Directions
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
    #endregion

}