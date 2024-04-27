using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
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
    .Select(car => new
        { engineType = car.Engine?.Model == "TDI" ? "diesel" : "petrol", hppl = car.Engine?.HorsePower });


foreach (var car in query)
{
    Console.WriteLine(car);
}

var query2 = from car in query
    group car by car.engineType
    into carGroup
    select new { carGroup.Key, avg = carGroup.Average(car => car.hppl) };

foreach (var carGroup in query2)
{
    Console.WriteLine(carGroup);
}

var path = "cars.xml";
XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
serializer.UnknownAttribute += (sender, e) => { Console.WriteLine("Unknown attribute: " + e.Attr.Name); };
serializer.UnknownElement += (sender, e) => { Console.WriteLine("Unknown element: " + e.Element.Name); };

using (StreamWriter writer = new StreamWriter(path))
{
    serializer.Serialize(writer, myCars);
}

// Deserialization
List<Car> deserializedCars;
using (FileStream fs = new FileStream(path, FileMode.Open))
{
    deserializedCars = (List<Car>)serializer.Deserialize(fs);
}

// Display deserialized data
foreach (var car in deserializedCars)
{
    Console.WriteLine(
        $"Make: {car.Model}, Year: {car.Year}, Engine: {car.Engine.Model}, Displacement: {car.Engine.Displacement}, HorsePower: {car.Engine.HorsePower}");
}

XElement root = XElement.Load("cars.xml");
double avgHp = (double)root.XPathEvaluate("sum(//Car/Engine/HorsePower) div count(//Car/Engine/HorsePower)");
IEnumerable<XElement> models = root.XPathSelectElements("//Car/Model");
IEnumerable<string> distinctModels = models.Select(model => model.Value).Distinct();

Console.WriteLine($"Average HorsePower: {avgHp}");
Console.WriteLine("Distinct models:");
foreach (var model in distinctModels)
{
    Console.WriteLine(model);
}

Console.WriteLine("LINQ to XML:");
var cars = root.Elements("Car")
    .Select(car => new Car(
        car.Element("Model").Value,
        new Engine(double.Parse(car.Element("Engine").Element("Displacement").Value.Replace(".",",")),
                    double.Parse(car.Element("Engine").Element("HorsePower").Value.Replace(".",",")),
                             car.Element("Engine").Attribute("Model").Value),
        int.Parse(car.Element("Year").Value)));

foreach (var car in cars)
{
    Console.WriteLine(
        $"Make: {car.Model}, Year: {car.Year}, Engine: {car.Engine?.Model}, Displacement: {car.Engine?.Displacement}, HorsePower: {car.Engine?.HorsePower}");
}

XDocument doc = new XDocument(
    new XDocumentType("htmol", null, null, null),
    new XElement("html",
        new XElement("head",
            new XElement("title", "Cars Table")
        ),
        new XElement("body",
            new XElement("table",
                new XElement("tr",
                    new XElement("th", "Model"),
                    new XElement("th", "Year"),
                    new XElement("th", "Engine Model"),
                    new XElement("th", "Displacement"),
                    new XElement("th", "HorsePower")
                ),
                from car in myCars
                select new XElement("tr",
                    new XElement("td", car.Model),
                    new XElement("td", car.Year),
                    new XElement("td", car.Engine.Model),
                    new XElement("td", car.Engine.Displacement),
                    new XElement("td", car.Engine.HorsePower)
                )
            )
        )
    )
    );
Console.WriteLine(doc);
doc.Save("cars.html");

XDocument doc2 = XDocument.Load("cars.xml");
foreach (var element in doc2.Descendants("horsePower").ToList())
{
    element.Name = "hp";
}

foreach (var carElement in doc2.Descendants("Car"))
{
    string yearValue = carElement.Element("Year").Value;
    carElement.Element("Year").Remove();
    carElement.Element("Model").SetAttributeValue("Year", yearValue);
}

doc2.Save("cars2.xml");