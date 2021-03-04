using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{

    // spellbook rotation
    [SerializeField] private Transform spellbook; // the spellbook transform
    [HideInInspector] public Vector3 mousePos; // the mouse position
    [SerializeField] private Camera mainCam; // the main camera
    [SerializeField] public SpriteRenderer playerSpr; 
    [SerializeField] public SpriteRenderer bookSpr;
    [HideInInspector] public float angle;
    public delegate void noRetNoPams();

    // shooting
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private float timeBtwShot = 0.3f;
    private float currentTimeBtwShot;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioSource shoot;
    [SerializeField] public int maxMana;
    public int mana;
    private bool reloading = false;

    private void Start()
    {
        mana = maxMana;
        mainCam = Camera.main;
        playerSpr = GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<SpriteRenderer>();
        currentTimeBtwShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        #region Logic

        #region Rotation
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); // the players mouse in in-game world units
        Vector2 lookDir = mousePos - playerSpr.gameObject.transform.position; // the difference between the mouse Postion and the player position
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; // the angle that the spellbook should point to in degrees
        spellbook.rotation = Quaternion.Euler(0, 0, angle); // rotate the spellbook
        #endregion

        #region Shooting
        if (Input.GetButtonDown("Fire1") && currentTimeBtwShot <= 0 && mana > 0 && !reloading)
        {
            bookSpr.enabled = true;
            Instantiate(bullet, ShootPoint.position, spellbook.rotation);
            currentTimeBtwShot = timeBtwShot;
            shoot.Play();
            mana--;
        }
        else if (currentTimeBtwShot > 0)
        {
            currentTimeBtwShot -= Time.deltaTime;
        } else if (mana <= 0)
        { 
            if (!reloading)
                Invoke("reload", 1.2f);
            reloading = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Invoke("reload", 1.2f);
            reloading = true;
        }

        if (currentTimeBtwShot <= 0)
        {
            bookSpr.enabled = false;
        }
        #endregion

        #endregion

        #region Visuals
        if (((angle > 95 && angle < 175) || (angle < -95 && angle > -175))) // left
        {
            spellbook.localScale = new Vector3(spellbook.localScale.x, -Mathf.Abs(spellbook.localScale.y), -1);
            spellbook.localPosition = new Vector3(-Mathf.Abs(spellbook.localPosition.x), spellbook.localPosition.y);

        }
        else if (((angle < 85 && angle > 5) || (angle > -85 && angle < -5))) // right
        {
            
            spellbook.localScale = new Vector3(spellbook.localScale.x, Mathf.Abs(spellbook.localScale.y), -1);
            spellbook.localPosition = new Vector3(Mathf.Abs(spellbook.localPosition.x), spellbook.localPosition.y);
        }


        //Debug.Log(angle);

       

        #endregion

    }

    public void AngleCheck(noRetNoPams right, noRetNoPams left, noRetNoPams top, noRetNoPams bottom)
    {
        if (angle > -45 && angle < 45)
        {
            right();
        }
        else if (angle < -135 || angle > 135)
        {
            left();
        }
        else if (angle > 45 && angle < 135)
        {
            top();
        }else
        {
            bottom();
        }
        
    }

    private void reload() 
    {
        mana = maxMana;
        reloading = false;
    }




}
