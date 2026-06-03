using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _playSceneName = "SimpleScene";

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("PlayButton").clicked += PlayGame;
        root.Q<Button>("QuitButton").clicked += QuitGame;

        root.schedule.Execute(() => root.Q("Content").AddToClassList("show")).StartingIn(100);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(_playSceneName);
    }

    private void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
