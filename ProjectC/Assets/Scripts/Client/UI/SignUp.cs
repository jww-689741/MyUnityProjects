using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SignUp : UI_Popup
{
    private enum Buttons { ChackID, SignupButton, Exit }
    private enum InputFields { IDField, PasswordField, ConfirmPasswordField, NameField, ForwardRRNField, BackwardRRNField }
    private enum Texts { SignupStateLog, ChackIDLog }

    private bool idCheckFlag = false;

    // 아이디 생성 규칙 정규식 ( 숫자, 영문자 최소 1개 이상, 최소 6문자 이상 )
    private Regex idNormalPattern = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9]).{6,}$", RegexOptions.IgnorePatternWhitespace);

    // 비밀번호 생성 규칙 정규식 ( 숫자, 영문자, 특수문자 최소 1개 이상, 최소 10문자 이상 )
    private Regex passwordNormalPattern = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$", RegexOptions.IgnorePatternWhitespace);

    // 주민등록번호 규칙 정규식
    private Regex rrnNormalPattern = new Regex(@"(\d{6}[ ,-]-?[1-4]\d{6})|(\d{6}[ ,-]?[1-4])", RegexOptions.IgnorePatternWhitespace);


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

        GetButton((int)Buttons.SignupButton).gameObject.BindEvent(SignupAction);
        GetButton((int)Buttons.ChackID).gameObject.BindEvent(CheckId);
        GetButton((int)Buttons.Exit).gameObject.BindEvent(ClosePopup);

    }

    // 회원가입
    public void SignupAction(PointerEventData data)
    {
        var log = GetText((int)Texts.SignupStateLog);
        var id = GetInputField((int)InputFields.IDField).text;
        var password = GetInputField((int)InputFields.PasswordField).text;
        var name = GetInputField((int)InputFields.NameField).text;
        var rrn = GetInputField((int)InputFields.ForwardRRNField).text + "-" + GetInputField((int)InputFields.BackwardRRNField).text;
        var canvasGroup = GetComponent<CanvasGroup>();

        if (!idCheckFlag)
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "아이디 중복검사를 진행해주세요";
            return;
        }
        else if (!CheckPassword())
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "비밀번호가 일치하지 않거나 사용 불가 문자 조합입니다";
            return;
        }
        else if(name.Equals("") || name == null)
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "성명을 입력해주세요";
            return;
        }
        else if(!rrnNormalPattern.IsMatch(rrn) || rrn == null)
        {
            Debug.Log(rrn);
            log.color = UIManager.Instance.errorLogColor;
            log.text = "주민등록번호를 확인해주세요";
            return;
        }
            
        // 데이터베이스 삽입
        if(DataManager.Instance.InsertData("Account(ID,Password,Name,ResidentRegistrationNumber)", $"('{id}','{password}','{name}','{rrn}')"))
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            log.color = UIManager.Instance.successLogColor;
            log.text = "회원가입이 정상적으로 처리되었습니다";
        }
        else
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "서버 연결에 실패했습니다 다시 시도해주세요";
            return;
        }
    }

    // 아이디 중복검사 - 데이터베이스 연동 필요
    public void CheckId(PointerEventData data)
    {
        var id = GetInputField((int)InputFields.IDField).text;
        var log = GetText((int)Texts.ChackIDLog);

        // ID가 중복이거나, 공란 혹은 생성 규칙에 어긋날 경우
        if (id.Equals(DataManager.Instance.SelectData("ID","Account",$"ID = '{id}'"))
            || id.Equals("")
            || !idNormalPattern.IsMatch(id))
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "사용불가";
            idCheckFlag = false;
        }
        // 생성 가능한 경우
        else
        {
            log.color = UIManager.Instance.successLogColor;
            log.text = "사용가능";
            idCheckFlag = true;
        }
    }

    // 패스워드 일치 검사
    public bool CheckPassword()
    {
        var password = GetInputField((int)InputFields.PasswordField).text;
        var confirmPassword = GetInputField((int)InputFields.ConfirmPasswordField).text;

        if (password.Equals(confirmPassword) || passwordNormalPattern.IsMatch(confirmPassword)) return true;
        else return false;
    }

}