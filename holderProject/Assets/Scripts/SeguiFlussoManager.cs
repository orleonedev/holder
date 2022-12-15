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
	List<string> possibleImagesCopy;
	List<string> imagesToShow;
	List<string> correctImages;
	List<GameObject> listOfButtons;
	[SerializeField]
	GameObject endgame;
	int answersGiven = 0;
	int correctAnswersGiven = 0;
	int amountofCorrectImages;
	public static int sequenceLenghtMax = 7;
	public static int numberOfSequence = 4;
	int iteration = 0;

	// Start is called before the first frame update
	void Start()
	{
		possibleImages = CreateList(CSVData.Instance.FilteredWithImages());
		imagesToShow = new List<string>();
		correctImages = new List<string>();
		listOfButtons = new List<GameObject>();
		possibleImagesCopy = new List<string>(possibleImages);
		amountofCorrectImages = Random.Range(2, 4);
		CriteriaOfExercise.GetComponent<TMP_Text>().text = amountofCorrectImages.ToString();
		int amountOfImagesToPick = Random.Range(amountofCorrectImages + 1, sequenceLenghtMax);
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
		iteration++;
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
			var sprite = (
            #if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+image.ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+image.ToLower()))
            #endif
       );
			tmp.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = sprite;
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
			var sprite = (
            #if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+imagesToShow[i].ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+imagesToShow[i].ToLower()))
            #endif
       		);
			tmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = sprite;
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
		if(answersGiven == amountofCorrectImages){
			print(correctAnswersGiven + " di " + amountofCorrectImages + " risposte sono corrette");
			if (iteration<numberOfSequence){
				NewSequence();
			} else {
				iteration = 0;
				endgame.GetComponent<EndGame>().startEndGame();
				print("FINITA SESSIONE");
			}
			
		}
			
	}

	public List<string> CreateList(CSVData.WordObject[] Data){
		List<string> newList = new List<string>();
		CSVData.WordObject[] CopyData = Data;

		for (int i = 0; i < sequenceLenghtMax*numberOfSequence; i++)
		{
			CSVData.WordObject random = CopyData.ElementAt(Random.Range(0,CopyData.Count()));
			newList.Add(random.Word);
			CopyData = CopyData.Where(c=> c!= random).ToArray();
		}

		return newList;
	}

	void ClearEverything(){
		for (var i = HorizontalLayoutButtons.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(HorizontalLayoutButtons.transform.GetChild(i).gameObject);
		}

	}

	void NewSequence(){
		ClearEverything();
		answersGiven = 0;
		correctAnswersGiven = 0;
		imagesToShow = new List<string>();
		correctImages = new List<string>();
		listOfButtons = new List<GameObject>();
		amountofCorrectImages = Random.Range(2, 4);
		CriteriaOfExercise.GetComponent<TMP_Text>().text = amountofCorrectImages.ToString();
		int amountOfImagesToPick = Random.Range(amountofCorrectImages + 1, sequenceLenghtMax);
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
		iteration++;
		DebugPrint(correctImages);
		SetupButtons();
		StartGame();
	}
}
