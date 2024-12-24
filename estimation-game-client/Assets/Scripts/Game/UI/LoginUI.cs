using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    //[SerializeField] private List<ButtonComponent> m_Buttons = new();
    //[SerializeField] private List<InputComponent> m_Inputs = new();
    //private Dictionary<string, UIComponent> m_Components = new();
    //public Dictionary<string, UIComponent> Components => m_Components;


    [SerializeField] private TMP_InputField m_Username;
    [SerializeField] private TMP_InputField m_Password;
    [SerializeField] private Button m_LoginButton;

    //private void Start()
    //{
    //    Init();
    //}

    //private void Init()
    //{
    //    //UIManager.Instance.LocalUI = this;
    //    foreach (var btn in m_Buttons)
    //        m_Components.Add(btn.Key, btn);
    //    foreach (var input in m_Inputs)
    //        m_Components.Add(input.Key, input);
    //}
    public void Connect()
    {
       
       
        string username = m_Username.text;
        string password = m_Password.text;
        NetworkEvents.OnConnectRequest(username, password);
    }

}