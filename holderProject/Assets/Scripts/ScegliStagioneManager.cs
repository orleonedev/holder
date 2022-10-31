using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class ScegliStagioneManager : MonoBehaviour
{
	private static ScegliStagioneManager instance;
	public static ScegliStagioneManager Instance { get { return instance; } }
	[SerializeField]
	GameObject HorizontalLayout;
	[SerializeField]
	GameObject ParolaDaIndovinare;
	[SerializeField]
	GameObject primaveraBtn;
	[SerializeField]
	GameObject estateBtn;
	[SerializeField]
	GameObject autunnoBtn;
	[SerializeField]
	GameObject invernoBtn;
	Dictionary<string,string> dictWithSolutions;
	string correctSeasonOfWord;
	int answersGiven;
	int correctAnswers;
    // Start is called before the first frame update
    void Start()
    {
        dictWithSolutions = new Dictionary<string, string>();
		dictWithSolutions.Add("ALBERO DI NATALE", "INVERNO");
		dictWithSolutions.Add("FRAGOLA", "PRIMAVERA");
		dictWithSolutions.Add("PELLICCIA", "INVERNO");
		dictWithSolutions.Add("OMBRELLO", "AUTUNNO");
		dictWithSolutions.Add("OMBRELLONE", "ESTATE");
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
		NewWord();
	}
	void NewWord(){
		int rnd = Random.Range(0 , dictWithSolutions.Count);
		ParolaDaIndovinare.GetComponent<TMP_Text>().text = dictWithSolutions.ElementAt(rnd).Key;
		correctSeasonOfWord = dictWithSolutions[ParolaDaIndovinare.GetComponent<TMP_Text>().text];
	}
}
