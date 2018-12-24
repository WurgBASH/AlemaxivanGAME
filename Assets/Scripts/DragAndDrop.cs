using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Reflection;

public class DragAndDrop : MonoBehaviour
{
    private int butCol = 3;
    private Player character;
    private Transform[] but;
    public GameObject RunButtonPrefab;
    public GameObject JumpButtonPrefab;
    public GameObject ShootButtonPrefab;
    bool drag = false;
    bool addingBut = false;
    int  start = 0;
    Dictionary<string, int> funcAndArg = new Dictionary<string, int>();

    public void CreateBut()
    {
        but = new Transform[butCol];
        for (int i = 0; i < but.Length; i++)
        {
            but[i] = transform.GetChild(i);
        }
        if (butCol > 3 && addingBut)
        {
            addingBut = false;
            but[0].position += new Vector3(0, 20, 0);
            for (int i = 1; i < but.Length; i++)
            {
                but[i].position = but[i-1].position + new Vector3(0, -44, 0); ;
            }
            var Start = GameObject.Find("Start");
            Start.transform.position += new Vector3(0, -20, 0);
        }
    }
    private void Awake()
    {
        character = FindObjectOfType<Player>();
        CreateBut();
    }
    void FixedUpdate()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < but.Length; i++)
            {
                if (Input.GetMouseButtonDown(1) && Math.Abs(Input.mousePosition.x) >= Math.Abs(but[i].position.x - 80.0F) && Math.Abs(Input.mousePosition.x) <= Math.Abs(but[i].position.x) + 80.0F &&
                    Math.Abs(Input.mousePosition.y) >= Math.Abs(but[i].position.y - 15.0F) && Math.Abs(Input.mousePosition.y) <= Math.Abs(but[i].position.y) + 15.0F)
                    {
                 
                        Vector2 vec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        var temp = but[i].position;
                        if (i < (butCol-1))
                        {
                            but[i].position = but[i + 1].position;
                            but[i].SetSiblingIndex(i + 1);
                            but[i + 1].position = temp;
                            but[i+1].SetSiblingIndex(i);
                            CreateBut();
                        }
                        else
                        {                        
                            but[i].position = but[0].position;
                            but[i].SetSiblingIndex(0);
                            but[0].position = temp;
                            but[0].SetSiblingIndex(i);
                            CreateBut();
                        }
                        break;
                }
            }
            
        }

    }
    public void GetFuncs()
    {
        start++;
        if (start==1)
        {
            character.CreateFuncArray(but.Length);
            for (int i = 0; i < but.Length; i++)
            {
                MethodInfo m = character.GetType().GetMethod(but[i].name[0] + "But");
                object[] parametersArray = new object[] { funcAndArg[but[i].name] };
                m.Invoke(character, parametersArray);                
            }
        }
        StartCoroutine(character.Starting());



    }
    public void GetArg(InputField inp)
    {
        funcAndArg.Add(inp.name, int.Parse(inp.text));       
    }
    public void AddRun()
    {
        AddBut(1);
    }
    public void AddJump()
    {
        AddBut(2);
    }
    public void AddShoot()
    {
        AddBut(3);
    }
    public void AddBut(int butName)
    {
        if (butCol < 5)
        {
            GameObject button;
            switch (butName)
            {
                case 1:
                    button = (GameObject)Instantiate(RunButtonPrefab);
                    break;
                case 2:
                    button = (GameObject)Instantiate(JumpButtonPrefab);
                    break;
                case 3:
                    button = (GameObject)Instantiate(ShootButtonPrefab);
                    break;
                default:
                    button = (GameObject)Instantiate(RunButtonPrefab);
                    break;

            }            
            button.transform.position = transform.position;
            button.transform.SetParent(transform, false);
            Vector3 pos = but[butCol - 1].position;
            button.transform.position = pos + new Vector3(0, -44, 0);
            RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
            rt.sizeDelta += new Vector2(0, 65);

            butCol++;
            addingBut = true;
            CreateBut();
        }
    }

}