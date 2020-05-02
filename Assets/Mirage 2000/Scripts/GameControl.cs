using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameControl : MonoBehaviour
{
    #region Variables
    public static GameControl instance;

    public bool isInLandingPage;
    public bool isInDetailsPage;
    public bool isTrainingStarts;

    public GameObject landingPageUIPanel;
    public GameObject detailsPageUIPanel;

    public SilantroController controller;

    public TMP_Dropdown inputDD;

    public GameObject flightDetailsCanvas;

    public GameObject warningMsgPanelInDetails;
    public bool isWarningDisplaysInDetailsPage;

    public GameObject warningMsgPanelInTraining;
    public bool isWarningDisplaysInTrainingPage;
    #endregion

    #region Builtin Methods
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LandingPage();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isInLandingPage == true)
            {
                QuitApplication();
            }
            else if (isInDetailsPage == true)
            {
                WarningMsgInDetailsPage();
            }
            else if(isTrainingStarts == true)
            {
                WarningMsgInTrainingPage();
            }
        }
    }
    #endregion

    #region Custom Methods
    public void InputSelection (TMP_Dropdown dd)
    {
        controller.InputSelection(dd.value);
    }

    public void SetInput (int id)
    {
        inputDD.value = id;
    }

    public void QuitApplication ()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void DetailsPage ()
    {
        landingPageUIPanel.SetActive(false);
        detailsPageUIPanel.SetActive(true);
        flightDetailsCanvas.SetActive(false);
        isInLandingPage = false;
        isInDetailsPage = true;

        if(isTrainingStarts == true)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
        isTrainingStarts = false;

        warningMsgPanelInDetails.SetActive(false);
        isWarningDisplaysInDetailsPage = false;

        warningMsgPanelInTraining.SetActive(false);
        isWarningDisplaysInTrainingPage = false;

        AudioListener.pause = true;
    }

    public void LandingPage()
    {
        landingPageUIPanel.SetActive(true);
        detailsPageUIPanel.SetActive(false);
        flightDetailsCanvas.SetActive(false);
        isInLandingPage = true;

        if(isInDetailsPage == true || isTrainingStarts == true)
        {
            SceneManager.LoadScene(gameObject.scene.name);
        }
        isInDetailsPage = false;
        isTrainingStarts = false;

        warningMsgPanelInDetails.SetActive(false);
        isWarningDisplaysInDetailsPage = false;

        warningMsgPanelInTraining.SetActive(false);
        isWarningDisplaysInTrainingPage = false;
        Time.timeScale = 1f;

        AudioListener.pause = true;
    }

    public void ToTraining()
    {
        landingPageUIPanel.SetActive(false);
        detailsPageUIPanel.SetActive(false);
        flightDetailsCanvas.SetActive(true);
        isInLandingPage = false;
        isInDetailsPage = false;
        isTrainingStarts = true;

        warningMsgPanelInDetails.SetActive(false);
        isWarningDisplaysInDetailsPage = false;

        warningMsgPanelInTraining.SetActive(false);
        isWarningDisplaysInTrainingPage = false;
        Time.timeScale = 1f;

        AudioListener.pause = false;
    }

    public void WarningMsgInDetailsPage ()
    {
        if(isWarningDisplaysInDetailsPage == false)
        {
            warningMsgPanelInDetails.SetActive(true);
            isWarningDisplaysInDetailsPage = true;
        }
        else
        {
            warningMsgPanelInDetails.SetActive(false);
            isWarningDisplaysInDetailsPage = false;
        }
    }

    public void WarningMsgInTrainingPage()
    {
        if (isWarningDisplaysInTrainingPage == false)
        {
            Time.timeScale = 0f;
            warningMsgPanelInTraining.SetActive(true);
            isWarningDisplaysInTrainingPage = true;
            AudioListener.pause = true;
        }
        else
        {
            warningMsgPanelInTraining.SetActive(false);
            isWarningDisplaysInTrainingPage = false;
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }

    #endregion
}
