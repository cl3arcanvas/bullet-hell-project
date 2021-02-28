using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{

    [SerializeField] private Transform arm;
    [SerializeField] private GameObject arrow;
    private Transform target;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private SpriteRenderer spr;

    [SerializeField] private float timeBtwShot;
    private float currentTimeBtwShot;

    // Start is called before the first frame update
    void Start()
    {

        currentTimeBtwShot = timeBtwShot;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 direction = transform.position - target.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arm.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (((angle > 90 && angle < 180) || (angle < -90 && angle > -180))) // right
        {
            spr.flipX = false;
            //arm.localScale = new Vector3(0.7f, -0.7f, -1);

        }
        else if (((angle < 90 && angle > 0) || (angle > -90 && angle < 0))) // left
        {
            spr.flipX = true;
            //arm.localScale = new Vector3(0.7f, 0.7f, -1);
        }



        if (currentTimeBtwShot <= 0 && GetComponent<EnemyBase>().shouldTrack) 
        {
            //shootPoint.localPosition = new Vector2(shootPoint.position.x,Random.Range(-0.1f, 0f));
            Instantiate(arrow, shootPoint.position, shootPoint.rotation);
            currentTimeBtwShot = timeBtwShot;
        } else 
        {
            currentTimeBtwShot -= Time.deltaTime;
        }


    }
}
