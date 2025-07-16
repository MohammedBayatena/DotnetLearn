
## 🧸 Plug Doesn’t Fit the Socket

You have a toy from **America** 🇺🇸, and it has a **flat plug** 🔌.  
But your wall socket in **Europe** 🇪🇺 has **round holes** 🕳️.

Oh no! The plug doesn’t fit. 😢

Your mom gives you a **plug adapter** — it **converts** the flat plug into a round plug.

Now it fits. 🎉 Your toy works perfectly!

**That’s the Adapter Pattern!**  
It lets things **work together** even if they were **designed differently**.

---

## 💡 What is the Adapter (Wrapper) Pattern?

The **Adapter Pattern** is a **structural design pattern** that:

- Converts one interface into another that a client expects
    
- Lets **incompatible classes work together**
    
- Wraps one class with a **different interface**
    

You can think of it as a **translator** 🗣️ between two people speaking different languages.

---

## ✅ When Should You Use It?

Use the Adapter Pattern when:

- You want to **reuse existing code** but its interface doesn't match what you need
    
- You want to **connect incompatible systems**
    
- You want to **wrap a legacy class** and expose a new, cleaner API
    

---

## 🧑‍💻 C# Example: Round Peg and Square Peg

### Scenario: You have a system that works with **round pegs**, but someone gives you a **square peg**.

Let’s make them work together using an **adapter**.

---

### Step 1: The Expected Interface (Target)

```csharp
public class RoundHole
{
    public double Radius { get; private set; }

    public RoundHole(double radius)
    {
        Radius = radius;
    }

    public bool Fits(RoundPeg peg)
    {
        return peg.GetRadius() <= Radius;
    }
}

public class RoundPeg
{
    protected double radius;

    public RoundPeg(double radius)
    {
        this.radius = radius;
    }

    public virtual double GetRadius()
    {
        return radius;
    }
}
```

---

### Step 2: Incompatible Class

```csharp
public class SquarePeg
{
    public double Width { get; private set; }

    public SquarePeg(double width)
    {
        Width = width;
    }

    public double GetWidth()
    {
        return Width;
    }
}
```

---

### Step 3: Create the Adapter (SquarePeg → RoundPeg)

```csharp
public class SquarePegAdapter : RoundPeg
{
    private SquarePeg squarePeg;

    public SquarePegAdapter(SquarePeg peg) : base(0)
    {
        this.squarePeg = peg;
    }

    public override double GetRadius()
    {
        // Calculate the radius of the smallest circle that can fit the square
        return squarePeg.GetWidth() * Math.Sqrt(2) / 2;
    }
}
```

---

### Step 4: Use It in Your Program

```csharp
class Program
{
    static void Main(string[] args)
    {
        var hole = new RoundHole(5);
        var roundPeg = new RoundPeg(5);

        Console.WriteLine(hole.Fits(roundPeg));  // True

        var smallSquarePeg = new SquarePeg(5);
        var largeSquarePeg = new SquarePeg(10);

        var smallAdapter = new SquarePegAdapter(smallSquarePeg);
        var largeAdapter = new SquarePegAdapter(largeSquarePeg);

        Console.WriteLine(hole.Fits(smallAdapter));  // True
        Console.WriteLine(hole.Fits(largeAdapter));  // False
    }
}
```

🖨️ **Output:**

```
True
True
False
```

---

## 🎯 Benefits of Adapter Pattern

|Benefit|Explanation|
|---|---|
|Reuse existing code|Even if its interface is different|
|No need to rewrite classes|Just wrap them|
|Decouples systems|Makes code more flexible and modular|
|Clean migration|Adapters help use legacy code with new systems|

---

## 🎁 Summary

|Concept|What It Means|
|---|---|
|Adapter|A **plug converter** or **translator**|
|Use Case|When **things don't fit**, make them compatible|
|Real World|Phone charger adapter, HDMI to VGA, SD card reader|

---

## 🧱 Real-World Examples

- 🖼️ Converting images between file formats (e.g. PNG to JPG)
    
- 🧾 Wrapping legacy payment systems to match modern API
    
- 🎮 Adapting game controller inputs to standard actions
    
- 🧪 .NET: `IDbCommand` adapters for different database providers