## 🔍 What is the Builder Pattern?

The **Builder Pattern** is a design pattern that lets you construct **complex objects** step-by-step.  
You can use the **same building process**, but create **different variations** of the object.

It separates the **construction logic** from the object itself.

## 🔍 When Should You Use It?

✅ When your object has **many optional parts**  
✅ When the object creation is **complex or needs to follow steps**  
✅ When you want different **representations** (versions) of the same object

---

You're a kid with a box of Lego blocks. You want to build a **toy house**.

Now, sometimes you want:

- A **small house** with 1 door and no garden
    
- A **fancy house** with 2 doors, 3 windows, a garage, and a garden

## 🧱 C# Example: Building a Toy House

### Step 1: Create the Product Class (The Complex Object)

```csharp
public class ToyHouse
{
    public bool HasGarden { get; set; }
    public int Doors { get; set; }
    public int Windows { get; set; }
    public bool HasGarage { get; set; }

    public void Show()
    {
        Console.WriteLine($"Toy House with {Doors} door(s), {Windows} window(s), " +
                          (HasGarden ? "a garden, " : "no garden, ") +
                          (HasGarage ? "and a garage." : "no garage."));
    }
}
```

---

### Step 2: Create the Builder Interface

```csharp
public interface IToyHouseBuilder
{
    void BuildDoors();
    void BuildWindows();
    void BuildGarage();
    void BuildGarden();
    ToyHouse GetResult();
}
```

---

### Step 3: Create Concrete Builders

#### ✅ Small House Builder

```csharp
public class SmallHouseBuilder : IToyHouseBuilder
{
    private ToyHouse house = new ToyHouse();

    public void BuildDoors() { house.Doors = 1; }
    public void BuildWindows() { house.Windows = 1; }
    public void BuildGarage() { house.HasGarage = false; }
    public void BuildGarden() { house.HasGarden = false; }

    public ToyHouse GetResult() => house;
}
```

#### ✅ Fancy House Builder

```csharp
public class FancyHouseBuilder : IToyHouseBuilder
{
    private ToyHouse house = new ToyHouse();

    public void BuildDoors() { house.Doors = 2; }
    public void BuildWindows() { house.Windows = 3; }
    public void BuildGarage() { house.HasGarage = true; }
    public void BuildGarden() { house.HasGarden = true; }

    public ToyHouse GetResult() => house;
}
```

---

### Step 4: Create the Director (Optional but Common)

The **Director** tells the builder **what steps to do and in what order**.

```csharp
public class HouseDirector
{
    public void Construct(IToyHouseBuilder builder)
    {
        builder.BuildDoors();
        builder.BuildWindows();
        builder.BuildGarage();
        builder.BuildGarden();
    }
}
```

---

### Step 5: Use the Builder in Your Program

```csharp
class Program
{
    static void Main(string[] args)
    {
        var director = new HouseDirector();

        // Build a small house
        var smallBuilder = new SmallHouseBuilder();
        director.Construct(smallBuilder);
        ToyHouse smallHouse = smallBuilder.GetResult();
        smallHouse.Show();

        // Build a fancy house
        var fancyBuilder = new FancyHouseBuilder();
        director.Construct(fancyBuilder);
        ToyHouse fancyHouse = fancyBuilder.GetResult();
        fancyHouse.Show();
    }
}
```

🖨️ Output:

```
Toy House with 1 door(s), 1 window(s), no garden, no garage.
Toy House with 2 door(s), 3 window(s), a garden, and a garage.
```

---

## 🎁 Summary:

- Factory: "Give me **a toy**." 🎲
    
- Abstract Factory: "Give me a **matching toy set**." 🎁
    
- Builder: "Let’s **build a toy castle step by step**, your way." 🧱🏰
    

---

Want a real-world use case? I can show how the Builder Pattern is used to build things like **HTML reports**, **SQL queries**, or **Game Characters**! Just say the word.