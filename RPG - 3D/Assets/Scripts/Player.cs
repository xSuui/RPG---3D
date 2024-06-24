using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Speed;
    public float RotSpeed;
    private float Rotation;
    public float Gravity;

    Vector3 MoveDirection;

    CharacterController controller;
    Animator anim;

    bool IsReady;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GetMouseInput();
    }

    void Move()
    {
         if(controller.isGrounded)
        {
            if(Input.GetKey(KeyCode.W))
            {
                if(!anim.GetBool("attacking"))
                { 
                    anim.SetBool("walking", true);
                    anim.SetInteger("transition", 1);
                    MoveDirection = Vector3.forward * Speed;
                    MoveDirection = transform.TransformDirection(MoveDirection);
                }

                else
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                    MoveDirection = Vector3.zero;
                    StartCoroutine(Attack(1));
                }
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("walking", false);
                anim.SetInteger("transition", 0);
                MoveDirection = Vector3.zero;
            }
        }

        Rotation += Input.GetAxis("Horizontal") * RotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, Rotation, 0);

        MoveDirection.y -= Gravity * Time.deltaTime;
        controller.Move(MoveDirection * Time.deltaTime);
    }

    void GetMouseInput()
    {
        if(controller.isGrounded)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(anim.GetBool("walking"))
                {
                    anim.SetBool("walking", false);
                    anim.SetInteger("transition", 0);
                }

                if(!anim.GetBool("walking"))
                {
                    StartCoroutine(Attack(0));
                }
            }
        }
    }

    IEnumerator Attack(int transitionValue)
    {
        if(!IsReady)
        {
            IsReady = true;
            anim.SetBool("attacking", true);
            anim.SetInteger("transition", 2);
            yield return new WaitForSeconds(1.3f);
            anim.SetInteger("transition", transitionValue);
            anim.SetBool("attacking", false);
            IsReady = false;
        }
    }
}
