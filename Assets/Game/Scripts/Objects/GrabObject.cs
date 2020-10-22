using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{

    private List<GameObject> playerInRange;
    private GameObject targetPlayer;
    private List<Transform> targetFound;
    private Transform targetHand;
    bool isGrounded = false;

    [Header("Name of the Player Hand")]
    public string handName;

    // Start is called before the first frame update
    void Start()
    {
        playerInRange = new List<GameObject>();
        targetPlayer = null;
        targetFound = new List<Transform>();
        init_rigidbody();
    }

    private void init_rigidbody()
    {
        if (this.GetComponent<Rigidbody>() == null)
            this.gameObject.AddComponent<Rigidbody>();
        this.GetComponent<Rigidbody>().constraints =
            RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ |
            RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void Update()
    {
        takeTheObject();
        followTarget();
        manageGravity();
    }

    private void manageGravity()
    {
        if (targetPlayer == null && !isGrounded)
        {
            this.transform.Translate(0, 0, -0.1f);
        }
    }

    private void findSubChild(Transform parent, string childName, List<Transform> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.name == childName)
                list.Add(child);
            findSubChild(child, childName, list);
        }
    }

    private void followTarget()
    {
        if (targetPlayer != null)
        {
            this.transform.position = targetHand.position;
        }
    }

    private void takeTheObject()
    {
        for (int i = 0; i != playerInRange.Count; i++)
            if (playerInRange[i].GetComponent<PlayerActions>().getGrab() == true &&
                playerInRange[i].GetComponent<PlayerActions>().canGrab == true) {
                targetPlayer = playerInRange[i];
                findSubChild(targetPlayer.transform, handName, targetFound);
                targetHand = targetFound[0];
                targetPlayer.GetComponent<PlayerActions>().canGrab = false;
            }

        if (targetPlayer != null && targetPlayer.GetComponent<PlayerActions>().getGrab() == false)
        {
            targetPlayer.GetComponent<PlayerActions>().canGrab = true;
            targetPlayer = null;
            targetFound = new List<Transform>();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
            playerInRange.Add(col.gameObject);
        if (col.tag == "Floor")
            isGrounded = true;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
            playerInRange.Remove(col.gameObject);
        if (col.tag == "Floor")
            isGrounded = false;
    }
}
