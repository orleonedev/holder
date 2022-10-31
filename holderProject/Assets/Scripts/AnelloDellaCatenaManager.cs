using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class AnelloDellaCatenaManager : MonoBehaviour
{
	private static AnelloDellaCatenaManager instance;
	public static AnelloDellaCatenaManager Instance {get { return instance; } }

	[SerializeField]
	GameObject keyboard;
	[SerializeField]
	GameObject DefinitionsLayout;
	[SerializeField]
	GameObject DefinitionPrefab;
	string wordToFind;

	private List<GameObject> answerCharacterHolders = new List<GameObject>();

	private int characterInserted = 0;

	private char[] wordInserted;
	//The count of characters of the word to find
	private int WordCharacterCount;

	Dictionary<string, List<string>> definitionsAndWords;
	[SerializeField]
	GameObject AnswerLayout;
	[SerializeField]
	GameObject AnswerCharacterPrefab;
	List<GameObject> definitionsGameObjectsContainer;
	Dictionary<int, int> keyWordPair;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
		definitionsAndWords = new Dictionary<string, List<string>>();
		definitionsAndWords.Add("PESCA", new List<string>{"FRUTTO", "PASSATEMPO"});
		definitionsAndWords.Add("CAPPUCCINO", new List<string>{"FRATE", "BEVANDA"});
		definitionsAndWords.Add("VIOLA", new List<string>{"FIORE", "STRUMENTO", "COLORE"});
		string randomWord = definitionsAndWords.ElementAt(Random.Range(0, definitionsAndWords.Count)).Key;
		List<string> definitionsOfWord = definitionsAndWords[randomWord];
		definitionsGameObjectsContainer = new List<GameObject>();
		for(int i = 0; i < definitionsOfWord.Count; i++){
			GameObject tmpDefinition = Instantiate(DefinitionPrefab, DefinitionsLayout.transform.GetComponent<RectTransform>());
			tmpDefinition.GetComponent<TMP_Text>().text = definitionsOfWord[i];
			definitionsGameObjectsContainer.Add(tmpDefinition);
		}
		WordCharacterCount = randomWord.Count();
		wordInserted = new char[WordCharacterCount];
		wordToFind = randomWord;
		keyboard.GetComponent<KeyboardScript>().wordToShuffle = wordToFind;
		for(int i = 0; i < WordCharacterCount; i++){
			GameObject answerHolder = Instantiate(AnswerCharacterPrefab, AnswerLayout.GetComponent<RectTransform>());
			answerHolder.GetComponentInChildren<AnswerCharacterScript>().CharacterPosition = i;
			answerCharacterHolders.Add(answerHolder);
		}
		keyWordPair = new Dictionary<int, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void Awake() {
		instance = this;
	}
	public void GetKeystroke(string character, int keyIndex){
		bool positionFound = false;
		if(characterInserted != WordCharacterCount){
			for(int i = 0; i < WordCharacterCount && !positionFound; i++){
				if(answerCharacterHolders[i].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text == string.Empty) {
					answerCharacterHolders[i].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = character;
					wordInserted[i] = character.ToCharArray()[0];
					keyWordPair.Add(i, keyIndex);
					positionFound = true;
				}
			}
			characterInserted++;
			if(characterInserted == WordCharacterCount){
				string finalWord = new string(wordInserted);
				if(finalWord == wordToFind){
					print("Bravo");
				}
				else{
					print("sbagliato");
				}
				ResetGame();
			}
		}
	}
	public void DeleteKeystroke(int position){
		answerCharacterHolders[position].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = string.Empty;
		keyboard.GetComponent<KeyboardScript>().buttons[keyWordPair[position]].GetComponent<UnityEngine.UI.Button>().interactable = true;
		keyWordPair.Remove(position);
		characterInserted--;
	}

		private void ResetGame(){
		for(int i = 0; i < WordCharacterCount; i++){
			Destroy(answerCharacterHolders[i].gameObject);
		}
		answerCharacterHolders.Clear();
		print(answerCharacterHolders.Count);
		List<string> definitionsOfWord = definitionsAndWords[wordToFind];
		for(int i = 0; i < definitionsOfWord.Count; i++){
			print(definitionsOfWord[i]);
			Destroy(definitionsGameObjectsContainer[i].gameObject);
		}
		definitionsGameObjectsContainer.Clear();
		string randomWord = definitionsAndWords.ElementAt(Random.Range(0, definitionsAndWords.Count)).Key;
		definitionsOfWord = definitionsAndWords[randomWord];
		for(int i = 0; i < definitionsOfWord.Count; i++){
			GameObject tmpDefinition = Instantiate(DefinitionPrefab, DefinitionsLayout.transform.GetComponent<RectTransform>());
			tmpDefinition.GetComponent<TMP_Text>().text = definitionsOfWord[i];
			definitionsGameObjectsContainer.Add(tmpDefinition);
		}
		WordCharacterCount = randomWord.Count();
		wordInserted = new char[WordCharacterCount];
		wordToFind = randomWord;
		for(int i = 0; i < WordCharacterCount; i++){
			GameObject answerHolder = Instantiate(AnswerCharacterPrefab, AnswerLayout.GetComponent<RectTransform>());
			answerHolder.GetComponentInChildren<AnswerCharacterScript>().CharacterPosition = i;
			answerCharacterHolders.Add(answerHolder);
		}
		characterInserted = 0;
		keyboard.GetComponent<KeyboardScript>().wordToShuffle = wordToFind;
		keyboard.GetComponent<KeyboardScript>().ResetKeyboard();
		keyWordPair.Clear();
	}
}
