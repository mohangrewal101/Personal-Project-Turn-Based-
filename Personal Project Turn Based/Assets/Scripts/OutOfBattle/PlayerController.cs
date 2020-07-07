using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isFalling;
    private bool isFacingRight;
    private bool isAttacking;

    private float movementInputdirection;
    private float storedInputdirection;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float timeAttackCooldown;
   [HideInInspector] public float forwardVelocity;
    public float maxSpeed;
    public float timeZeroToMax;
    public float timeMaxToZero;
    public float attackCooldown;
    public float attackRange;

    private Transform enemyPositon;
    private BattleSystem battleSystem;
    private OnGroundScript groundScript;

    public float jumpVelocity;

    private Rigidbody2D rb;
    public Transform attackCheck;
    public SceneSwitcher sceneSwitcher;

    void Start()
    {
        groundScript = GetComponent<OnGroundScript>();
        enemyPositon = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
    }


    void Update()
    {
        CheckInput();
        CheckIfMoving();
        CheckMovementDirection();
        isPlayerFalling();
        
        
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        timeAttackCooldown--;
    }

    void CheckInput()
    {
        movementInputdirection = Input.GetAxisRaw("Horizontal");

        if (movementInputdirection != 0)
        {
            storedInputdirection = movementInputdirection;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown("c"))
        {
            Attack();
        }
    }

    //EFFECTS: Checks the various movement directions of the character
    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputdirection < 0)
        {
            Flip();

        }
        else
        {
            if (!isFacingRight && movementInputdirection > 0)
            {
                Flip();

            }
        }
    }

    //EFFECTS: Rotates the sprite 180 degrees on the y-axis
    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        
    }

    //MODIFIES: this
    //EFFECTS: Apply movement left to right depending on movementInputdirection
    private void ApplyMovement()
    {
        if (movementInputdirection == 1)
        {
            if (forwardVelocity < 0)
            {
                forwardVelocity = 0;
            }
            forwardVelocity += accelRatePerSec * Time.unscaledDeltaTime;
            rb.velocity = new Vector2(forwardVelocity, rb.velocity.y); if (forwardVelocity >= maxSpeed)
            {
                forwardVelocity = maxSpeed;
            }
        }

        if (movementInputdirection == -1)
        {
            if (forwardVelocity > 0)
            {
                forwardVelocity = 0;
            }
            forwardVelocity += -accelRatePerSec * Time.unscaledDeltaTime;
            rb.velocity = new Vector2(forwardVelocity, rb.velocity.y);
            if (forwardVelocity <= -maxSpeed)
            {
                forwardVelocity = -maxSpeed;
            }
        }
    }
        

    //REQUIRES: canJump() to be called
    //MODIFIES: this
    //EFFECTS: Executes player jump if player is grounded and not falling
    void Jump()
    {
        if (groundScript.isGrounded)
        {
           rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            
        }
    }

    //EFFECTS: Player attacks enemy
    void Attack()
    {
        if (timeAttackCooldown <= 0)
        {
            timeAttackCooldown = attackCooldown;
            if (Vector3.Distance(attackCheck.position, enemyPositon.position) <= attackRange) // Vector3.Distance checks the distance between two Vector3's
            {
                sceneSwitcher.SwitchScenes("BattleScene");
                //PLAYER ATTACKS IF ATTACK HITS ENEMY
                
            }
        }
    }

    //MODIFIES: this
    //EFFECTS: Sets isFalling to whether object is falling or not
    void isPlayerFalling()
    {
        isFalling = !groundScript.isGrounded && rb.velocity.y < 0; 
    }

    public void CheckIfMoving()
    {
        if (movementInputdirection == 0)
        {
            switch (storedInputdirection)
            {
                case 1:
                    forwardVelocity += decelRatePerSec * Time.unscaledDeltaTime;
                    if (forwardVelocity < 0)
                    {
                        forwardVelocity = 0;
                    }
                    break;
                case -1:
                    forwardVelocity -= decelRatePerSec * Time.unscaledDeltaTime;
                    if (forwardVelocity > 0)
                    {
                        forwardVelocity = 0;
                    }
                    break;

            }

            rb.velocity = new Vector2(forwardVelocity, rb.velocity.y);
        }
    }
}
