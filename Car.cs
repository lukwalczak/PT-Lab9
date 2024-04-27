namespace PT_Lab9;

public class Car
{
    private string model;

    private int year;

    private Engine engine;

    public Car(string model, int year)
    {
        this.model = model;
        this.year = year;
    }

    public Car(string model, int year, Engine engine) : this(model, year)
    {
        this.engine = engine;
    }
}