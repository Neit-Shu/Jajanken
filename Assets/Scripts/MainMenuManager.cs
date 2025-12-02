using UnityEngine;                                             

public class MainMenuManager : MonoBehaviour                   
{
    public void StartGame()                                    
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
    }

    public void QuitGame()                                     
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();          
#endif
        Application.Quit();                                    
    }
}
