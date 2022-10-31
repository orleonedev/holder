using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeystrokeScript : MonoBehaviour
{
	private string character;
	public string Character { get { return character; } set { character = value; } }
	public int keyIndex;
	// Start is called before the first frame update
	void Start()
	{
		this.GetComponentInChildren<TMP_Text>().text = character;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void UserStroke()
	{
		KeyboardScript keyboard = this.GetComponentInParent<KeyboardScript>();
		switch (keyboard.Type)
		{
			case TypeOfKeyboard.ParolaGiusta:
				ParolaGiustaManager.Instance.GetKeystroke(character, keyIndex);
				break;
			case TypeOfKeyboard.AnelloCatena:
				AnelloDellaCatenaManager.Instance.GetKeystroke(character, keyIndex);
				break;
			case TypeOfKeyboard.QuattroImmaginiUnaCategoria:
				QuattroImmaginiUnaCategoriaManager.Instance.GetKeystroke(character, keyIndex);
				break;
		}
		this.GetComponent<UnityEngine.UI.Button>().interactable = false;
	}
}
