using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Get Character Properties
    private Transform Player;
    private CharacterController CharController;
    public Transform PlayerCam;

    public GameObject DiscHolder;
    public GameObject discPrefab;

    private bool isDashCooldown;

    //Initialize Variables
    private float vertical;
    private float horizontal;
    private float velocityY;
    private Vector3 velocity;
    private Vector3 movement;
    private Vector3 currentImpact;
    public float moveSpeed = 5f;
    public float mass = 1f;
    public float damping = 5f;
    public float jumpForce = 4f;
    public float dashForce = 4f;
    public float hitStunDuration = 1.0f;
    private float gravity = Physics.gravity.y;
    [HideInInspector]
    public bool isGrounded = false;

    private float forwardMovement;
    private float horizontalMovement;

    private int doubleJumpCheck = 0;

    private void Start()
    {
        DiscHolder.SetActive(false);
        CharController = GetComponent<CharacterController>();
    }

    //Any Input(Keyboard or Mouse) should be in Update function
    private void Update()
    {
        //Checks if Player is on the ground, if true set Y Velocity to 0
        if (isGrounded && velocityY < 0f)
        {
            doubleJumpCheck = 0;
            velocityY = 0f;
        }

        vertical = Input.GetAxis("Vertical");
        forwardMovement = vertical;

        horizontal = Input.GetAxis("Horizontal");
        horizontalMovement = horizontal;

        //Create a Vector to store the overall movement
        movement = new Vector3(horizontalMovement, 0, forwardMovement);

        /*Takes the movement vector and converts the position from Local Space to World Space and stores 
        it back in the movement variable*/
        movement = transform.TransformDirection(movement);

        //Calculates gravity and stores it in variable
        velocityY += gravity * Time.deltaTime;

        //Vector which stores the overall effect of gravity on the Character's position
        velocity = movement * moveSpeed * Time.deltaTime * 1.2f + Vector3.up * velocityY;

        //sets the velocity to take all forces into account
        if (currentImpact.magnitude > 0.2f)
        {
            velocity += currentImpact;
        }

        CharController.Move(velocity * Time.fixedDeltaTime);
        Dash();

        /*This is so that when you press W and A at the same time for instance, the player doesn't become faster,
        it remains the same speed*/
        if (forwardMovement != 0 && horizontalMovement != 0)
        {
            forwardMovement *= 0.7071f;
            horizontalMovement *= 0.7071f;
        }

        //Jump pls
        Jump();

        isGrounded = CharController.isGrounded;

        //Interpolates the effects of forces for smooth movement
        currentImpact = Vector3.Lerp(currentImpact, Vector3.zero, damping * Time.deltaTime);

        /*if (isDashCooldown)
        {
            dashIconIMG.color = new Color(dashIconIMG.color.r, dashIconIMG.color.g, dashIconIMG.color.b, 1f);
            dashCooldownIMG.fillAmount += 1 / dashCooldown * Time.deltaTime;
            dashCountdownUI -= Time.deltaTime;
            if (dashCooldownIMG.fillAmount >= 1)
            {
                dashCountdownUI = 0;
                dashCooldownIMG.fillAmount = 0;
                dashCountdownUI = dashCooldown + 1;
                dashCooldownText.text = "";
                isDashCooldown = false;
            }
            else
            {
                dashIconIMG.color = new Color(dashIconIMG.color.r, dashIconIMG.color.g, dashIconIMG.color.b, 0.1f);
                dashCooldownText.text = dashCountdownUI.ToString();
            }
        }*/
    }

    //Resets all forces
    private void ResetImpact()
    {
        currentImpact = Vector3.zero;
        velocityY = 0f;
    }


    //Resets forces on the Y axis
    private void ResetImpactY()
    {
        currentImpact.y = 0f;
        velocityY = 0f;
    }

    //Jump Function
    private void Jump()
    {
        //Checks if Space was pressed
        if (Input.GetButtonDown("Jump"))
        {
            //Checks if player is on the ground, if true then character can jump
            if (doubleJumpCheck == 0 && CharController.isGrounded == true)
            {
                //Custom AddForce function which applies jumpForce in the upward direction
                AddForce(Vector3.up, jumpForce);
            }
            doubleJumpCheck++;
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            AddForce(PlayerCam.forward, dashForce);
        }
    }

    //Custom AddForce function, not to be mistakened with Rigidbody.AddForce()
    public void AddForce(Vector3 direction, float magnitude)
    {
        //Adds the force to the current amount of forces being applied to the game object
        currentImpact += direction.normalized * magnitude / mass;
    }

    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<DiscScript>())
        {
            if (!DiscHolder.activeSelf)
            {
                DiscHolder.SetActive(true);
            }
            Destroy(hit.gameObject);
        }
    }*/
}
