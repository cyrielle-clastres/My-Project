/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>
namespace M2MqttUnity.Examples
{
    /// <summary>
    /// Script for testing M2MQTT with a Unity UI
    /// </summary>
    public class M2MqttUnityTest : M2MqttUnityClient
    {
        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
        [Header("User Interface")]
        public InputField consoleInputField;
        public Toggle encryptedToggle;
        public InputField addressInputField;
        public InputField portInputField;
        public Button connectButton;
        public Button disconnectButton;
        public Button testPublishButton;
        public Button clearButton;

        public PlacementObjets placement_objets;
        public RobotReel robot_reel;
        public RobotVirtuel robot_virtuel;
        public ValidationTrajectoire validation_trajectoire;
        public MatricesOutils matrices_outils;
        public GameObject cube;
        public GameObject triedre_effecteur;
        public GameObject triedre_effecteur_virtuel;

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;

        private int count = 0;

        public void PublishPosition()
        {
            count++;
            if ((cube != null) && (count % 2 == 0) && (placement_objets.fixe == 2))
            {
                Matrix4x4 m = Matrix4x4.TRS(cube.transform.position, cube.transform.rotation, new Vector3(1, 1, 1));
                Matrix4x4 cube_table = placement_objets.mat_monde_robot * m;

                Vector3 Point = new Vector3(cube_table[0, 3], cube_table[2, 3], cube_table[1, 3]);
                Quaternion Quaternion = new Quaternion(-cube_table.rotation.x, -cube_table.rotation.z, -cube_table.rotation.y, cube_table.rotation.w);

                String pos = JsonUtility.ToJson(Point);
                String rot = JsonUtility.ToJson(Quaternion);
                String final = "{\"position\":" + pos + " , \"orientation\":" + rot + "}";
                client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(final)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        public void PublishTriedrePosition()
        {
            if ((placement_objets.fixe == 2) && (robot_virtuel.SetJoints == true) && (robot_virtuel.SetTriedre == true))
            {
                JointState joint = new JointState();
                joint.position = robot_virtuel.GetPosition();
                joint.velocity = new float[6];
                joint.effort = new float[6];
                String position_robot = JsonUtility.ToJson(joint);
                client.Publish("joint_state_virtual", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(position_robot)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

                Matrix4x4 m = Matrix4x4.TRS(triedre_effecteur_virtuel.transform.position, triedre_effecteur_virtuel.transform.rotation, new Vector3(1, 1, 1));
                Matrix4x4 triedre_robot = placement_objets.mat_monde_robot * m;
                triedre_robot = triedre_robot * matrices_outils.mat_tool0_flange;
                triedre_robot = matrices_outils.mat_base_link_base * triedre_robot;

                Vector3 Point = new Vector3(triedre_robot[0, 3], triedre_robot[2, 3], triedre_robot[1, 3]);
                Quaternion Quaternion = new Quaternion(-triedre_robot.rotation.x, -triedre_robot.rotation.z, -triedre_robot.rotation.y, triedre_robot.rotation.w);

                String pos = JsonUtility.ToJson(Point);
                String rot = JsonUtility.ToJson(Quaternion);
                String final = "{\"position\":" + pos + " , \"orientation\":" + rot + "}";
                client.Publish("robot_pose", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(final)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        public void PublishTrajectoire()
        {
            if (robot_virtuel.TrajectoireFinie == true)
            {
                robot_virtuel.TrajectoireEnCours = true;
                String trajectoire_robot = JsonUtility.ToJson(robot_virtuel.trajectoire);
                client.Publish("trajectoire_robot", System.Text.Encoding.UTF8.GetBytes(Convert.ToString(trajectoire_robot)), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                robot_virtuel.SetJoints = true;
                robot_virtuel.SetTriedre = true;
            }
        }

        public void TestPublish()
        {
            client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            AddUiMessage("Test message published.");
        }

        public void SetBrokerAddress(string brokerAddress)
        {
            if (addressInputField && !updateUI)
            {
                this.brokerAddress = brokerAddress;
            }
        }

        public void SetBrokerPort(string brokerPort)
        {
            if (portInputField && !updateUI)
            {
                int.TryParse(brokerPort, out this.brokerPort);
            }
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        public void SetUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text = msg;
                updateUI = true;
            }
        }

        public void AddUiMessage(string msg)
        {
            if (consoleInputField != null)
            {
                consoleInputField.text += msg + "\n";
                updateUI = true;
            }
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            if (autoTest)
            {
                TestPublish();
            }
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "position/mqtt" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "position_robot" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "position_robot_virtuel_casque" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "trajectoire_finie" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "position/mqtt" });
            client.Unsubscribe(new string[] { "position_robot" });
            client.Unsubscribe(new string[] { "position_robot_virtuel_casque" });
            client.Unsubscribe(new string[] { "trajectoire_finie" });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            AddUiMessage("CONNECTION LOST!");
        }

        private void UpdateUI()
        {
            if (client == null)
            {
                if (connectButton != null)
                {
                    connectButton.interactable = true;
                    disconnectButton.interactable = false;
                    testPublishButton.interactable = false;
                }
            }
            else
            {
                if (testPublishButton != null)
                {
                    testPublishButton.interactable = client.IsConnected;
                }
                if (disconnectButton != null)
                {
                    disconnectButton.interactable = client.IsConnected;
                }
                if (connectButton != null)
                {
                    connectButton.interactable = !client.IsConnected;
                }
            }
            if (addressInputField != null && connectButton != null)
            {
                addressInputField.interactable = connectButton.interactable;
                addressInputField.text = brokerAddress;
            }
            if (portInputField != null && connectButton != null)
            {
                portInputField.interactable = connectButton.interactable;
                portInputField.text = brokerPort.ToString();
            }
            if (encryptedToggle != null && connectButton != null)
            {
                encryptedToggle.interactable = connectButton.interactable;
                encryptedToggle.isOn = isEncrypted;
            }
            if (clearButton != null && connectButton != null)
            {
                clearButton.interactable = connectButton.interactable;
            }
            updateUI = false;
        }

        protected override void Start()
        {
            SetUiMessage("Ready.");
            updateUI = true;
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            StoreMessage(msg);
            if (topic == "position/mqtt")
            {
                Pose PositionAndRotation = JsonUtility.FromJson<Pose>(msg);
                if ((triedre_effecteur != null) && (placement_objets.fixe == 2))
                {
                    PositionAndRotation.position = new Vector3(PositionAndRotation.position.x, PositionAndRotation.position.z, PositionAndRotation.position.y);
                    PositionAndRotation.orientation = new Quaternion(-PositionAndRotation.orientation.x, -PositionAndRotation.orientation.z, -PositionAndRotation.orientation.y, PositionAndRotation.orientation.w);

                    Matrix4x4 m = Matrix4x4.TRS(PositionAndRotation.position, PositionAndRotation.orientation, new Vector3(1, 1, 1));
                    Matrix4x4 triedre_monde = placement_objets.mat_robot_monde * m;

                    triedre_effecteur.transform.position = new Vector3(triedre_monde[0, 3], triedre_monde[1, 3], triedre_monde[2, 3]);
                    triedre_effecteur.transform.rotation = new Quaternion(triedre_monde.rotation.x, triedre_monde.rotation.y, triedre_monde.rotation.z, triedre_monde.rotation.w);

                    if (((robot_virtuel.SetTriedre == false) && (robot_virtuel.TrajectoireEnCours == false)) || ((robot_virtuel.SetTriedre == true) && (robot_virtuel.TrajectoireEnCours == true)))
                    {
                        triedre_effecteur_virtuel.transform.position = new Vector3(triedre_monde[0, 3], triedre_monde[1, 3], triedre_monde[2, 3]);
                        triedre_effecteur_virtuel.transform.rotation = new Quaternion(triedre_monde.rotation.x, triedre_monde.rotation.y, triedre_monde.rotation.z, triedre_monde.rotation.w);
                        robot_virtuel.SetTriedre = true;
                    }
                }
            }

            if ((topic == "position_robot") && ((placement_objets.fixe == 2) || (placement_objets.fixe == 1)))
            {
                JointState Joint_State = JsonUtility.FromJson<JointState>(msg);
                robot_reel.UpdatePosition(Joint_State.position);

                if (((robot_virtuel.SetJoints == false) && (robot_virtuel.TrajectoireEnCours == false)) || ((robot_virtuel.SetJoints == true) && (robot_virtuel.TrajectoireEnCours == true)))
                {
                        robot_virtuel.UpdatePosition(Joint_State.position);
                    robot_virtuel.SetJoints = true;
                }
            }

            if (topic == "position_robot_virtuel_casque")
            {
                if (robot_virtuel.TrajectoireFinie == false)
                {
                    JointState Joint_State = JsonUtility.FromJson<JointState>(msg);

                    robot_virtuel.UpdatePosition(Joint_State.position);
                    if (validation_trajectoire.SetPremierPoint == true)
                    {
                        JointTrajectoryPoint point = new JointTrajectoryPoint();
                        point.positions = Joint_State.position;
                        robot_virtuel.point.Add(point);
                    }
                }
            }

            if (topic == "trajectoire_finie")
            {
                if (robot_virtuel.TrajectoireEnCours == true)
                {
                    validation_trajectoire.FinTrajectoire();
                }
            }
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            AddUiMessage("Received: " + msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            if (updateUI)
            {
                UpdateUI();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            if (autoTest)
            {
                autoConnect = true;
            }
        }
    }
}
