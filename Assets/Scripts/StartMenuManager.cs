using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour {
    public GameObject createNewPanel, profileBasePanel, newUserPanel, playerProfsPanel,chooseProfilePanel, canvasBasePanel;
    public InputField enteredName;
    public Button createBtn;

    public void StartBtnClicked()
    {
        //enable the profilePanel
        //diable the remanining UI
        canvasBasePanel.SetActive(false);
        chooseProfilePanel.SetActive(true);
    }

    public void ChangeLevel(int i)
    {
        UIManager.UM.LoadScene(i);
    }

    public void OnEnterNameValueChanged()
    {
        if(enteredName.IsActive())
        {
            if(enteredName.text.Length > 0)
            {
                createBtn.interactable = true;
            }
            else
            {
                createBtn.interactable = false;
            }
        }
    }

    public void OnCreateBtnPressed()
    {
        //store the value of the string of Input field
        // deactivate CreateNewPanel
        // add a new profile with the name present in the input name
        // enable the Profile Panel other elements
        string nameOfUser = enteredName.text.ToString();
        Text userName = newUserPanel.GetComponentInChildren<Text>();
        userName.text = nameOfUser;
        GameObject newProfile = Instantiate(newUserPanel, Vector3.zero, Quaternion.identity) as GameObject;
        newProfile.transform.SetParent(playerProfsPanel.transform, false);
        createNewPanel.SetActive(false);
        profileBasePanel.SetActive(true);
    }

    public void OnCreateNewBtnPressed()
    {
        //activate the CreateNewPanel
        //Deactivate the remaining elements of Profile Panel
        profileBasePanel.SetActive(false);
        createNewPanel.SetActive(true);
    }
}
