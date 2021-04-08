using UnityEngine;

/// <summary>
/// Script which can be added to a player with a camera. allows the user to click and drag on objects to move them around with a spring joint.
/// </summary>
public class ObjectPicker : MonoBehaviour
{
    /// <summary>
    /// Reference to player object
    /// </summary>
    [SerializeField]
    private GameObject player = null;

    private GameObject currentPickedObject = null;//reference to the gameobject being picked

    private GameObject grabAnchor = null;//empty gameobject of which the springjoint constraint is attached to, to simulate gravbbing in a dynamic way

    private Camera playerCam = null;//reference to the players camera object

    private SpringJoint grabSpring = null;//spring connected to grabbed object

    private Rigidbody grabAnchorBody = null;//rigid body of grab anchor, required for spring attachments.

    private bool error = false;

    /// <summary>
    /// Get all gameobjects and configure
    /// </summary>
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

    /// <summary>
    /// Detect input and manipulate scene
    /// </summary>
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

    /// <summary>
    /// Attaches and configures a springjoint to the rigidbody of the picked object so it can be dragged around
    /// </summary>
    /// <param name="grabPos">3D position at which the springjoint will be attached</param>
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

    /// <summary>
    /// Called when the mouse is released while an object is grabbed. Removes the correct SpringJoint constraint from the target
    /// </summary>
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
