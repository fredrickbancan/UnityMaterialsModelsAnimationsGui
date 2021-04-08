using UnityEngine;

/// <summary>
/// Script for a gameobject which helps provide information and call functions in a singleton gameobject
/// Usefull for saving states and level information between scene loading.
/// </summary>
public class GameManagerHelper : MonoBehaviour
{
    /// <summary>
    /// Reference to the GUI panel which is shown/hidden depending on the game being paused
    /// </summary>
    [SerializeField]
    private GameObject pausePanel = null;

    private bool error = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.init(pausePanel);
    }

    public void respawnPlayer()
    {
        GameManager.instance.respawnPlayer();
    }

    public void quitGame()
    {
        GameManager.instance.quitGame();
    }

    public void pauseUnpause()
    {
        GameManager.pauseUnpauseGame();
    }

    void Update()
    {
        if (error) return;
        //provide static gamemanager with information here if needed
    }
}
