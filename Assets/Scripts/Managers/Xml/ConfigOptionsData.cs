using System.Xml;
using System.Xml.Serialization;

public class ConfigOptionsData
{
    // set witch options can be saved and their default values
    [XmlAttribute("musicVolume")]
    public int musicVolume = 4;
    [XmlAttribute("sfxVolume")]
    public int sfxVolume = 3;
    // game global high score
    [XmlAttribute("globalHighScore")]
    public int globalHighScore = 0;
}
