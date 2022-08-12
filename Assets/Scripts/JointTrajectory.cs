using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointTrajectory
{
    public string[] joint_names;
    public JointTrajectoryPoint[] points;

    public JointTrajectory()
    {
        joint_names = null;
        points = null;
    }
}
