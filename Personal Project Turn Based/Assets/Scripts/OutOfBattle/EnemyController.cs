using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyBehaviour
    {
        Patrol, Chase
    }

    private EnemyBehaviour enemyBehaviour;
    private Transform playerMovement;
    private Rigidbody2D rb;

    public SceneSwitcher sceneSwitcher;

    [Header("EnemyStats")]
    public float enemyHealth;
    public float enemyDamage;

    public float movementSpeed;

    [Header("EnemyChase")]
    public int cooldownTime;
    private int followCooldown;
    private bool isInRange;

    public float chaseSpeed;
    public float chaseRange;

    // Start is called before the first frame update
    void Start()
    {
        enemyBehaviour = EnemyBehaviour.Patrol;
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        CanFollowPlayer();
    }

    private void FixedUpdate()
    {
        if (enemyBehaviour == EnemyBehaviour.Chase)
        {
            ChasePlayerHorizontally();
        }
        else
        {
            //Patrolling enemy!
        }
    }

    //MODIFIES: this
    //EFFECTS: Checks to see if enemy is close enough to chase player!
    private void CanFollowPlayer()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerMovement.position);
        if (distanceToTarget <= chaseRange)
        {
            enemyBehaviour = EnemyBehaviour.Chase;
            isInRange = true;
        }
        else
        {
            if (followCooldown > 0)
            {
                enemyBehaviour = EnemyBehaviour.Chase;
            }
            else
            {
                if (isInRange) followCooldown = cooldownTime;
                enemyBehaviour = EnemyBehaviour.Patrol;
            }
            isInRange = false;
        }


    }

    //MODIFIES: this
    //EFFECTS: Chases the target depending on their position
    private void ChasePlayerHorizontally()
    {
        if (playerMovement.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(movementSpeed * chaseSpeed, rb.velocity.y);

        }
        else
        {

            rb.velocity = new Vector2(movementSpeed * -chaseSpeed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("has collide");
            //Call scene switcher with scene name
            sceneSwitcher.SwitchScenes("BattleScene");
        }
    }
}
