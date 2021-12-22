using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMechSystem : MonoBehaviour
{
    
    public MonoBehaviour MechController;
    public Transform mech;
    public Transform player;

    [Header("Camera")]
    public GameObject playerCam;
    public GameObject mechCam; 

    public GameObject DriveUI;

    bool canDrive;
    bool driving;

    private MechController mechController;
    // Start is called before the first frame update
    void Start()
    {

        MechController.enabled = false;
        DriveUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canDrive)
        {
            //DriveUI.gameObject.SetActive(true);
        }
        else
        {
            //DriveUI.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.E) && canDrive)
        {
            MechController.enabled = true;
            //DriveUI.SetActive(false);
            //driving = true;
            player.transform.SetParent(mech);
            player.gameObject.SetActive(false);
            playerCam.gameObject.SetActive(false);
            mechCam.gameObject.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            MechController.enabled = false;
            //driving = false;
            player.transform.SetParent(null);
            player.gameObject.SetActive(true);
            playerCam.gameObject.SetActive(true);
            mechCam.gameObject.SetActive(false);
            mech.gameObject.SetActive(false);
            
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            //DriveUI.SetActive(true);
            canDrive = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            //DriveUI.SetActive(false);
            canDrive = false;
        }
    }
}
