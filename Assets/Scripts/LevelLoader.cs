using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public KeyCode keyToLoad = KeyCode.Return;
    public SceneAsset sceneToLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToLoad))
        {
            Debug.Log("Loading Scene " + sceneToLoad.name);
            SceneManager.LoadScene(sceneToLoad.name, LoadSceneMode.Single);
        }
    }
}
