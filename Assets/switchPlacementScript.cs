using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class switchPlacementScript : MonoBehaviour {
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;

    public GameObject[] switchObjects;
    public KMSelectable[] switchSelectables;
    public MeshRenderer[] switchColors;

    private Switch[] switches = new Switch[12];
    private readonly Coroutine[] coroutines = new Coroutine[12];
    private int keyRow, keyCol, keySquare;

    private List<Color> rgb = new List<Color>() {Color.black, Color.blue, Color.green, Color.cyan, Color.red, Color.magenta, Color.yellow, Color.white};
    private const int switchAngle = 55;
    private float[] posOffsets = {-0.06f, -0.03f, 0f, 0.03f, 0.06f};

    private bool moduleStarted = false;
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    private int[,] grid;
    private int fIndex;
    private List<List<List<int>>> fGrids = new List<List<List<int>>> {
        new List<List<int>>{
            new List<int>{23, 8, 21, 16, 25}, // up
            new List<int>{20, 15, 24, 7, 12}, 
            new List<int>{9, 22, 13, 4, 17}, 
            new List<int>{14, 19, 2, 11, 6}, 
            new List<int>{1, 10, 5, 18, 3}},
        new List<List<int>>{
            new List<int>{6, 17, 13, 4, 25}, // left
            new List<int>{1, 22, 8, 14, 20}, 
            new List<int>{21, 12, 3, 19, 10}, 
            new List<int>{16, 2, 23, 9, 15}, 
            new List<int>{11, 7, 18, 24, 5}},
        new List<List<int>>{
            new List<int>{11, 1, 6, 16, 21}, // right
            new List<int>{17, 7, 22, 2, 12}, 
            new List<int>{23, 13, 18, 8, 3}, 
            new List<int>{9, 24, 4, 14, 19}, 
            new List<int>{5, 20, 15, 25, 10}},
        new List<List<int>>{
            new List<int>{1, 2, 5, 6, 7}, // down
            new List<int>{24, 3, 4, 9, 8}, 
            new List<int>{23, 22, 25, 10, 11}, 
            new List<int>{20, 21, 16, 15, 12}, 
            new List<int>{19, 18, 17, 14, 13}}
    };

    void Awake () {
        moduleId = moduleIdCounter++; // version 1.0.0
        
        grid = GenerateGrid();
        for (int i = 0; i < 12; i++) {
            switches[i] = new Switch(grid[i, 0], grid[i, 1], UnityEngine.Random.Range(0, 2) == 0,
                rgb[UnityEngine.Random.Range(0, 8)]);
            switchObjects[i].transform.localPosition = new Vector3(
                posOffsets[switches[i].cell1 % 5] + (!switches[i].vert ? 0.015f : 0), 
                0.017f, 
                -posOffsets[switches[i].cell1 / 5] - (switches[i].vert ? 0.015f : 0));
            if (!switches[i].vert) {
                switchObjects[i].transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            switchColors[i].material.color = switches[i].color;
        }
    }

    void Start () {
        List<int> keyCols = new List<int>() {0, 0, 0};
        int hv = 0;

        for (int i = 0; i < 12; i++) {
            switchSelectables[i].OnInteract = SwitchFlip(i);
            switchSelectables[i].OnInteract(); // set switches

            for (int j = 2; j >= 0; j--) { // get RGB channels
                if (rgb.IndexOf(switches[i].color) % Math.Pow(2, j + 1) >= Math.Pow(2, j)) {
                    keyCols[2 - j] += 1;
                }
            }
            hv += (switches[i].vert ? -1 : 1); // horizontal - vertical expression
            
            // finding F for table
            if (switches[i].cell2 == 12 && switches[i].vert) {
                fIndex = 0;
                Debug.LogFormat("[Switch Placement #{0}] Table F = ↑ is in use.", moduleId);
            }
            else if (switches[i].cell2 == 12 && !switches[i].vert) {
                fIndex = 1;
                Debug.LogFormat("[Switch Placement #{0}] Table F = ← is in use.", moduleId);
            }
            else if (switches[i].cell1 == 12 && !switches[i].vert) {
                fIndex = 2;
                Debug.LogFormat("[Switch Placement #{0}] Table F = → is in use.", moduleId);
            }
            else if (switches[i].cell1 == 12 && switches[i].vert) {
                fIndex = 3;
                Debug.LogFormat("[Switch Placement #{0}] Table F = ↓ is in use.", moduleId);
            }
        }
        
        List<int> keyColsSorted = new List<int>() {keyCols[0], keyCols[1], keyCols[2]};
        keyColsSorted.Sort();
        if (keyColsSorted[0] == keyColsSorted[1] && keyColsSorted[1] == keyColsSorted[2]) {
            keyCol = 4;
        }
        else if (keyColsSorted[1] == keyColsSorted[2]) {
            keyCol = 3;
        }
        else {
            keyCol = keyCols.IndexOf(keyColsSorted[2]);
        }

        keyRow = Math.Min(Math.Max(hv / 2, -2), 2) + 2;
        keySquare = keyRow * 5 + keyCol;
        Debug.LogFormat("[Switch Placement #{0}] There are {1} red switches, {2} green switches, and {3} blue switches. " +
                        "There are {4} horizontal switches and {5} vertical switches.", moduleId, keyCols[0], keyCols[1],
            keyCols[2], hv / 2 + 6, -hv / 2 + 6);
        Debug.LogFormat("[Switch Placement #{0}] The key column is column {1}. The key row is row {2}.", moduleId,
            keyCol + 1, keyRow + 1);
        
        for (int i = 0; i < 12; i++) {
            AssignValid(switches[i]);
        }
        moduleStarted = true;
    }

    // generates a valid set of 12 pairs of coordinates for the switches
    // restarts algorithm on rare edge case
    private int[,] GenerateGrid() {
        int[,] result = new int[12, 2];
        bool[,] taken = new bool[5, 5];
        for (int i = 0; i < 25; i++) {
            taken[i / 5, i % 5] = false; // entire grid is untaken
        }
        taken[0, 4] = true; // except for the top right

        int ci = 0, cj = 0; // row and col pointers
        bool vert;
        for (int i = 0; i < 12; i++) {
            while (taken[ci, cj]) {
                cj++;
                if (cj == 5) {
                    cj = 0;
                    ci++;
                }
            }

            if (ci == 4 && taken[ci, cj + 1]) return GenerateGrid(); // bail on edge case
            
            if (ci == 4) vert = false;
            else if (cj == 4 || cj == 3 && ci == 0 || taken[ci, cj + 1]) vert = true;
            else vert = UnityEngine.Random.Range(0, 10) < (ci < 3 ? 6 : 3); // weight direction based on location
            
            taken[ci, cj] = true;
            taken[vert ? ci + 1 : ci, vert ? cj : cj + 1] = true;
            result[i, 0] = ci * 5 + cj;
            result[i, 1] = (vert ? ci + 1 : ci) * 5 + (vert ? cj : cj + 1);
        }

        return result;
    }

    private void AssignValid(Switch s) {
        if (s.vert && s.cell1 / 5 == keyRow || !s.vert && s.cell1 % 5 == keyCol) {
            s.goalUp = true;
        }
        else if (s.vert && s.cell2 / 5 == keyRow || !s.vert && s.cell2 % 5 == keyCol) {
            s.goalUp = false;
        }
        else {
            int val1 = fGrids[fIndex][s.cell1 / 5][s.cell1 % 5] - fGrids[fIndex][keySquare / 5][keySquare % 5];
            if (val1 < 0) val1 += 25;
            int val2 = fGrids[fIndex][s.cell2 / 5][s.cell2 % 5] - fGrids[fIndex][keySquare / 5][keySquare % 5];
            if (val2 < 0) val2 += 25;
            s.goalUp = val1 < val2;
        }
    }

    // handles switch flips
    private KMSelectable.OnInteractHandler SwitchFlip(int i) {
        return delegate {
            if (moduleSolved) return false;

            if (coroutines[i] == null) {
                switchSelectables[i].AddInteractionPunch(.05f);
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, switchSelectables[i].transform);
                switches[i].up ^= true;
                coroutines[i] = StartCoroutine(ToggleSwitch(i));
                if (moduleStarted && switches.All(s => s.up == s.goalUp)) {
                    moduleSolved = true;
                    Module.HandlePass();
                    Audio.PlaySoundAtTransform("solve", Module.transform);
                    Debug.LogFormat("[Switch Placement #{0}] All switches correctly flipped. Module solved.", moduleId);
                }
            }
            
            return false;
        };
    }
    
    private IEnumerator ToggleSwitch(int i) {
        int switchFrom = switches[i].up ? -switchAngle : switchAngle;
        int switchTo = switches[i].up ? switchAngle : -switchAngle;
        var startTime = Time.fixedTime;
        const float duration = .3f;

        do {
            switchSelectables[i].transform.localEulerAngles = new Vector3(Easing.OutSine(Time.fixedTime - startTime, switchFrom, switchTo, duration), 0, 0);
            yield return null;
        } while (Time.fixedTime < startTime + duration);
        switchSelectables[i].transform.localEulerAngles = new Vector3(switchTo, 0, 0);
        coroutines[i] = null;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use <!{0} A2> to flip the switch occupying the cell in the first column and second row. Chain flips with spaces.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string command) {
        command = command.Trim().ToUpperInvariant();
        List<string> parameters = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        
        if (!parameters.All(s => s.Length == 2 && s != "E1"&& Regex.Match(s, @"[A-E][1-5]", 
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant).Success)) {
            yield return "sendtochaterror Invalid command";
        }
        else {
            foreach (string s in parameters) {
                while (coroutines.Any(c => c != null)) {
                    yield return new WaitForSeconds(0.1f);
                }
                int val = (s[1] - '1') * 5 + s[0] - 'A';
                for (int i = 0; i < 12; i++) {
                    if (switches[i].cell1 == val || switches[i].cell2 == val) {
                        switchSelectables[i].OnInteract();
                        break;
                    }
                }
            }
        }
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve () {
        yield return null;
    }
}

public class Switch {
    public int cell1, cell2; // positions in grid
    public bool up, vert, goalUp; // is it up/left? is it vertical? is the goal up/left?
    public Color color;

    public Switch(int cell1, int cell2, bool up, Color color) {
        this.cell1 = cell1;
        this.cell2 = cell2;
        this.up = up;
        this.color = color;
        this.vert = cell2 - cell1 == 5;
    }
}
