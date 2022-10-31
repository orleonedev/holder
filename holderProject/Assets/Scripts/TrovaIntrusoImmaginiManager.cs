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
	int amountOfRows;
	int CorrectAnswers;
	int answersCount;
	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		amountOfRows = Random.Range(1, 3);
		CorrectAnswers = 0;
		answersCount = 0;
		dictOfWords = new List<Dictionary<string, bool>>();
		dictOfWords.Add(new Dictionary<string, bool>(){ { "Image 1" , false }, { "Image 2" , false }, { "Image 3" , false }, { "Sol 1" , true }, });
		dictOfWords.Add(new Dictionary<string, bool>(){ { "Image 4" , false }, { "Image 5" , false }, { "Image 6" , false }, { "Sol 2" , true }, });
		/*dictOfWords.Add(new Dictionary<string, bool>(){ { "ITALIA" , false }, { "GERMANIA" , false }, { "AFRICA" , true }, { "RUSSIA" , false }, });
		dictOfWords.Add(new Dictionary<string, bool>(){ { "SCOPA" , false }, { "ASPIRAPOLVERE" , false }, { "TAGLIAERBA" , true }, { "LUCIDATRICE" , false }, });
		dictOfWords.Add(new Dictionary<string, bool>(){ { "PRANZO" , false }, { "CENA" , false }, { "COLAZIONE" , false }, { "POLPETTA" , true }, });
		dictOfWords.Add(new Dictionary<string, bool>(){ { "RISOTTO" , false }, { "PIZZA" , false }, { "MINESTRONE" , false }, { "GELATO" , true }, });
		dictOfWords.Add(new Dictionary<string, bool>(){ { "GATTO" , false }, { "CANARINO" , false }, { "LEONE" , true }, { "CANE" , false }, });
		dictOfWords.Add(new Dictionary<string, bool>(){ { "OCCHI" , false }, { "GAMBE" , true }, { "NASO" , false }, { "BOCCA" , false }, });*/
		var copyOfDictWords = dictOfWords.ToList();
		for(int i = 0; i < amountOfRows; i++){
			int rnd = Random.Range(0, copyOfDictWords.Count);
			System.Random rng = new System.Random();
			copyOfDictWords[rnd] = copyOfDictWords[rnd].OrderBy(a => rng.Next()).ToDictionary(entry => entry.Key,
											   entry => entry.Value);
			for(int j = 0; j < copyOfDictWords[rnd].Count; j++){
				GameObject Wordtmp = Instantiate(TrovaIntrusoImmaginiPrefab, GridLayout.GetComponent<RectTransform>().transform);
				//Wordtmp.GetComponentInChildren<TMP_Text>().text = copyOfDictWords[rnd].ElementAt(j).Key;
				Wordtmp.GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>( "testImages/" + copyOfDictWords[rnd].ElementAt(j).Key );
				Wordtmp.GetComponentInChildren<TrovaIntrusoImmaginiScript>().outOfPlace = copyOfDictWords[rnd].ElementAt(j).Value;
			}
			copyOfDictWords.RemoveAt(rnd);
		}
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
		if(answersCount == amountOfRows)
			CheckResult();
	}
	public void CheckResult(){
		print(CorrectAnswers + " di " + amountOfRows + " risposte sono corrette");
	}
}
