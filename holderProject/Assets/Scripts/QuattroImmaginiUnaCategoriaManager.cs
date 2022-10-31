using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class QuattroImmaginiUnaCategoriaManager : MonoBehaviour
{
   private static QuattroImmaginiUnaCategoriaManager instance;
	public static QuattroImmaginiUnaCategoriaManager Instance {get { return instance; } }
	[SerializeField]
	GameObject keyboard;
	[SerializeField]
	GameObject ImagesLayout;
	[SerializeField]
	GameObject AnswerLayout;
	[SerializeField]
	GameObject AnswerCharacterPrefab;
	Dictionary<string, string[]> wordAndImages;
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
		wordAndImages = new Dictionary<string, string[]>();
		wordAndImages.Add("MESTIERE", new string[]{"Image 1", "Image 2", "Image 3", "Image 4"});
		wordAndImages.Add("FRUTTA", new string[]{"Image 5", "Image 6", "Image 7", "Image 8"});
		wordAndImages.Add("ANIMALE", new string[]{"Image 9", "Image 10", "Sol 1", "Sol 2"});
		string randomWord = wordAndImages.ElementAt(Random.Range(0, wordAndImages.Count)).Key;
		//wordDefinition.GetComponentInChildren<TMP_Text>().text = randomWord;
		wordToFind = randomWord;
		WordCharacterCount = wordToFind.Count();
		wordInserted = new char[WordCharacterCount];
		keyboard.GetComponent<KeyboardScript>().wordToShuffle = wordToFind;
		for(int i = 0; i < WordCharacterCount; i++){
			GameObject answerHolder = Instantiate(AnswerCharacterPrefab, AnswerLayout.GetComponent<RectTransform>());
			answerHolder.GetComponentInChildren<AnswerCharacterScript>().CharacterPosition = i;
			answerCharacterHolders.Add(answerHolder);
		}
		for(int i = 0; i < ImagesLayout.transform.childCount; i++){
			ImagesLayout.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + wordAndImages[wordToFind][i]);
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
		string randomWord = wordAndImages.ElementAt(Random.Range(0, wordAndImages.Count)).Key;
		wordToFind = randomWord;
		WordCharacterCount = wordToFind.Count();
		wordInserted = new char[WordCharacterCount];
		for(int i = 0; i < WordCharacterCount; i++){
			GameObject answerHolder = Instantiate(AnswerCharacterPrefab, AnswerLayout.GetComponent<RectTransform>());
			answerHolder.GetComponentInChildren<AnswerCharacterScript>().CharacterPosition = i;
			answerCharacterHolders.Add(answerHolder);
		}
		for(int i = 0; i < ImagesLayout.transform.childCount; i++){
			ImagesLayout.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + wordAndImages[wordToFind][i]);
		}
		characterInserted = 0;
		keyboard.GetComponent<KeyboardScript>().wordToShuffle = wordToFind;
		keyboard.GetComponent<KeyboardScript>().ResetKeyboard();
		keyWordPair.Clear();
	}
}
