using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClass : MonoBehaviour
{
    public void TiaoZhuanOne()
    {
        SceneManager.LoadScene("one");
    }
    public void TiaoZhuanTwo()
    {
        SceneManager.LoadScene("two");
    }
    public void TiaoZhuanThree()
    {
        SceneManager.LoadScene("three");
    }
    
    public void TuiChuChangJing()
    {

        SceneManager.LoadScene("login");

    }
}
