using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlManager
{
    private ConfigData configData = new ConfigData();
    private readonly string configPath = "config";
    private readonly string configFilename = "config-beta.xml";

    private SaveData saveData = new SaveData();
    private readonly string savePath = "save";
    private readonly string saveFilename = "save-beta.xml";

    private NewSaveData newSaveData = new NewSaveData();

    private string filePath;
    private string fileName;

    //private static readonly string PrivateKey = SystemInfo.deviceUniqueIdentifier.Replace("-", string.Empty);
    //private static readonly string PrivateKey = "[in]visible-ed.ac.uk";

    // prefereances public methods
    public int GetSfxVolume()
    {
        return configData.configOptionsData.sfxVolume;
    }

    public void SetSfxVolume(int volume)
    {
        configData.configOptionsData.sfxVolume = volume;
    }
    public int GetMusicVolume()
    {
        return configData.configOptionsData.musicVolume;
    }

    public void SetMusicVolume(int volume)
    {
        configData.configOptionsData.musicVolume = volume;
    }

    public bool DoSaveExists()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        string path = Path.Combine(directory, saveFilename);

        // if file doesn't exist –> create it
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // set type of file needed
    private void SetFileType(string fileType)
    {
        if (fileType == "config")
        {
            filePath = configPath;
            fileName = configFilename;
        }
        else
        {
            filePath = savePath;
            fileName = saveFilename;
        }
    }

    // xml file write method
    public void SaveFile(string fileType)
    {
        SetFileType(fileType);

        string directory = Path.Combine(Application.persistentDataPath, filePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, fileName);

        if (fileType == "config")
        {
            // write through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, configData);
            stream.Close();
        }
        else
        {
            // write through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, saveData);
            stream.Close();
        }
    }

    // if new save –> overwrite exsting with defaut values
    public void NewSaveFile()
    {
        string directory = Path.Combine(Application.persistentDataPath, savePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, saveFilename);

        // write through xml file
        XmlSerializer serializer = new XmlSerializer(typeof(NewSaveData));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, newSaveData);

        stream.Close();
    }

    // xml file read method
    public void LoadFile(string fileType)
    {
        SetFileType(fileType);

        string directory = Path.Combine(Application.persistentDataPath, filePath);

        // if directory does not exist –> create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, fileName);

        // if file doesn't exist –> create it
        if (!File.Exists(path))
        {
            SaveFile(fileType);
        }

        if (fileType == "config")
        {
            // read through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
            StreamReader fileStream = new StreamReader(path);
            configData = serializer.Deserialize(fileStream) as ConfigData;
            fileStream.Close();
        }
        else
        {
            // read through xml file
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            StreamReader fileStream = new StreamReader(path);
            saveData = serializer.Deserialize(fileStream) as SaveData;
            fileStream.Close();
        }
    }

    public void DeleteFile(string fileType)
    {
        SetFileType(fileType);
        string directory = Path.Combine(Application.persistentDataPath, filePath);
        string path = Path.Combine(directory, fileName);

        File.Delete(path);
    }

    public void SaveCharacters(string[] character)
    {
        for (int i = 0; i < character.Length; i++)
        {
            saveData.saveOptionsData.characters[i] = character[i];
        }
    }

    // prefereances public methods
    public string[] GetCharacters()
    {
        return saveData.saveOptionsData.characters;
    }

    public void SetLevelScore(int levelScore, int scoreID)
    {
        saveData.saveOptionsData.scores[scoreID] = levelScore;
    }

    public int[] GetScores()
    {
        return saveData.saveOptionsData.scores;
    }

    public int GetLevelScore(int levelID)
    {
        return saveData.saveOptionsData.scores[levelID];
    }

    public void SetGlobalHighScore(int score)
    {
        configData.configOptionsData.globalHighScore = score;
    }

    public int GetGlobalHighScore()
    {
        return configData.configOptionsData.globalHighScore;
    }
}