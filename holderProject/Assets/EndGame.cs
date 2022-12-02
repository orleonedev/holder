using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public bool isOn = false;
    public int timer = 60*3;


    // Start is called before the first frame update
    void Start()
    {
        isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn){
            timer -= 1; 
            if (timer < 0) {
            isOn = false;
            Navigator.Instance.exitGame();
        }
        }
        
    }

    public void startEndGame(){
        gameObject.SetActive(true);
        
    }
}
