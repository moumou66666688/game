using UnityEngine;

public class CraneManager : MonoBehaviour
{
    public static CraneManager instance;
    public Animator craneGong;  // ����ǧֽ��
    public Animator craneShang; // ����ǧֽ��
    public Animator craneJue;   // ����ǧֽ��
    public Animator craneZhi;   // ����ǧֽ��
    public Animator craneYu;    // ����ǧֽ��

    void Awake() => instance = this;

    public void PlayAnimation(string noteName)
    {
        switch (noteName)
        {
            case "��": craneGong.Play("Jump"); break;
            case "��": craneShang.Play("Jump"); break;
            case "��": craneJue.Play("Jump"); break;
            case "��": craneZhi.Play("Jump"); break;
            case "��": craneYu.Play("Jump"); break;
        }
    }
}