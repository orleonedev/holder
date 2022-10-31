using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercorsiScript : MonoBehaviour
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
	public void GetClick()
	{
		PercorsiManager.Instance.GetClick(index);
		/*PescaParolaManager.Instance.GetClick(correct);
		if (correct)
			PescaParolaManager.Instance.wordsShown++;
		PescaParolaManager.Instance.StartGame();
		Destroy(this.gameObject);*/
	}
}
