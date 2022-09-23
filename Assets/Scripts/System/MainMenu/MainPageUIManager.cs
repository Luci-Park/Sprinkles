using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]public enum PageTypes { Main, CreateInvite, Enter, Loading }
public class MainPageUIManager : MonoBehaviour
{
    [SerializeField] GameObject[] pages;

    [SerializeField] Text showCodeTxt;
    
    [SerializeField] InputField codeInputer;

    [SerializeField] GameObject click2Copy;
    [SerializeField] GameObject copied;

    [SerializeField] GameObject wrongCodeWarningImage;
    [SerializeField] Animator wrongCodeInputAnim;

    Stack<PageTypes> pageHistory = new Stack<PageTypes>();

    public static MainPageUIManager instance;

    //---------------------------------------------
    #region Pages Functions
    //---------------------------------------------

    public void TurnOnPage(PageTypes page)
    {
        if (PageTypes.Main == page)
        {
            TurnOnMainPage();
        }
        else if (PageTypes.CreateInvite == page)
        {
            TurnOnCreateInvitePage();
        }
        else if (PageTypes.Enter == page)
        {
            TurnOnEnterInvitePage();
        }
        else if (PageTypes.Loading == page)
        {
            TurnOnLoadingPage();
        }
    }

    //---------------------------------------------

    public void TurnOnPrevPage()
    {
        pageHistory.Pop();//currentPage
        TurnOnPage(pageHistory.Pop()); //prevPage
    }

    //---------------------------------------------

    void TurnOnMainPage()
    {
        if (NetworkManager.instance != null)
        {
            NetworkManager.instance.LeaveNetworkRoom();
        }
        TurnOnPageUI(PageTypes.Main);
    }

    //---------------------------------------------

    void TurnOnEnterInvitePage()
    {
        wrongCodeWarningImage.SetActive(false);
        TurnOnPageUI(PageTypes.Enter);
    }

    //---------------------------------------------

    void TurnOnLoadingPage()
    {
        TurnOnPageUI(PageTypes.Loading);
    }

    //---------------------------------------------

    void TurnOnCreateInvitePage()
    {
        TurnOnPageUI(PageTypes.CreateInvite);
        CopyUIReset();
    }

    //---------------------------------------------

    void TurnOnPageUI(PageTypes page)
    {
        for (int i = 0; i < pages.Length; ++i)
        {
            if (i == (int)page)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
        pageHistory.Push(page);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Code UI Functions
    //---------------------------------------------
    
    public void OnCodeCreationConfirmed(string code)
    {
        ShowCode(code);
    }

    //---------------------------------------------

    public string GetCode()
    {
        return codeInputer.text;
    }

    //---------------------------------------------

    public void CopyCode()
    {
        if (string.IsNullOrEmpty(showCodeTxt.text))
        {
            return;
        }
        click2Copy.SetActive(false);
        copied.SetActive(true);

        showCodeTxt.text.CopyToClipboard();

    }

    //--------------------------------------------

    public void CopyUIReset()
    {
        click2Copy.SetActive(true);
        copied.SetActive(false);
    }

    //---------------------------------------------

    public void OnCodeEntered()
    {
        NetworkManager.instance.JoinRoomWithCode();
    }
    
    //---------------------------------------------

    public void WrongCode()
    {
        wrongCodeWarningImage.SetActive(true);
        wrongCodeInputAnim.SetTrigger("WrongInput");
    }

    //---------------------------------------------
    
    void ShowCode(string code)
    {
        showCodeTxt.text = code;
    }

    
    //---------------------------------------------
    #endregion
    //---------------------------------------------


    //---------------------------------------------
    #region ETC
    //---------------------------------------------

    void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------

    private void Start()
    {
        Singleton();
        TurnOnMainPage();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



}
