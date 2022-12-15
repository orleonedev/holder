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

	[SerializeField]
	GameObject endgame;

	public static int imagesNumber = 4;
	public static int size = 40;

    // Start is called before the first frame update
    void Start()
    {
		Application.targetFrameRate = 60;
		wordAndImages = CreateDictionary(CSVData.Instance.FilteredWithImages());
		string randomWord = wordAndImages.ElementAt(Random.Range(0, wordAndImages.Count)).Key;
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
			print("immagine: " + wordAndImages[wordToFind][i]);
			ImagesLayout.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/abatjour-guidare/" + wordAndImages[wordToFind][i]);
		}
		keyWordPair = new Dictionary<int, int>();
		wordAndImages.Remove(randomWord);
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
				if (wordAndImages.Count != 0){
					ResetGame();
				} else {
					endgame.GetComponent<EndGame>().startEndGame();
					print("FINITA GAME SESSION");
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
			print("immagine: " + wordAndImages[wordToFind][i]);
			ImagesLayout.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/abatjour-guidare/" + wordAndImages[wordToFind][i]);
		}
		characterInserted = 0;
		keyboard.GetComponent<KeyboardScript>().wordToShuffle = wordToFind;
		keyboard.GetComponent<KeyboardScript>().ResetKeyboard();
		keyWordPair.Clear();
		wordAndImages.Remove(randomWord);
	}

	public Dictionary<string, string[]> CreateDictionary(CSVData.WordObject[] Data){
		List<string> imagesName = new List<string>();
		Dictionary<string,string[]> sessionDict = new Dictionary<string, string[]>();
		CSVData.WordObject[] CopyData = Data;
		CSVData.WordObject[] filteredData = CopyData;
		string categoryToUse;
		List<string> categoriesUsed = new List<string>();

		for (int i = 0; i < size; i++){
			imagesName.Clear();
			CSVData.WordObject random = new CSVData.WordObject();
			do
			{
				random = CopyData.ElementAt(Random.Range(0, CopyData.Count()));
				
				List<string> categories = new List<string>();
				categories.Add(random.Cat_1);
				if (random.Cat_2 != ""){
					categories.Add(random.Cat_2);
				}
				if (random.Cat_3 != ""){
					categories.Add(random.Cat_3);
				}
				print(categories.Count);
				categoryToUse = categories.ElementAt(Random.Range(0,categories.Count));
				print("try: "+ categoryToUse);
				filteredData = CopyData.Where(c => 
					c != random && ( 
					c.Cat_1 == categoryToUse || c.Cat_2 == categoryToUse || c.Cat_3 == categoryToUse
					)
				).ToArray();
				print(filteredData.Count());
			} while ((filteredData.Count() < imagesNumber-1) || categoriesUsed.Contains(categoryToUse));

			imagesName.Add(random.Word.ToLower());
			categoriesUsed.Add(categoryToUse);

			for (int j = 0; j < imagesNumber-1; j++)
			{
				CSVData.WordObject another = filteredData.ElementAt(Random.Range(0, filteredData.Count()));
				imagesName.Add(another.Word.ToLower());
				filteredData = filteredData.Where(c=> c!=another).ToArray();
			}
			print("added: "+ categoryToUse);
			sessionDict.Add(categoryToUse,imagesName.ToArray());
			
		}



		return sessionDict;
	}

	public void setParameters(){
		size = 15;
		imagesNumber = 4;
	}
}
