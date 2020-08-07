using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = 20.0f;
    public float jumpSpeed = 6.0f;
    public Camera cam;
    public SwingPendulum pendulum;

    CharacterController controller;
    enum State {Swinging, Falling, Walking};
    State state;
    float distGround;
    Vector3 hitPos;
    Vector3 prevPos;

    private Vector3 moveDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        state = State.Walking;
        pendulum.playerTr.transform.parent = pendulum.tether.tetherTr;
        prevPos = transform.localPosition;
        distGround = GetComponent<CapsuleCollider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        DetermineState();

        switch (state)
        {
            case State.Swinging:
                DoSwingAction();
                break;

            case State.Falling:
                DoFallingAction();
                break;

            case State.Walking:
                DoWalkingAction();
                break;
        }
        prevPos = transform.localPosition;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distGround + 1.0f);
    }

    void DetermineState()
    {
        if (IsGrounded())
            state = State.Walking;

        else if (Input.GetButtonDown("Fire1") && state == State.Falling)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            
            if (Physics.Raycast(ray, out RaycastHit hit) )
            {
                print(hit.distance);
                print("hit");
                if (state == State.Walking)
                    pendulum.player.velocity = moveDir;

                pendulum.SwitchTether(hit.point);
                state = State.Swinging;
            }
        }
        
        else if (Input.GetButtonDown("Fire2"))
        {
            if (state == State.Swinging)
                state = State.Falling;
        }
    }

    void DoSwingAction()
    {
        if (Input.GetKey(KeyCode.W))
            pendulum.player.velocity += pendulum.player.velocity.normalized * 2.0f;

        if (Input.GetKey(KeyCode.A))
            pendulum.player.velocity += -cam.transform.right * 1.2f;

        if (Input.GetKey(KeyCode.D))
            pendulum.player.velocity += cam.transform.right * 1.2f;

        transform.localPosition = pendulum.MovePlayer(transform.localPosition, prevPos, Time.deltaTime);
        prevPos = transform.localPosition;
    }

    void DoFallingAction()
    {
        pendulum.web.length = Mathf.Infinity;
        transform.localPosition = pendulum.Fall(transform.localPosition, Time.deltaTime);
        prevPos = transform.localPosition;
    }

    void DoWalkingAction()
    {
        pendulum.player.velocity = Vector3.zero;

        if (controller.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDir = Camera.main.transform.TransformDirection(moveDir);
            moveDir.y = 0.0f;
            moveDir *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDir.y = jumpSpeed;
            }

        }
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.name == "Respawn")
        {
            //if too far from arena, reset level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print("COLLISION");
        //Vector3 undesiredMotion = collision.contacts[0].normal * Vector3.Dot(pendulum.player.velocity, collision.contacts[0].normal);
        pendulum.player.velocity -= (collision.contacts[0].normal * Vector3.Dot(pendulum.player.velocity, collision.contacts[0].normal) * 1.2f);
        hitPos = transform.position;

        if (collision.gameObject.name == "Respawn")
        {
            //if too far from arena, reset level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
