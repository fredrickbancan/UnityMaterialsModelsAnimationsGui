using UnityEngine;

/// <summary>
/// Script which can be added to a trigger, provided a platfrom object, and simulates a simple elevator.
/// </summary>
public class ElevatorTrigger : MonoBehaviour
{
    /// <summary>
    /// Reference to gameobject which acts as elevator platorm and is moved up/down
    /// </summary>
    [SerializeField]
    private GameObject elevatorPlatform = null;

    /// <summary>
    /// Height in y units at which the elevator platform will be at when the elevator is at "floor 0"
    /// </summary>
    [SerializeField]
    private float groundFloorHeight = 0.5F;

    /// <summary>
    /// Height in y units at which the elevator platform will be at when the elevator is at "top floor"
    /// </summary>
    [SerializeField]
    private float topFloorHeight = 15.0F;

    /// <summary>
    /// number of seconds for the elevator to wait when object enters or exits the trigger before moving
    /// </summary>
    [SerializeField]
    private float enterExitTimerSeconds = 1.0F;
    
    /// <summary>
    /// units per second at which the elevator will move
    /// </summary>
    [SerializeField]
    private float moveSpeed = 1.0F;

    private bool isProgressing = false;//true if elevator is moving towards top floor, else elevator is moving towards bottom floor.

    private bool hasPassenger = false;//true if an object is in the trigger

    private float touchTimer = 0.0F;//how long an object has been in the trigger

    private CharacterController playerPassenger = null;//used for moving the player manually to avoid physics issues

    private bool error;

    void Start()
    {
        if (!elevatorPlatform)
        {
            Debug.LogError("Elevator script provided with null elevator platform object!");
            error = true;
        }
    }

    void Update()
    {
        if (error) return;

        if(isProgressing)//move towards top floor
        {
            if (topFloorHeight > groundFloorHeight)//incase somebody wants to make an elevator that works downwards instead
            {
                if (elevatorPlatform.transform.position.y < topFloorHeight)
                {
                    elevatorPlatform.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);

                    if (playerPassenger != null)//if the passenger is a player
                    {
                        playerPassenger.Move(Vector3.up * moveSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                if (elevatorPlatform.transform.position.y > topFloorHeight)
                    elevatorPlatform.transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime, Space.World);

                if (playerPassenger != null)//if the passenger is a player
                {
                    playerPassenger.Move(-Vector3.up * moveSpeed * Time.deltaTime);
                }
            }
        }
        else//move towards bottom floor
        {
            if (topFloorHeight > groundFloorHeight)//incase somebody wants to make an elevator that works downwards instead
            {
                if (elevatorPlatform.transform.position.y > groundFloorHeight)
                    elevatorPlatform.transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime, Space.World);

                if (playerPassenger != null)//if the passenger is a player
                {
                    playerPassenger.Move(-Vector3.up * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (elevatorPlatform.transform.position.y < groundFloorHeight)
                    elevatorPlatform.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);

                if (playerPassenger != null)//if the passenger is a player
                {
                    playerPassenger.Move(Vector3.up * moveSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (hasPassenger)//as long as there is passenger on elevator, continue to count
        {
            if (touchTimer >= enterExitTimerSeconds)
            {
                isProgressing = true;
            }
            else
            {
                touchTimer += Time.deltaTime;
            }

            hasPassenger = false;//reset to false, if there is still a passenger detected in the next oncollisionstay() then it will be true again.
        }
        else
        {
            isProgressing = false;
            touchTimer = 0.0F;
        }
    }

    /// <summary>
    /// if the player enters this trigger, get their character controller for translation when elevator moves.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerPassenger = other.gameObject.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerStay()
    {
        hasPassenger = true;
    }

    /// <summary>
    /// if the player leaves then reset the playerPassenger var to null so we stop translating the player.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerPassenger = null;
        }
    }
}
