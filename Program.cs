using PT_Lab9;

List<Car> myCars = new List<Car>()
{
    new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
    new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
    new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
    new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
    new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
    new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
    new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
    new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
    new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
};

var query = myCars.Where(car => car.Model == "A6")
    .Select(car => new { engineType = car.Engine?.Model == "TDI" ? "diesel" : "petrol", hppl = car.Engine?.HorsePower });


foreach (var car in query)
{
    Console.WriteLine(car);
}

var query2 = from car in query
    group car by car.engineType into carGroup
    select new { carGroup.Key, avg = carGroup.Average(car => car.hppl) };

foreach (var carGroup in query2)
{
    Console.WriteLine(carGroup);
}

var path = "cars.xml";
System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Car>));
using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
{
    serializer.Serialize(writer, myCars);
}

