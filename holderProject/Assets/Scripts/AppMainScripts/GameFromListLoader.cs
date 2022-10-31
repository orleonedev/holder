using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFromListLoader : MonoBehaviour
{
    public void loadGame() {
        SceneManager.LoadScene(gameObject.name);
    }
}
