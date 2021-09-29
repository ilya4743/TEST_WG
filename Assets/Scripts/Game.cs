using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Button[,] buttons;
    public static Button[] rgb;
    public GameObject WinPnl;
    public GameObject GameFieldPnl;
    public GameObject RGBPnl;
    Controller controller;

    void Start()
    {
        WinPnl.SetActive(false);
        GameFieldPnl.SetActive(true);
        RGBPnl.SetActive(true);
        InitButtons();
        InitRGBButtons();
        controller = new Controller(ShowBox, ShowRGBPanel, ShowWinPanel);
        controller.Start();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }
    private Color GetColor(int color)
    {
        switch (color)
        {
            case 0:
                return Color.white;
            case 1:
                return Color.red;
            case 2:
                return Color.green;
            case 3:
                return Color.blue;
            case -1:
                return Color.black;
            default:
                return Color.white;
        }
    }
    public void ShowWinPanel()
    {
        WinPnl.SetActive(true);
        GameFieldPnl.SetActive(false);
        RGBPnl.SetActive(false);
    }
    public void ShowBox(int x, int y, int color)
    {
        buttons[x, y].GetComponent<Image>().color = GetColor(color);
    }
    public void ShowRGBPanel(List<int> list)
    {
        for (int i = 0; i < 3; i++)
            rgb[i].GetComponent<Image>().color = GetColor(list[i]);
    }
    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int nr = GetNumber(name);
        int x = nr % 5;
        int y = nr / 5;
        controller.Click(x,y);
    }
    public void NewGameClick()
    {
        Start();
    }
    public void ExitBtnClick()
    {
        Application.Quit();
    }
    public static void Shuffle<T>(List<T> list)
    {
        System.Random rand = new System.Random();

        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);
            T tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
    }
    private void InitRGBButtons()
    {
        rgb = new Button[3];
        for(int i=0; i<3; i++)
        {
            rgb[i]= GameObject.Find($"rgb{i+1}").GetComponent<Button>();
        }
    }
    private void InitButtons()
    {
        buttons = new Button[5, 5];
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                buttons[j, i] = GameObject.Find($"Button ({i * 5 + j})").GetComponent<Button>();
    }
    private int GetNumber(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
            throw new Exception("Error");
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }
}
