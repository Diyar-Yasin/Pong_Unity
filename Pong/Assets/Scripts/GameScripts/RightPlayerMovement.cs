using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moves the player up and down
public class RightPlayerMovement : MonoBehaviour
{
    // The StateMachine for dashing was taken from
    // https://answers.unity.com/questions/892955/dashing-mechanic-using-rigidbodyaddforce.html
    public DashState dashState;
    public float dashTimer;
    public float maxDash = 3f;

    public Vector2 savedVelocity;

    public float speed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    void Start()
    {
        rb.velocity = new Vector2(0, speed);
    }

    void Update()
    {
        //Collects in all W and S inputs
        movement.y = Input.GetAxisRaw("VerticalRight");

    }

    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    void FixedUpdate()
    {
        //Moves the rigidbody of the player object up/down
        rb.MovePosition(rb.position + movement * rb.velocity * Time.fixedDeltaTime);

        switch (dashState)
        {
            case DashState.Ready:
                bool isDashing = Input.GetKeyDown(KeyCode.RightShift);
 
                if (isDashing)
                {
                    savedVelocity = rb.velocity;
                    rb.velocity = new Vector2(0, rb.velocity.y * 2f);
                    dashState = DashState.Dashing;
                }
                break;

            case DashState.Dashing:
                dashTimer += Time.deltaTime * 3;

                if (dashTimer >= maxDash)
                {
                    dashTimer = maxDash;
                    rb.velocity = savedVelocity;
                    dashState = DashState.Cooldown;
                }
                break;

            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;

                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }

}
