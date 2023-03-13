using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Object = System.Object;
using Random = UnityEngine.Random;

public static class Utilities
{
    public static Action OnLoad;
    public static Action OnLoadDone;
#if UNITY_EDITOR
    public static ESaveTypes DataSaveType = ESaveTypes.Readable;

#else
    public static ESaveTypes DataSaveType = ESaveTypes.Unreadable;
#endif
    public static string SavePath { get; set; } = Path.Combine(Application.persistentDataPath, "Save");

    public static string SaveExtensionName = ".cup";
    public static double _checkCastToDouble;
    static bool IsEnvironmentVariableSet = false;
    static int PlayServicesAvailability = -1;
    public enum ESaveTypes
    {
        Readable,
        Unreadable,
    }
    public enum EAspectRatio
    {
        Aspect_3_4,
        Aspect_9_16,
        Aspect_10_16,
        Aspect_2_3,
        Aspect_9_195,
        AspectUnkown
    }
    public static EAspectRatio GetAspectRatio()
    {
        float r = ((float)Screen.width) / ((float)Screen.height);
        string _r = r.ToString("F2");
        string ratio = _r.Substring(0, 4);
        switch (ratio)
        {
            case "1.33"://4:3
            case "0.75"://4:3
                {
                    Debug.Log("4:3");
                    return EAspectRatio.Aspect_3_4;
                }
            case "1.50"://3:2
            case "0.67"://3:2
                {
                    Debug.Log("3:2");
                    return EAspectRatio.Aspect_2_3;
                }
            case "0.56"://9:16
            case "1.77"://9:16
                {
                    Debug.Log("9:16");
                    return EAspectRatio.Aspect_9_16;
                }
            case "0.62"://10:16
            case "1.60"://10:16
                {
                    Debug.Log("10:16");
                    return EAspectRatio.Aspect_10_16;
                }
            case "2.16"://19.5:9
            case "0.46"://19.5:9
                {
                    Debug.Log("9:19.5");
                    return EAspectRatio.Aspect_9_195;
                }
            default: return EAspectRatio.AspectUnkown;
        }
    }
    public static float GetRandomNumberAround(this float inValue, float up, float down)
    {
        float highRandom = Random.Range(inValue, inValue + up);
        float lowRandom = Random.Range(inValue - down, inValue);

        return Random.Range(lowRandom, highRandom);
    }
    public static string CreateDirectoryMd5(string srcPath)
    {
        var filePaths = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories).OrderBy(p => p).ToArray();

