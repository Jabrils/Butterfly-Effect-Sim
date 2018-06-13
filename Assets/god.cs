using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BriljaSanLib
{
    // This class is designed to Get things
    public class Get
    {
        // This loads a texture from any path, make sure to add the extentions like .png, & I haven't tested this outside of PNGs
        public static Texture2D LoadTXT(string filePath)
        {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            }
            return tex;
        }

        // This dramatically shortens the Unity Engine GetComponent Function
        public static T QuickComponent<T>(string Find) where T : class
        {
            return GameObject.Find(Find).GetComponent<T>();
        }

        // This shoots a ray from 100 whatever above down to give you a random Vector3 location that will be colliding with the nearest ground collider
        public static Vector3 RandomMapPosition(float xMin, float xMax, float zMin, float zMax)
        {
            Vector3 starting = new Vector3(Random.Range(xMin / 2, xMax / 2), 100, Random.Range(zMin / 2, zMax / 2));
            Ray groundRay = new Ray(starting, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(groundRay, out hit))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
            //Debug.DrawLine(starting, hit.point, Color.red,5f);
        }

        public static Vector3 RandomSqRadPos(Vector3 origin, float sqRad)
        {
            Vector3 starting = new Vector3(Random.Range(origin.x - sqRad, origin.x + sqRad), 100, Random.Range(origin.z - sqRad, origin.z + sqRad));
            Ray groundRay = new Ray(starting, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(groundRay, out hit))
            {
                return hit.point;
            }
            else
            {
                return origin;
            }
        }
    }

    // This class is designed to Generate things
    public class Generate
    {
        /// <summary>
        /// This will return a time stamp to your string input
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string AddTimeStamp(string n)
        {
            string comb = n + "_" + System.DateTime.Now;
            comb = comb.Replace('/', '-');
            comb = comb.Replace(':', '-');
            comb = comb.Replace(' ', '_');
            return comb;
        }

        /// <summary>
        /// This capitalises the first letter in a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// This generates a random name, there are currently 2 type, type 0 is the most random, type 1 is more user input
        /// </summary>
        /// <param name="type"></param>
        /// <param name="charas"></param>
        /// <returns></returns>
        public static string Name(int type, int charas)
        {
            // This creates a randomly generated name length
            if (charas == 0)
            {
                charas = Random.Range(3, 8);
            }

            string theName = "";

            // The first type is randomly generated, the second type is a different approach
            if (type == 0)
            {
                string[] vowels = new string[]
                {
                "a","e","i","o","u"
                };
                string[] constonants = new string[]
                {
                "b","c","d","f","g","h","j","k","l","m","n","p","q","r","s","t","v","x","z","w","y"
                };

                int con = Random.Range(0, 2);
                int vow = Random.Range(0, 2);
                for (int i = 0; i < charas; i++)
                {
                    if (con < vow)
                    {
                        theName += constonants[Random.Range(0, constonants.Length)];
                        con++;
                    }
                    else
                    {
                        theName += vowels[Random.Range(0, vowels.Length)];// + constonants[Random.Range(0, constonants.Length)] + vowels[Random.Range(0, vowels.Length)] + constonants[Random.Range(0, constonants.Length)] + vowels[Random.Range(0, vowels.Length)];
                        vow++;
                        //
                    }
                }
            }
            else if (type == 1)
            {
                string[] prefix = new string[]
                {
                "Ra","Dra","Fu", "Se","Tin", "Fik", "Broth", "Ruf"
                };
                string[] root = new string[]
                    {
                "mini","kyte","lyn","ferk","sert","tryne", ""
                    };
                string[] suffix = new string[]
                    {
                "ly", "mon", "dile", "ang","son", "a", ""
                    };
                theName = prefix[Random.Range(0, prefix.Length)] + root[Random.Range(0, root.Length)] + suffix[Random.Range(0, suffix.Length)];
            }

            return UppercaseFirst(theName);
        }

        public static float RandomFloat(float min, float max)
        {
            float newF = Random.Range(min, max);

            return newF;
        }

        public static int RandomInt(int min, int max)
        {
            int newI = Random.Range(min, max);

            return newI;
        }
    }

    public class Enable
    {
        public static void ExitGame(KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                Application.Quit();
            }
        }
    }

    public class Make
    {
        public static void Rotate(Transform trans, float roSpeed, float axisX, float axisY, float axisZ)
        {
            trans.Rotate(new Vector3(roSpeed * axisX, roSpeed * axisY, roSpeed * axisZ));
        }
    }

    public class Alg
    {
        public static int[] BubbleSort(bool low2High, int[] probs)
        {
            int checker = 1;
            while (checker != 0)
            {
                checker = 0;

                if (low2High)
                {
                    for (int i = 1; i < probs.Length; i++)
                    {
                        if (probs[i - 1] > probs[i])
                        {
                            int temp = probs[i - 1];
                            probs[i - 1] = probs[i];
                            probs[i] = temp;
                            checker++;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < probs.Length; i++)
                    {
                        if (probs[i - 1] < probs[i])
                        {
                            int temp = probs[i - 1];
                            probs[i - 1] = probs[i];
                            probs[i] = temp;
                            checker++;
                        }
                    }
                }
            }

            return probs;
        }
        public static double[] BubbleSort(bool low2High, double[] probs)
        {
            int checker = 1;
            while (checker != 0)
            {
                checker = 0;

                if (low2High)
                {
                    for (int i = 1; i < probs.Length; i++)
                    {
                        if (probs[i - 1] > probs[i])
                        {
                            double temp = probs[i - 1];
                            probs[i - 1] = probs[i];
                            probs[i] = temp;
                            checker++;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < probs.Length; i++)
                    {
                        if (probs[i - 1] < probs[i])
                        {
                            double temp = probs[i - 1];
                            probs[i - 1] = probs[i];
                            probs[i] = temp;
                            checker++;
                        }
                    }
                }
            }

            return probs;
        }
        public static float[] BubbleSort(bool low2High, float[] probs)
        {
            int checker = 1;
            while (checker != 0)
            {
                checker = 0;

                if (low2High)
                {
                    for (int i = 1; i < probs.Length; i++)
                    {
                        if (probs[i - 1] > probs[i])
                        {
                            float temp = probs[i - 1];
                            probs[i - 1] = probs[i];
                            probs[i] = temp;
                            checker++;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < probs.Length; i++)
                    {
                        if (probs[i - 1] < probs[i])
                        {
                            float temp = probs[i - 1];
                            probs[i - 1] = probs[i];
                            probs[i] = temp;
                            checker++;
                        }
                    }
                }
            }

            return probs;
        }
    }
}