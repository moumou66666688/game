using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LoginClass : MonoBehaviour
{
    //进入前变量
    public InputField username, password, confirmPassword;
    public Text reminderText;
    public int errorsNum;
    public Button loginButton;
    public GameObject hallSetUI, loginUI;
    //进入后变量
    public static string myUsername;

    public void Register()
    {
        if (PlayerPrefs.GetString(username.text) == "")
        {
            if (password.text == confirmPassword.text)
            {
                PlayerPrefs.SetString(username.text, username.text);
                PlayerPrefs.SetString(username.text + "password", password.text);
                reminderText.text = "注册成功！";
            }
            else
            {
                reminderText.text = "两次密码输入不一致";
            }
        }
        else
        {
            reminderText.text = "用户已存在";
        }

    }
    private void Recovery()
    {
        loginButton.interactable = true;
    }
    public void Login()
    {
        if (PlayerPrefs.GetString(username.text) != "")
        {
            if (PlayerPrefs.GetString(username.text + "password") == password.text)
            {
                reminderText.text = "登录成功";

                myUsername = username.text;
                hallSetUI.SetActive(true);
                loginUI.SetActive(false);
                SceneManager.LoadScene(1);
            }
            else
            {
                reminderText.text = "密码错误";
                errorsNum++;
                if (errorsNum >= 3)
                {
                    reminderText.text = "连续错误3次，请30秒后再试！";
                    loginButton.interactable = false;
                    Invoke("Recovery", 5);
                    errorsNum = 0;
                }
            }
        }
        else
        {
            reminderText.text = "账号不存在";
        }
    }
}

