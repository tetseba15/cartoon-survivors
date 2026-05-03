using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject characterSelector;

    private void Start()
    {
        #if UNITY_WEBGL
        exitButton.SetActive(false);
        #endif
    }


    public void StartGame()
    {
        characterSelector.SetActive(true);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
        Application.Quit();
    #endif
    }
}
