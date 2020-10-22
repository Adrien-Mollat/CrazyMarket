using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    // Start is called before the first frame update
    private bool objGrabbed;
    [HideInInspector]
    public bool canGrab = true;
    private List<GameObject> carryableInRange;
    GameObject carryedObj;

    void Start()
    {
        objGrabbed = false;
        carryedObj = null;
        carryableInRange = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Carryable")
            carryableInRange.Add(col.gameObject);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Carryable")
            carryableInRange.Remove(col.gameObject);
    }

    private void Update()
    {
        if (carryableInRange.Count != 0 && Input.GetKeyUp(KeyCode.E))
            objGrabbed = !objGrabbed;
        if (objGrabbed == true)
        {
            carryedObj = carryableInRange[0];
            if (Input.GetKeyDown(KeyCode.Space))
            {
                carryableInRange.Remove(carryedObj);
                Destroy(carryedObj);
                objGrabbed = !objGrabbed;
            }
        }
        else
            carryedObj = null;
    }

    // Update is called once per frame
    public void setGrab(bool isgrab)
    {
        objGrabbed = isgrab;
    }

    public bool getGrab()
    {
        return objGrabbed;
    }
        
}
