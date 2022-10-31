using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CercaElementoManager : MonoBehaviour
{
	private static CercaElementoManager instance;
	public static CercaElementoManager Instance { get { return instance; } }
	[SerializeField]
	GameObject spawnArea;
	[SerializeField]
	GameObject CercaElementoPrefab;
	List<string> possibleImages;
	public int imagesShown;
	public int correctAnswers;
	int rndImageIndexToShow;
	int amountOfElementToShow;
	int destroyObjectAfterTapDelay;
	// Start is called before the first frame update
	void Start()
	{
		possibleImages = new List<string>();
		possibleImages.Add("Image 1");
		possibleImages.Add("Image 2");
		possibleImages.Add("Image 3");
		possibleImages.Add("Image 4");
		possibleImages.Add("Image 5");
		possibleImages.Add("Image 6");
		possibleImages.Add("Image 7");
		possibleImages.Add("Image 8");
		possibleImages.Add("Image 9");
		possibleImages.Add("Image 10");
		amountOfElementToShow = 10;
		destroyObjectAfterTapDelay = 2;
		StartGame();
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void Awake()
	{
		instance = this;
	}
	public void StartGame()
	{
		GameObject tmp = null;
		rndImageIndexToShow = Random.Range(0, possibleImages.Count);
		StartCoroutine(WaitForSeconds(tmp));
	}
	public void GetClick()
	{
		print(correctAnswers + " di " + amountOfElementToShow + " sono corrette");
	}
	IEnumerator WaitForSeconds(GameObject obj)
	{
		for(int i = 0; i < amountOfElementToShow; i++){
			SpawnElement(obj);
			yield return new WaitForSeconds(2f);
		}
	}
	void SpawnElement(GameObject tmp){
		float rndX = Random.Range(-768.0f, 768.0f);
		float rndY = Random.Range(-600.0f, 600.0f);
		tmp = Instantiate(CercaElementoPrefab, spawnArea.transform);
		tmp.transform.localPosition = new Vector2(rndX, rndY);
		tmp.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + possibleImages[rndImageIndexToShow]);
		tmp.GetComponent<CercaElementoScript>().destroyDelay = destroyObjectAfterTapDelay;
	}
}
