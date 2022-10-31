using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelezionaBersaglioScript : MonoBehaviour
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
		SelezionaBersaglioManager.Instance.GetClick(correct);
		if (correct)
			SelezionaBersaglioManager.Instance.imagesShown++;
		SelezionaBersaglioManager.Instance.StartGame();
		Destroy(this.gameObject);
	}
}
