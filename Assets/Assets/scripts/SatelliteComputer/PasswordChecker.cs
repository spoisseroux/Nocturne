using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordChecker : MonoBehaviour
{
    // TextMeshPro to display selected digits
    [SerializeField] GameObject passwordWindow;
    TextMeshProUGUI passwordDisplay;

    // Storing the correct password and the current entered password
    [SerializeField] private string password;
    [SerializeField] private string digitsEntered;

    // Tracking our current digit, and hashmap
    [SerializeField] private int addIndex = 9;
    [SerializeField] private int deleteIndex = 9;
    private Dictionary<int, char> charMap = new Dictionary<int, char>();

    // AudioClips
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip failure;

    // Successful password event
    public static event HandleSuccessfulPassword CorrectPassword;
    public delegate void HandleSuccessfulPassword();

    // Event for playing audio in SatelliteComputer script
    public static event HandleAudio PlayAudio;
    public delegate void HandleAudio(AudioClip clip);

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to our text box
        passwordDisplay = passwordWindow.GetComponent<TextMeshProUGUI>();
        if (passwordDisplay == null)
        {
            Debug.Log("PasswordChecker::Start() --> passwordDisplay variable is null");
        }
        passwordDisplay.text = digitsEntered;

        // set up hashmap
        charMap[8] = '#';
        charMap[10] = '?';
        charMap[11] = '!';
    }

    // Deletes a digit from the password, and reflects this in both the backend and frontend, triggered by an OnClick
    public void DeleteDigit()
    {
        // delete digit
        if (deleteIndex >= 8)
        {
            // rewriting string (strings are immutable in c# ???)
            StringBuilder sb = new StringBuilder(digitsEntered);
            sb[deleteIndex] = charMap[deleteIndex];
            digitsEntered = sb.ToString();
            Debug.Log(digitsEntered);

            // move down our currentIndex pointer
            deleteIndex--;
            addIndex--;
            if (deleteIndex == 9)
            {
                deleteIndex--;
                addIndex = 10;
            }
            else if (addIndex == 9)
            {
                addIndex = 8;
            }

            // update our password text
            passwordDisplay.text = digitsEntered;
        }
    }

    // Adds a digit to the password, and reflects this in the backend and frontend, triggered by an OnClick
    public void AddDigit(char digit)
    {
        // add digit
        if (addIndex < 12)
        {
            // Rrwrite our string using StringBuilder
            StringBuilder sb = new StringBuilder(digitsEntered);
            sb[addIndex] = digit;
            digitsEntered = sb.ToString();

            // move up our currentIndex pointer
            addIndex++;
            if (addIndex == 9)
            {
                addIndex = 10;
                deleteIndex = 8;
            }
            else if (addIndex >= 11)
            {
                deleteIndex = addIndex - 1;
            }

            // update our password text
            passwordDisplay.text = digitsEntered;
        }
    }

    // Checks if our password is correct or not after a request from OnClick event
    public void CheckPassword()
    {
        if (digitsEntered == password)
        {
            OnSuccessfulPassword();
        }
        else
        {
            OnFailedPassword();
        }
    }

    // Handles a successful password entry
    private void OnSuccessfulPassword()
    {
        // play sound
        PlayAudio?.Invoke(success);

        // trigger some behavior in the SatelliteComputer via CorrectPassword?.Invoke()
        CorrectPassword?.Invoke();
    }

    // Handles an incorrect password entry
    private void OnFailedPassword()
    {
        // play sound
        PlayAudio?.Invoke(failure);

        // lock out buttons for a second (??)

    }
}
