using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDisc : MonoBehaviour
{
    public GameObject DiscHolder;
    public GameObject discPrefab;
    public Camera camObj;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && DiscHolder.activeSelf)
        {
            GameObject spawnedDisc;
            spawnedDisc = Instantiate(discPrefab, DiscHolder.transform.position, DiscHolder.transform.rotation);
            spawnedDisc.GetComponent<DiscScript>().wasThrown = true;
            DiscHolder.SetActive(false);
            spawnedDisc.GetComponent<Rigidbody>().velocity = camObj.transform.forward * 150;
            spawnedDisc.GetComponent<Rigidbody>().angularVelocity = spawnedDisc.transform.up * 5000;
            spawnedDisc.GetComponent<Rigidbody>().AddForce(spawnedDisc.transform.up * 10, ForceMode.VelocityChange);
            spawnedDisc.GetComponent<Rigidbody>().AddTorque(spawnedDisc.transform.up * 500, ForceMode.VelocityChange);
        }
    }
}
