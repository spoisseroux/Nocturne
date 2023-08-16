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

    // Passwords
    [SerializeField] private string password;
    [SerializeField] private string digitsEntered;

    // Tracking our place
    [SerializeField] private int addIndex = 10;
    [SerializeField] private int deleteIndex = 9;

    // AudioClips
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip failure;

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
    }

    public void DeleteDigit()
    {
        // delete digit
        if (deleteIndex >= 10)
        {
            // Rewriting string because for some reason strings are immutable in c# :///
            StringBuilder sb = new StringBuilder(digitsEntered);
            sb[deleteIndex] = 'X'; // needs to be char
            digitsEntered = sb.ToString();
            Debug.Log(digitsEntered);

            // move down our currentIndex pointer
            deleteIndex--;
            addIndex--;

            // update our password text
            passwordDisplay.text = digitsEntered;
        }
    }

    public void AddDigit(char digit)
    {
        // add digit
        if (addIndex < 13)
        {
            // Rewrite our string using StringBuilder
            StringBuilder sb = new StringBuilder(digitsEntered);
            sb[addIndex] = digit;
            digitsEntered = sb.ToString();

            // move up our currentIndex pointer
            addIndex++;
            deleteIndex++;

            // update our password text
            passwordDisplay.text = digitsEntered;
        }
    }

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

    private void OnSuccessfulPassword()
    {
        // idk yet, probly dissolve out window and cue cutscene, shift to purple level
    }

    private void OnFailedPassword()
    {
        // play sound, (???) clear the password completely or (???) leave it alone
    }
}
