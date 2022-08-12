using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRobot : MonoBehaviour
{
    public bool ClockWise;
    public bool CounterClockWise;
    public bool Right;
    public bool Left;

    public void MoveClockWise()
    {
        ClockWise = true;
        CounterClockWise = false;
    }

    public void StopMove()
    {
        ClockWise = false;
        CounterClockWise = false;
    }

    public void MoveCounterClockWise()
    {
        CounterClockWise = true;
        ClockWise = false;
    }

    public void ChangeJointR()
    {
        Right = true;
        ClockWise = false;
        CounterClockWise = false;
    }

    public void ChangeJointL()
    {
        Left = true;
        ClockWise = false;
        CounterClockWise = false;
    }
}