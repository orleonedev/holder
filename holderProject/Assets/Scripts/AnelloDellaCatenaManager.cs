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

	[SerializeField]
	GameObject endgame;

	public static int nSchede = 15;
	public static int nhints = 2;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
		definitionsAndWords = CreateDictionary(CSVData.Instance.FilteredForAnello());
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
		definitionsAndWords.Remove(wordToFind);
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
				if (definitionsAndWords.Count != 0){
					ResetGame();
				} else {
					endgame.GetComponent<EndGame>().startEndGame();
					print("FINITA SESSIONE");
				}
				
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
		for(int i = 0; i < nhints; i++){
			Destroy(definitionsGameObjectsContainer[i].gameObject);
		}
		definitionsGameObjectsContainer.Clear();
		string randomWord = definitionsAndWords.ElementAt(Random.Range(0, definitionsAndWords.Count)).Key;
		List<string> definitionsOfWord = definitionsAndWords[randomWord];
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
		definitionsAndWords.Remove(wordToFind);
	}

	public Dictionary<string, List<string>> CreateDictionary(CSVData.WordObject[] Data ){
		Dictionary<string, List<string>> newDict = new Dictionary<string, List<string>>();
		CSVData.WordObject[] CopyData = Data;

		for (int i = 0; i < nSchede; i++)
		{
			CSVData.WordObject wordToFind = CopyData.ElementAt(Random.Range(0,CopyData.Count()));
			List<CSVData.WordObject> sameWordList = CopyData.Where(c=> c.Anello == wordToFind.Anello).ToList();
			List<string> categories = new List<string>();
			print("parola: "+ wordToFind.Word);
			sameWordList.ForEach(delegate(CSVData.WordObject word){
				List<string> tmp = new List<string>();
				if (!tmp.Contains(word.Cat_1)){
					print("Aggiungo cat 1: " + word.Cat_1 );
					#if UNITY_IOS || UNITY_EDITOR_OSX
					tmp.Add(word.Cat_1.Normalize(System.Text.NormalizationForm.FormD));
					#else 
					tmp.Add(word.Cat_1);
					#endif
				}
				
				if (word.Cat_2 != "" && !tmp.Contains(word.Cat_2 )){
					print("Aggiungo cat 2: " + word.Cat_2 );
					#if UNITY_IOS || UNITY_EDITOR_OSX
					tmp.Add(word.Cat_2.Normalize(System.Text.NormalizationForm.FormD));
					#else 
					tmp.Add(word.Cat_2);
					#endif
				}
				if (word.Cat_3 != "" && !tmp.Contains(word.Cat_3)){
					print("Aggiungo cat 3: " + word.Cat_3 );
					#if UNITY_IOS || UNITY_EDITOR_OSX
					tmp.Add(word.Cat_3.Normalize(System.Text.NormalizationForm.FormD));
					#else 
					tmp.Add(word.Cat_3);
					#endif
				}

				categories.Add(tmp.ElementAt(Random.Range(0,tmp.Count)));
			});

			List<string> hints = new List<string>();
			for (int j = 0; j < nhints; j++)
			{
				string hint = categories.ElementAt(Random.Range(0,categories.Count));
				print("aggiungo: " + hint);
				hints.Add(hint);
				categories.Remove(hint);
			}

			newDict.Add(wordToFind.Word, hints);
			CopyData = CopyData.Where(c=> c.Anello != wordToFind.Anello).ToArray();
		}

		return newDict;

	}
}
