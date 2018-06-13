using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using world;
using BriljaSanLib;

public class CTRL : MonoBehaviour
{
    public int local, spot;
    public int time = -1, dayTh;
    int year = 365;
    int myCompat = 42;
    float friProb = .97f;
    public bool reRun;
    string quickEasyLocation;

    public int numbFriends { get { return friends.Count; } }
    // 0
    public int cpuSci { get { return majors[0]; } }
    // 1
    public int bio { get { return majors[1]; } }
    // 2
    public int astrophys { get { return majors[2]; } }
    // 3
    public int visArts { get { return majors[3]; } }
    // 4
    public int marketing { get { return majors[4]; } }

    public int hour { get { return time % 24; } }
    public int day { get { return time / 24; } }
    public int[] majors = new int[5];
    List<Friend> friends = new List<Friend>();
    Friend[] busPeople = new Friend[8], parkPeople = new Friend[4];
    string[] mNames = new string[] { "Computer Science", "Biology", "Astrophysics", "Visual Arts", "Marketing" };
    List<Friend> notifNames = new List<Friend>();
    string file,file2,fD;
    bool fullData;

    [Range(0, 10)]
    public float secPerHour = 1f;
    GameObject[] busPass = new GameObject[8], parkFolk = new GameObject[4];
    Text dayTxt, friendsTxt, tData;
    RectTransform clockTick;
    Image clockImg, show;
    Transform sun;
    GameObject p, v, bd, roadAni;
    World w = new World();
    Vector3 timeDay = new Vector3(50, -30, 0);
    Vector3 timeNight = new Vector3(-90, -30, 0);
    Vector3[] busStartPos, parkStartPos;
    int[] busSeat = new int[8];
    int[] parkSeat = new int[4];
    Friend f, f1, f2;

    void Start()
    {
        w.seed = GameObject.Find("transfer").GetComponent<menu>().seed;
        w.momOff = GameObject.Find("transfer").GetComponent<menu>().rWM;

        w.seed = w.seed < 0 ? Random.Range(0, 9999999) : w.seed;
        Random.InitState(w.seed);

        file = Path.Combine(Application.dataPath, Generate.AddTimeStamp(w.seed+"_SimF")+".csv");
        file2 = Path.Combine(Application.dataPath, Generate.AddTimeStamp(w.seed + "_SimS") +".csv");
        fD = Path.Combine(Application.dataPath, Generate.AddTimeStamp(w.seed + "_F_Hist") + ".csv");


        p = GameObject.Find("person");
        v = GameObject.Find("friend");
        bd = GameObject.Find("bus driver");
        sun = GameObject.Find("Sun").GetComponent<Transform>();
        roadAni = GameObject.Find("rLoop");
        clockImg = GameObject.Find("clockImg").GetComponent<Image>();
        clockTick = GameObject.Find("cTick").GetComponent<RectTransform>();
        dayTxt = GameObject.Find("dTxt").GetComponent<Text>();
        friendsTxt = GameObject.Find("fTxt").GetComponent<Text>();
        tData = GameObject.Find("tData").GetComponent<Text>();
        show = GameObject.Find("show").GetComponent<Image>();
        show.transform.localPosition = Vector3.down * 650;

        // 
        for (int i = 0; i < busPass.Length; i++)
        {
            busPass[i] = GameObject.Find("pass" + i);
        }

        // 
        for (int i = 0; i < parkFolk.Length; i++)
        {
            parkFolk[i] = GameObject.Find("parkFolk" + i);
        }

        SetAppearance(bd, w.skinType[5], Color.gray, Color.blue, Color.black, Vector3.forward*60 + Vector3.up*60);

        AddAsFriend(new Friend("Mom", 4, 3, myCompat + w.momOff, w.skinType[0], new Color32(98, 0, 0, 255), Color.yellow, Color.green, Vector3.forward * 80));
        SetAppearance(v, friends[0].skin, friends[0].hair, friends[0].shirt, friends[0].pants, friends[0].hairStyle);
        f = friends[0];

        busStartPos = new Vector3[8];
        parkStartPos = new Vector3[4];

        // 
        for (int i = 0; i < busStartPos.Length; i++)
        {
            busStartPos[i] = busPass[i].transform.position;
        }

        // 
        for (int i = 0; i < parkStartPos.Length; i++)
        {
            parkStartPos[i] = parkFolk[i].transform.position;
        }

        // 
        if (!File.Exists(file) && fullData)
        {
            StreamWriter sw = new StreamWriter(file);
            sw.Write(w.seed +"\nDay,Computer Science,Biology,Astrophysics,Visual Arts,Marketing\n");
            sw.Close();
        }

        if (!File.Exists(file2) && !fullData)
        {
            StreamWriter sw = new StreamWriter(file2);
            sw.Write(w.seed + "\nDay,Computer Science,Biology,Astrophysics,Visual Arts,Marketing\n");
            sw.Close();
        }

        StartCoroutine(IncreaseTime());
    }

