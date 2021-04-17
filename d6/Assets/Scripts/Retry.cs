using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public void Reload()
    {
        SceneManager.LoadScene("Game");
    }
}
