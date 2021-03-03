using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Mage : MonoBehaviour
{

    [SerializeField] private Transform rotator;
    [SerializeField] private float rotateForce = 300f;
    [SerializeField] private float timeBtwShot = 5f;
    private float currentTimeBtwShot;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float timeBtwNextBullet;
    private float currentTimeBtwNextBullet;
    private bool shooting;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeBtwShot = timeBtwShot;
    }

    // Update is called once per frame
    void Update()
    {

        

        if (currentTimeBtwShot <= 0 && !shooting && gameObject.GetComponent<EnemyBase>().shouldTrack)
        {
            shooting = true;
            Invoke("StopShoot", 2f);
            gameObject.GetComponent<EnemyBase>().idle = true;
            gameObject.GetComponent<EnemyBase>().invincible = true;
            currentTimeBtwShot = timeBtwShot;

        }
        else 
        {
            currentTimeBtwShot -= Time.deltaTime;
        }

        rotator.eulerAngles += new Vector3(0, 0, rotateForce * Time.deltaTime);

        if (shooting) 
        {
            if (currentTimeBtwNextBullet <= 0) 
            {
                Instantiate(bullet, rotator.GetChild(0).position, rotator.GetChild(0).rotation);
                currentTimeBtwNextBullet = timeBtwNextBullet;

            } else  
            {
                currentTimeBtwNextBullet -= Time.deltaTime;
            }
        }

       

    }

    private void StopShoot() 
    {
        gameObject.GetComponent<EnemyBase>().invincible = false;
        gameObject.GetComponent<EnemyBase>().idle = false;
        shooting = false;

    }
}
