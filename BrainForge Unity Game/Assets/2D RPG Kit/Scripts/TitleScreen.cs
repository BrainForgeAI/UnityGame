using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.IO;

public class TitleScreen : MonoBehaviour
{
    public int music;
    public string newGameScene;

    public GameObject continueButton;
    public GameObject mainMenu;
    public GameObject difficultySettings;
    public Text pressStartText;
    public GameObject pressStart;
    public Button newGameBtn;
    public Button continueBtn;
    public Button normalBtn;

    public GameObject player;
    public EventSystem es;

    public string loadGameScene;

    public GameObject fileUploadScreen;
    public Button selectFileButton;
    public TMP_Text selectedFileText;
    public Button uploadButton;

    private string selectedFilePath;


    // variable declaration how could I have forgotten this
    private CanvasGroup fileUploadCanvasGroup;


    // Use this for initialization
    void Start()
    {
        // initialization
        fileUploadCanvasGroup = fileUploadScreen.GetComponent<CanvasGroup>();
        if (fileUploadCanvasGroup == null)
        {
            fileUploadCanvasGroup = fileUploadScreen.AddComponent<CanvasGroup>();
        }

        player = GameObject.Find("Player(Clone)");
        player.transform.position = new Vector2(3, 1);

        if (ControlManager.instance.mobile)
        {
            Screen.SetResolution(1280, 720, true);
        }
        HideFileUploadScreen();
        Screen.SetResolution(1280, 720, true);
        StopCoroutine(PressStartCo());
        StartCoroutine(PressStartCo());
        ScreenFade.instance.fadeScreenObject.SetActive(false);
        AudioManager.instance.PlayBGM(music);
        // Added listeners for file upload buttons
        selectFileButton.onClick.AddListener(SelectFile);
        uploadButton.onClick.AddListener(() => StartCoroutine(UploadFile()));

        StartCoroutine(DontShowcontrols());
    }

    public IEnumerator DontShowcontrols()
    {
        yield return new WaitForEndOfFrame();
        GameMenu.instance.touchMenuButton.SetActive(false);
        GameMenu.instance.touchController.SetActive(false);
        GameMenu.instance.touchConfirmButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || Input.GetButtonDown("Submit"))
        {
            if (!mainMenu.activeInHierarchy)
            {
                StopCoroutine(PressStartCo());
                PlayButtonSound(0);

                if (!difficultySettings.activeInHierarchy)
                {
                    ShowMainMenu();
                }
            }
        }
    }

    public void PlayButtonSound(int buttonSound)
    {
        AudioManager.instance.PlaySFX(buttonSound);
    }

    public void ShowMainMenu()
    {
        pressStart.SetActive(false);
        mainMenu.SetActive(true);
        // Added new method
        HideFileUploadScreen();

        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueButton.SetActive(true);
            continueBtn.interactable = true;
        }
        else
        {
            continueBtn.interactable = false;
        }

        if (ControlManager.instance.mobile == false)
        {
            es.SetSelectedGameObject(continueBtn.gameObject);
            continueBtn.Select();
            continueBtn.OnSelect(null);
            StartCoroutine(WaitForButton());
        }
    }

    public void Continue()
    {
        ScreenFade.instance.fadeScreenObject.SetActive(true);

        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }

        GameManager.instance.cutSceneActive = false;
        SceneManager.LoadScene(loadGameScene);
    }

    public void OpenDifficultySettings()
{
    mainMenu.SetActive(false);
    difficultySettings.SetActive(true);

    if (ControlManager.instance.mobile == false)
    {
        es.SetSelectedGameObject(normalBtn.gameObject);
        normalBtn.Select();
        normalBtn.OnSelect(null);
    }

    // Set up difficulty buttons if not already done
    SetupDifficultyButtons();
}

