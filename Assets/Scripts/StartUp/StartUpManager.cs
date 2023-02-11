using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))    //TODO
        {
            Debug.Log($"{GetType().Name}.cs > Space key pressed: opening main menu");

            SceneManager.LoadScene(1);
        }
    }
}
