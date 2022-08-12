using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatricesOutils : MonoBehaviour
{
    public Matrix4x4 mat_base_link_base = Matrix4x4.identity;
    public Matrix4x4 mat_flange_tool0 = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_tool0_flange = Matrix4x4.identity;

    void Start()
    {
        mat_tool0_flange = mat_flange_tool0.inverse;
    }
}