private void SetupDifficultyButtons()
{
    // Assuming you have buttons for each difficulty
    Button easyBtn = difficultySettings.transform.Find("EasyButton").GetComponent<Button>();
    Button normalBtn = difficultySettings.transform.Find("NormalButton").GetComponent<Button>();
    Button hardBtn = difficultySettings.transform.Find("HardButton").GetComponent<Button>();

    easyBtn.onClick.RemoveAllListeners();
    normalBtn.onClick.RemoveAllListeners();
    hardBtn.onClick.RemoveAllListeners();

    easyBtn.onClick.AddListener(() => StartNewGame(0));
    normalBtn.onClick.AddListener(() => StartNewGame(1));
    hardBtn.onClick.AddListener(() => StartNewGame(2));
}

private void StartNewGame(int difficulty)
{
    SetDifficulty(difficulty);
    ScreenFade.instance.fadeScreenObject.SetActive(true);
    SceneManager.LoadScene(newGameScene);
}

public void NewGame()
    {
        Debug.Log("NewGame method called");
        mainMenu.SetActive(false);
        ShowFileUploadScreen();

        // Make sure these are set up only once, not every time NewGame is called
        if (selectFileButton.onClick.GetPersistentEventCount() == 0)
            selectFileButton.onClick.AddListener(SelectFile);
        if (uploadButton.onClick.GetPersistentEventCount() == 0)
            uploadButton.onClick.AddListener(() => StartCoroutine(UploadFile()));
    }

    private void ShowFileUploadScreen()
    {
        Debug.Log("ShowFileUploadScreen method called");
        fileUploadScreen.SetActive(true);  // Ensure the GameObject is active

        if (fileUploadCanvasGroup != null)
        {
            fileUploadCanvasGroup.alpha = 1;
            fileUploadCanvasGroup.interactable = true;
            fileUploadCanvasGroup.blocksRaycasts = true;
        }

        fileUploadScreen.transform.SetAsLastSibling();

        selectedFileText.text = "No file selected";
        uploadButton.interactable = false;

        if (mainMenu != null) mainMenu.SetActive(false);
        if (difficultySettings != null) difficultySettings.SetActive(false);

        Debug.Log("File upload screen should be visible now");
    }

    private void HideFileUploadScreen()
    {
        fileUploadScreen.SetActive(false);
        CanvasGroup canvasGroup = fileUploadScreen.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void SelectFile()
    {
        #if UNITY_EDITOR
        selectedFilePath = UnityEditor.EditorUtility.OpenFilePanel("Select Syllabus", "", "pdf,doc,docx");
        #elif UNITY_STANDALONE
        selectedFilePath = StandaloneFileBrowser.OpenFilePanel("Select Syllabus", "", "pdf,doc,docx", false)[0];
        #endif

        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            selectedFileText.text = Path.GetFileName(selectedFilePath);
            Debug.Log(selectedFileText.text);
            uploadButton.interactable = true;
        }
    }

    private IEnumerator UploadFile()
    {
        if (string.IsNullOrEmpty(Path.GetFileName(selectedFilePath)))
        {
            Debug.LogError("No file selected");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", File.ReadAllBytes(selectedFilePath), Path.GetFileName(selectedFilePath));

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5001/load_syllabus", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("File upload complete!");

                HideFileUploadScreen();
                OpenDifficultySettings();
            }
        }
    }

    private void SetDifficulty(int difficulty)
    {
        GameManager.instance.easy = difficulty == 0;
        GameManager.instance.normal = difficulty == 1;
        GameManager.instance.hard = difficulty == 2;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public IEnumerator PressStartCo()
    {
        while (true)
        {
            switch (pressStartText.color.a.ToString())
            {
                case "0":
                    pressStartText.color = new Color(pressStartText.color.r, pressStartText.color.g, pressStartText.color.b, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    pressStartText.color = new Color(pressStartText.color.r, pressStartText.color.g, pressStartText.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }

    public IEnumerator WaitForButton()
    {
        yield return new WaitForSeconds(0.1f);

        newGameBtn.interactable = true;

        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueBtn.interactable = true;
        }

        yield return new WaitForSeconds(0.1f);

        pressStart.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
