using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private GameObject playerSpawn = null;
    private GameObject[] ragdollSpawns = null;
    [SerializeField]
    private GameObject playerPrefab = null;

    [SerializeField]
    private GameObject ragdollPrefab = null;

    private GameObject pausePanel = null;

    private bool paused = false;

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
            Debug.Log("GameManager Awake()");
        }
    }

    public void init(GameObject pausePanelRef)
    {
        if(this != instance)
        {
            Debug.LogError("Singleton inconsistency detected (GameManager.init())");
        }

        Debug.Log("GameManager Initializing");

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

    public static void pauseUnpauseGame()
    {
        instance.paused = !instance.paused;
        if(instance.paused)
        {
            Cursor.lockState = CursorLockMode.None;
            instance.pausePanel.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
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
        UnityEditor.EditorApplication.isPlaying = false;
     #else
        Application.Quit();
     #endif
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