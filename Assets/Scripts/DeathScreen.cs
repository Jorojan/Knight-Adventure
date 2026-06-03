using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathScreen : MonoBehaviour
{
    private UIDocument _uiDocument;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _uiDocument.enabled = false;
    }

    private void Start()
    {
        Player.Instantce.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath(object sender, System.EventArgs e)
    {
        _uiDocument.enabled = true;

        var root = _uiDocument.rootVisualElement;
        root.Q<Button>("RestartButton").clicked += Restart;
        root.Q<Button>("MenuButton").clicked += GoToMenu;
    }

    private void Restart()
    {
        SceneManager.LoadScene("SimpleScene");
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDisable()
    {
        if (Player.Instantce != null)
            Player.Instantce.OnPlayerDeath -= OnPlayerDeath;
    }
}
