using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PercorsiManager : MonoBehaviour
{
	private static PercorsiManager instance;
	public static PercorsiManager Instance { get { return instance; } }
	[SerializeField]
	GameObject description;
	[SerializeField]
	GameObject spawnArea;
	[SerializeField]
	GameObject PercorsiPrefab;
	List<string> possibleWords;
	List<string> possibleWordsCopy;
	int buttonsClicked;
	bool isOrderInverted = false;
	List<GameObject> sequencePresented;
	string chosenWord;
	// Start is called before the first frame update
	void Start()
	{
		possibleWords = new List<string>();
		sequencePresented = new List<GameObject>();
		possibleWords.Add("CONIGLIO");
		possibleWords.Add("ELICOTTERO");
		possibleWords.Add("PASTA");
		possibleWords.Add("ACQUA");
		possibleWords.Add("MARE");
		possibleWords.Add("CONSIGLIO");
		possibleWords.Add("2135496");
		possibleWordsCopy = new List<string>(possibleWords);
		if (Random.Range(0, 100) > 60)
			isOrderInverted = true;

		int rnd = Random.Range(0, possibleWords.Count);
		chosenWord = possibleWords[rnd];
		buttonsClicked = 0;
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
	public void StartGame()
	{
		//print(correctAnswers + " di " + wordsShown + " sono corrette");
		StartCoroutine(SpawnSequence());
	}
	public void GetClick(int index)
	{
		if(isOrderInverted){
			if(chosenWord.Count() - (index+1) == buttonsClicked){
				buttonsClicked++;
			}
			else{
				print("Sbagliato");
			}
		}else{
			if(index == buttonsClicked){
				buttonsClicked++;
			}
			else{
				print("Sbagliato");
			}
		}
		if(buttonsClicked == chosenWord.Count())
			print("Bravo");
	}
	IEnumerator SpawnSequence()
	{
		for (int i = 0; i < chosenWord.Count(); i++)
		{
			float rndX = Random.Range(-768.0f, 768.0f);
			float rndY = Random.Range(-600.0f, 600.0f);
			var tmp = Instantiate(PercorsiPrefab, spawnArea.transform);
			tmp.transform.localPosition = new Vector2(rndX, rndY);
			tmp.GetComponentInChildren<TMP_Text>().text = chosenWord[i].ToString();
			tmp.GetComponent<UnityEngine.UI.Button>().interactable = false;
			tmp.GetComponent<PercorsiScript>().index = i;
			sequencePresented.Add(tmp);
			yield return new WaitForSeconds(1);
		}
		if(isOrderInverted)
			description.GetComponent<TMP_Text>().text = "Riproduci la sequenza al contrario";
		else
			description.GetComponent<TMP_Text>().text = "Riproduci la sequenza";
		foreach(GameObject obj in sequencePresented){
			obj.GetComponent<UnityEngine.UI.Button>().interactable = true;
		}
	}
}
