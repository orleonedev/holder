using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguiFlussoBtnScript : MonoBehaviour
{
	public bool correct = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void GetClick(){
		this.GetComponent<UnityEngine.UI.Button>().interactable = false;
		SeguiFlussoManager.Instance.GetClick(correct);
	}
}
