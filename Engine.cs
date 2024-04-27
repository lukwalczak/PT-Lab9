namespace PT_Lab9;

public class Engine
{
    private double displacement { get; set; }
    private double horsePower { get; set; }
    private string model { get; set; }

    public Engine(double displacement = default, double horsePower = default, string model = null)
    {
        this.displacement = displacement;
        this.horsePower = horsePower;
        this.model = model;
    }
}