using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CercaElementoScript : MonoBehaviour
{
	public int amountPressed = 0;
	public int destroyDelay;
	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(WaitForSeconds());
	}

	// Update is called once per frame
	void Update()
	{

	}
	public void GetClick()
	{
		/*SelezionaBersaglioManager.Instance.GetClick(correct);
		if (correct)
			SelezionaBersaglioManager.Instance.imagesShown++;
		SelezionaBersaglioManager.Instance.StartGame();
		Destroy(this.gameObject);*/
		if (amountPressed == 0)
			CercaElementoManager.Instance.correctAnswers++;
		else if (amountPressed == 1)
		{
			CercaElementoManager.Instance.correctAnswers--;
		}
		amountPressed++;
		CercaElementoManager.Instance.GetClick();
	}
	IEnumerator WaitForSeconds()
	{
		yield return new WaitForSeconds(destroyDelay);
		Destroy(this.gameObject);
	}
}
