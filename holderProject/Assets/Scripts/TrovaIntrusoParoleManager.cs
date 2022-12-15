using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class TrovaIntrusoParoleManager : MonoBehaviour
{
	private static TrovaIntrusoParoleManager instance;
	public static TrovaIntrusoParoleManager Instance { get { return instance; } }
	[SerializeField]
	GameObject GridLayout;
	[SerializeField]
	GameObject TrovaIntrusoParolePrefab;
	[SerializeField]
	GameObject endgame;
	List<Dictionary<string, bool>> dictOfWords;
	public static int amountOfRows;
	public static int amountOfColumns = 4;
	public static int nSchede = 50;
	int CorrectAnswers;
	int answersCount;
	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		amountOfRows = Random.Range(2, 4);
		CorrectAnswers = 0;
		answersCount = 0;
		dictOfWords = CreateList(CSVData.Instance.holderWordList.wordobject);
		var copyOfDictWords = dictOfWords.ToList();
		for(int i = 0; i < amountOfRows; i++){
			int rnd = Random.Range(0, copyOfDictWords.Count);
			System.Random rng = new System.Random();
			copyOfDictWords[rnd] = copyOfDictWords[rnd].OrderBy(a => rng.Next()).ToDictionary(entry => entry.Key,
											   entry => entry.Value);
			for(int j = 0; j < copyOfDictWords[rnd].Count; j++){
				GameObject Wordtmp = Instantiate(TrovaIntrusoParolePrefab, GridLayout.GetComponent<RectTransform>().transform);
				Wordtmp.GetComponentInChildren<TMP_Text>().text = copyOfDictWords[rnd].ElementAt(j).Key;
				Wordtmp.GetComponentInChildren<TrovaIntrusoParoleScript>().outOfPlace = copyOfDictWords[rnd].ElementAt(j).Value;
			}
			copyOfDictWords.RemoveAt(rnd);
		}
		
		nSchede -= 1;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void Awake()
	{
		instance = this;
	}

	public void GetClick(bool outOfPlace){
		answersCount++;
		if(outOfPlace)
			CorrectAnswers++;
		if(answersCount == amountOfRows)
			CheckResult();
	}
	public void CheckResult(){
		print(CorrectAnswers + " di " + amountOfRows + " risposte sono corrette");
		if (nSchede > 0){
			NewScheda();
		} else {
			nSchede = 3;
			endgame.GetComponent<EndGame>().startEndGame();
			print("FINITA SESSIONE");
		}
	}

	private void NewScheda(){

		for (var i = GridLayout.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(GridLayout.transform.GetChild(i).gameObject);
		}
		amountOfRows = Random.Range(2, 4);
		CorrectAnswers = 0;
		answersCount = 0;
		dictOfWords = CreateList(CSVData.Instance.holderWordList.wordobject);
		var copyOfDictWords = dictOfWords.ToList();
		for(int i = 0; i < amountOfRows; i++){
			int rnd = Random.Range(0, copyOfDictWords.Count);
			System.Random rng = new System.Random();
			copyOfDictWords[rnd] = copyOfDictWords[rnd].OrderBy(a => rng.Next()).ToDictionary(entry => entry.Key,
											   entry => entry.Value);
			for(int j = 0; j < copyOfDictWords[rnd].Count; j++){
				GameObject Wordtmp = Instantiate(TrovaIntrusoParolePrefab, GridLayout.GetComponent<RectTransform>().transform);
				Wordtmp.GetComponentInChildren<TMP_Text>().text = copyOfDictWords[rnd].ElementAt(j).Key;
				Wordtmp.GetComponentInChildren<TrovaIntrusoParoleScript>().outOfPlace = copyOfDictWords[rnd].ElementAt(j).Value;
			}
			copyOfDictWords.RemoveAt(rnd);
		}
		
		nSchede -= 1;
	}

	public List<Dictionary<string,bool>> CreateList(CSVData.WordObject[] Data) {
		List<Dictionary<string,bool>> newList = new List<Dictionary<string, bool>>();
		CSVData.WordObject[] CopyData = Data;
		CSVData.WordObject[] filteredData = CopyData;
		string categoryToUse;
		List<string> categoriesUsed = new List<string>();

		for (int i = 0; i < amountOfRows; i++)
		{
			CSVData.WordObject random1 = new CSVData.WordObject();
			do
			{
				random1 = CopyData.ElementAt(Random.Range(0,CopyData.Count()));
				List<string> categories = new List<string>();
				categories.Add(random1.Cat_1);
				if (random1.Cat_2 != ""){
					categories.Add(random1.Cat_2);
				}
				if (random1.Cat_3 != ""){
					categories.Add(random1.Cat_3);
				}
				
				categoryToUse = categories.ElementAt(Random.Range(0,categories.Count));
				
				filteredData = CopyData.Where(c => 
					c != random1 && ( 
					c.Cat_1 == categoryToUse || c.Cat_2 == categoryToUse || c.Cat_3 == categoryToUse
					)
				).ToArray();
				
			} while (filteredData.Count() < amountOfColumns-2 || categoriesUsed.Contains(categoryToUse));

			List<string> sameCategoryWords = new List<string>();
			sameCategoryWords.Add(random1.Word);
			for (int j = 0; j < amountOfColumns-2; j++)
			{
				CSVData.WordObject anotherRandom = filteredData.ElementAt(Random.Range(0,filteredData.Count()));
				sameCategoryWords.Add(anotherRandom.Word);
				filteredData = filteredData.Where(c => c != anotherRandom).ToArray();
			}
			
			CopyData = CopyData.Where(c=> !sameCategoryWords.Contains(c.Word)).ToArray();

			Dictionary<string,bool> row = new Dictionary<string, bool>();
			sameCategoryWords.ForEach(delegate(string sameCategoryWord){
				row.Add(sameCategoryWord, false);
			});
			
			CSVData.WordObject[] likelyIntruder = CopyData.Where(c=> 
			(c.Cat_1 != categoryToUse) && (c.Cat_2 != categoryToUse) && (c.Cat_3 != categoryToUse)).ToArray();
			CSVData.WordObject intrude = likelyIntruder.ElementAt(Random.Range(0,likelyIntruder.Count()));
			row.Add(
				intrude.Word,
				true
			);
			CopyData = CopyData.Where(c=> c != intrude).ToArray();
			newList.Add(row);
		}


		return newList;
	}
}
