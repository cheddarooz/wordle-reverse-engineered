using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject center;
    public GameObject containedLetter;

    string guessedWord;
    string wordToGuess;

    char[] guessedLetters = new char[5];
    bool[] guessedWordCheck = new bool[5];

    int wordGuessRow = 1;
    int wordGuessColumn = 6;

    char emptySpace = ' ';
    
    void Start()
    {
       //lettersGuessed = new bool[wordToGuess.Length];

        center = GameObject.Find("centerReferencePoint");

        wordToGuess = "apple";
        guessedLetters = wordToGuess.ToCharArray();

        initializeLetterSpaces();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    public void initializeLetterSpaces()
    {
        for (int k = 1; k < 7; k++)
        {
            for (int i = 1; i < 6; i++)
            {
                Vector3 letterPosition = new Vector3(center.transform.position.x + ((i - 6/2.0f) * 150), center.transform.position.y + ((k - 3) * 150), center.transform.position.z);
                GameObject letterToSpawn = (GameObject)Instantiate(containedLetter, letterPosition, Quaternion.identity);
                letterToSpawn.name = "letter" + i + "." + k;
                letterToSpawn.transform.SetParent(GameObject.Find("Canvas").transform);
            }
        }
    }

    public void checkInput()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0))
        {
            char currentKeyBoardLetterInput = Input.inputString.ToCharArray()[0];
            int currentKeyboardLetterAsAscii = System.Convert.ToInt32(currentKeyBoardLetterInput);

            if (currentKeyboardLetterAsAscii >= 97 && currentKeyboardLetterAsAscii <= 122)
            {
                if(wordGuessRow <= 5)
                {
                    updateOnScreenLetterGuess(currentKeyBoardLetterInput);
                    //GameObject.Find("wordGuessed").GetComponent<Text>().text = Input.inputString.ToString();
                    guessedWord = guessedWord + Input.inputString.ToString();
                    print(guessedWord);
                    wordGuessRow++;
                    print(guessedWord.Length);
                }
            }   
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (wordGuessRow >= 1)
            {
                guessedWord = guessedWord.Remove(guessedWord.Length - 1);
                wordGuessRow--;
                updateOnScreenLetterGuess(emptySpace);
            }
        }
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(guessedWord.Length == 5)
            {
                print("doing some shit");
                wordGuessRow = 1;
                wordGuessColumn--;
                guessedWord = "";
                checkFinalWordInput();
                // logic to check and parse the word AND changing the Y value to Y + 1
            }
            if(guessedWord.Length < 5)
            {
                // logic to throw a GUI for some seconds telling the user the following message "Not enough letters"
            }
            //check if the guessedWord length is equal to is equal to five, if its not then throw a gui
        }
        
    }    

    public void updateOnScreenLetterGuess(char c)
    {
        GameObject.Find("letter" + wordGuessRow + "." + wordGuessColumn).GetComponent<Text>().text = c.ToString();
    }    
    public void displayWarningToUser()
    {

    }
    public void checkFinalWordInput()
    {

    }    
}
