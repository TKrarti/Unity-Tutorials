using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    //States
    bool isAlive = true;
    bool doubleJumpActive = false;
    [SerializeField] Vector2 enemyKick = new Vector2(2f, 2f);

    //Cached component references
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D bodyCollider2D;
    BoxCollider2D feetCollider2D;
    float gravityScaleAtStart;
    
    //Messages then methods
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) { return; }
        Run();
        ClimbLadder();
        Jump();
        Die();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal"); // value is between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        FlipSprite(playerHasHorizontalSpeed);
    }

    private void ClimbLadder()
    {
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = gravityScaleAtStart;
            return; 
        }


        float controlThrow = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0.1f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void Jump()
    {
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
        {
            if (Input.GetButtonDown("Jump")) 
            {
                if (doubleJumpActive)
                {
                    Vector2 newVelocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
                    myRigidbody.velocity = newVelocity;
                }
                doubleJumpActive = false; 
            }
            if (!doubleJumpActive) { return; }
        }

        if (Input.GetButtonDown("Jump"))
        {
            doubleJumpActive = true;
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void FlipSprite(bool playerHasHorizontalSpeed)
    {
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private void Die()
    {
        if(bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("isDying");
            GetComponent<Rigidbody2D>().velocity = enemyKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
