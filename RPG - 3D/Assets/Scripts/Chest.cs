using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;
    public float ColliderRadius;
    public bool IsOpened;

    public List<Item> Items = new List<Item>();

    void Start()
    {
        anim = GetComponent<Animator>();
    }

   
    void Update()
    {
        GetPlayer();   
    }

    void GetPlayer()
    {
        if(!IsOpened)
        {
            foreach (Collider c in Physics.OverlapSphere((transform.position + transform.forward * ColliderRadius), ColliderRadius))
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    if(Input.GetMouseButtonDown(0))
                    OpenChest();
                }
            }
        }
    }

    void OpenChest()
    {
        foreach(Item i in Items)
        {
            i.GetAction();
        }
        anim.SetTrigger("open");
        IsOpened = true;
    }
}
