using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{

    private BoxCollider2D bCollider;
    [SerializeField] private GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            boss.SetActive(true);
        }
    }

}
