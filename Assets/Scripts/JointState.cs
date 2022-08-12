using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointState
{
    public string[] name;
    public float[] position;
    public float[] velocity;
    public float[] effort;

    public JointState()
    {
        name = new string[6];
        position = null;
        velocity = null;
        effort = null;
    }
}