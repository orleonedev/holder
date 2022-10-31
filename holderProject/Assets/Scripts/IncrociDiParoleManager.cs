using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum WordPositionOrientation
{
	Horizontal = 0,
	Vertical = 1,
	VerticalAndHorizontal = 2,
}

public class IncrociDiParoleManager : MonoBehaviour
{
	private static IncrociDiParoleManager instance;
	public static IncrociDiParoleManager Instance { get { return instance; } }
	[SerializeField]
	GameObject GameGridLayout;
	[SerializeField]
	GameObject WordsToFindGridLayout;
	[SerializeField]
	GameObject IncrociDiParoleButtonPrefab;
	[SerializeField]
	GameObject IncrociParoleDaTrovarePrefab;
	List<string> wordsToFind;
	List<string> pickedWords;
	List<GameObject> listOfWordsToFindObject;
	List<GameObject> gridLettersGameObject;
	List<string> insertedWord;

	// Start is called before the first frame update
	void Start()
	{
		wordsToFind = new List<string>();
		listOfWordsToFindObject = new List<GameObject>();
		gridLettersGameObject = new List<GameObject>();
		pickedWords = new List<string>();
		insertedWord = new List<string>();
		wordsToFind.Add("ASIA");
		wordsToFind.Add("PIA");
		wordsToFind.Add("MARA");
		wordsToFind.Add("UGO");
		wordsToFind.Add("BLU");
		wordsToFind.Add("ROSA");
		wordsToFind.Add("ORO");
		wordsToFind.Add("VERDE");
		wordsToFind.Add("ALICI");
		wordsToFind.Add("ROMBI");
		wordsToFind.Add("CARPA");
		wordsToFind.Add("TONNO");
		List<string> wordsToFindCopy = new List<string>(wordsToFind);
		//int amountOfWordsToPick = Random.Range(4, wordsToFind.Count);
		int amountOfWordsToPick = Random.Range(4, 7);
		for (int i = 0; i < amountOfWordsToPick; i++)
		{
			int rnd = Random.Range(0, wordsToFindCopy.Count);
			var tmp = Instantiate(IncrociParoleDaTrovarePrefab, WordsToFindGridLayout.transform);
			tmp.GetComponent<TMP_Text>().text = wordsToFindCopy[rnd];
			pickedWords.Add(wordsToFindCopy[rnd]);
			wordsToFindCopy.RemoveAt(rnd);
			listOfWordsToFindObject.Add(tmp);
		}
		SetupGrid();
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void Awake()
	{
		instance = this;
	}
	void SetupGrid()
	{
		
		string[] alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}; 
		int orientationRnd = Random.Range(0, 3);
		WordPositionOrientation orientation = (WordPositionOrientation)orientationRnd;
		var finalValue = pickedWords.OrderBy(n => n.Length).Last();
		print(orientation);
		for (int i = 0; i < finalValue.Length * pickedWords.Count; i++)
		{
			var tmp = Instantiate(IncrociDiParoleButtonPrefab, GameGridLayout.transform);
			tmp.GetComponentInChildren<TMP_Text>().text = alphabet[Random.Range(0, alphabet.Length)];
			gridLettersGameObject.Add(tmp);
		}
		
		if (orientation == WordPositionOrientation.Horizontal)
		{
			GameGridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
			GameGridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraintCount = finalValue.Count() + Random.Range(0, 1);
			for (int i = 0; i < pickedWords.Count; i++)
			{
				var currentWord = pickedWords[i].ToCharArray();
				//used for random positioning of word
				var difference = finalValue.Length - currentWord.Count();
				var spacing = Random.Range(0, difference+1);
				for(int j = 0; j < pickedWords[i].Length; j++){
					gridLettersGameObject[i*finalValue.Length+j+spacing].GetComponentInChildren<TMP_Text>().text = currentWord[j].ToString();
				}
			}
		}
		else
		{
			GameGridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedRowCount;
			GameGridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraintCount = finalValue.Count() + Random.Range(0, 1);
			for (int i = 0; i < pickedWords.Count; i++)
			{
				var currentWord = pickedWords[i].ToCharArray();
				//used for random positioning of word
				var difference = finalValue.Length - currentWord.Count();
				var spacing = Random.Range(0, difference+1);
				for(int j = 0; j < pickedWords[i].Length; j++){
					gridLettersGameObject[(j+spacing)*pickedWords.Count+i].GetComponentInChildren<TMP_Text>().text = currentWord[j].ToString();
				}
			}
		}
	}
	public void GetCharacter(IncrociParoleButtonScript btn){
		if(!btn.pressed){
			insertedWord.Add(btn.GetComponentInChildren<TMP_Text>().text);
			btn.insertedAt = insertedWord.LastIndexOf(btn.GetComponentInChildren<TMP_Text>().text);
			btn.GetComponent<UnityEngine.UI.Image>().color = Color.green;
		}
		/*else{
			insertedWord.RemoveAt(btn.insertedAt);
			btn.insertedAt = 0;
		}*/
		char[] inputWord = new char[insertedWord.Count];
		int i = 0;
		foreach(string chr in insertedWord){
			inputWord[i] = chr.ToCharArray().First();
			i++;
		}
		string finalInputWord = new string(inputWord);
		i = 0;
		foreach(string word in pickedWords){
			if(finalInputWord == word){
				WordFound(i);
				break;
			}
			i++;
		}
		foreach(string chr in insertedWord){
			print(chr);
		}
	}
	void WordFound(int index){
		listOfWordsToFindObject[index].GetComponent<TMP_Text>().fontStyle = FontStyles.Strikethrough;
		insertedWord.Clear();
		foreach(GameObject btn in gridLettersGameObject){
			btn.GetComponent<IncrociParoleButtonScript>().pressed = false;
			btn.GetComponent<UnityEngine.UI.Image>().color = Color.white;
		}
	}
	public void Delete(){
		insertedWord.Clear();
		foreach(GameObject btn in gridLettersGameObject){
			btn.GetComponent<IncrociParoleButtonScript>().pressed = false;
			btn.GetComponent<UnityEngine.UI.Image>().color = Color.white;
		}
	}
}
