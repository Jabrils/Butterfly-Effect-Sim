using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace world
{
    [System.Serializable]
    public class World
    {
        public int seed;
        public int momOff;
        public FriendManager fM = new FriendManager();

        public Location[] location = new Location[4];
        public Color32[] skinType = new Color32[]
        {
            new Color32(122,71,16,255),
            new Color32(243,209,171,255),
            new Color32(251,237,221,255),
            new Color32(79,47,11,255),
            new Color32(255,238,168,255),
            new Color32(255,250,149,255),
            new Color32(251,217,202,255),
            new Color32(100,36,9,255),
            new Color32(234,220,153,255),
        };
        public World()
        {
            location[0] = new Location(6);
            location[1] = new Location(8);
            location[2] = new Location(3);
            location[3] = new Location(2);

            // Home

            // TV
            SetLocSpot(0, 0, "DAY", new Vector3(-8.16f, .73f, -13.46f), new Vector3(0, 90, 0));
            // Sleep
            SetLocSpot(0, 1, "RAND", new Vector3(9.78419f, 2.88f, -25.26f), new Vector3(0, 270, 90));
            // Eat
            SetLocSpot(0, 2, "BOTT", new Vector3(-2.31f, 1.29f, -0.29f), new Vector3(0, 180, 0));
            // Bath
            SetLocSpot(0, 3, "NEG", new Vector3(18.08f, -0.03f, 9.73f), new Vector3(0, 90, 0));
            // Read
            SetLocSpot(0, 4, "TOP", new Vector3(2.28f, .7f, -15.54f), new Vector3(0, 180, 0));
            // Visitor
            SetLocSpot(0, 5, "FRD", new Vector3(-14.05f, 0, -1.23f), new Vector3(0, 90, 0));

            // Bus Seats
            SetLocSpot(1, 0, "FRD", new Vector3(-3.51f, 3.65f, -201.17f), new Vector3(0, 180, 0));
            SetLocSpot(1, 1, "FRD", new Vector3(.02f, 3.65f, -201.17f), new Vector3(0, 180, 0));
            SetLocSpot(1, 2, "FRD", new Vector3(3.82f, 3.65f, -201.17f), new Vector3(0, 180, 0));
            SetLocSpot(1, 3, "FRD", new Vector3(7.68f, 3.65f, -201.17f), new Vector3(0, 180, 0));
            SetLocSpot(1, 4, "FRD", new Vector3(7.68f, 3.65f, -198.48f), new Vector3(0, 180, 0));
            SetLocSpot(1, 5, "FRD", new Vector3(4.07f, 3.65f, -198.48f), new Vector3(0, 180, 0));
            SetLocSpot(1, 6, "FRD", new Vector3(.04f, 3.65f, -198.48f), new Vector3(0, 180, 0));
            SetLocSpot(1, 7, "FRD", new Vector3(-3.51f, 3.65f, -198.48f), new Vector3(0, 180, 0));

            // Park

            // 4way Bench
            SetLocSpot(2, 0, "FRD", new Vector3(192.21f, 1.5f, 2.55f), new Vector3(0, 270, 0));
            // Pier
            SetLocSpot(2, 1, "BOTT", new Vector3(194.31f, .83f, -2.32f), new Vector3(0, 90, 0));
            // 2way Bench
            SetLocSpot(2, 2, "FRD", new Vector3(190.42f, 1.42f, -14f), new Vector3(0, 180, 0));

            // Friends

            // tv
            SetLocSpot(3, 0, "FRD", new Vector3(190.69f, 1.44f, -205.14f), new Vector3(0, 0, 0));
            // game table
            SetLocSpot(3, 1, "FRD", new Vector3(191.31f, .96f, -195.67f), new Vector3(0, 0, 0));

        }

        void SetLocSpot(int l, int sp, string m, Vector3 p, Vector3 r)
        {
            location[l].spot[sp].majors = m;
            location[l].spot[sp].pos = p;
            location[l].spot[sp].rot = r;
        }

        public string ReturnInterests(int l, int s)
        {
            return location[l].spot[s].majors;
        }

        public Vector3 ReturnCanPos(int l)
        {
            Vector3 ret = Vector3.zero;

            // 
            if (l == 0)
            {
                ret = new Vector3(-66.77f, 57, -75.23f);
            }
            // 
            else if (l == 1)
            {
                ret = new Vector3(-65f, 57, -265f);
            }
            else if (l == 2)
            {
                ret = new Vector3(132.9f, 57, -73.1f);
            }
            else if (l == 3)
            {
                ret = new Vector3(134.69f, 57, -265f);
            }
            return ret;
        }

        public float ReturnCamSize(int l)
        {
            float ret = 0;
            if (l == 0)
            {
                ret = 17;
            }
            else if (l==1)
            {
                ret = 10;
                    }
            else if (l == 2)
            {
                ret = 17;
            }
            else if (l == 3)
            {
                ret = 14;
            }

            return ret;
        }

        public class Location
        {
            public Spot[] spot;

            public Location(int many)
            {
                spot = new Spot[many];

                for (int i = 0; i < spot.Length; i++)
                {
                    spot[i] = new Spot();
                }
            }

            public class Spot
            {
                public string majors;
                public Vector3 pos;
                public Vector3 rot;
            }
        }
    }

    [System.Serializable]
    public class Friend
    {
        string _name;
        public string name { get { return _name; } }
        float _compatibility;
        public float compatibility { get { return _compatibility; } }
        int _like;
        public int like { get { return _like; } }
        int _dislike;
        public int dislike { get { return _dislike; } }
        Vector3 _hairStyle;
        public Vector3 hairStyle { get { return _hairStyle; } }
        Color32 _hair;
        public Color32 hair { get { return _hair; } }
        Color32 _shirt;
        public Color32 shirt { get { return _shirt; } }
        Color32 _pants;
        public Color32 pants { get { return _pants; } }
        Color32 _skin;
        public Color32 skin { get { return _skin; } }
        int _relationship;
        public int relationship { get { return _relationship; } }
        bool _friend = true;
        public bool friend { get { return _friend; } }

        public Friend(string n, int l, int d, float comp, Color32 Skin, Color32 Hair, Color32 Shirt, Color32 Pants, Vector3 HairStyle)
        {
            _name = n;
            _like = l;
            _dislike = d;
            _shirt = Shirt;
            _pants = Pants;
            _skin = Skin;
            _hair = Hair;
            _hairStyle = HairStyle;
            _compatibility = comp;
        }

        public void AddToRelationship()
        {
            _relationship++;
        }

        public void CancelFriendship()
        {
            _relationship = 0;
            _friend = false;
        }
    }

    public class FriendManager
    {
        // 
        public List<string> who = new List<string>();

        // 
        public void Add(List<Friend> f)
        {
            for (int i = 0; i < f.Count; i++)
            {
                if (f[i].relationship > 0 && !who.Contains(f[i].name + Additive(f[i])))
                {
                    who.Add(f[i].name + Additive(f[i]));
                }
            }
        }

        public static string Additive(Friend fr)
        {
            return " (" +Convert(fr.like) + ":" + Convert(fr.dislike)+")";
        }

        static string Convert(int i)
        {
            string[] ret = new string[]{
                "C","B","A","V","M",
            };

            return ret[i];
        }

        // 
        public string retAll()
        {
            string ret = "";

            for (int i = 0; i < who.Count; i++)
            {
                ret += who[i] + ",";
            }

            return ret;
        }
    }
}