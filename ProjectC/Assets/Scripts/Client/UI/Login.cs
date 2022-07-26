using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Login : UI_Scene
{
    private enum Buttons { LoginButton, SignupButton, SaveID, SearchAccount, SettingButton, QuitButton }
    private enum InputFields { IDField, PasswordField }
    private enum Texts { LoginStateLog }

    // 오브젝트가 활성상태일 경우 실행
    private void Start()
    {
        Init();
    }

    // UI, 이벤트 바인딩
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetButton((int)Buttons.LoginButton).gameObject.BindEvent(StartLogin);
        GetButton((int)Buttons.SignupButton).gameObject.BindEvent(StartSignup);
        GetButton((int)Buttons.SearchAccount).gameObject.BindEvent(StartSearchAccount);
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(StartSetting);
        GetButton((int)Buttons.QuitButton).gameObject.BindEvent(QuitGame);
    }

    // 로그인 기능
    public void StartLogin(PointerEventData data)
    {
        var log = GetText((int)Texts.LoginStateLog);
        var id = GetInputField((int)InputFields.IDField).text;
        var password = GetInputField((int)InputFields.PasswordField).text;
        var canvasGroup = GetComponent<CanvasGroup>();
        var searchData = DataManager.Instance.SelectData(new string[] { "ID", "Password" }, "Account", $"ID = '{id}'");

        // 아이디 혹은 비밀번호가 입력된 값이 없을 경우
        if (id.Equals("") || password.Equals(""))
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "아이디 혹은 비밀번호를 입력해주세요";
            ResetInputField();
            return;
        }
        // 아이디 혹은 비밀번호가 일치하지 않을 경우
        else if (!id.Equals(searchData[0]) || !password.Equals(searchData[1]))
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "아이디 혹은 비밀번호가 일치하지 않습니다";
            ResetInputField();
            return;
        }
        // 아이디, 비밀번호가 모두 일치하는 경우
        else if (id.Equals(searchData[0]) && password.Equals(searchData[1]))
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            log.color = UIManager.Instance.successLogColor;
            log.text = "로그인 성공";
            ResetInputField();
            NetWorkManager.ConnectMaster();
            TimeManager.Instance.StartCoroutine(TimeManager.Instance.TimerForAction(1.0f, 10.0f, WaitMaster, Timeout));
        }
    }

    // 마스터 서버 접속 대기
    public void WaitMaster()
    {
        var log = GetText((int)Texts.LoginStateLog);
        if (StateManager.Instance.currentState_net == (int)StateManager.Net.Master)
        {
            log.color = UIManager.Instance.successLogColor;
            log.text = "서버 접속 성공";
        }
        else
        {
            log.text = "서버 접속중";
        }
    }

    // 접속 타임아웃
    public void Timeout()
    {
        var log = GetText((int)Texts.LoginStateLog);
        if(StateManager.Instance.currentState_net != (int)StateManager.Net.Master)
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "서버 접속 실패 로그인을 다시 시도해주세요(Error : Time Out)";
        }
    }

    // 회원가입 팝업 출력 기능
    public void StartSignup(PointerEventData data)
    {
        UIManager.Instance.ShowPopupUI<UI_Popup>("Signup");
    }

    // 계정찾기 팝업 출력 기능
    public void StartSearchAccount(PointerEventData data)
    {
        UIManager.Instance.ShowPopupUI<UI_Popup>("SearchAccount");
    }

    // 환경설정 팝업 출력 기능
    public void StartSetting(PointerEventData data)
    {

    }

    // 게임종료 기능
    public void QuitGame(PointerEventData data)
    {
        Application.Quit();
    }

    // 계정 입력란 초기화
    public void ResetInputField()
    {
        GetInputField((int)InputFields.IDField).text = "";
        GetInputField((int)InputFields.PasswordField).text = "";
    }
}
