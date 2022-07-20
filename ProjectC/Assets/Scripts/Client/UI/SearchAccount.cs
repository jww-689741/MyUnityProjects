using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SearchAccount : UI_Popup
{
    private enum Buttons { SearchID, ResetPassword, Exit}
    private enum InputFields { NameField, IDCardNumberField_Forward, IDCardNumberField_Backward, IDField, PasswordField, ConfirmPasswordField }
    private enum Texts { Log }

    // 비밀번호 생성 규칙 정규식 ( 숫자, 영문자, 특수문자 최소 1개 이상, 최소 10문자 이상 )
    private Regex passwordNormalPattern = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$", RegexOptions.IgnorePatternWhitespace);

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
        //GetButton((int)Buttons.ResetPassword).gameObject.BindEvent(ResetPassword);
        GetButton((int)Buttons.Exit).gameObject.BindEvent(ClosePopup);
    }

    // 아이디 검색
    public void SearchID(PointerEventData data)
    {
        var name = GetInputField((int)InputFields.NameField).text;
        var idCardNumber = GetInputField((int)InputFields.IDCardNumberField_Forward).text + "-" + GetInputField((int)InputFields.IDCardNumberField_Backward).text;
        var log = GetText((int)Texts.Log);


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
