using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PointsBar : MonoBehaviour
{
    public Text points;
    public Player character;


    void Start()
    {
        points.text = 0.ToString();
    }
    void Update()
    {
        points.text = character.Points.ToString();
    }
}
