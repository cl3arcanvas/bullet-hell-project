using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{

    private BoxCollider2D bCollider;
    [SerializeField] private GameObject boss;
    
    [SerializeField] private GameObject[] gates;
    [SerializeField] private Vector3[] positions;
    private bool closedGate = false;

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
        if (collision.CompareTag("Player") && !closedGate) 
        {
            if (!closedGate)
                Invoke("TurnOnGate", 0.5f);
            closedGate = true;
            boss.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().stopMoving = true;
            bCollider.enabled = false;
        }
    }

    private void TurnOnGate() 
    {
        int i = 0;
        foreach (GameObject gate in gates)
        {
            
            gate.SetActive(true);
            gate.transform.position = positions[i];
            i++;
        }
    }

}
