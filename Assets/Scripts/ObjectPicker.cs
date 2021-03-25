using UnityEngine;

/// <summary>
/// Script which can be added to a player with a camera. allows the user to click and drag on objects to move them around with a spring joint.
/// </summary>
public class ObjectPicker : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;

    private GameObject currentPickedObject = null;

    private GameObject grabAnchor = null;

    private Camera playerCam = null;

    private SpringJoint grabSpring = null;//spring connected to grabbed object

    private Rigidbody grabAnchorBody = null;//rigid body of grab anchor, required for spring attachments.

    private bool error = false;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("PlayerController is provided with null player object!");
            error = true;
            return;
        }

        playerCam = player.GetComponentInChildren<Camera>();

        if (playerCam == null)
        {
            Debug.LogError("PlayerController can not find camera on provided player object!");
            error = true;
            return;
        }

        grabAnchor = new GameObject("Grab Anchor");
        grabAnchor.transform.parent = playerCam.transform;
        grabAnchorBody = grabAnchor.AddComponent<Rigidbody>();
        grabAnchorBody.useGravity = false;
        grabAnchorBody.isKinematic = true;
        grabAnchorBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Update()
    {
        if (error) return;

        if(Input.GetMouseButton(0))//left mouse button is currently down
        {
            if(currentPickedObject == null)
            {
                //make ray from camera through crosshair and detect object
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(new Ray(playerCam.transform.position, playerCam.transform.forward), out hitInfo, 1000.0F))
                {
                    if(!hitInfo.collider.isTrigger && hitInfo.collider.GetComponent<Rigidbody>() != null && hitInfo.collider.gameObject != player)
                    {
                        //pick the object if it has a rigidbody and is not the player.
                        currentPickedObject = hitInfo.collider.gameObject;
                        doStuffWithPickedObject(hitInfo.point);
                    }
                }
            }
        }
        else if(currentPickedObject != null)
        {
            //un-pick the object
            onObjectUnPick();
            currentPickedObject = null;
        }
    }


    private void doStuffWithPickedObject(Vector3 grabPos)
    {
        grabAnchor.transform.position = grabPos;
        grabSpring = currentPickedObject.AddComponent<SpringJoint>();
        grabSpring.autoConfigureConnectedAnchor = false;
        grabSpring.connectedBody = grabAnchor.GetComponent<Rigidbody>();
        grabSpring.connectedAnchor = new Vector3(0.0001F, 0, 0);
        grabSpring.anchor = new Vector3(0.0001F, 0, 0);
        grabSpring.damper = 2.6F;
        grabSpring.spring = 10.0F;
        grabSpring.massScale = 100.0F;
    }


    private void onObjectUnPick()
    {
        SpringJoint[] pickedObjectSprings = currentPickedObject.GetComponents<SpringJoint>();
        if(pickedObjectSprings != null)
        {
            foreach(SpringJoint sj in pickedObjectSprings)
            {
                if(sj.connectedBody.gameObject == grabAnchor)
                {
                    Destroy(sj);
                    return;
                }
            }
        }
    }


}
