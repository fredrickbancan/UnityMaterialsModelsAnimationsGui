using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private GameObject playerSpawn = null;
    private GameObject[] ragdollSpawns = null;
    
    [SerializeField]
    private GameObject playerPrefab = null;

    [SerializeField]
    private GameObject ragdollPrefab = null;

    private bool error = false;

    /// <summary>
    /// creating singleton instance
    /// </summary>
    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }

    /// <summary>
    /// get all requried objects and spawn ragdolls
    /// </summary>
    void Start()
    {
        playerSpawn = GameObject.Find("PlayerSpawn");
        if(!playerSpawn)
        {
            Debug.LogError("GameManager could not find player spawn point object!");
            error = true;
            return;
        }

        spawnPlayer();

        ragdollSpawns = GameObject.FindGameObjectsWithTag("Ragdoll Spawn");

        if(ragdollSpawns == null || ragdollSpawns.Length == 0)
        {
            Debug.LogError("GameManager could not find any ragdoll spawn points!");
            error = true;
            return;
        }

        spawnRagdolls();
    }

    public void spawnPlayer()
    {
        if (!playerPrefab)
        {
            Debug.LogError("GameManager could not spawn player! object is provided with null player prefab!");
            error = true;
            return;
        }
        Instantiate(playerPrefab, playerSpawn.transform.position, playerSpawn.transform.rotation);
    }

    public void spawnRagdolls()
    {
        if (!ragdollPrefab)
        {
            Debug.LogError("GameManager could not spawn ragdolls! object is provided with null ragdoll prefab!");
            error = true;
            return;
        }

        foreach (GameObject g in ragdollSpawns)
        {
            Instantiate(ragdollPrefab, g.transform.position, g.transform.rotation);
        }
    }

    void Update()
    {
        if (error) return;
    }
}