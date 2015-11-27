using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("LevelCollection")]
public class LevelContainer {

    [XmlArray("Levels")]
    [XmlArrayItem("Level")]
    public List<Level> Levels = new List<Level>();

    public LevelContainer()
    {
    }

    public LevelContainer(List<Level> levels)
    {
        this.Levels = levels;
    }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(LevelContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static LevelContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(LevelContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as LevelContainer;
        }
    }

}
