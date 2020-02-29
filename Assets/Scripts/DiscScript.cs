using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscScript : MonoBehaviour
{
    [HideInInspector]
    public bool wasThrown = false;

    private void Update()
    {
        if (GetComponent<Rigidbody>())
        {
            Debug.Log(transform.localEulerAngles);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerScript;
        playerScript = other.gameObject.GetComponent<PlayerMovement>();
        if (playerScript && !wasThrown)
        {
            if (!playerScript.DiscHolder.activeSelf)
            {
                playerScript.DiscHolder.SetActive(true);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement playerScript;
        playerScript = other.gameObject.GetComponent<PlayerMovement>();
        if (playerScript)
        {
            wasThrown = false;
        }
    }
}
