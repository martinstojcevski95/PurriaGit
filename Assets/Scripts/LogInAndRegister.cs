using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInAndRegister : MonoBehaviour
{
    // Start is called before the first frame update
    FirebaseAuth auth;
    public InputField RegistrationEmail, RegistrationPassword, LogInEmail, LogInPassword;
    bool isDone;
    public string userID;
    public string UserName;


    public static LogInAndRegister Instance;

    public string user, password;
    public int AutoLogIn;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnLoadedScene;
        AutoLogIn = PlayerPrefs.GetInt("autolog");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    }

    private void OnLoadedScene(Scene arg0, LoadSceneMode arg1)
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AutoLogIn == 1)
        {
            LogInEmail.text = PlayerPrefs.GetString("user");
            LogInPassword.text = PlayerPrefs.GetString("password");

        }
    }




    //LOGIN/REGISTER PART

    public void Registration()
    {


        auth.CreateUserWithEmailAndPasswordAsync(RegistrationEmail.text, RegistrationPassword.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            UserName = newUser.UserId;
            Debug.Log("za da se predade " + newUser.UserId);
            Debug.Log("USER NAME " + UserName);

            UIManager.Instance.FromRegisterToLogin();
        });
    }




    public void LogIn()
    {
        auth.SignInWithEmailAndPasswordAsync(LogInEmail.text, LogInPassword.text).ContinueWith(task =>
              {
                  if (task.IsCanceled)
                  {
                      Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                      return;
                  }
                  if (task.IsFaulted)
                  {
                      Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                      return;
                  }
                  Firebase.Auth.FirebaseUser newUser = task.Result;
                  Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                  UserName = newUser.UserId;

                  Debug.Log("za da se predade " + newUser.UserId);
                  Debug.Log("USER NAME " + UserName);

              });
     //   TimeManager.Instance.canCount = 1;
        UIManager.Instance.CloseLogRegUI();
        SetLogInCreds();
    }

    void SetLogInCreds()
    {

        PlayerPrefs.SetString("user", LogInEmail.text);
        PlayerPrefs.SetString("password", LogInPassword.text);
        AutoLogIn = 1;
        PlayerPrefs.SetInt("autolog", AutoLogIn);

    }

    void OnApplicationQuit()
    {
        TimeManager.Instance.canCount = 0;
    }
}
