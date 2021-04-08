using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Static singleton gamemanager object. Usefull for preserving information and states between scene loading.
/// A non-static helper script may be needed to provide this script with correct information
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static singleton instance of the game manager
    /// </summary>
    public static GameManager instance = null;

    /// <summary>
    /// Empty gameobject where the player will spawn
    /// </summary>
    private GameObject playerSpawn = null;

    /// <summary>
    /// Array of empty gameobjects where ragdolls should be spawned
    /// </summary>
    private GameObject[] ragdollSpawns = null;

    /// <summary>
    /// Prefab of player object for being spawned in level
    /// </summary>
    [SerializeField]
    private GameObject playerPrefab = null;

    /// <summary>
    /// Prefab of ragdoll object for being spawned in level
    /// </summary>
    [SerializeField]
    private GameObject ragdollPrefab = null;

    private GameObject pausePanel = null;//reference to GUI panel which is  shown/hidden depending on if the game is paused

    private bool paused = false;//true if game is paused

    private bool error = false;

    /// <summary>
    /// creating singleton instance
    /// </summary>
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
            Debug.Log("GameManager Awake()");//debugging purposes
        }
    }

    /// <summary>
    /// Initialize or re-initialize game objects and states
    /// </summary>
    /// <param name="pausePanelRef">reference to GUI panel which is  shown/hidden depending on if the game is paused</param>
    public void init(GameObject pausePanelRef)
    {
        if(this != instance)
        {
            Debug.LogError("Singleton inconsistency detected (GameManager.init())");
        }

        Debug.Log("GameManager Initializing");//debugging purposes

        playerSpawn = GameObject.Find("PlayerSpawn");

        if (!playerSpawn)
        {
            Debug.LogError("GameManager could not find player spawn point object!");
            error = true;
            return;
        }

        spawnPlayer();

        ragdollSpawns = GameObject.FindGameObjectsWithTag("Ragdoll Spawn");

        if (ragdollSpawns == null || ragdollSpawns.Length == 0)
        {
            Debug.LogError("GameManager could not find any ragdoll spawn points!");
            error = true;
            return;
        }

        spawnRagdolls();

        pausePanel = pausePanelRef;
        if (pausePanel == null)
        {
            Debug.LogError("GameManager is provided with null pausePanel object!");
            error = true;
            return;
        }

        pauseUnpauseGame();
    }

    /// <summary>
    /// Spawn player at the player spawn location and rotation
    /// </summary>
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

    public static bool isPaused()
    {
        return instance.paused;
    }

    /// <summary>
    /// Toggles the game being paused
    /// </summary>
    public static void pauseUnpauseGame()
    {
        instance.paused = !instance.paused;
        if(instance.paused)
        {
            //show and unlock curser, and set the pause panel to active
            Cursor.lockState = CursorLockMode.None;
            instance.pausePanel.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
            //hide and lock curser, and set the pause panel to inactive
            Cursor.lockState = CursorLockMode.Confined;
            instance.pausePanel.SetActive(false);
            Cursor.visible = false;
        }
    }

    public void respawnPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitGame()
    { 
     #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//If this app is being run in the unity editor then stop the playing, otherwise Application.Quit() can be called
     #else
        Application.Quit();
     #endif
    }

    /// <summary>
    /// Spawns instances of ragdoll prefab at each ragdoll spawn location and rotation
    /// </summary>
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