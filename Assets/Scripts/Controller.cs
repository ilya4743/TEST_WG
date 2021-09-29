using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void ShowBox(int x, int y, int b);
public delegate void ShowRGBPanel(List<int> list);
public delegate void ShowWinPanel();

public class Controller
{
    ShowBox showBox;
    bool isSelected;
    int fromX, fromY;
    ShowRGBPanel showRGBPanel;
    ShowWinPanel showWinPanel;
    int[,] table;
    List<int> colorrgb;
    public Controller(ShowBox showBox, ShowRGBPanel showRGBPanel, ShowWinPanel showWinPanel)
    {
        this.showBox = showBox;
        this.showRGBPanel = showRGBPanel;
        this.showWinPanel = showWinPanel;
        table = new int[9, 9];
    }
    public void Start()
    {
        isSelected = false;
        ClearMap();
        InitGameMap();
        InitRGBPnlColor();
    }
    private static void Shuffle(List<int> list)
    {
        System.Random rand = new System.Random();

        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = rand.Next(i + 1);

            int tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
    }
    private void MakeColorList(List<int> arr)
    {
        for (int i = 0; i < 5; i++)
            arr.Add(1);

        for (int i = 0; i < 5; i++)
            arr.Add(2);

        for (int i = 0; i < 5; i++)
            arr.Add(3);

        Shuffle(arr);
    }
    private void InitGameMap()
    {
        List<int> arr = new List<int>();
        MakeColorList(arr);
        for (int i = 1; i < 5; i = i + 2)
            for (int j = 0; j < 5; j = j + 2)
                SetMap(i,j,-1);

        for (int i = 1; i < 5; i = i + 2)
            for (int j = 1; j < 5; j = j + 2)
                SetMap(i, j, 0);

        int c = 0;
        for (int i = 0; i < 5; i += 2)
            for (int j = 0; j < 5; j++)
            {
                SetMap(i, j,arr[c]);
                c++;
            }
    }
    private void InitRGBPnlColor()
    {
        colorrgb = new List<int>();
        for (int i = 0; i < 3; )    
            colorrgb.Add(++i);
        Shuffle(colorrgb);
        showRGBPanel(colorrgb);
    }
    public void Click(int x, int y)
    {
        if (table[x, y] > 0)
            SelectCell(x, y);
        if(table[x, y] == 0)
            MoveCell(x, y);
        if (IsWin() == true) 
            showWinPanel();
    }
    private bool IsWin()
    {
        int c=0;
        for (int i = 0; i < 5; i += 2)
        { 
            for (int j = 0; j < 5; j++)
                if (colorrgb[c] != table[i, j]) return false;
            c++;
        }
        return true;
    }
    private void SelectCell(int x, int y)
    {
        fromX = x;
        fromY = y;
        isSelected = true;
    }
    private void ClearMap()
    {
        for(int x=0; x<5; x++)
            for(int y=0; y<5; y++)
                SetMap(x,y,0);
    }
    private void SetMap(int x, int y, int color)
    {
        table[x, y] = color;
        showBox(x, y, color);
    }
    private void MoveCell(int x, int y)
    {
        if (!isSelected) return;
        if (!CanMove(x, y)) return;
        SetMap(x, y, table[fromX, fromY]);
        SetMap(fromX, fromY, 0);
    }
    private bool isField(int x, int y)
    {
        return x >= 0 && x <= 4 && y >= 0 && y <= 4;
    }
    private void FindWay(int x, int y, bool [,]vacantMatrix, bool start=false)
    {
        if(!start)
        { 
            if (!isField(x, y)) return;
            if (table[x, y] != 0) return;
            if (vacantMatrix[x, y]) return;
        }
        vacantMatrix[x, y] = true;
        FindWay(x+1, y, vacantMatrix);
        FindWay(x-1, y, vacantMatrix);
        FindWay(x, y+1, vacantMatrix);
        FindWay(x, y-1, vacantMatrix);
    }
    private bool CanMove(int x, int y)
    {
        bool[,] vacantMatrix = new bool[5, 5];
        FindWay(fromX, fromY, vacantMatrix, true);
        return vacantMatrix[x,y];
    }
}