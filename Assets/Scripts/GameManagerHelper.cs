using UnityEngine;

public class GameManagerHelper : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        if (error) return;
    }
}
