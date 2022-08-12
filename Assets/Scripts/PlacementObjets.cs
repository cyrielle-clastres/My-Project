using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class PlacementObjets : MonoBehaviour
{
    public Vector3 directionu1;
    public Vector3 directionu2;
    public Vector3 normal;

    public int fixe = 0;

    protected GameObject button_defixer;
    protected GameObject buttons_translation;
    protected GameObject buttons_trajectoire;

    protected GameObject triedre_feuille;
    protected GameObject triedre_table;
    protected GameObject triedre_robot;

    protected GameObject triedre_haut_gauche;
    protected GameObject triedre_haut_droite;
    protected GameObject triedre_bas_gauche;
    protected GameObject triedre_bas_droite;

    public RobotReel robot_reel;
    public RobotVirtuel robot_virtuel;

    [HideInInspector]
    public Matrix4x4 mat_tablecalib_monde = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_table_monde = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_monde_table = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_table_tablecalib = Matrix4x4.identity;
    protected Matrix4x4 mat_robot_table = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_robot_monde = Matrix4x4.identity;
    [HideInInspector]
    public Matrix4x4 mat_monde_robot = Matrix4x4.identity;

    void Start()
    {
        triedre_feuille = GameObject.Find("Triedre feuille");
        triedre_table = GameObject.Find("Triedre table");
        triedre_robot = GameObject.Find("Triedre Robot");
        button_defixer = GameObject.Find("Boutons defixer table");
        buttons_translation = GameObject.Find("Boutons translation");
        buttons_trajectoire = GameObject.Find("Boutons trajectoire");

        mat_robot_table.SetColumn(0, new Vector4(0, 0, 1, 0));
        mat_robot_table.SetColumn(1, new Vector4(0, 1, 0, 0));
        mat_robot_table.SetColumn(2, new Vector4(-1, 0, 0, 0));
        mat_robot_table.SetColumn(3, new Vector4(0.259f, 0, 0.385f, 1));
    }

    void Update()
    {
        triedre_haut_gauche = GameObject.Find("Triedre haut gauche");
        triedre_haut_droite = GameObject.Find("Triedre haut droite");
        triedre_bas_gauche = GameObject.Find("Triedre bas gauche");
        triedre_bas_droite = GameObject.Find("Triedre bas droite");

        if ((triedre_haut_gauche != null) && (triedre_haut_droite != null) && (triedre_bas_gauche != null) && (triedre_bas_droite != null))
        {
            Vector3 pos = new Vector3();
            Quaternion rot = new Quaternion();
            pos.x = (triedre_haut_gauche.transform.position.x + triedre_haut_droite.transform.position.x + triedre_bas_gauche.transform.position.x + triedre_bas_droite.transform.position.x) / 4;
            pos.y = (triedre_haut_gauche.transform.position.y + triedre_haut_droite.transform.position.y + triedre_bas_gauche.transform.position.y + triedre_bas_droite.transform.position.y) / 4;
            pos.z = (triedre_haut_gauche.transform.position.z + triedre_bas_gauche.transform.position.z + triedre_haut_droite.transform.position.z + triedre_bas_droite.transform.position.z) / 4;

            rot.x = (triedre_haut_gauche.transform.rotation.x + triedre_haut_droite.transform.rotation.x + triedre_bas_gauche.transform.rotation.x + triedre_bas_droite.transform.rotation.x) / 4;
            rot.y = (triedre_haut_gauche.transform.rotation.y + triedre_haut_droite.transform.rotation.y + triedre_bas_gauche.transform.rotation.y + triedre_bas_droite.transform.rotation.y) / 4;
            rot.z = (triedre_haut_gauche.transform.rotation.z + triedre_haut_droite.transform.rotation.z + triedre_bas_gauche.transform.rotation.z + triedre_bas_droite.transform.rotation.z) / 4;
            rot.w = (triedre_haut_gauche.transform.rotation.w + triedre_haut_droite.transform.rotation.w + triedre_bas_gauche.transform.rotation.w + triedre_bas_droite.transform.rotation.w) / 4;

            TracerTriedres(pos, rot);
        }
    }

    private void TracerTriedres(Vector3 pos, Quaternion rot)
    {
        if (fixe == 0)
        {
            Matrix4x4 mat_tablecalib_aruco = Matrix4x4.identity;
            Matrix4x4 mat_aruco_monde = Matrix4x4.identity;

            directionu1 = triedre_haut_droite.transform.position - triedre_haut_gauche.transform.position;
            directionu2 = triedre_haut_droite.transform.position - triedre_bas_droite.transform.position;
            normal = Vector3.Cross(directionu1, directionu2);

            Vector3 pos_table = pos - directionu1.normalized * 1.0f - directionu2.normalized * 0.525f - normal.normalized * 0.63f;

            mat_tablecalib_aruco.SetColumn(0, new Vector4(1, 0, 0, 0));
            mat_tablecalib_aruco.SetColumn(1, new Vector4(0, 1, 0, 0));
            mat_tablecalib_aruco.SetColumn(2, new Vector4(0, 0, 1, 0));
            mat_tablecalib_aruco.SetColumn(3, new Vector4(pos_table.x - pos.x, pos_table.y - pos.y, pos_table.z - pos.z, 1));

            mat_aruco_monde.SetColumn(0, new Vector4((directionu1.normalized).x, (directionu1.normalized).y, (directionu1.normalized).z, 0));
            mat_aruco_monde.SetColumn(1, new Vector4((directionu2.normalized).x, (directionu2.normalized).y, (directionu2.normalized).z, 0));
            mat_aruco_monde.SetColumn(2, new Vector4((normal.normalized).x, (normal.normalized).y, (normal.normalized).z, 0));
            mat_aruco_monde.SetColumn(3, new Vector4(pos.x, pos.y, pos.z, 1));

            mat_tablecalib_monde = mat_tablecalib_aruco * mat_aruco_monde;

            mat_monde_table = mat_tablecalib_monde.inverse;

            triedre_table.transform.position = pos_table;
            triedre_table.transform.rotation = mat_aruco_monde.rotation;

            triedre_feuille.transform.rotation = mat_aruco_monde.rotation;
            triedre_feuille.transform.position = pos - normal.normalized * 0.03f;

            button_defixer.transform.position = pos + directionu1.normalized * 0.30f - directionu2.normalized * 0.1f - normal.normalized * 0.55f;
            button_defixer.transform.rotation = mat_aruco_monde.rotation;
        }

        else if (fixe == 1)
        {
            buttons_translation.transform.position = pos - directionu1.normalized * 1.0f - directionu2.normalized * 0.3f - normal.normalized * 0.63f;
            buttons_translation.transform.rotation = mat_tablecalib_monde.rotation;

            mat_table_monde = mat_table_tablecalib * mat_tablecalib_monde;
            mat_monde_table = mat_table_monde.inverse;
            mat_robot_monde = mat_table_monde * mat_robot_table;
            mat_monde_robot = mat_robot_monde.inverse;

            Vector3 rotation = mat_robot_monde.rotation.eulerAngles;
            rotation = new Vector3(rotation.x, rotation.y + 90, rotation.z);
            robot_virtuel.first.TeleportRoot(new Vector3(mat_robot_monde[0, 3] - directionu2.normalized.x * 0.015f, mat_robot_monde[1, 3] - directionu2.normalized.y * 0.015f, mat_robot_monde[2, 3] - directionu2.normalized.z * 0.015f), Quaternion.Euler(rotation));
        }

        else if (fixe == 2)
        {
            triedre_robot.transform.rotation = mat_robot_monde.rotation;
            triedre_robot.transform.position = new Vector3(mat_robot_monde[0, 3] - directionu2.normalized.x * 0.015f, mat_robot_monde[1, 3] - directionu2.normalized.y * 0.015f, mat_robot_monde[2, 3] - directionu2.normalized.z * 0.015f);

            Vector3 rotation = mat_robot_monde.rotation.eulerAngles;
            rotation = new Vector3(rotation.x, rotation.y + 90, rotation.z);
            robot_virtuel.first.TeleportRoot(new Vector3(mat_robot_monde[0, 3] - directionu2.normalized.x * 0.015f, mat_robot_monde[1, 3] - directionu2.normalized.y * 0.015f, mat_robot_monde[2, 3] - directionu2.normalized.z * 0.015f), Quaternion.Euler(rotation));
            robot_reel.first.TeleportRoot(new Vector3(mat_robot_monde[0, 3] + directionu1.normalized.x * 0.5f - directionu2.normalized.x * 0.015f, mat_robot_monde[1, 3] + directionu1.normalized.y * 0.5f - directionu2.normalized.y * 0.015f, mat_robot_monde[2, 3] + directionu1.normalized.z * 0.5f - directionu2.normalized.z * 0.015f), Quaternion.Euler(rotation));

            buttons_trajectoire.transform.position = pos - directionu1.normalized * 0.9f - directionu2.normalized * 0.1f - normal.normalized * 0.63f;
            buttons_trajectoire.transform.rotation = mat_table_monde.rotation;
        }
    }
}
