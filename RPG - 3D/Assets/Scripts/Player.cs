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

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
         if(controller.isGrounded)
        {
            if(Input.GetKey(KeyCode.W))
            {
                MoveDirection = Vector3.forward * Speed;
                MoveDirection = transform.TransformDirection(MoveDirection);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                MoveDirection = Vector3.zero;
            }
        }

        Rotation += Input.GetAxis("Horizontal") * RotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, Rotation, 0);

        MoveDirection.y -= Gravity * Time.deltaTime;
        controller.Move(MoveDirection * Time.deltaTime);
    }
}
