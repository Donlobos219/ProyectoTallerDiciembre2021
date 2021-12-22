using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMech : MonoBehaviour
{
    public Transform player;

    public bool mechActive;
    bool isInTransition;
    public Transform seatPoint;
    public Vector3 sittinfOffSet;
    public Transform exitPoint;
    [Space]
    public float transitionSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(mechActive && isInTransition) Exit();
        else if (!mechActive && isInTransition) Enter();

        if(Input.GetKeyDown(KeyCode.E))
        {
            isInTransition = true;
        }
    }

    private void Enter()
    {
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<Rigidbody>().useGravity = false;

        player.position = Vector3.Lerp(player.position, seatPoint.position + sittinfOffSet, transitionSpeed);
        player.rotation = Quaternion.Slerp(player.rotation, seatPoint.rotation, transitionSpeed);

        player.GetComponentInChildren<Animator>().SetBool("Sitting", true);

        if(player.position == seatPoint.position + sittinfOffSet) { isInTransition = false; mechActive = true; }
    }

    void Exit()
    {
        player.position = Vector3.Lerp(player.position, exitPoint.position, transitionSpeed);

        player.GetComponentInChildren<Animator>().SetBool("Stting", false);

        if(player.position == exitPoint.position) { isInTransition = false; mechActive = false; }

        player.GetComponentInChildren<CapsuleCollider>().enabled = true;
        player.GetComponentInChildren<Rigidbody>().useGravity = true;
         

    }
}
