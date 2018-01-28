using UnityEngine;
using System.Collections;
using UnityEngine.UI; // include UI namespace since references UI Buttons directly
using UnityEngine.EventSystems; // include EventSystems namespace so can set initial input for controller support
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour {


	// references to Submenus
	public GameObject _MainMenu;
	public GameObject _AboutMenu;
	//public GameObject HC1;
	//public GameObject BC1;
	// references to Button GameObjects
	public GameObject MenuDefaultButton;
	public GameObject AboutDefaultButton;
	public GameObject QuitButton;

	
	// reference the titleText so we can change it dynamically
	public Text titleText;

	// store the initial title so we can set it back
	private string _mainTitle;

	// init the menu
	void Awake()
	{
		// store the initial title so we can set it back
		_mainTitle = titleText.text;

		// determine if Quit button should be shown
		displayQuitWhenAppropriate();

		// Show the proper menu
		ShowMenu("MainMenu");
	}
		

	// determine if the QUIT button should be present based on what platform the game is running on
	void displayQuitWhenAppropriate() 
	{
		switch (Application.platform) {
			// platforms that should have quit button
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.LinuxPlayer:
				 QuitButton.SetActive(true);
				break;

			// platforms that should not have quit button
			// note: included just for demonstration purposed since
			// default will cover all of these. 
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.WebGLPlayer: 
				QuitButton.SetActive(false);
				break;

			// all other platforms default to no quit button
			default:
				QuitButton.SetActive(false);
				break;
		}
	}

	// Public functions below that are available via the UI Event Triggers, such as on Buttons.

	// Show the proper menu
	public void ShowMenu(string name)
	{
		// turn all menus off
		_MainMenu.SetActive (false);
		_AboutMenu.SetActive(false);
		//HC1.SetActive (false);
		//BC1.SetActive (false);

		// turn on desired menu and set default selected button for controller input
		switch(name) {
		case "MainMenu":
			_MainMenu.SetActive (true);
			//HC1.SetActive (true);
			//BC1.SetActive (true);
			EventSystem.current.SetSelectedGameObject (MenuDefaultButton);
			titleText.text = _mainTitle;
			break;
		case "About":
			_AboutMenu.SetActive(true);
			EventSystem.current.SetSelectedGameObject (AboutDefaultButton);
			break;
		}
	}

	// load the specified Unity level
	public void loadLevel(string leveltoLoad)
	{
		
		// load the specified level
		SceneManager.LoadScene (leveltoLoad);

	}
}
