using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionScreen : MonoBehaviour
{
    public void SelectCharacterAndStart(CharacterData chosenCharacter)
    {
        GameManager.SelectedCharacter = chosenCharacter;

        SceneManager.LoadScene("Game"); 
    }
}
