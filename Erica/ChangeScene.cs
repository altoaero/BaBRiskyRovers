using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void MoveToScene(int sceneID) // the int from the build settings
    {
        SceneManager.LoadScene(sceneID); // accesses the scene manager to load the scene from build settings 
    }


}
