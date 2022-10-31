using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiumeParoleBtnScript : MonoBehaviour
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
		FiumeParoleManager.Instance.GetClick(correct);
	}
}