    void WriteData(bool full, string wr)
    {
        // Majors
        StreamReader sr = new StreamReader(full ? file : file2);
        string save = sr.ReadToEnd();
        sr.Close();

        StreamWriter sw = new StreamWriter(full ? file : file2);
        sw.Write(save+wr);
        sw.Close();


        // Friends

        // Check if file exists
        if (File.Exists(fD))
        {
            // Read all of the data on the file
            StreamReader sr2 = new StreamReader(fD);
            // store that read data to this string
            string fLoadAll = sr2.ReadToEnd();
            // close the file
            sr2.Close();

            // now we need a new string that will store the new data to add
            string[] temp = fLoadAll.Split('\n'); //day + "," + w.fM.retAll();

            string fLoadN = "Day," + w.fM.retAll();

            // 
            for (int i = 1; i < temp.Length; i++)
            {
                fLoadN += "\n" + temp[i];
            }

            List<string> finder = w.fM.who;
            int[] store = new int[finder.Count];

            for (int i = 0; i < finder.Count; i++)
            {
                for (int j = 0; j < friends.Count; j++)
                {
                    if (finder[i] == friends[j].name + FriendManager.Additive(friends[j]))
                    {
                        store[i] = friends[j].relationship;
                    }
                }
            }

            string addTo = "";

            for (int i = 0; i < store.Length; i++)
            {
                addTo += store[i] + ",";
            }

            string newDataLine = ""+day+","+addTo;

            fLoadN += "\n"+newDataLine;
            // find the new data line to add to file

            // write the new data line to the data file by first writing the previous data, & then new line, add fLoadN
            StreamWriter sw3 = new StreamWriter(fD);
            sw3.Write(fLoadN);
            sw3.Close();
        }
        else // create a new file with only the day column on it
        {
            StreamWriter sw2 = new StreamWriter(fD);
            sw2.Write("Day,");
            sw2.Close();
        }
    }

