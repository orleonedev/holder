using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SeguiFlussoManager : MonoBehaviour
{
	private static SeguiFlussoManager instance;
	public static SeguiFlussoManager Instance { get { return instance; } }
	[SerializeField]
	GameObject Description;
	[SerializeField]
	GameObject CriteriaOfExercise;
	[SerializeField]
	GameObject DescriptionPart2;
	[SerializeField]
	GameObject HorizontalLayoutButtons;
	[SerializeField]
	GameObject SeguiFlussoButtonPrefab;
	[SerializeField]
	GameObject SeguiFlussoImagePrefab;
	List<string> possibleImages;
	List<string> imagesToShow;
	List<string> correctImages;
	List<GameObject> listOfButtons;
	int answersGiven = 0;
	int correctAnswersGiven = 0;
	int amountofCorrectImages;

	// Start is called before the first frame update
	void Start()
	{
		possibleImages = new List<string>();
		imagesToShow = new List<string>();
		correctImages = new List<string>();
		listOfButtons = new List<GameObject>();
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
		var possibleImagesCopy = new List<string>(possibleImages);
		amountofCorrectImages = Random.Range(2, 4);
		CriteriaOfExercise.GetComponent<TMP_Text>().text = amountofCorrectImages.ToString();
		int amountOfImagesToPick = Random.Range(amountofCorrectImages + 2, amountofCorrectImages + 5);
		for (int i = 0; i < amountOfImagesToPick; i++)
		{
			int rnd = Random.Range(0, possibleImagesCopy.Count);
			imagesToShow.Add(possibleImagesCopy[rnd]);
			if (i >= amountOfImagesToPick - amountofCorrectImages)
			{
				correctImages.Add(possibleImagesCopy[rnd]);
			}
			possibleImagesCopy.RemoveAt(rnd);
		}
		//DebugPrint(imagesToShow);
		DebugPrint(correctImages);
		SetupButtons();
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
	void DebugPrint(List<string> list)
	{
		foreach (string item in list)
		{
			print(item);
		}
	}
	void SetupButtons()
	{
		var imagesToShowCopy = new List<string>(imagesToShow);
		foreach (string image in correctImages)
		{
			imagesToShowCopy.Remove(image);
		}
		var listOfBtnsImages = new List<string>(correctImages);
		for (int i = 0; i < imagesToShow.Count - listOfBtnsImages.Count; i++)
		{
			int rnd = Random.Range(0, imagesToShowCopy.Count);
			listOfBtnsImages.Add(imagesToShowCopy[rnd]);
			imagesToShowCopy.RemoveAt(rnd);
		}
		System.Random rng = new System.Random();
		var shuffledlistOfBtnsImages = listOfBtnsImages.OrderBy(a => rng.Next()).ToList();
		foreach (string image in shuffledlistOfBtnsImages)
		{
			var tmp = Instantiate(SeguiFlussoButtonPrefab, HorizontalLayoutButtons.transform);
			tmp.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + image);
			listOfButtons.Add(tmp);
			if(correctImages.Contains(image))
				tmp.GetComponent<SeguiFlussoBtnScript>().correct = true;
			tmp.SetActive(false);
		}
	}
	void StartGame()
	{
		StartCoroutine(SmoothLerp(2f));
	}
	private IEnumerator SmoothLerp(float time)
	{
		int i = 0;
		while (i < imagesToShow.Count)
		{
			var tmp = Instantiate(SeguiFlussoImagePrefab, this.gameObject.transform);
			tmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imagesToShow[i]);
			tmp.transform.localPosition = new Vector3(-1400f, 0f, 0f);
			Vector3 startingPos = tmp.transform.localPosition;
			Vector3 finalPos = new Vector3(0f, 0f, 0f);
			float elapsedTime = 0;

			while (elapsedTime < time)
			{
				tmp.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			startingPos = tmp.transform.localPosition;
			finalPos = new Vector3(1400f, 0f, 0f);
			elapsedTime = 0;

			while (elapsedTime < time)
			{
				tmp.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			Destroy(tmp);
			i++;
		}
		ShowButtons();
	}
	void ShowButtons(){
		foreach(GameObject button in listOfButtons){
			button.SetActive(true);
		}
	}
	public void GetClick(bool correct){
		if(correct)
			correctAnswersGiven++;
		answersGiven++;
		if(answersGiven == amountofCorrectImages)
			print(correctAnswersGiven + " di " + amountofCorrectImages + " risposte sono corrette");
	}
}
