using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GameClient gameClient;

    void Awake()
    {
        DontDestroyOnLoad(this);
        this.gameClient = new GameClient(this);
        Login();
    }

    void OnDestroy()
    {
        Debug.Log("Shutting down");
        this.gameClient.Disconnect();
    }

    void Login()
    {
        // TODO: Get username and password from input fields
        this.gameClient.Login("test", "12345", LoginSuccess, LoginError);
    }

    void Logout()
    {
        // TODO: Load login scene
        this.gameClient.Disconnect();
    }

    void LoginSuccess()
    {
        Debug.Log("Login success! Welcome back, " + this.gameClient.GetUser().GetId());
        // TODO: Load lobby scene
    }

    void LoginError()
    {
        Debug.Log("Login error");
        // TODO: Display error
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(0);
    }
}