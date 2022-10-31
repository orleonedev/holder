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
	Dictionary<string,string> dictWithSolutions;
	string correctSeasonOfWord;
	int answersGiven;
	int correctAnswers;
    // Start is called before the first frame update
    void Start()
    {
        dictWithSolutions = new Dictionary<string, string>();
		dictWithSolutions.Add("Image 1", "PRIMAVERA");
		dictWithSolutions.Add("Image 2", "ESTATE");
		dictWithSolutions.Add("Image 3", "AUTUNNO");
		dictWithSolutions.Add("Image 4", "INVERNO");
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
		ImmagineDaIndovinare.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>( "testImages/" + dictWithSolutions.ElementAt(rnd).Key );
		correctSeasonOfWord = dictWithSolutions[dictWithSolutions.ElementAt(rnd).Key];
	}
}
