using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundScript : MonoBehaviour
{
    [HideInInspector]
    public bool isGrounded;

    public float groundCheckRadius;

    public Transform groundCheck;
    public LayerMask whatisGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatisGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