        using (var md5 = MD5.Create())
        {
            foreach (var filePath in filePaths)
            {
                // hash path
                byte[] pathBytes = Encoding.UTF8.GetBytes(filePath);
                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                // hash contents
                byte[] contentBytes = File.ReadAllBytes(filePath);

                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            }

            //Handles empty filePaths case
            md5.TransformFinalBlock(new byte[0], 0, 0);

            return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
        }
    }
    public static float Fraction(this float num, float percentage)
    {
        return num * (percentage / 100f);
    }
    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    // Convert a byte array to an Object
    public static object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        object obj = (object)binForm.Deserialize(memStream);

        return obj;
    }
    public static void SetEnvironmentVariableForSerialization()
    {
        if (IsEnvironmentVariableSet)
            return;

#if UNITY_IOS
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        IsEnvironmentVariableSet = true;
#endif

    }
    public static string SaveJson(this string data, string key, string location = null, string extension = null, bool useChecksum = true)
    {
        string saveFilePath = Path.Combine((location ?? SavePath), key + (extension ?? SaveExtensionName));
        string saveContent = String.Empty;
        SetEnvironmentVariableForSerialization();
        if (!Directory.Exists(location ?? SavePath))
        {
            Directory.CreateDirectory(location ?? SavePath);
        }
        switch (DataSaveType)
        {
            case ESaveTypes.Readable:
                {
                    File.WriteAllText(saveFilePath, data);
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(saveFilePath))
                        {
                            if (useChecksum)
                                PlayerPrefs.SetString(key + "MD5", md5.ComputeHash(stream).GenerateUniqueId());
                        }
                    }

                    saveContent = data;
                    break;

                }
            case ESaveTypes.Unreadable:
                {
                    using (FileStream file = File.Open(saveFilePath, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        var base64String = Convert.ToBase64String(ObjectToByteArray(data));

                        binaryFormatter.Serialize(file, base64String);

                        using (var md5 = MD5.Create())
                        {
                            if (useChecksum)
                                PlayerPrefs.SetString(key + "MD5", md5.ComputeHash(file).GenerateUniqueId());
                        }
                        saveContent = base64String;
                    }

                    break;
                }
        }
        return saveContent;

    }
    public static void Save(this object data, string key, string location = null, string extension = null, bool useChecksum = true)
    {
        string saveFilePath = Path.Combine((location ?? SavePath), key + (extension ?? SaveExtensionName));
        string saveContent = String.Empty;
        SetEnvironmentVariableForSerialization();
        if (!Directory.Exists(location ?? SavePath))
        {
            Directory.CreateDirectory(location ?? SavePath);
        }
        using (FileStream file = File.Open(saveFilePath, FileMode.OpenOrCreate))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            var base64String = Convert.ToBase64String(ObjectToByteArray(data));

            binaryFormatter.Serialize(file, base64String);

            using (var md5 = MD5.Create())
            {
                if (useChecksum)
                    PlayerPrefs.SetString(key + "MD5", md5.ComputeHash(file).GenerateUniqueId());
            }
        }

    }
    public static object Load(string key, string location = null, string extension = null, bool useChecksum = true)
    {
        string saveFilePath = Path.Combine((location ?? SavePath), key + (extension ?? SaveExtensionName));

        if (Directory.Exists(location ?? SavePath) && File.Exists(saveFilePath))
        {
            using (FileStream file = File.Open(saveFilePath, FileMode.Open))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    var saveData = binaryFormatter.Deserialize(file);
                    var base64Data = (string)saveData;
                    var actualData = Convert.FromBase64String(base64Data);
                    var objectData = ByteArrayToObject(actualData);

                    string hash = String.Empty;
                    using (var md5 = MD5.Create())
                    {
                        hash = md5.ComputeHash(file).GenerateUniqueId();
                    }
                    if (useChecksum)
                    {
                        if (PlayerPrefs.HasKey(key + "MD5") && String.Equals(PlayerPrefs.GetString(key + "MD5"), hash))

                            return objectData;
                    }
                    else
                        return objectData;

                    return null;

                }
                catch (Exception e)
                {
                    return null;
                }
            }

        }
        return null;

    }
    //public static T LoadJsonData<T>(string key, string location = null, string extension = null, bool useChecksum = true) where T : class
    //{
    //    SetEnvironmentVariableForSerialization();
    //    var data = LoadJsonData(key, location, extension, useChecksum);
    //    if (data != null)
    //        return JsonConvert.DeserializeObject<T>(data);

    //    return default(T);
    //}
    public static string LoadJsonData(string key, string location = null, string extension = null, bool useChecksum = true)
    {
        string saveFilePath = Path.Combine((location ?? SavePath), key + (extension ?? SaveExtensionName));

        if (Directory.Exists(location ?? SavePath) && File.Exists(saveFilePath))
        {
            switch (DataSaveType)
            {
                case ESaveTypes.Readable:
                    {
                        try
                        {
                            var json = File.ReadAllText(saveFilePath);
                            string hash = String.Empty;
                            using (var md5 = MD5.Create())
                            {
                                using (var stream = File.OpenRead(saveFilePath))
                                {
                                    if (useChecksum)
                                        hash = md5.ComputeHash(stream).GenerateUniqueId();
                                }
                            }
                            if (useChecksum)
                            {
                                if (PlayerPrefs.HasKey(key + "MD5") &&
                                                               String.Equals(PlayerPrefs.GetString(key + "MD5"), hash))
                                    return json.Trim(new char[] { '\uFEFF' });
                            }
                            else
                            {
                                return json.Trim(new char[] { '\uFEFF' });
                            }


                            return null;

                        }
                        catch
                        {
                            return null;
                        }
                    }
                case ESaveTypes.Unreadable:
                    {

                        using (FileStream file = File.Open(saveFilePath, FileMode.Open))
                        {
                            try
                            {
                                BinaryFormatter binaryFormatter = new BinaryFormatter();
                                var saveData = binaryFormatter.Deserialize(file);
                                var base64Data = (string)saveData;
                                var actualData = Convert.FromBase64String(base64Data);
                                var objectData = ByteArrayToObject(actualData);

                                string hash = String.Empty;
                                using (var md5 = MD5.Create())
                                {
                                    if (useChecksum)
                                        hash = md5.ComputeHash(file).GenerateUniqueId();
                                }

                                if (useChecksum)
                                {
                                    if (PlayerPrefs.HasKey(key + "MD5") &&
                                                                   String.Equals(PlayerPrefs.GetString(key + "MD5"), hash))
                                        return objectData.ToString().Trim(new char[] { '\uFEFF' });
                                }
                                else
                                {
                                    return objectData.ToString().Trim(new char[] { '\uFEFF' });
                                }

                                return null;

                            }
                            catch
                            {
                                return null;
                            }
                        }
                    }

            }

        }
        return null;

    }

    public static List<int> GetIntFromString(this string inString)
    {
        string[] numbers = Regex.Split(inString, @"\D+");
        List<int> recoveredInts = new List<int>();

        foreach (string value in numbers)
        {
            if (!String.IsNullOrEmpty(value))
            {
                int i = Int32.Parse(value);
                recoveredInts.Add(i);
            }
        }

        return recoveredInts;
    }

    public static int LimitToRange(
        this int value, int inclusiveMinimum, int inclusiveMaximum)
    {
        if (value < inclusiveMinimum)
        {
            return inclusiveMinimum;
        }
        if (value > inclusiveMaximum)
        {
            return inclusiveMaximum;
        }
        return value;
    }

    public static uint TryParseConvinience(string str, uint failResult)
    {
        uint parseResult;
        if (UInt32.TryParse(str, out parseResult))
            return parseResult;
        else
            return failResult;
    }

    public static T Clone<T>(this T objSource)
    {
        MemoryStream memoryStream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, objSource);

        memoryStream.Position = 0;
        T returnValue = (T)binaryFormatter.Deserialize(memoryStream);

        memoryStream.Close();
        memoryStream.Dispose();

        return returnValue;
    }

    public static string GetMD5CheckSum(this object input)
    {
        // step 1, calculate MD5 hash from input

        byte[] hash;
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = ObjectToByteArray(input);

            hash = md5.ComputeHash(inputBytes);
        }

        // step 2, convert byte array to hex string

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }

        return sb.ToString();
    }
    public static string GenerateUniqueId()
    {
        return GenerateUniqueId(Guid.NewGuid());
    }
    public static string GenerateUniqueId(this object data)
    {
        byte[] uidBytes;
        using (var sha256 = SHA256.Create())
        {
            byte[] inputBytes = ObjectToByteArray(data);

            uidBytes = sha256.ComputeHash(inputBytes);
        }

        var a = default(UInt64);

        int l = 6;
        for (int i = 0; i < l; i++)
        {
            var shift = Math.Abs(Convert.ToInt32((l - i - 1) * 8));
            a |= Convert.ToUInt64(uidBytes[i]) << shift;
        }

        return a.ToString();
    }

    public static void Empty(this DirectoryInfo directory)
    {
        foreach (FileInfo file in directory.GetFiles())
            file.Delete();
        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            subDirectory.Delete(true);
    }

    public static T ToEnum<T>(this string value, T defaultValue)
    {
        if (String.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        try
        {
            var result = (T)Enum.Parse(typeof(T), value);
            return result;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static float RoundTo(this float value, int digits)
    {
        return Convert.ToSingle(Math.Round(value, digits));
    }

    public static bool DifferenceCompare(this float value, float other, float tolerance)
    {
        var diff = Mathf.Abs(value - other);

        return (diff < tolerance);
    }



}