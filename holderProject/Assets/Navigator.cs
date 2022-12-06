using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    private static Navigator instance;
	public static Navigator Instance {get { return instance; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void Awake() {
		instance = this;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadGame(string gameName){
        switch (gameName) {
            case "LaParolaGiusta":
            SceneManager.LoadScene(gameName+"Scene");
            break;
            case "AccoppiaLeParole":
            SceneManager.LoadScene(gameName+"Scene");
            break;
            case "4Immagini1Categoria":
            SceneManager.LoadScene(gameName+"Scene");
            break;
            case "AccoppiaLeImmagini":
            SceneManager.LoadScene(gameName+"Scene");
            break;
            default:
            print("Gioco non trovato");
            break;
        }
    }

    public void exitGame(){
        SceneManager.LoadScene(0);
    }

}
