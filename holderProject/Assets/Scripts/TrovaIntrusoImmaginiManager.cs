using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class TrovaIntrusoImmaginiManager : MonoBehaviour
{
	private static TrovaIntrusoImmaginiManager instance;
	public static TrovaIntrusoImmaginiManager Instance { get { return instance; } }

	[SerializeField]
	GameObject GridLayout;
	[SerializeField]
	GameObject TrovaIntrusoImmaginiPrefab;
	List<Dictionary<string, bool>> dictOfWords;
	List<Dictionary<string,bool>> copyOfDictWords;
	
	int CorrectAnswers;
	int answersCount;

	public static int nSchede = 20;
	public static int nImages = 4;

	[SerializeField]
	GameObject endgame;
	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		
		CorrectAnswers = 0;
		answersCount = 0;
		dictOfWords = CreateDictionary(CSVData.Instance.FilteredWithImages());
		copyOfDictWords = dictOfWords.ToList();
		
		int rnd = Random.Range(0, copyOfDictWords.Count);
		System.Random rng = new System.Random();
		copyOfDictWords[rnd] = copyOfDictWords[rnd].OrderBy(a => rng.Next()).ToDictionary(entry => entry.Key,
											   entry => entry.Value);
		for(int j = 0; j < copyOfDictWords[rnd].Count; j++){
			GameObject Wordtmp = Instantiate(TrovaIntrusoImmaginiPrefab, GridLayout.GetComponent<RectTransform>().transform);
			Wordtmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>( "Images/abatjour-guidare/" + copyOfDictWords[rnd].ElementAt(j).Key );
			Wordtmp.GetComponentInChildren<TrovaIntrusoImmaginiScript>().outOfPlace = copyOfDictWords[rnd].ElementAt(j).Value;
			}
		copyOfDictWords.RemoveAt(rnd);
		
		print(dictOfWords.Count);
		print(copyOfDictWords.Count);
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
		
		CheckResult();
		if (answersCount == dictOfWords.Count){
			endgame.GetComponent<EndGame>().startEndGame();
			print("FINITA SESSIONE");
		} else {
			newScheda();
		}

	}
	  public void CheckResult(){
	  	print(CorrectAnswers + " di " + answersCount + " risposte sono corrette");
	  }

	  public void newScheda(){

		for (var i = GridLayout.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(GridLayout.transform.GetChild(i).gameObject);
		}

		int rnd = Random.Range(0, copyOfDictWords.Count);
		System.Random rng = new System.Random();
		copyOfDictWords[rnd] = copyOfDictWords[rnd].OrderBy(a => rng.Next()).ToDictionary(entry => entry.Key,
											   entry => entry.Value);
		for(int j = 0; j < copyOfDictWords[rnd].Count; j++){
			GameObject Wordtmp = Instantiate(TrovaIntrusoImmaginiPrefab, GridLayout.GetComponent<RectTransform>().transform);
			Wordtmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>( "Images/abatjour-guidare/" + copyOfDictWords[rnd].ElementAt(j).Key );
			Wordtmp.GetComponentInChildren<TrovaIntrusoImmaginiScript>().outOfPlace = copyOfDictWords[rnd].ElementAt(j).Value;
			}
		copyOfDictWords.RemoveAt(rnd);
		
		print(dictOfWords.Count);
		print(copyOfDictWords.Count);
	  }

	public List<Dictionary<string,bool>> CreateDictionary(CSVData.WordObject[] Data) {
		List<Dictionary<string,bool>> newDictList = new List<Dictionary<string,bool>>();
		CSVData.WordObject[] CopyData = Data;
		CSVData.WordObject[] filteredData = CopyData;
		string categoryToUse;
		List<string> categoriesUsed = new List<string>();

		for (int i = 0; i < nSchede; i++)
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


			} while (filteredData.Count() < nImages-2 || categoriesUsed.Contains(categoryToUse));

			List<string> sameCategoryWords = new List<string>();
			sameCategoryWords.Add(random1.Word);
			for (int j = 0; j < nImages-2; j++)
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
			newDictList.Add(row);
		}


		return newDictList;
	}
}
