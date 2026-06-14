using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (FindObjectOfType<UIManager>() == null)
        {
            GameObject go = new GameObject("UIManager");
            go.AddComponent<UIManager>();
            Object.DontDestroyOnLoad(go);
        }
    }
}
