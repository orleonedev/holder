using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class KeyboardScript : MonoBehaviour
{
	private string[] alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}; 
	public string wordToShuffle;
	[SerializeField]
	public List<GameObject> buttons;
	//public List<GameObject> Buttons { get { return buttons; } set { buttons = value;} }

	[SerializeField]
	GameObject keyboardButtonPrefab;
	[SerializeField]
	TypeOfKeyboard type;

	public TypeOfKeyboard Type { get { return type; } }

    // Start is called before the first frame update
    void Start()
    {
		SetupKeyboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SetupKeyboard(){
		char[] test = wordToShuffle.ToCharArray();
		List<string> wordToShuffleList = new List<string>();
		foreach (char str in test){
			wordToShuffleList.Add(str.ToString());
		}
		List<string> alphabetCopy = new List<string>();
		foreach (string str in alphabet){
			if(!wordToShuffleList.Contains(str))
				alphabetCopy.Add(str);
		}
		int amountOfRandLetters = Random.Range(3, 10);
		for(int i = 0; i < amountOfRandLetters; i++){
			wordToShuffleList.Add(alphabetCopy[Random.Range(0, alphabetCopy.Count)]);
		}
		System.Random rng = new System.Random();
		var shuffledWords = wordToShuffleList.OrderBy(a => rng.Next()).ToList();
		for(int i = 0; i < shuffledWords.Count; i++){
			GameObject button = Instantiate(keyboardButtonPrefab, GetComponent<RectTransform>());
			button.GetComponent<KeystrokeScript>().Character = shuffledWords[i];
			button.GetComponent<KeystrokeScript>().keyIndex = i;
			buttons.Add(button);
		}
	}
	public void ResetKeyboard(){
		foreach(GameObject btn in buttons){
			Destroy(btn);
		}
		buttons.Clear();
		SetupKeyboard();
	}
}

public enum TypeOfKeyboard
{
    ParolaGiusta,
    AnelloCatena,
	QuattroImmaginiUnaCategoria
}
