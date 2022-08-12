using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class Translation : MonoBehaviour
{
    public PlacementObjets placement_objets;
    public GameObject triedre_table_translation;

    public void PlusX()
    {
        Vector3 deplacement = placement_objets.directionu1.normalized * 0.002f;

        Matrix4x4 trans_p_x = Matrix4x4.identity;
        trans_p_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        placement_objets.mat_table_tablecalib = placement_objets.mat_table_tablecalib * trans_p_x;

        placement_objets.mat_table_monde = placement_objets.mat_table_tablecalib * placement_objets.mat_tablecalib_monde;
        placement_objets.mat_monde_table = placement_objets.mat_table_monde.inverse;

        triedre_table_translation.transform.position = new Vector3(placement_objets.mat_table_monde[0, 3], placement_objets.mat_table_monde[1, 3], placement_objets.mat_table_monde[2, 3]);
    }

    public void MinusX()
    {
        Vector3 deplacement = -placement_objets.directionu1.normalized * 0.002f;

        Matrix4x4 trans_m_x = Matrix4x4.identity;
        trans_m_x.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        placement_objets.mat_table_tablecalib = placement_objets.mat_table_tablecalib * trans_m_x;

        placement_objets.mat_table_monde = placement_objets.mat_table_tablecalib * placement_objets.mat_tablecalib_monde;
        placement_objets.mat_monde_table = placement_objets.mat_table_monde.inverse;

        triedre_table_translation.transform.position = new Vector3(placement_objets.mat_table_monde[0, 3], placement_objets.mat_table_monde[1, 3], placement_objets.mat_table_monde[2, 3]);
    }

    public void PlusY()
    {
        Vector3 deplacement = placement_objets.normal.normalized * 0.002f;

        Matrix4x4 trans_p_y = Matrix4x4.identity;
        trans_p_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        placement_objets.mat_table_tablecalib = placement_objets.mat_table_tablecalib * trans_p_y;

        placement_objets.mat_table_monde = placement_objets.mat_table_tablecalib * placement_objets.mat_tablecalib_monde;
        placement_objets.mat_monde_table = placement_objets.mat_table_monde.inverse;

        triedre_table_translation.transform.position = new Vector3(placement_objets.mat_table_monde[0, 3], placement_objets.mat_table_monde[1, 3], placement_objets.mat_table_monde[2, 3]);
    }

    public void MinusY()
    {
        Vector3 deplacement = -placement_objets.normal.normalized * 0.002f;

        Matrix4x4 trans_m_y = Matrix4x4.identity;
        trans_m_y.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        placement_objets.mat_table_tablecalib = placement_objets.mat_table_tablecalib * trans_m_y;

        placement_objets.mat_table_monde = placement_objets.mat_table_tablecalib * placement_objets.mat_tablecalib_monde;
        placement_objets.mat_monde_table = placement_objets.mat_table_monde.inverse;

        triedre_table_translation.transform.position = new Vector3(placement_objets.mat_table_monde[0, 3], placement_objets.mat_table_monde[1, 3], placement_objets.mat_table_monde[2, 3]);
    }

    public void PlusZ()
    {
        Vector3 deplacement = placement_objets.directionu2.normalized * 0.002f;

        Matrix4x4 trans_p_z = Matrix4x4.identity;
        trans_p_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        placement_objets.mat_table_tablecalib = placement_objets.mat_table_tablecalib * trans_p_z;

        placement_objets.mat_table_monde = placement_objets.mat_table_tablecalib * placement_objets.mat_tablecalib_monde;
        placement_objets.mat_monde_table = placement_objets.mat_table_monde.inverse;

        triedre_table_translation.transform.position = new Vector3(placement_objets.mat_table_monde[0, 3], placement_objets.mat_table_monde[1, 3], placement_objets.mat_table_monde[2, 3]);
    }

    public void MinusZ()
    {
        Vector3 deplacement = -placement_objets.directionu2.normalized * 0.002f;

        Matrix4x4 trans_m_z = Matrix4x4.identity;
        trans_m_z.SetColumn(3, new Vector4(deplacement.x, deplacement.y, deplacement.z, 1));

        placement_objets.mat_table_tablecalib = placement_objets.mat_table_tablecalib * trans_m_z;

        placement_objets.mat_table_monde = placement_objets.mat_table_tablecalib * placement_objets.mat_tablecalib_monde;
        placement_objets.mat_monde_table = placement_objets.mat_table_monde.inverse;

        triedre_table_translation.transform.position = new Vector3(placement_objets.mat_table_monde[0, 3], placement_objets.mat_table_monde[1, 3], placement_objets.mat_table_monde[2, 3]);
    }
}
