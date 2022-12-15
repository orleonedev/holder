using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class FiumeParoleManager : MonoBehaviour
{
	private static FiumeParoleManager instance;
	public static FiumeParoleManager Instance { get { return instance; } }
	[SerializeField]
	GameObject Description;
	[SerializeField]
	GameObject CriteriaOfExercise;
	[SerializeField]
	GameObject DescriptionPart2;
	[SerializeField]
	GameObject HorizontalLayoutButtons;
	[SerializeField]
	GameObject FiumeParoleButtonPrefab;
	[SerializeField]
	GameObject FiumeParoleWordPrefab;
	[SerializeField]
	GameObject endgame;
	List<string> possibleWords;
	List<string> possibleWordsCopy;
	List<string> wordsToShow;
	List<string> correctWords;
	List<GameObject> listOfButtons;
	int answersGiven = 0;
	int correctAnswersGiven = 0;
	int amountofCorrectWords;
	public static int sequenceLenghtMax = 7;
	public static int numberOfSequence = 4;
	int iteration = 0;

	// Start is called before the first frame update
	void Start()
	{
		possibleWords = CreateList(CSVData.Instance.holderWordList.wordobject);
		wordsToShow = new List<string>();
		correctWords = new List<string>();
		listOfButtons = new List<GameObject>();
		possibleWordsCopy = new List<string>(possibleWords);
		amountofCorrectWords = Random.Range(2, 4);
		CriteriaOfExercise.GetComponent<TMP_Text>().text = amountofCorrectWords.ToString();
		int amountOfWordsToPick = Random.Range(amountofCorrectWords + 1, sequenceLenghtMax);
		for (int i = 0; i < amountOfWordsToPick; i++)
		{
			int rnd = Random.Range(0, possibleWordsCopy.Count);
			wordsToShow.Add(possibleWordsCopy[rnd]);
			if (i >= amountOfWordsToPick - amountofCorrectWords)
			{
				correctWords.Add(possibleWordsCopy[rnd]);
			}
			possibleWordsCopy.RemoveAt(rnd);
		}
		iteration++;
		//DebugPrint(wordsToShow);
		DebugPrint(correctWords);
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
		var wordsToShowCopy = new List<string>(wordsToShow);
		foreach (string word in correctWords)
		{
			wordsToShowCopy.Remove(word);
		}
		var listOfBtnsWords = new List<string>(correctWords);
		for (int i = 0; i < wordsToShow.Count - listOfBtnsWords.Count; i++)
		{
			int rnd = Random.Range(0, wordsToShowCopy.Count);
			listOfBtnsWords.Add(wordsToShowCopy[rnd]);
			wordsToShowCopy.RemoveAt(rnd);
		}
		System.Random rng = new System.Random();
		var shuffledlistOfBtnsWords = listOfBtnsWords.OrderBy(a => rng.Next()).ToList();
		foreach (string word in shuffledlistOfBtnsWords)
		{
			var tmp = Instantiate(FiumeParoleButtonPrefab, HorizontalLayoutButtons.transform);
			tmp.GetComponentInChildren<TMP_Text>().text = word;
			listOfButtons.Add(tmp);
			if(correctWords.Contains(word))
				tmp.GetComponent<FiumeParoleBtnScript>().correct = true;
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
		while (i < wordsToShow.Count)
		{
			var tmp = Instantiate(FiumeParoleWordPrefab, this.gameObject.transform);
			tmp.GetComponent<TMP_Text>().text = wordsToShow[i];
			tmp.transform.localPosition = new Vector3(-1200f, 0f, 0f);
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
			finalPos = new Vector3(1200f, 0f, 0f);
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
		if(answersGiven == amountofCorrectWords){
			print(correctAnswersGiven + " di " + amountofCorrectWords + " risposte sono corrette");
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
		wordsToShow = new List<string>();
		correctWords = new List<string>();
		listOfButtons = new List<GameObject>();
		amountofCorrectWords = Random.Range(2, 4);
		CriteriaOfExercise.GetComponent<TMP_Text>().text = amountofCorrectWords.ToString();
		int amountOfWordsToPick = Random.Range(amountofCorrectWords + 1, sequenceLenghtMax);
		for (int i = 0; i < amountOfWordsToPick; i++)
		{
			int rnd = Random.Range(0, possibleWordsCopy.Count);
			wordsToShow.Add(possibleWordsCopy[rnd]);
			if (i >= amountOfWordsToPick - amountofCorrectWords)
			{
				correctWords.Add(possibleWordsCopy[rnd]);
			}
			possibleWordsCopy.RemoveAt(rnd);
		}
		iteration++;
		//DebugPrint(wordsToShow);
		DebugPrint(correctWords);
		SetupButtons();
		StartGame();
	}
}
