using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveVistoPrefabScript : MonoBehaviour
{
	public int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void GetClick(){
		DoveVistoManager.Instance.GetClick(index);
	}
}
