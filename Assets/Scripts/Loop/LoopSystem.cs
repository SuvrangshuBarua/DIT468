using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the time spent in the loop, including the start and end flow
public class LoopSystem : MonoBehaviour
{
    public void Loop()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
