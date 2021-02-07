using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 movementVector;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Animator anim;
    private bool isMoving = false;
    [SerializeField] private Spellbook book;
    private bool dashing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        isMoving = movementVector != Vector2.zero;


        anim.SetBool("Running", isMoving);
        

        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (dashing)
            book.enabled = false;


        book.AngleCheck(right, left, top, bottom);

        // Update is called once per frame
       
    }

    void FixedUpdate()
    {
        // TODO: dodging, 
        if (!dashing)
            rb.MovePosition(rb.position + ((movementVector * speed) * Time.deltaTime));
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
    }
    void left() {
        book.bookSpr.sortingOrder = 2;
        anim.SetInteger("DirectionX", -1);
        anim.SetInteger("DirectionY", 0);
    }



}