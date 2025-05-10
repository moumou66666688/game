using UnityEngine;

public class CraneManager : MonoBehaviour
{
    public static CraneManager instance;
    public Animator craneGong;  // ¹¬ÏÒÇ§Ö½º×
    public Animator craneShang; // ÉÌÏÒÇ§Ö½º×
    public Animator craneJue;   // ½ÇÏÒÇ§Ö½º×
    public Animator craneZhi;   // áçÏÒÇ§Ö½º×
    public Animator craneYu;    // ÓðÏÒÇ§Ö½º×

    void Awake() => instance = this;

    public void PlayAnimation(string noteName)
    {
        switch (noteName)
        {
            case "¹¬": craneGong.Play("Jump"); break;
            case "ÉÌ": craneShang.Play("Jump"); break;
            case "½Ç": craneJue.Play("Jump"); break;
            case "áç": craneZhi.Play("Jump"); break;
            case "Óð": craneYu.Play("Jump"); break;
        }
    }
}