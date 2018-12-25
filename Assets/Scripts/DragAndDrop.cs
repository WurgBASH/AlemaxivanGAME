using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Reflection;

public class DragAndDrop : MonoBehaviour
{
    public int butCol = 2;
    private Player character;
    private Transform[] but;
    public GameObject RunButtonPrefab;
    public GameObject JumpButtonPrefab;
    public GameObject ShootButtonPrefab;
    bool drag = false;
    int activeDrag;
    bool addingBut = false;
    bool deleteBut = false;
    Dictionary<string, int> funcAndArg = new Dictionary<string, int>();

    public void CreateBut()
    {
        but = new Transform[butCol];
        for (int i = 0; i < but.Length; i++)
        {
            but[i] = transform.GetChild(i);
        }
        if (addingBut)
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
        if (deleteBut)
        {
            deleteBut = false;
            but[0].position -= new Vector3(0, 20, 0);
            for (int i = 1; i < but.Length; i++)
            {
                but[i].position = but[i - 1].position + new Vector3(0, -44, 0); ;
            }
            var Start = GameObject.Find("Start");
            Start.transform.position += new Vector3(0, +20, 0);
        }
    }
    private void Awake()
    {
        character = FindObjectOfType<Player>();
        CreateBut();
    }
    void Update()
    {
        if (drag)
        {           
            if (but[activeDrag].position.y+15.0F > Input.mousePosition.y)
            {
                if (activeDrag < (butCol-1))
                {
                    if (Input.mousePosition.y <= (but[activeDrag + 1].position.y + 15.0F) && Input.mousePosition.y >= (but[activeDrag + 1].position.y - 15.0F))
                    {
                    
                        var temp = but[activeDrag].position;
                        but[activeDrag].position = but[activeDrag + 1].position;
                        but[activeDrag].SetSiblingIndex(activeDrag + 1);
                        but[activeDrag + 1].position = temp;
                        but[activeDrag+1].SetSiblingIndex(activeDrag);
                        CreateBut();
                        drag = false;
                    }                    
                }
            }
            else
            {
                if (activeDrag > 0)
                {
                    var temp = but[activeDrag].position;
                    but[activeDrag].position = but[activeDrag - 1].position;
                    but[activeDrag].SetSiblingIndex(activeDrag - 1);
                    but[activeDrag - 1].position = temp;
                    but[activeDrag-1].SetSiblingIndex(activeDrag);
                    CreateBut();
                    drag = false;
                }
            }
        }
        if (Input.GetMouseButtonDown(1) )
        {
            for (int i = 0; i < but.Length; i++)
            {
                if (Input.GetMouseButtonDown(1) && Math.Abs(Input.mousePosition.x) >= Math.Abs(but[i].position.x - 60.0F) && Math.Abs(Input.mousePosition.x) <= Math.Abs(but[i].position.x) + 60.0F &&
                    Math.Abs(Input.mousePosition.y) >= Math.Abs(but[i].position.y - 15.0F) && Math.Abs(Input.mousePosition.y) <= Math.Abs(but[i].position.y) + 15.0F)
                    {
                        drag = true;
                        activeDrag = i;
                }
            }
            
        }

    }
    public void GetFuncs(Button start)
    {
        if(funcAndArg.Count == butCol)
        {
            start.interactable = false;
            character.CreateFuncArray(but.Length);
            for (int i = 0; i < but.Length; i++)
            {
                MethodInfo m = character.GetType().GetMethod(but[i].name[0] + "But");
                object[] parametersArray = new object[] { funcAndArg[but[i].name] };
                m.Invoke(character, parametersArray);

            }
            StartCoroutine(character.Starting());
            
        }
        
    }
    public void DeleteBut(InputField inp)
    {
        if (inp.GetComponent<DeleteFuncBut>().canDelete)
        {            
            for(int i=0;i<but.Length;i++)
            {  
                if(but[i].name == inp.name)
                {
                    if (funcAndArg.ContainsKey(but[i].name))
                        funcAndArg.Remove(but[i].name);
                    RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
                    Destroy(GameObject.Find(but[i].name));
                    butCol--;
                    rt.sizeDelta -= new Vector2(0, 70);
                    deleteBut = true;
                    for (int j = i; j < but.Length-1; j++)
                    {
                        but[i + 1].SetSiblingIndex(i);
                    }
                    CreateBut();
                    break; 
                }
            }
        }
    }
    public void GetArg(InputField inp)
    {
        if(!funcAndArg.ContainsKey(inp.name))
            funcAndArg.Add(inp.name, int.Parse(inp.text));
        else
            funcAndArg[inp.name] = int.Parse(inp.text);
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
        if (butCol < 7)
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
            button.name += butCol;
            button.GetComponent<DeleteFuncBut>().canDelete = true;
            button.transform.position = transform.position;
            button.transform.SetParent(transform, false);
            Vector3 pos = but[butCol - 1].position;
            button.transform.position = pos + new Vector3(0, -44, 0);
            RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
            rt.sizeDelta += new Vector2(0, 70);

            butCol++;
            addingBut = true;
            CreateBut();
        }
    }

}