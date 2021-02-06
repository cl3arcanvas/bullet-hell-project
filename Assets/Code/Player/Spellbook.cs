using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{

    // spellbook rotation
    [SerializeField] private Transform spellbook; // the spellbook transform
    Vector3 mousePos; // the mouse position
    [SerializeField] private Camera mainCam; // the main camera
    [SerializeField] public SpriteRenderer playerSpr; 
    [SerializeField] public SpriteRenderer bookSpr;
    public delegate void noRetNoPams();

    // shooting

    [SerializeField] private Transform ShootPoint;
    [SerializeField] private float timeBtwShot = 0.5f;
    private float currentTimeBtwShot;
    [SerializeField] private GameObject bullet;
    [HideInInspector] public bool top;
    [HideInInspector] public bool right;
    [HideInInspector] public float angle;

    private void Start()
    {
        currentTimeBtwShot = timeBtwShot;
    }

    // Update is called once per frame
    void Update()
    {
        #region Logic

        #region Rotation
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); // the players mouse in in-game world units
        Vector2 lookDir = mousePos - transform.position; // the difference between the mouse Postion and the spellbook position
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; // the angle that the spellbook should point to in degrees
        spellbook.rotation = Quaternion.Euler(0, 0, angle); // rotate the spellbook
        #endregion

        #region Shooting
        if (Input.GetButton("Fire1") && currentTimeBtwShot <= 0)
        {
            Instantiate(bullet, ShootPoint.position, spellbook.rotation);
            currentTimeBtwShot = timeBtwShot;
        }
        else
        {
            currentTimeBtwShot -= Time.deltaTime;
        }
        #endregion

        #endregion

        #region Visuals
        if (((angle > 95 && angle < 175) || (angle < -95 && angle > -175))) // left
        {
            playerSpr.flipX = true;
            spellbook.localScale = new Vector3(spellbook.localScale.x, -Mathf.Abs(spellbook.localScale.y), -1);
            spellbook.localPosition = new Vector3(-Mathf.Abs(spellbook.localPosition.x), spellbook.localPosition.y);

        }
        else if (((angle < 85 && angle > 5) || (angle > -85 && angle < -5))) // right
        {
            playerSpr.flipX = false;
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




}
