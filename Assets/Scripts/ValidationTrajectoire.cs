using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidationTrajectoire : MonoBehaviour
{
    public RobotVirtuel robot_virtuel;

    private GameObject button_valider_trajectoire;
    private GameObject button_premier_point;
    private GameObject button_annuler_trajectoire;

    public bool SetPremierPoint = false;

    void Start()
    {
        button_valider_trajectoire = GameObject.Find("Bouton valider trajectoire");
        button_premier_point = GameObject.Find("Bouton premier point");
        button_annuler_trajectoire = GameObject.Find("Bouton annuler trajectoire");
    }

    public void ValiderPremierPoint()
    {
        SetPremierPoint = true;
        robot_virtuel.TrajectoireFinie = false;
        robot_virtuel.TrajectoireEnCours = false;
        robot_virtuel.point = new List<JointTrajectoryPoint>();
        robot_virtuel.trajectoire.points = null;
        robot_virtuel.triedre_effecteur.GetComponent<Collider>().enabled = true;
        button_valider_trajectoire.SetActive(true);
        button_annuler_trajectoire.SetActive(true);
        button_premier_point.SetActive(false);
    }

    public void ValiderTrajectoire()
    {
        if (robot_virtuel.TrajectoireFinie == false)
        {
            robot_virtuel.TrajectoireFinie = true;
            robot_virtuel.trajectoire.points = robot_virtuel.point.ToArray();
            robot_virtuel.triedre_effecteur.GetComponent<Collider>().enabled = false;
            button_valider_trajectoire.SetActive(false);
            button_annuler_trajectoire.SetActive(false);
        }
    }

    public void FinTrajectoire()
    {
        robot_virtuel.TrajectoireFinie = false;
        robot_virtuel.TrajectoireEnCours = false;
        robot_virtuel.trajectoire.points = null;
        robot_virtuel.point = new List<JointTrajectoryPoint>();
        robot_virtuel.triedre_effecteur.GetComponent<Collider>().enabled = true;
        button_premier_point.SetActive(true);
        robot_virtuel.SetJoints = false;
        robot_virtuel.SetTriedre = false;
    }

    public void AnnulerTrajectoire()
    {
        robot_virtuel.TrajectoireFinie = false;
        robot_virtuel.TrajectoireEnCours = false;
        robot_virtuel.trajectoire.points = null;
        robot_virtuel.point = new List<JointTrajectoryPoint>();
        robot_virtuel.triedre_effecteur.GetComponent<Collider>().enabled = true;
        button_valider_trajectoire.SetActive(false);
        button_annuler_trajectoire.SetActive(false);
        button_premier_point.SetActive(true);
        robot_virtuel.SetJoints = false;
        robot_virtuel.SetTriedre = false;
    }
}