    void SetAppearance(GameObject who, Color32 skin, Color32 hair, Color32 shirt, Color32 pants, Vector3 hairStyle)
    {
        Texture2D tex = new Texture2D(32, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.SetPixel(1, 0, skin);
        tex.SetPixel(2, 0, shirt);
        tex.SetPixel(3, 0, pants);
        tex.SetPixel(4, 0, hair);
        tex.Apply();

        Renderer[] allR = who.GetComponentsInChildren<Renderer>();
        allR[0].material.mainTexture = tex;
        allR[1].material.mainTexture = tex;

        SkinnedMeshRenderer sMR = who.GetComponentInChildren<SkinnedMeshRenderer>();
        sMR.SetBlendShapeWeight(0, hairStyle.x);
        sMR.SetBlendShapeWeight(1, hairStyle.y);
        sMR.SetBlendShapeWeight(2, hairStyle.z);
    }

    IEnumerator IncreaseTime()
    {
        time++;
        clockImg.color = hour < 19 && hour > 7 ? new Color32(255, 227, 51, 255) : new Color32(0, 17, 163, 255);
        sun.transform.eulerAngles = hour < 19 && hour > 7 ? timeDay : timeNight;
        bool newDay = (time % 24 == 1);

        float dice = Random.value;

        if (newDay)
        {
            dayTh = Random.Range(0, majors.Length);
        }

        dice = Random.value;
        float where = (local == 0) ? .8f : .7f;
        int store = local;
        Friend busPartner;

        if (dice > where)
        {
            local = Random.Range(0, w.location.Length);

            // 
            if (store != 1 && local == 1)
            {
                GenerateBusStrangers();

                // 
                for (int i = 0; i < busSeat.Length; i++)
                {
                    busSeat[i] = Random.Range(0, 2);
                    busPass[i].transform.position = busStartPos[i] + (Vector3.down * busSeat[i] * 100);
                    SetAppearance(busPass[i], busPeople[i].skin, busPeople[i].hair, busPeople[i].shirt, busPeople[i].pants, busPeople[i].hairStyle);
                }

                busPartner = (busSeat[spot] == 0) ? busPeople[spot] : null;

                if (busPartner != null)
                {
                    float dice2 = Random.value;

                    if (dice2 > friProb)
                    {
                        AddAsFriend(busPartner);
                    }
                }

                f = busPartner;
            }
            else if (store != 2 && local == 2)
            {
                GenerateParkStrangers();

                // 
                for (int i = 0; i < parkSeat.Length; i++)
                {
                    parkSeat[i] = Random.Range(0, 2);
                    parkFolk[i].transform.position = parkStartPos[i] + (Vector3.down * parkSeat[i] * 100);
                    SetAppearance(parkFolk[i], parkPeople[i].skin, parkPeople[i].hair, parkPeople[i].shirt, parkPeople[i].pants, parkPeople[i].hairStyle);
                }
            }
            else if (store != 3 && local == 3)
            {
                ChooseFriend();
            }
        }

        if (local != 1)
        {
            spot = Random.Range(0, w.location[local].spot.Length);
        }

        if (local == 0)
        {
            f1 = null;
            f2 = null;

            if (spot == 5)
            {
                ChooseFriend();

                v.transform.position = new Vector3(-14.05f, 0, -3.06f);
                v.transform.eulerAngles = Vector3.up * 270;

                if (f != friends[0])
                {
                    if (dice > .98f)
                    {
        quickEasyLocation += day + "," + f.name + "," + LocalConvert(local)+ "," + f.relationship + ",REM\n";
                        f.CancelFriendship();
                        AddToNotification(f);
                        friends.Remove(f);
                    }
                }
            }
            else
            {
                v.transform.position = Vector3.down * 10000;
            }
        }
        else if (local == 2)
        {
            if (spot == 0)
            {
                f = (parkSeat[0] == 0) ? parkPeople[0] : null;
                f1 = (parkSeat[1] == 1) ? parkPeople[1] : null;
                f2 = (parkSeat[2] == 2) ? parkPeople[2] : null;

                float[] dices = new float[3];

                for (int i = 0; i < dices.Length; i++)
                {
                    dices[i] = Random.value;

                    if (dices[i] > friProb)
                    {
                        AddAsFriend(parkPeople[i]);
                    }
                }
            }
            else if (spot == 2)
            {
                f = (parkSeat[parkSeat.Length-1] == 0) ? parkPeople[parkPeople.Length - 1] : null;

                float dice2 = Random.value;

                if (dice2 > friProb)
                {
                    AddAsFriend(parkPeople[parkPeople.Length - 1]);
                }
            }
            else
            {
                f = null;
                f1 = null;
                f2 = null;
            }
        }
        else if (local == 3)
        {
            f1 = null;
            f2 = null;


            if (spot == 0)
            {
                v.transform.position = new Vector3(195.17f, 1.34f, -205.18f);
                v.transform.eulerAngles = Vector3.up * 180;
            }
            else if (spot == 1)
            {
                v.transform.position = new Vector3(191.01f, 0.85f, -192.45f);
                v.transform.eulerAngles = Vector3.up * 0;
            }
            else
            {
                v.transform.position = Vector3.down * 10000;
            }
        }
        else
        {
            f1 = null;
            f2 = null;
        }

        addInterest();
        clockTick.eulerAngles = new Vector3(0, 180, hour * 360 / 12);
        dayTxt.text = "Day:" + day;
        friendsTxt.text = "Friends:" + numbFriends;

        p.transform.position = w.location[local].spot[spot].pos;
        p.transform.eulerAngles = w.location[local].spot[spot].rot;
        Camera.main.transform.position = w.ReturnCanPos(local);
        Camera.main.orthographicSize = w.ReturnCamSize(local);

        tData.text = "Major Interests (" + secPerHour + ")SPH"
            +"\nSeed: " +w.seed + " - "+ w.momOff
            + "\n-----" 
            + "\nCpu Science: " + cpuSci
            + "\nBiology: " + bio
            + "\nAstrophysics: " + astrophys
            + "\nVis Arts: " + visArts
            + "\nMarketing: " + marketing
            ;

        w.fM.Add(friends);

        // 
        if (fullData)
        {
            WriteData(true, string.Format("{0},{1},{2},{3},{4},{5},\n", day, (float)cpuSci / 1000, (float)bio / 1000, (float)astrophys / 1000, (float)visArts / 1000, (float)marketing / 1000));
        }
        else if (!fullData & newDay)
        {
            WriteData(false, string.Format("{0},{1},{2},{3},{4},{5},\n", day, (float)cpuSci / 1000, (float)bio / 1000, (float)astrophys / 1000, (float)visArts / 1000, (float)marketing / 1000));
        }
        // 
        if (day < year)
        {
            yield return new WaitForSeconds(secPerHour);
            StartCoroutine(IncreaseTime());
        }
        else
        {
            print("Clarissa declared " + mNames[ReturnHighest(majors)] + " as her major! This was probably due to all the time that she spent with ");
            StreamWriter sW = new StreamWriter(Path.Combine(Application.dataPath, Generate.AddTimeStamp("DayNameLocal")+".csv"));
            sW.Write(quickEasyLocation);
            sW.Close();

            // 
            if (reRun)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    void AddAsFriend(Friend who)
    {
        if (!friends.Contains(who))
        {
            friends.Add(who);
            AddToNotification(who);
        }

        quickEasyLocation += day + "," + who.name + "," + LocalConvert(local)+ ",ADD\n";
    }

    string LocalConvert(int l)
    {
        string[] ret = new string[]{
            "Home", "Bus", "Park", "Visiting"
        };

        return ret[l];
    }

    void AddToNotification(Friend who)
    {
        Friend copy = new Friend(who.name, 0, 0, 0, Color.clear, Color.clear, who.shirt, Color.clear, Vector3.zero);

        if(!who.friend)
        {
            copy.CancelFriendship();
        }

        notifNames.Add(copy);

        if (notifNames.Count == 1)
        {
            StartCoroutine(Notification());
        }
    }

    IEnumerator Notification()
    {
        show.transform.localPosition = Vector3.down * 320;
        Color c = notifNames[0].shirt;
        show.color = new Color(c.r,c.g,c.b,.5f);
        show.GetComponentInChildren<Text>().text = notifNames[0].name + " " + ((notifNames[0].friend) ? "has been befriended!" : "is no longer your friend!");

        yield return new WaitForSeconds(3);
        notifNames.RemoveAt(0);
        show.transform.localPosition = Vector3.down * 650;

        // 
        if (notifNames.Count > 0)
        {
            StartCoroutine(Notification());
        }
    }

    int ReturnHighest(int[] h)
    {
        int l = 0;
        float st = 0;

        for (int i = 0; i < h.Length; i++)
        {
            if (st < h[i])
            {
                st = h[i];
                l = i;
            }
        }

        return l;
    }

    void ChooseFriend()
    {
        // First we need an array of 5 maximum ( if she has that many friends ) to store the randomly chosen friends
        int[] fPick = new int[Mathf.CeilToInt(friends.Count*.5f)];

        // Then we much randomly choose that number of friends
        for (int i = 0; i < fPick.Length; i++)
        {
            fPick[i] = Random.Range(0, friends.Count);

            // Just double check that you aren't chosing the same friend twice
            for (int j = 0; j < i; j++)
            {
                if (fPick[i] == fPick[j])
                {
                    fPick[i] = Random.Range(0, friends.Count);
                }
            }
        }

        // Now we need a new array to store this remapping of those friends
        float[] newMap = new float[fPick.Length]; 

        // now lets loop through that array & assign a compatibility score to all the randomly chosen friends
        for (int i = 0; i < newMap.Length; i++)
        {
            newMap[i] = Mathf.Abs(myCompat-friends[fPick[i]].compatibility);
            //print(friends[fPick[i]].name + ": " + newMap[i]);
        }

        // Now we need to find the best compatible by initiating these variables, forClosest will store the closes to 0 difference during the loop
        // & forReturn will remember the friend ID that was most compatible
        float forClosest = Mathf.Infinity;
        int forReturn = 0;

        // Now ofcourse we loop through the newMap & save the closest compatibility with forClosest, & store its location id with forReturn
        for (int i = 0; i < newMap.Length; i++)
        {
            bool closer = forClosest > newMap[i];

            forClosest = (closer) ? newMap[i] : forClosest;
            forReturn = (closer) ? i : forReturn;
        }

        // Lastly we just need to redefine that friend
        int bestFri = fPick[forReturn];

        // & now that we know who it is, we can pass it onto Clarissa! :)
        f = friends[bestFri];
        friends[bestFri].AddToRelationship();
        SetAppearance(v, f.skin, f.hair, f.shirt, f.pants, f.hairStyle);

        //print(friends[bestFri].name + " was chosen");
    }

    void GenerateBusStrangers()
    {
        for (int i = 0; i < busPeople.Length; i++)
        {
            busPeople[i] = new Friend(Generate.Name(0, 0), Random.Range(0, majors.Length), Random.Range(0, majors.Length), Random.Range(-100f, 100f), w.skinType[Random.Range(0, w.skinType.Length)], RandomColor(), RandomColor(), RandomColor(), new Vector3(Random.value * 100, Random.value * 100, Random.value * 100));
        }
    }

    void GenerateParkStrangers()
    {
        for (int i = 0; i < parkPeople.Length; i++)
        {
            parkPeople[i] = new Friend(Generate.Name(0, 0), Random.Range(0, majors.Length), Random.Range(0, majors.Length), Random.Range(-100f, 100f), w.skinType[Random.Range(0, w.skinType.Length)], RandomColor(), RandomColor(), RandomColor(), new Vector3(Random.value * 100, Random.value * 100, Random.value * 100));
        }
    }

    Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1);
    }

    void FixedUpdate()
    {
        Enable.ExitGame(KeyCode.Escape);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            secPerHour -= .5f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            secPerHour += .5f;
        }

        secPerHour = Mathf.Clamp(secPerHour, 0, 15);

        roadAni.transform.position += Vector3.right * .75f;

        roadAni.transform.position = roadAni.transform.position.x > 40f ? new Vector3(-20, roadAni.transform.position.y, roadAni.transform.position.z) : roadAni.transform.position;
    }

