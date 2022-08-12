using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class Affichage : MonoBehaviour, IMixedRealityTouchHandler
{
    public PlacementObjets placement_objets;

    public RobotReel robot_reel;
    public RobotVirtuel robot_virtuel;

    public ValidationTrajectoire validation_trajectoire;

    private GameObject aruco_controller;

    protected GameObject button_valider_trajectoire;
    protected GameObject button_premier_point;
    protected GameObject button_annuler_trajectoire;

    protected GameObject buttons_translation;
    protected GameObject button_retour_calibration;

    private GameObject triedre_table_translation;
    private GameObject triedre_table_calib;
    private GameObject triedre_robot;
    private GameObject triedre_effecteur;
    private GameObject triedre_effecteur_virtuel;

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        if (placement_objets.fixe == 0)
        {
            placement_objets.fixe = 1;
            aruco_controller.SetActive(false);
            buttons_translation.SetActive(true);
            triedre_table_calib.SetActive(true);
            triedre_table_translation.SetActive(true);

            triedre_table_translation.transform.position = triedre_table_calib.transform.position;
            triedre_table_translation.transform.rotation = triedre_table_calib.transform.rotation;
        }
    }

    public void OnTouchCompleted(HandTrackingInputEventData eventData) { }

    public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

    public void Defixer()
    {
        placement_objets.fixe = 0;
        aruco_controller.SetActive(true);

        buttons_translation.SetActive(false);
        button_retour_calibration.SetActive(false);
        triedre_table_calib.SetActive(false);
        triedre_table_translation.SetActive(false);
        triedre_robot.SetActive(false);

        validation_trajectoire.AnnulerTrajectoire();
        button_premier_point.SetActive(false);

        placement_objets.mat_table_tablecalib = Matrix4x4.identity;

        robot_reel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
        robot_virtuel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));

        triedre_effecteur.SetActive(false);
        triedre_effecteur_virtuel.SetActive(false);
    }

    public void FixerTable()
    {
        if (placement_objets.fixe == 1)
        {
            placement_objets.fixe = 2;
            buttons_translation.SetActive(false);
            triedre_table_calib.SetActive(false);
            triedre_robot.SetActive(true);
            button_retour_calibration.SetActive(true);

            triedre_effecteur.SetActive(true);
            triedre_effecteur_virtuel.SetActive(true);

            button_premier_point.SetActive(true);
        }
    }

    public void RetourCalibration()
    {
        if (placement_objets.fixe == 2)
        {
            placement_objets.fixe = 1;

            button_retour_calibration.SetActive(false);
            buttons_translation.SetActive(true);
            triedre_table_calib.SetActive(true);
            triedre_robot.SetActive(false);

            triedre_effecteur.SetActive(false);
            triedre_effecteur_virtuel.SetActive(false);
            robot_reel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));

            validation_trajectoire.AnnulerTrajectoire();
            button_premier_point.SetActive(false);
        }
    }

    public void Start()
    {
        aruco_controller = GameObject.Find("ARUWP Controller");
        buttons_translation = GameObject.Find("Boutons translation");
        button_retour_calibration = GameObject.Find("Bouton retour calibration");
        button_valider_trajectoire = GameObject.Find("Bouton valider trajectoire");
        button_premier_point = GameObject.Find("Bouton premier point");
        button_annuler_trajectoire = GameObject.Find("Bouton annuler trajectoire");
        triedre_table_translation = GameObject.Find("Triedre table translation");
        triedre_table_calib = GameObject.Find("Triedre table");
        triedre_robot = GameObject.Find("Triedre Robot");
        triedre_effecteur = GameObject.Find("Triedre effecteur");
        triedre_effecteur_virtuel = GameObject.Find("Triedre effecteur virtuel");

        if (buttons_translation != null)
        {
            buttons_translation.SetActive(false);
        }

        if (button_retour_calibration != null)
        {
            button_retour_calibration.SetActive(false);
        }

        if (button_valider_trajectoire != null)
        {
            button_valider_trajectoire.SetActive(false);
        }

        if (button_premier_point != null)
        {
            button_premier_point.SetActive(false);
        }

        if (button_annuler_trajectoire != null)
        {
            button_annuler_trajectoire.SetActive(false);
        }

        if (triedre_table_translation != null)
        {
            triedre_table_translation.SetActive(false);
        }

        if (triedre_table_calib != null)
        {
            triedre_table_calib.SetActive(false);
        }

        if (triedre_robot != null)
        {
            triedre_robot.SetActive(false);
        }

        if (triedre_effecteur != null)
        {
            triedre_effecteur.SetActive(false);
        }

        if (triedre_effecteur_virtuel != null)
        {
            triedre_effecteur_virtuel.SetActive(false);
        }

        robot_reel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
        robot_virtuel.first.TeleportRoot(new Vector3(100, 100, 100), new Quaternion(0, 0, 0, 1));
    }
}