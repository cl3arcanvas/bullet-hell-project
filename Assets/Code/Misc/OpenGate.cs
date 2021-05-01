using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OpenGate : MonoBehaviour
{

    [SerializeField] private GameObject[] gates;
    ArrayList EnemiesDefeated = new ArrayList();
    [SerializeField] private AstarPath pathfinder;
    private bool hasScanned = false;
    private bool GateGone; // for performance

    // Start is called before the first frame update
    void Start()
    {
        GateGone = false;   
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.childCount <= 0 && !GateGone) 
        {
            foreach (GameObject gate in gates)
            {
                gate.GetComponent<BoxCollider2D>().enabled = false;
                gate.transform.position += new Vector3(0, 25 * Time.deltaTime);
                
                if (!hasScanned) {
                    pathfinder.Scan();
                    hasScanned = true;
                }
            }
        } 

        /*
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject Go = transform.GetChild(i).gameObject;
            if (Go.activeSelf == false)
            {
                EnemiesDefeated.Add(true);
            }
        }

        if (EnemiesDefeated.Count == transform.childCount && !GateGone)
        { 
            GateGone = true;
            
            Invoke("turnOffGate", 1f);
        }

        if (GateGone) 
        {
            foreach (GameObject gate in gates)
            {
                gate.GetComponent<BoxCollider2D>().enabled = false;
                gate.transform.position += new Vector3(0, 25 * Time.deltaTime);

            }
            
        }
        */
        
        
    }

    private void turnOffGate() 
    {

        foreach (GameObject gate in gates)
        {
            gate.SetActive(false);
        }
        pathfinder.Scan();
        
    }
}