    void addInterest()
    {
        string rMaj = w.ReturnInterests(local, spot);

        //
        if (rMaj == "NEG")
        {
            int dice = Random.Range(0, majors.Length);

            majors[dice]--;
        }
        else if (rMaj == "RAND")
        {
            int dice = Random.Range(0, majors.Length);

            majors[dice]++;
        }
        else if (rMaj == "TOP")
        {
            // Set storing int
            int store = 0;

            // Find highest
            for (int i = 0; i < majors.Length; i++)
            {
                store = (majors[i] > store) ? majors[i] : store;
            }

            // Set a counter
            int count = 0;

            // Set an index 
            List<int> index = new List<int>();

            // Find out if there is only one highest or not
            for (int i = 0; i < majors.Length; i++)
            {
                if (majors[i] == store)
                {
                    count++;
                    index.Add(i);
                }
            }

            // if so add to it
            if (count == 1)
            {
                majors[index[0]] += 1;
            }
            // else add to one of them randomly
            else
            {
                majors[index[Random.Range(0, index.Count)]] += 1;
            }
        }
        else if (rMaj == "BOTT")
        {
            // Set storing int
            float store = Mathf.Infinity;

            // Find lowest
            for (int i = 0; i < majors.Length; i++)
            {
                store = (majors[i] < store) ? majors[i] : store;
            }

            // Set a counter
            int count = 0;

            // Set an index 
            List<int> index = new List<int>();

            // Find out if there is only one lowest or not
            for (int i = 0; i < majors.Length; i++)
            {
                if (majors[i] == store)
                {
                    count++;
                    index.Add(i);
                }
            }

            // if so add to it
            if (count == 1)
            {
                majors[index[0]] += 1;
            }
            // else add to one of them randomly
            else
            {
                majors[index[Random.Range(0, index.Count)]] += 1;
            }
        }
        else if (rMaj == "DAY")
        {
            majors[dayTh]++;
        }
        else if (rMaj == "FRD")
        {
            if (f != null)
            {
                majors[f.like] += 2;
                majors[f.dislike] -= 2;

                if (local == 2 && spot == 0)
                {
                    if (f1 != null)
                    {
                        majors[f1.like] += 2;
                        majors[f1.dislike] -= 2;
                    }

                    if (f2 != null)
                    {
                        majors[f2.like] += 2;
                        majors[f2.dislike] -= 2;
                    }
                }
            }
            else
            {
                majors[dayTh]++;
                majors[Random.Range(0, majors.Length)]--;
            }
        }
    }
}
