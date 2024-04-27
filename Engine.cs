using System.Xml.Serialization;

namespace PT_Lab9;

public class Engine
{
    public double Displacement { get; set; }

    public double HorsePower { get; set; }
    
    [XmlAttribute("model")]
    public string Model { get; set; }

    public Engine()
    {
    }

    public Engine(double displacement = default, double horsePower = default, string model = null)
    {
        this.Displacement = displacement;
        this.HorsePower = horsePower;
        this.Model = model;
    }
}