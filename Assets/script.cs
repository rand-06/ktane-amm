﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class script : MonoBehaviour {

    public VideoPlayer Video;
    public VideoClip[] solveVideos = new VideoClip[64];
    public KMAudio Audio;
    public AudioClip[] solveAudios = new AudioClip[64];
    public GameObject videoPlayer;

    public KMBombInfo bombInfo;
    public TextMesh[] lines = new TextMesh[20];
    public SpriteRenderer face;
    public Sprite[] faces = new Sprite[24];
    public GameObject[] states = new GameObject[10];
    public KMBombModule module;
    public TextMesh[] graph = new TextMesh[52];
    public TextMesh question;
    public TextMesh answer;

    private bool holdBool = false;
    private bool charging = false;
    private bool start = false;

    public TextMesh[] amounts = new TextMesh[12];
    public TextMesh[] brackets = new TextMesh[15];
    public TextMesh[] letters = new TextMesh[45];
    private int[] itemAmount = new int[12];
    private int[] itemBuffer = new int[12];
    private int[] wireComposerConfig = new int[15];
    private int wireComposerIndex = 0;
    private int batteryAmount = 1;

    private int wireComposerCircuit = -1;

    public TextMesh avgText;
    public TextMesh avgJudg;
    public TextMesh peakShow;
    public TextMesh peakList;
    public TextMesh invArrow;
    public TextMesh varList;

    public TextMesh[] ammWires = new TextMesh[4];
    public TextMesh colorblind;
    public TextMesh[] wires = new TextMesh[6];
    public TextMesh[] wireConfigs = new TextMesh[6];
    public TextMesh batteryConfig;


    public TextMesh lockAEAN;

    private string[] lockSentences = new string[] { "Hope you're proud\nof yourself.", "You had one job.", "Restart the mission.", "Was it worth it?", "Task successfully failed.\nCongratulations.", "What did you expect?", "Happy now?"};

    private int state = 1;
    private bool selected = false;
    private bool appendBusy = false;

    private Color offwhite = rgb(230, 223, 215);

    private Color offred = rgb(203, 60, 60);
    private Color offyellow = rgb(236, 219, 68);
    private Color offgreen = rgb(110, 197, 92);
    private Color offblue = rgb(98, 139, 243);

    private Color offorange = rgb(220, 140, 64);
    private Color offblack = rgb(88, 88, 88);
    private Color offbrown = rgb(139, 76, 22);
    private Color offpurple = rgb(178, 93, 214);
    private Color offgray = rgb(159, 156, 152);
    private Color offpink = rgb(217, 142, 138);
    private Color offcyan = rgb(104, 168, 168);
    private Color offjade = rgb(106, 178, 142);
    private Color offazure = rgb(102, 158, 193);
    private Color offrose = rgb(168, 86, 121);

    private float[] itemRes = new float[12]{1f, 4f, 10f, 25f, 40f, 200f, 100f, 250f, 440f, 720f, 1000f, 5000f};

    private int[] limits = new int[11] { 1000, 1600, 2300, 2600, 4100, 4900, 5600, 7200, 8200, 9100, 10000 };
    private int[] freqs = new int[10];
    private int[] table = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] graphInts = new int[52];
    private int[] peakFreqs;
    private int[] peakAmps;
    private int[] peakIds;
    private int page = 0;
    private int peakAmount = 0;
    private int avgAmp = 0;

    private string[] varNames = new string[12] { "HVO1", "HVO2", "HVO3", "HVO4", "HVO5", "LVO1", "LVO2", "LVO3", "LVO4", "LVO5", "AEAN", "BTR%" };
    private int inventoryIndex = 0;
    private int[] ABCD = new int[40] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    private int[] voltages = new int[10];
    private int[] nomVoltages = new int[10] { 416000, 750000, 1200000, 3000000, 4000000, 500, 800, 1200, 2000, 4500 };
    private int[] nomResistances = new int[10] { 52000,60000,100000,50000,250000,200,360,600,200,2250 };


    private int AEAN;      // human i remember you're
    private int BTR;
    private float dAEAN = 1f;
    private bool picked = false;
    private int faceID = 20;
    private int distance;
    private int chargeDigit = 0;
    private int[] wireCode = new int[4];
    private int[] batteryCode = new int[4];
    private bool wireCharge = false;
    public GameObject wireChargeMenu;
    public GameObject batteryChargeMenu;
    private string[] itemNames = new string[] { "Screwdriver", "Hammer", "Compressed Air", "Oilcan", "Black Wire", "Red Wire", "Yellow Wire", "Blue Wire", "White Wire", "Green Wire", "NKN-Resistor", "RGN-Resistor", "YYN-Resistor", "PRN-Resistor", "NKR-Resistor", "GKR-Resistor", "Car Battery" };
    public TextMesh inventory;
    private string sourceChargeConfig = "";


    static Color rgb(int r, int g, int b) { return new Color(r / 255f, g / 255f, b / 255f); }

    string divideBy1000(int num) {
        if ((num % 1000) < 10) return (num / 1000).ToString() + ".00" + (num % 1000).ToString();
        else if((num % 1000) < 100) return (num / 1000).ToString() + ".0" + (num % 1000).ToString();
        else return (num / 1000).ToString() + "." + (num % 1000).ToString();
    }
    string divideBy100(int num)
    {
        if ((num % 100) < 10) return (num / 100).ToString() + ".0" + (num % 100).ToString();
        else return (num / 100).ToString() + "." + (num % 100).ToString();
    }
    string spaces(int amount)
    {
        string ans = "";
        for (int i = 0; i < amount; i++) ans += " ";
        return ans;
    }

    IEnumerator onDef()
    {
        yield return new WaitForSeconds(3f);
        if (!selected)
        {
            states[state].SetActive(false);
            states[0].SetActive(true);
        }
        yield return null;
    }
    IEnumerator onFoc()
    {
        if (!start)
        {
            start = true;
            faceID = 18;
            face.sprite = faces[faceID];
        }
        yield return new WaitForSeconds(.5f);
        updateFace();
        states[0].SetActive(false);
        states[state].SetActive(true);
        yield return null;
    }

    void deleteItem(int index)
    {
        int[] peakFreqs1 = new int[peakFreqs.Length-1];
        int[] peakAmps1 = new int[peakFreqs.Length-1];
        int[] peakIds1 = new int[peakFreqs.Length-1];
        for (int i=0; i< peakFreqs.Length-1; i++)
        {
            peakFreqs1[i] = peakFreqs[i + (i < index ? 0 : 1)];
            peakAmps1[i] = peakAmps[i + (i < index ? 0 : 1)];
            peakIds1[i] = peakIds[i + (i < index ? 0 : 1)];
        }
        peakFreqs = peakFreqs1;
        peakAmps = peakAmps1;
        peakIds = peakIds1;
        peakAmount--;
        page = 0;
    }
    void redrawPeakDisplay()
    {
        if (peakAmount > 0)
        {
            peakShow.text = "Local peak " + page + ":\n\n"+ divideBy1000(peakAmps[page]) +" dB @ "+ divideBy1000(peakFreqs[page]) + " kHz";
            peakList.text = (page + 1).ToString() + "/" + peakAmount;
            for (int i = 0; i< peakAmount; i++)
            {
                graph[peakIds[i]].color = i == page ? offyellow : offwhite;
            }
        }
        else
        {
            peakShow.text = "";
            peakList.text = "";
        }
    }
    void redrawIndex()
    {
        invArrow.text = "";
        for (int i = 0; i < inventoryIndex; i++) invArrow.text += "\n";
        invArrow.text += ">";
    }
    void redrawVariables()
    {
        string ans = "";
        for (int i=0; i<10; i++)
        {
            ans += varNames[i] + ":" + spaces(17 - divideBy100(voltages[i]).Length) + divideBy100(voltages[i])+"\n";
        }
        ans += "\n\n";
        ans += varNames[10] + ":" + spaces(17 - divideBy100(AEAN).Length) + divideBy100(AEAN) + "\n";
        ans += varNames[11] + ":" + spaces(17 - divideBy100(BTR).Length) + divideBy100(BTR) + "\n";
        varList.text = ans;
    }
    void setState(int newState)
    {
        states[state].SetActive(false);
        state = newState;
        states[state].SetActive(true);
    }
    void generate()
    {
        int amount = Random.Range(7, 13);
        int nonCrit = Random.Range(2, 6);
        for (int i=0; i<amount; i++)
        {
            int n = Random.Range(0, 20);
            while(table[n]!=0) n = Random.Range(0, 20);
            table[n] = 1;
            if (n < 10) peakAmount++;
        }
        for (int i = 0; i < nonCrit; i++)
        {
            int n = Random.Range(0, 20);
            while (table[n] != 0) n = Random.Range(0, 20);
            table[n] = 2;
            if (n < 10) peakAmount++;
        }
        for (int i=0; i<10; i++)
        {
            freqs[i] = Random.Range(limits[i] + 1, limits[i + 1]);
        }
        peakFreqs = new int[peakAmount];
        peakAmps  = new int[peakAmount];
        peakIds   = new int[peakAmount];
        int k = 0;
        int j = 0;
        int sum = 0;
        for (int i=0; i<52; i++)
        {
            if (j<10 && (1000 + i * 9000 / 52) <= freqs[j] && (1000 + (i + 1) * 9000 / 52) > freqs[j])
            {
                graphInts[i] = table[j] == 1 ? Random.Range(5000, 7500) : table[j] == 2 ? Random.Range(3500, 5000) : Random.Range(2000, 2800);
                if (table[j] > 0)
                {
                    peakAmps[k] = graphInts[i];
                    peakFreqs[k] = freqs[j];
                    peakIds[k] = i;
                    k++;
                }
                j++;
            }
            else graphInts[i] = Random.Range(2000, 2800);
            sum += graphInts[i];
        }
        avgAmp = sum / 52;
        redrawGraphScreen();

        for (int i=0; i<10; i++)
        {
            if (table[10 + i] == 0) voltages[i] = Random.Range(
                    (int)((i < 5 ? 0.97f : 0.9f) * nomVoltages[i] + 1),
                    (int)((i < 5 ? 1.03f : 1.1f) * nomVoltages[i])
                    );
            else if (table[10 + i] == 1) voltages[i] = i < 5 ? Random.Range(0, 19) : 0;
            else
                voltages[i] = Random.Range(nomVoltages[i] / 2, nomVoltages[i] * 2); 
                while ((float)voltages[i]/nomVoltages[i]>= (i < 5 ? 0.97f : 0.9f) && (float)voltages[i] / nomVoltages[i] <= (i < 5 ? 1.03f : 1.1f)) voltages[i] = Random.Range(nomVoltages[i] / 2, nomVoltages[i] * 2);
            // good luck with that one, you have 10^15 combinations in wire composer
        }
        redrawVariables();

        for (int i=0; i<12; i++)
        {
            itemAmount[i] = Random.Range(50, (i!=5 && i!=6)?100:60);
        }

    }
    void refreshQuestion()
    {
        question.text = wireComposerCircuit < 10 ? "Which circuit needs\nto be adjusted ?" : "How do you want\nto connect it ?\n\n1 - Serial\n0 - Parallel";
        answer.text = wireComposerCircuit > -1 && wireComposerCircuit < 10 ? varNames[wireComposerCircuit] : "";
    }
    void refreshWireComposer()
    {
        for (int i = 0; i < 15; i++) wireComposerConfig[i] = -1;
        wireComposerIndex = 0;
        for (int i=0; i<12; i++) itemBuffer[i] = itemAmount[i];
        redrawWireComposerScreen();
    }
    void redrawWireComposerScreen()
    {
        for (int i=0;i<12;i++)
        {
            amounts[i].text = "x" + itemBuffer[i];
        }
        for(int i=0; i<15; i++)
        {
            if (wireComposerConfig[i] > -1 && wireComposerConfig[i] < 6) brackets[i].text = "-- --";
            else if (wireComposerConfig[i] > 5) brackets[i].text = "#   #";
            else brackets[i].text = (i == wireComposerIndex) ? "> ? <" : "";
            switch (wireComposerConfig[i])
            {
                case -1:
                    {

                        letters[3 * i + 0].text = "";
                        letters[3 * i + 1].text = "";
                        letters[3 * i + 2].text = "";

                        break;
                    }
                case 0:
                    {
                        
                        letters[3 * i + 0].text = "";
                        letters[3 * i + 1].text = "K";
                        letters[3 * i + 2].text = "";
                        letters[3 * i + 1].color = offblack;
                        
                        break;
                    }
                case 1:
                    {
                        letters[3 * i + 0].text = "";
                        letters[3 * i + 1].text = "R";
                        letters[3 * i + 2].text = "";
                        letters[3 * i + 1].color = offred;
                        break;
                    }
                case 2:
                    {
                        letters[3 * i + 0].text = "";
                        letters[3 * i + 1].text = "Y";
                        letters[3 * i + 2].text = "";
                        letters[3 * i + 1].color = offyellow;
                        break;
                    }
                case 3:
                    {
                        letters[3 * i + 0].text = "";
                        letters[3 * i + 1].text = "B";
                        letters[3 * i + 2].text = "";
                        letters[3 * i + 1].color = offblue;
                        break;
                    }
                case 4:
                    {
                        letters[3 * i + 0].text = "";
                        letters[3 * i + 1].text = "W";
                        letters[3 * i + 2].text = "";
                        letters[3 * i + 1].color = offwhite;
                        break;
                    }
                case 5:
                    {
                        letters[3 * i + 0].text = "";
                        letters[3 * i + 1].text = "G";
                        letters[3 * i + 2].text = "";
                        letters[3 * i + 1].color = offgreen;
                        break;
                    }
                case 6:
                    {
                        letters[3 * i + 0].text = "N";
                        letters[3 * i + 1].text = "K";
                        letters[3 * i + 2].text = "N";
                        letters[3 * i + 0].color = offbrown;
                        letters[3 * i + 1].color = offblack;
                        letters[3 * i + 2].color = offbrown;
                        break;
                    }
                case 7:
                    {
                        letters[3 * i + 0].text = "R";
                        letters[3 * i + 1].text = "G";
                        letters[3 * i + 2].text = "N";
                        letters[3 * i + 0].color = offred;
                        letters[3 * i + 1].color = offgreen;
                        letters[3 * i + 2].color = offbrown;
                        break;
                    }
                case 8:
                    {
                        letters[3 * i + 0].text = "Y";
                        letters[3 * i + 1].text = "Y";
                        letters[3 * i + 2].text = "N";
                        letters[3 * i + 0].color = offyellow;
                        letters[3 * i + 1].color = offyellow;
                        letters[3 * i + 2].color = offbrown;
                        break;
                    }
                case 9:
                    {
                        letters[3 * i + 0].text = "P";
                        letters[3 * i + 1].text = "R";
                        letters[3 * i + 2].text = "N";
                        letters[3 * i + 0].color = offpurple;
                        letters[3 * i + 1].color = offred;
                        letters[3 * i + 2].color = offbrown;
                        break;
                    }
                case 10:
                    {
                        letters[3 * i + 0].text = "N";
                        letters[3 * i + 1].text = "K";
                        letters[3 * i + 2].text = "R";
                        letters[3 * i + 0].color = offbrown;
                        letters[3 * i + 1].color = offblack;
                        letters[3 * i + 2].color = offred;
                        break;
                    }
                case 11:
                    {
                        letters[3 * i + 0].text = "G";
                        letters[3 * i + 1].text = "K";
                        letters[3 * i + 2].text = "R";
                        letters[3 * i + 0].color = offgreen;
                        letters[3 * i + 1].color = offblack;
                        letters[3 * i + 2].color = offred;
                        break;
                    }
            }
            brackets[i].color = (i == wireComposerIndex) ? offyellow : offwhite;
        }
    }
    void selectInWireComposer(int item)
    {
        if (item == -1)
        {
            if (wireComposerConfig[wireComposerIndex] != -1) itemBuffer[wireComposerConfig[wireComposerIndex]]++;
            wireComposerConfig[wireComposerIndex] = item;
        } else
        if (wireComposerConfig[wireComposerIndex] == item) {
            itemBuffer[item]++;
            wireComposerConfig[wireComposerIndex] = -1;
        }
        else if (itemBuffer[item] != 0) {
            itemBuffer[item]--;
            if (wireComposerConfig[wireComposerIndex]!=-1) itemBuffer[wireComposerConfig[wireComposerIndex]]++;
            wireComposerConfig[wireComposerIndex] = item;
        }
        redrawWireComposerScreen();
    }
    void redrawGraphScreen()
    {
        for (int i = 0; i < 52; i++) graph[i].transform.localScale = new Vector3(1f, graphInts[i] / 7500f, 1f);
        avgText.text = "Avg: " + divideBy1000(avgAmp);
        if (avgAmp < 2800)
        {
            avgJudg.text = "PASS";
            avgJudg.color = Color.green;
        }
        else if (avgAmp < 3200)
        {
            avgJudg.text = "WARN";
            avgJudg.color = offyellow;
        }
        else
        {
            avgJudg.text = "FAIL";
            avgJudg.color = Color.red;
        }
    }
    void lineFeed()
    {
        for (int i=0; i<19; i++)
        {
            lines[i].text = lines[i + 1].text;
            lines[i].color = lines[i + 1].color;
            lines[i].fontStyle = lines[i + 1].fontStyle;
        }
        lines[19].text = "";
        lines[19].color = offwhite;
        lines[19].fontStyle = FontStyle.Normal;
    }
    void clearScreen()
    {
        for (int i = 0; i < 20; i++)
        {
            lines[i].text = "";
            lines[i].color = offwhite;
            lines[i].fontStyle = 0;
        }
    }
    IEnumerator appendText(string text, Color color, bool rightAlign=false, FontStyle style = 0, bool anim =true)
    {
        if (!appendBusy)
        {
            appendBusy = true;
            for (int i = 0; i < text.Length; i++)
            {
                if ((i % 30) == 0)
                {
                    lineFeed();
                    lines[19].text = rightAlign ? "                              " : "";
                    lines[19].color = color;
                    lines[19].fontStyle = style;
                }
                if (anim) yield return new WaitForSeconds(.03f);
                lines[19].text = (rightAlign ? lines[19].text.Substring(1) : lines[19].text) + text[i].ToString();
            }
            appendBusy = false;
        }
        yield return null;
    }
    IEnumerator cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            faceID ^= 1;
            face.sprite = faces[faceID];
        }
    }
    void AbcdCalc()
    {
        int j = 0;
        for (int i=0; i<10; i++)
        {
            if (table[i] != 0)
            {
                ABCD[i] = peakFreqs[j] % 100 /10 ;
                ABCD[10 + i] = peakFreqs[j] % 10;
                ABCD[20 + i] = (peakAmps[j] - avgAmp) % 100 / 10;
                ABCD[30 + i] = (peakAmps[j] - avgAmp) % 10;
                j++;
            }
            else
            {
                ABCD[i] = -1;
                ABCD[10+i] = -1;
                ABCD[20+i] = -1;
                ABCD[30+i] = -1;
            }
        }
    }
    void LOCK()
    {
        GetComponent<KMSelectable>().OnFocus = null;
        GetComponent<KMSelectable>().OnDefocus = null;
        selected = false;
        lockAEAN.text = "AMM ran away.\n\n" + lockSentences[Random.Range(0, lockSentences.Length)] + "\n\nAEAN: " + divideBy100(AEAN);
        setState(4);
    }
    IEnumerator SOLVE()
    {
        GetComponent<KMSelectable>().OnFocus = null;
        GetComponent<KMSelectable>().OnDefocus = null;
        selected = false;
        int ind = Random.Range(0, 64);
        Video.clip = solveVideos[ind];
        Video.Play();
        videoPlayer.SetActive(true);
        Audio.PlaySoundAtTransform(solveAudios[ind].name,transform);
        yield return new WaitForSeconds(solveAudios[ind].length);
        videoPlayer.SetActive(false);
        setState(6);
        module.HandlePass();

        yield return null;
    }
    void check()
    {
        if (AEAN >= 12000) { LOCK(); return; }
        bool judge = true;
        for (int i=0; i<20; i++) if (table[i] == 1) return; else if (table[i] == 1) judge = false;
        if ((AEAN < 2000 || judge) && (BTR > 499 || charging)) StartCoroutine(SOLVE());
    }
    void updateFace()
    {
        if (picked && AEAN < 9000) faceID = 22;
        else if (AEAN < 3000) faceID = 0;
        else if (AEAN < 4500) faceID = 2;
        else if (AEAN < 6000) faceID = 4;
        else if (AEAN < 8000) faceID = 6;
        else if (AEAN < 9000) faceID = 8;
        else if (AEAN < 10500) faceID = 10;
        else if (AEAN < 12000) faceID = 12;

    }
    void pass(int index)
    {
        if (index < 10)
        {
            int ind = 0;
            for (int i = 0; i < index; i++) if (table[i] != 0) ind++;
            table[index] = 0;
            int old = graphInts[peakIds[ind]];
            graphInts[peakIds[ind]] = Random.Range(2000, 2800);
            avgAmp += (graphInts[peakIds[ind]] - old) / 52;
            deleteItem(ind);
            for (int i = 0; i < 52; i++) graph[i].color = offwhite;
            redrawPeakDisplay();
            redrawGraphScreen();
            AbcdCalc();
        }
        else
        {
            table[index] = 0;
            for (int i = 0; i < 10; i++) itemAmount[i] = itemBuffer[i];
            redrawInventory();
        }
        if (dAEAN < 0) dAEAN = 1f;
        else dAEAN += dAEAN == 2f ? 0f : .25f;
        AEAN -= (int)(Random.Range(500, 1000) * dAEAN);
        if (AEAN < 0) AEAN = 0;
        redrawVariables();
        setState(1);
        updateFace();
        check();
    }
    void fail(int index)
    {
        if (dAEAN > 0) dAEAN = -1f;
        else dAEAN -= dAEAN == -2f ? 0f : .25f;
        AEAN -= (int)(Random.Range(500, 1000) * dAEAN);
        redrawVariables();
        setState(1);
        updateFace();
        check();
    }
    float resistance()
    {
        float ans=0;
        for (int j=0; j<3; j++)
        {
            float midAns=0;
            for (int i=0; i<5; i++)
            {
                if (wireComposerConfig[j * 5 + i] != -1) midAns += 1 / itemRes[wireComposerConfig[j * 5 + i]];
            }
            if (midAns != 0) ans += 1 / midAns;
        }
        return ans;
    }
    void checkRes(bool parallel = false)
    {
        int index = wireComposerCircuit % 10;
        int newVolt;
        int res = table[index+10]==2?(int)(nomResistances[index] * ((float)voltages[index] / nomVoltages[index])):0;
        int newres = parallel ? (int)(100 / (1 / resistance() + 1 / (res / 100f))) : res + (int)(resistance() * 100);
        newVolt = (int)(nomVoltages[index] * ((float)newres / nomResistances[index]));
        Debug.Log("res = " + resistance() + ", newres = " + divideBy100(newres) + ", newvolt = " + divideBy100(newVolt));
        if ((float)newVolt / nomVoltages[index] >= (index < 5 ? 0.97f : 0.9f) && (float)newVolt / nomVoltages[index] <= (index < 5 ? 1.03f : 1.1f))
        {
            voltages[index] = newVolt;
            pass(10 + index);
        }
        else
        {
            fail(10 + index);
        }
    }

    void generateChargeConfigs()
    {
        string colorString = "WRYGBOKNPAICJZS";
        Color[] colorArray = new Color[] { offwhite, offred, offyellow, offgreen, offblue, offorange, offblack, offbrown, offpurple, offgray, offpink, offcyan, offjade, offazure, offrose };

        int[] wireNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Shuffle().ToArray();         //length: 6 {4 8 6 3 1 5} значит цифры 4 для первого, 8 для второго...
        int[] wireConfig = new List<int> { 0, 1, 2, 3, 4, 5}.Shuffle().ToArray();                       //length: 4 {5 2 1 4} значит красный к пятому, желтый ко второму...
        int[] wireIndexes = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Shuffle().ToArray();         //length: 6 {8 3 2 7 6 0} значит цвета 8 для первого, 3 для второго...
        int[] batteryNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Shuffle().ToArray();      //length: 4 {3 6 2 7} значит цифра для первого цвета - 3...
        int[] batteryWireIndexes = new List<int> { 0, 1, 2, 3 }.Shuffle().ToArray();                    //length: 4 
        int[] wiresLocal = new List<int> { 0, 1, 2, 3 }.Shuffle().ToArray();                            //length: 4
        bool[] batteryShuffler = new List<bool> { true, true, true, false, false, false, false }.Shuffle().ToArray();
        sourceChargeConfig = "";
        for (int i=0; i<6; i++)
        {
            wireConfigs[i].text = colorString[wireIndexes[i] + 5].ToString() + "          " + wireNumbers[i];
            wires[i].color = colorArray[wireIndexes[i] + 5];
        }
        int switcher = 0;
        string batteryConfigAns = "";
        for(int i=0; i<4; i++)
        {
            ammWires[i].color = colorArray[1 + wiresLocal[i]];
        }
        for (int i=0; i<7; i++)
        {
            if (batteryShuffler[i])
            {
                batteryConfigAns += colorString[batteryWireIndexes[switcher / 16]+1].ToString();
                switcher += 16;
            }
            else
            {
                batteryConfigAns += batteryNumbers[switcher % 4].ToString();
                switcher += 1;
            }
        }
        colorblind.text = colorString[1 + wiresLocal[0]].ToString() + colorString[1 + wiresLocal[1]].ToString() + "         " + colorString[1 + wiresLocal[2]].ToString() + colorString[1 + wiresLocal[3]].ToString();
        batteryConfig.text = batteryConfigAns;

        for (int i = 0; i < 4; i++)
        {
            wireCode[i] = wireNumbers[wireConfig[wiresLocal[i]]];
            batteryCode[i] = batteryNumbers[Array.IndexOf(batteryWireIndexes,wiresLocal[i])];
            sourceChargeConfig += colorString[wireIndexes[wireConfig[i]] + 5].ToString();
        }
        //Debug.Log(wireCode[0].ToString() + wireCode[1].ToString() + wireCode[2].ToString() + wireCode[3].ToString());
        //Debug.Log(batteryCode[0].ToString() + batteryCode[1].ToString() + batteryCode[2].ToString() + batteryCode[3].ToString());
    }

    int min(int a, int b) { return a > b ? b : a; }
    IEnumerator btrIncrement()
    {
        int batteryCapacity = wireCharge ? 10000 - BTR : min(10000 - BTR,Random.Range(7500, 8500));
        while (batteryCapacity > 0)
        {
            yield return new WaitForSeconds(1.44f);
            BTR++;
            batteryCapacity--;
            redrawVariables();
        }
        
    }

    void redrawInventory()
    {
        string ans = "";
        for (int i=0; i<itemNames.Length; i++)
        {
            ans += itemNames[i];
            if (i > 3) ans += (spaces(25-itemNames[i].Length- (i == itemNames.Length - 1 ? batteryAmount : itemAmount[i - 4]).ToString().Length - 1) + "x" + (i == itemNames.Length - 1 ? batteryAmount : itemAmount[i - 4]).ToString());
            ans += "\n";
        }
        inventory.text = ans;
    }
    void chargePress(int digit)
    {
        if (digit == (wireCharge?wireCode[chargeDigit]:batteryCode[chargeDigit]))
        {
            chargeDigit++;
            if (chargeDigit == 4) {
                StartCoroutine(btrIncrement());
                StartCoroutine(appendText("Charging...", offgreen));
                setState(1);
                charging = true;
                chargeDigit = 0;
                check();
            }
        }
        else
        {
            chargeDigit = 0;
            generateChargeConfigs();
            setState(1);
            AEAN -= Random.Range(300, 600);
        }
    }

    IEnumerator search()
    {
        while(picked && distance != 0)
        {
            yield return new WaitForSeconds(0.05f);
            distance--;
        }
        if (distance==0) StartCoroutine(appendText("Found power source. Config: " + sourceChargeConfig, offyellow));
    }
    IEnumerator holding()
    {
        holdBool = true;
        yield return new WaitForSeconds(5f);
        if (holdBool)
        {
            StartCoroutine(appendText(picked?"Put down.":"Picked up.", offgreen));
            picked = !picked;
            holdBool = false;
            StartCoroutine(search());
        }
        if (holdBool == false && distance == 0)
        {
            wireCharge = true;
            wireChargeMenu.SetActive(true);
            batteryChargeMenu.SetActive(false);
            setState(9);
        }
        updateFace();
        yield return null;
    }


    void Start () {
        videoPlayer.SetActive(false);
        distance = Random.Range(150, 301) * 20;
        AEAN = Random.Range(6000, 8000);
        BTR = Random.Range(10, 1000);
        states[0].SetActive(true);
        states[1].SetActive(false);
        states[2].SetActive(false);
        states[3].SetActive(false);
        states[4].SetActive(false);
        states[5].SetActive(false);
        states[6].SetActive(false);
        states[7].SetActive(false);
        states[8].SetActive(false);
        states[9].SetActive(false);
        face.color = new Color(1, 1, 1, 1);
        GetComponent<KMSelectable>().OnFocus += delegate () { selected = true; StartCoroutine(onFoc()); };
        GetComponent<KMSelectable>().OnDefocus += delegate () { selected = false; StartCoroutine(onDef()); };
        face.sprite = faces[faceID];
        generate();
        AbcdCalc();
        redrawPeakDisplay();
        StartCoroutine(cycle());
        refreshWireComposer();
        generateChargeConfigs();
        redrawInventory();
        clearScreen();
        StartCoroutine(appendText("Initial message.", offwhite, false, FontStyle.Bold,false));
        //setState(9);
    }

    void Update()
    {
        if (selected)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                states[state].SetActive(false);
                state = state == 3 ? 1 : 3;
                states[state].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                states[state].SetActive(false);
                state = state == 2 ? 1 : 2;
                states[state].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                states[state].SetActive(false);
                state = state == 5 ? 1 : 5;
                states[state].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (state == 2)
                {
                    page = page == 0 ? peakAmount - 1 : page - 1; redrawPeakDisplay();
                }
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (state == 2)
                {
                    page = page == peakAmount - 1 ? 0 : page + 1; redrawPeakDisplay();
                }
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (state == 5)
                {
                    inventoryIndex = inventoryIndex == 0 ? 16 : inventoryIndex - 1; redrawIndex();
                }
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (state == 5)
                {
                    inventoryIndex = inventoryIndex == 16 ? 0 : inventoryIndex + 1; redrawIndex();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 0:
                            { //Screwdriver
                                if (last == (
                                    ((10 * (
                                    (ABCD[4] + ABCD[14])) / (ABCD[24] + ABCD[34] + 1)
                                    )) % 10) && table[4] != 0)
                                    pass(4);
                                else fail(4);
                                break;
                            }
                        case 1:
                            { //Hammer
                                if (last == ((ABCD[3] + ABCD[33]) * (ABCD[23] + ABCD[13]) % 10) && table[3] != 0)
                                    pass(3);
                                else fail(3);
                                break;
                            }
                        case 2:
                            { //Compressed Air
                                if (last != ABCD[7] && last != ABCD[17] && last != ABCD[27] && last != ABCD[37] && table[7] != 0)
                                    pass(7);
                                else fail(7);
                                break;
                            }
                        case 3:
                            { //Oilcan
                                if (last == 0 && table[0] != 0)
                                    pass(0);
                                else fail(0);
                                break;
                            }
                        default: return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 1:
                            { //Hammer
                                if (last == ((ABCD[6] + ABCD[36]) * (ABCD[26] + ABCD[16]) % 10) && table[6] != 0)
                                    pass(6);
                                else fail(6);
                                break;
                            }
                        case 2:
                            { //Compressed Air
                                if (last != ABCD[8] && last != ABCD[18] && last != ABCD[28] && last != ABCD[38] && table[8] != 0)
                                    pass(8);
                                else fail(8);
                                break;
                            }
                        case 3:
                            { //Oilcan
                                if (last == 0 && table[1] != 0)
                                    pass(1);
                                else fail(1);
                                break;
                            }
                        default: return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 2:
                            { //Compressed Air
                                if (last != ABCD[9] && last != ABCD[19] && last != ABCD[29] && last != ABCD[39] && table[9] != 0)
                                    pass(9);
                                else fail(9);
                                break;
                            }
                        case 3:
                            { //Oilcan
                                if (last == 0 && table[2] != 0)
                                    pass(2);
                                else fail(2);
                                break;
                            }
                        default: return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (state == 5)
                    if (inventoryIndex == 3)
                        if ((int)bombInfo.GetTime() % 10 == 0 && table[5] != 0) pass(5);
                        else fail(5);
                    else return;
            }


            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                if (state == 7)
                {
                    selectInWireComposer(-1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (state == 7)
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) selectInWireComposer(6); else selectInWireComposer(0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (state == 7)
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) selectInWireComposer(7); else selectInWireComposer(1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (state == 7)
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) selectInWireComposer(8); else selectInWireComposer(2);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (state == 7)
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) selectInWireComposer(9); else selectInWireComposer(3);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                if (state == 7)
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) selectInWireComposer(10); else selectInWireComposer(4);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                if (state == 7)
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) selectInWireComposer(11); else selectInWireComposer(5);
                }
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (state == 7)
                {
                    wireComposerIndex = 5 * (wireComposerIndex / 5) + (wireComposerIndex + 4) % 5;
                    redrawWireComposerScreen();
                }
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (state == 7)
                {
                    wireComposerIndex = 5 * (wireComposerIndex / 5) + (wireComposerIndex + 1) % 5;
                    redrawWireComposerScreen();
                }
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (state == 7)
                {
                    wireComposerIndex = (wireComposerIndex + 10) % 15;
                    redrawWireComposerScreen();
                }
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (state == 7)
                {
                    wireComposerIndex = (wireComposerIndex + 5) % 15;
                    redrawWireComposerScreen();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (state == 5)
                {
                    if (inventoryIndex > 3 && inventoryIndex < 16) setState(7);
                    else if (inventoryIndex == 16  && !charging && batteryAmount>0)
                    {
                        batteryAmount--;
                        redrawInventory();
                        wireCharge = false;
                        wireChargeMenu.SetActive(false);
                        batteryChargeMenu.SetActive(true);
                        setState(9);
                    }
                } else if (state == 7)
                {
                    if (resistance() != 0)
                    {
                        wireComposerCircuit = -1;
                        refreshQuestion();
                        setState(8);
                    }
                } else if (state == 8)
                {
                    if (wireComposerCircuit > -1 && wireComposerCircuit < 10)
                    {
                        if (table[10 + wireComposerCircuit] == 0) fail(10 + wireComposerCircuit);
                        else if (table[10 + wireComposerCircuit] == 1) checkRes();
                        else if (table[10 + wireComposerCircuit] == 2)
                        {
                            wireComposerCircuit += 10;
                            refreshQuestion();
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (state == 7) setState(5);
                else if (state == 8) setState(7);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 9; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 0; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 1; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 2; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 3; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 4; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 5; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 6; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 7; refreshQuestion(); }
            if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) if (state == 8 && wireComposerCircuit < 10) { wireComposerCircuit = 8; refreshQuestion(); }

            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) if (state == 8 && wireComposerCircuit > 9) { checkRes(false); }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) if (state == 8 && wireComposerCircuit > 9) { checkRes(true); }

            if (Input.GetKeyDown(KeyCode.Q) && !holdBool && !charging) StartCoroutine(holding());
            if (Input.GetKeyUp(KeyCode.Q) && holdBool && !charging)
            {
                holdBool = false;
                picked = false;
                AEAN -= Random.Range(500, 1000);
                updateFace();
                StartCoroutine(appendText("Dropped.", offred));
                StopCoroutine(holding());
            }

            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) if (state == 9) { chargePress(0); }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) if (state == 9) { chargePress(1); }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) if (state == 9) { chargePress(2); }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) if (state == 9) { chargePress(3); }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) if (state == 9) { chargePress(4); }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) if (state == 9) { chargePress(5); }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) if (state == 9) { chargePress(6); }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) if (state == 9) { chargePress(7); }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) if (state == 9) { chargePress(8); }
            if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) if (state == 9) { chargePress(9); }

            if (Input.GetKeyDown(KeyCode.Home)) { StartCoroutine(SOLVE()); }
        }
    }
}
