using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject discHolder;
    public float speed = 10.0f;
    private float translation;
    private float straffe;
    private Rigidbody rb;
    public LayerMask Ground;
    public float GroundDistance = 0.2f;
    public Transform groundChecker;
    private bool isGrounded = false;
    public float JumpHeight = 2f;
    public float DashDistance = 5f;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
    }

    // Update is called once per frame
    void Update()
    {
        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 movement = new Vector3(straffe, 0, translation);
        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement);
        rb.MovePosition(newPosition);
        //transform.Translate(straffe, 0, translation);

        if (Input.GetKeyDown("escape"))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime)));
            rb.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DiscScript>())
        {
            if (!discHolder.activeSelf)
            {
                discHolder.SetActive(true);
            }
            Destroy(collision.gameObject);
        }
    }
}
