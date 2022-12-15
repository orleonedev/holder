using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class CicliStagioniManager : MonoBehaviour
{
	private static CicliStagioniManager instance;
	public static CicliStagioniManager Instance { get { return instance; } }
	[SerializeField]
	GameObject HorizontalLayout;
	[SerializeField]
	GameObject ImmagineDaIndovinare;
	[SerializeField]
	GameObject primaveraBtn;
	[SerializeField]
	GameObject estateBtn;
	[SerializeField]
	GameObject autunnoBtn;
	[SerializeField]
	GameObject invernoBtn;
	[SerializeField]
	GameObject endgame;
	Dictionary<string,string> dictWithSolutions;
	Dictionary<string,string> dictWithSolutionsCopy;
	public static int nSchede = 30;
	string correctSeasonOfWord;
	int answersGiven;
	int correctAnswers;
    // Start is called before the first frame update
    void Start()
    {
        dictWithSolutions = CreateDictionary(CSVData.Instance.FilteredForSeasonWithImages());
		dictWithSolutionsCopy = dictWithSolutions;
		NewWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void Awake()
	{
		instance = this;
	}
	public void GetClick(string season){
		if(season == correctSeasonOfWord){
			correctAnswers++;
		}
		answersGiven++;
		print(correctAnswers + " di " + answersGiven + " sono corrette");
		if (dictWithSolutionsCopy.Count != 0){
			NewWord();
		}
		else{
			endgame.GetComponent<EndGame>().startEndGame();
			print("FINITA SESSIONE");
		}
		
	}
	void NewWord(){
		int rnd = Random.Range(0 , dictWithSolutionsCopy.Count);
		var sprite = (
            #if UNITY_IOS || UNITY_EDITOR_OSX
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+dictWithSolutionsCopy.ElementAt(rnd).Key.ToLower().Normalize(System.Text.NormalizationForm.FormD)))
            #else
            (Resources.Load<Sprite>("Images/abatjour-guidare/"+dictWithSolutionsCopy.ElementAt(rnd).Key.ToLower()))
            #endif
       );
		ImmagineDaIndovinare.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
		correctSeasonOfWord = dictWithSolutionsCopy[dictWithSolutionsCopy.ElementAt(rnd).Key];
		print(dictWithSolutionsCopy.ElementAt(rnd).Key + " - " + correctSeasonOfWord);
		dictWithSolutionsCopy.Remove(dictWithSolutionsCopy.ElementAt(rnd).Key);
	}

	public Dictionary<string,string> CreateDictionary(CSVData.WordObject[] Data){
		Dictionary<string,string> newDict = new Dictionary<string, string>();
		CSVData.WordObject[] CopyData = Data;

		for (int i = 0; i < nSchede; i++)
		{
			CSVData.WordObject random = CopyData.ElementAt(Random.Range(0,CopyData.Count()));
			newDict.Add(random.Word, random.Season);
			CopyData = CopyData.Where(c=> c!=random).ToArray();
		}

		return newDict;
	}
}
