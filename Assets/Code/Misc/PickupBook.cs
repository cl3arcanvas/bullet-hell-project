using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBook : MonoBehaviour
{
    [SerializeField] private GameObject book;
    private bool pickedUp = false;
    [SerializeField] private Vector2 checkArea;
    [SerializeField] private LayerMask player;
    [SerializeField] private GameObject bookSprite;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool inRange = Physics2D.OverlapBox(transform.position, checkArea, 0, player);

        if (Input.GetKeyDown(KeyCode.E) && !pickedUp && inRange) 
        {
            anim.gameObject.SetActive(true);
            book.SetActive(true);
            pickedUp = true;
            bookSprite.SetActive(false);
            Invoke("fadeOut", 1.3f);
        }


    }
    
    private void fadeOut() 
    {
        anim.SetTrigger("FadeOut");
    }


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, checkArea);
    }

}
