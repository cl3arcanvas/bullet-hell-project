using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEffect : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] private float animTime;
    private float currentAnimTime;

    private void Start()
    {
        currentAnimTime = animTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAnimTime <= 0) 
        {
            Destroy(gameObject);

        } else 
        {
            currentAnimTime -= Time.deltaTime;
        }
    }
}
