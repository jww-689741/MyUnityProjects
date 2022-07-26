using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SearchAccount : UI_Popup
{
    private enum Buttons { SearchID, ResetPassword, Exit }
    private enum InputFields { NameField, RRNField_Forward, RRNField_Backward, IDField, PasswordField, ConfirmPasswordField }
    private enum Texts { Log }

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

        GetButton((int)Buttons.SearchID).gameObject.BindEvent(SearchID);
        GetButton((int)Buttons.ResetPassword).gameObject.BindEvent(ResetPassword);
        GetButton((int)Buttons.Exit).gameObject.BindEvent(ClosePopup);
    }

    // 아이디 검색
    public void SearchID(PointerEventData data)
    {
        var name = GetInputField((int)InputFields.NameField).text;
        var rrn = GetInputField((int)InputFields.RRNField_Forward).text + "-" + GetInputField((int)InputFields.RRNField_Backward).text;
        var log = GetText((int)Texts.Log);
        var result = DataManager.Instance.SelectData("ID", "Account", $"Name = '{name}' AND ResidentRegistrationNumber = '{rrn}'");
        Debug.Log(result);
        if(result == null)
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "계정정보가 존재하지않습니다";
            ResetInputField();
        }
        else
        {
            log.color = UIManager.Instance.successLogColor;
            log.text = result;
            GetButton((int)Buttons.ResetPassword).interactable = true;
            GetInputField((int)InputFields.IDField).interactable = true;
            GetInputField((int)InputFields.PasswordField).interactable = true;
            GetInputField((int)InputFields.ConfirmPasswordField).interactable = true;
        }   
    }

    // 패스워드 재설정
    public void ResetPassword(PointerEventData data)
    {
        var log = GetText((int)Texts.Log);
        var id = GetInputField((int)InputFields.IDField).text;
        var password = GetInputField((int)InputFields.PasswordField).text;
        if (DataManager.Instance.UpdateData("Account",$"Password = '{password}'",$"ID = '{id}'"))
        {
            log.color = UIManager.Instance.successLogColor;
            log.text = "비밀번호가 정상적으로 변경되었습니다";
        }
        else
        {
            log.color = UIManager.Instance.errorLogColor;
            log.text = "비밀번호 변경에 실패했습니다";
            ResetInputField();
        }
    }

    // 패스워드 일치 검사
    public bool CheckPassword()
    {
        var password = GetInputField((int)InputFields.PasswordField).text;
        var confirmPassword = GetInputField((int)InputFields.ConfirmPasswordField).text;

        if (password.Equals(confirmPassword) || DataManager.Instance.passwordNormalPattern.IsMatch(confirmPassword)) return true;
        else return false;
    }

    // 입력란 초기화
    public void ResetInputField()
    {
        for(int i = 0;i < Enum.GetNames(typeof(InputFields)).Length; i++)
        {
            GetInputField((int)(InputFields)Enum.ToObject(typeof(InputFields), i)).text = "";
        }
    }
}
