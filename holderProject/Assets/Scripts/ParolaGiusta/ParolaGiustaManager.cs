using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ParolaGiustaManager : MonoBehaviour
{
	private static ParolaGiustaManager instance;
	public static ParolaGiustaManager Instance {get { return instance; } }
	[SerializeField]
	GameObject keyboard;
	[SerializeField]
	GameObject wordDefinition;
	[SerializeField]
	GameObject AnswerLayout;
	[SerializeField]
	GameObject AnswerCharacterPrefab;
	Dictionary<string, string> definitionsAndWords;
	string wordToFind;

	private List<GameObject> answerCharacterHolders = new List<GameObject>();

	private int characterInserted = 0;

	private char[] wordInserted;
	//The count of characters of the word to find
	private int WordCharacterCount;
	Dictionary<int, int> keyWordPair;

    // Start is called before the first frame update
    void Start()
    {
		Application.targetFrameRate = 60;
		definitionsAndWords = new Dictionary<string, string>();
		definitionsAndWords.Add("Formaggio fresco, ingrediente principale della pizza", "MOZZARELLA");
		definitionsAndWords.Add("La 'Polare' che orienta di notte", "STELLA");
		string randomDefinition = definitionsAndWords.ElementAt(Random.Range(0, definitionsAndWords.Count)).Key;
		wordDefinition.GetComponentInChildren<TMP_Text>().text = randomDefinition;
		wordToFind = definitionsAndWords[randomDefinition];
		WordCharacterCount = wordToFind.Count();
		wordInserted = new char[WordCharacterCount];
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
					positionFound = true;
					keyWordPair.Add(i, keyIndex);
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
		string randomDefinition = definitionsAndWords.ElementAt(Random.Range(0, definitionsAndWords.Count)).Key;
		wordDefinition.GetComponentInChildren<TMP_Text>().text = randomDefinition;
		wordToFind = definitionsAndWords[randomDefinition];
		WordCharacterCount = wordToFind.Count();
		wordInserted = new char[WordCharacterCount];
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
