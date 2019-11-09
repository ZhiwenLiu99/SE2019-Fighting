using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public float maxSpeed = 50;
    public float jumpTakeOffSpeed = 60;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // public float moveSpeed = 50;
    // public float jumpSpeed = 30;
    public float volume;
    public double freq;
    public float freqThreshold = 600;

    // Rigidbody2D rig2d;
    // private float playerHalfWidth;
    // private float playerHalfHeight;

    // Use this for initialization
    void Awake()
    {
        /*
        rig2d = transform.GetComponent<Rigidbody2D>();
        playerHalfWidth = transform.GetComponent<RectTransform>().sizeDelta.x / 2;
        playerHalfHeight = transform.GetComponent<RectTransform>().sizeDelta.y / 2;
        */

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        volume = GameObject.Find("MicroInput").GetComponent<MicroInput>().volume;
        freq = GameObject.Find("MicroInput").GetComponent<MicroInput>().freq;

        Vector2 move = Vector2.zero;

        // move.x = Input.GetAxis ("Horizontal");
        if(volume>0.1)
        {
            move.x = volume + (float)0.5;
        }
        else
        {
            move.x = volume;
        }

        /*
        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }
        */

        if (freq > freqThreshold && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (freq > freqThreshold)
        {           
            if (velocity.y > 0)
            {
                velocity.y *= 0.8f;
            }           
        }

        /*
        if (freq > freqThreshold)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position - new Vector3(playerHalfWidth, 0),
                Vector3.down, 2 * playerHalfHeight, 1 << LayerMask.NameToLayer("plane"));
            if (hitInfo.collider == null)
            {
                hitInfo = Physics2D.Raycast(transform.position + new Vector3(playerHalfWidth, 0),
                Vector3.down, 2 * playerHalfHeight, 1 << LayerMask.NameToLayer("plane"));
            }

            if (hitInfo.collider != null)
            {
                if (hitInfo.distance < playerHalfHeight + 0.2)
                {
                    velocity.y = jumpSpeed;
                }
            }
        }
        */

        if (move.x > 0.01f)
        {
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (move.x < -0.01f)
        {
            if (spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }
}