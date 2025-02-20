using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringController : MonoBehaviour
{
    public enum Note { Gong, Shang, Jue, Zhi, Yu }; // 五声音阶
    public Note currentNote; // 当前琴弦的音高
    public float[] noteFrequencies = { 261.63f, 293.66f, 329.63f, 392.00f, 440.00f }; // 对应频率

    void Update()
    {
        // 通过键盘切换琴弦
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentNote = Note.Gong;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentNote = Note.Shang;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentNote = Note.Jue;
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentNote = Note.Zhi;
        if (Input.GetKeyDown(KeyCode.Alpha5)) currentNote = Note.Yu;
    }

    public float GetCurrentFrequency()
    {
        return noteFrequencies[(int)currentNote];
    }
}
