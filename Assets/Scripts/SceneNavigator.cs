using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
