using System.Xml;
using System.Xml.Serialization;

public class SaveOptionsData
{
    // set witch options can be saved and their default values
    [XmlAttribute("newGame")]
    public bool newGame = true;

    // levels
    // 0 : Shanghai
    // 1 : London
    // 2 : Moscow
    // 3 : Paris

    // characters features
    // x___ : behavior (0 -> 3)
    // 0 : doesn't want to bn seen++
    // 1 : doesn't want to be seen
    // 2 : want to be seen
    // 3 : want to be seen++
    // _x__ : skills and features (0 -> 3)
    // 0 : none
    // 1 : person of interest
    // 2 : old person
    // 3 : young person
    // __x_ : name (0 –> 9)
    // 0 : Mary
    // 1 : John
    // 2 : Linda
    // 3 : Jesse
    // 4 : Karen
    // 5 : Dany
    // 6 : Chris
    // 7 : Erin
    // 8 : Riley
    // 9 : Logan
    // ___x : model (0 -> 3)
    // 0 : char_01
    // 1 : char_02
    // 2 : char_03
    // 3 : char_04

    [XmlAttribute("characters")]
    public string[] characters = new string[] { "0000", "0000", "0000", "0000" };

    // levels stats score
    [XmlAttribute("scores")]
    public int[] scores = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
}