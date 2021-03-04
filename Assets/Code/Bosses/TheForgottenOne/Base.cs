using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, IDamageable
{

    [SerializeField]private Animator anim;
    [SerializeField] private Transform player;
    private bool Dashing = false;
    [SerializeField] private SpriteRenderer GFX;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform checkPos;
    [SerializeField] private Vector2 checkSize;
    [SerializeField] private LayerMask Player;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform rotator;
    [SerializeField] private float spinforce;
    [SerializeField] private float timeBtwShot = 0.5f;
    [SerializeField] private float health;
    private float currentTimeBtwShot;
    [SerializeField] private float timeBtwCircularShot = 0.5f;
    private float currentTimeBtwCircularShot;
    private bool shooting = false;
    private bool canChangePhase = false;
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private Transform[] spawnLocs;
    private bool checkNextSpawnLoc = false;
    [HideInInspector] public bool dead = false;
    private bool resetAngle = false;
    private bool circularShoot = false;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private AudioSource hit_sound;
    [SerializeField] private AudioSource death_sound;


    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCam.SetActive(false);
        Invoke("disableCamera", 4);
        Invoke("AllowChangingPhase", 5.5f);
    }


    // Update is called once per frame
    void Update()
    {

       

        if (health <=  0 && !dead) 
        {
            dead = true;
            death_sound.Play();
            anim.SetTrigger("Dead");
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("Enemy")) 
            {
                Destroy(i);
            }
            GetComponent<BoxCollider2D>().enabled = false;
            Invoke("DisableBoss", 2f);
        }

        int shouldShoot = Random.Range(0, 600);
        if (shouldShoot == 0 && !shooting && canChangePhase && !dead)
        {
            shooting = true;
            Invoke("StopShooting", 0.7f);
        }

        if (shooting && currentTimeBtwShot <= 0 ) 
        {
            Vector2 direction = transform.position - player.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotator.rotation = Quaternion.Euler(0, 0, angle + 180);
            Instantiate(bullet, shootPoint.position, rotator.rotation);
            Debug.Log("Shoot");
            currentTimeBtwShot = timeBtwShot;
        } else 
        {
            currentTimeBtwShot -= Time.deltaTime;
        }


        
        int shouldCircularShoot = Random.Range(0, 1000);
        if (shouldCircularShoot == 0 && canChangePhase && !circularShoot && !dead)
        {
            Invoke("StopCircularShoot", 2f);
            canChangePhase = false;
            circularShoot = true;
            Debug.Log("CircularShoot");
            if (!resetAngle)
            {
                resetAngle = true;
                rotator.eulerAngles = new Vector3(0, 0, 0);
            }
            
        }

        if (circularShoot) 
        {
            rotator.eulerAngles += new Vector3(0, 0, spinforce * Time.deltaTime);
            if (currentTimeBtwCircularShot <= 0) 
            {
                Instantiate(bullet, shootPoint.position, rotator.rotation);
                currentTimeBtwCircularShot = timeBtwCircularShot;
            } else 
            {
                currentTimeBtwCircularShot -= Time.deltaTime;
            }
        }

        int shouldDash = Random.Range(0, 500);
        if (shouldDash == 0 && !Dashing && canChangePhase && !dead)
        {
            Dashing = true;
            canChangePhase = false;
            Invoke("StopDashing", 0.8f);
            Debug.Log("Dash");
            anim.SetBool("Moving", true);
        }

        int shouldSpawn = Random.Range(0, 2000);
        if (shouldSpawn == 0 && canChangePhase && !dead)
        {
            canChangePhase = false;
            Invoke("AllowChangingPhase", 0.4f);
            for (int i = 0; i < spawnLocs.Length; i++)
            {
                while (true)
                {
                    for (int x = 0; x < enemiesToSpawn.Length; x++)
                    {

                        int spawn = Random.Range(0, 10);
                        if (spawn == 0)
                        {
                            Instantiate(enemiesToSpawn[x], spawnLocs[i].position, spawnLocs[i].rotation);
                            checkNextSpawnLoc = true;
                            break;
                        }


                    }
                    if (checkNextSpawnLoc)
                        break;
                }

                
            }
            
        }

        if (player.position.x > transform.position.x && !dead)
        {
            GFX.flipX = true;
        }
        else if (player.position.x < transform.position.x && !dead)
        {
            GFX.flipX = false;
        }


    }

    private void DisableBoss() 
    {
        Destroy(gameObject); 
    }
    private void OnDrawGizmos()
    {
       
    }

    private void disableCamera() 
    {
        mainCam.SetActive(true);
        cam.SetActive(false);

    }

    private void StopCircularShoot() 
    {
        resetAngle = false;
        circularShoot = false;
        canChangePhase = true;
    
    }

    private void AllowChangingPhase() 
    {
        canChangePhase = true;
    }

    private void StopDashing()
    {
        Dashing = false;
        anim.SetBool("Moving", false);
        canChangePhase = true;
    }

    private void StopShooting()
    {
        shooting = false;
        anim.SetBool("Shooting", false);
        canChangePhase = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dead) 
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dead)
        {
            collision.gameObject.GetComponent<IDamageable>().Damage(1);
        }
    }


    public void Damage(int amount)
    {
        if (health > 1)
        {
            hit_sound.Play();
            
        }
        health -= amount;
    }

}
