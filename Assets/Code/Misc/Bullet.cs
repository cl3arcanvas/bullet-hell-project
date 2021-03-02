using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private bool Passive = false;
    [SerializeField] private bool shouldspin = false;
    [SerializeField] private GameObject spriteHolder = null;
    [SerializeField] private float spinforce = 3f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float timeToDestroy = 3f;
    bool hasCollided = false;
    public LayerMask colliding;
    public Vector2 checkBoxSize;
    private bool shouldDestroy = false;
    private float currentTimeToDestroy;

    private void Start()
    {
        currentTimeToDestroy = timeToDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        hasCollided = Physics2D.OverlapBox(transform.position, checkBoxSize, 0, colliding);

        if (currentTimeToDestroy <= 0)
            Destroy(gameObject);
        else
            currentTimeToDestroy -= Time.deltaTime;
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));


        if (shouldDestroy)
        {
            Destroy(gameObject);
        }




        
        if (shouldspin) 
        {
            spriteHolder.transform.eulerAngles += new Vector3(0, 0, spinforce * Time.deltaTime);

        }




    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, checkBoxSize);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        shouldDestroy = true;

        if (collision.gameObject.CompareTag("Player") && !Passive && !collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(1);

        }
        else if (collision.gameObject.CompareTag("Player") && Passive)
        {
            shouldDestroy = false;


        }
        else if (Passive)
        {
            if (collision.gameObject.GetComponent<IDamageable>() != null)
                collision.gameObject.GetComponent<IDamageable>().Damage(1);
            shouldDestroy = true;
        }


        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) && !Passive)
        {
            shouldDestroy = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        shouldDestroy = true;

        if (collision.gameObject.CompareTag("Player") && !Passive && !collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(1);

        }
        else if (collision.gameObject.CompareTag("Player") && Passive)
        {
            shouldDestroy = false;


        } else if (Passive)
        {
                if (collision.gameObject.GetComponent<IDamageable>() != null)
                    collision.gameObject.GetComponent<IDamageable>().Damage(1);
                shouldDestroy = true;
        }


        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) && !Passive)
        {
            shouldDestroy = false;
        }

        
    }



}
