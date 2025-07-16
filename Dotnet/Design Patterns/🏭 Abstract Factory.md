
---

## 🔍 What is the **Abstract Factory Pattern**:
  
It’s a design pattern that provides a way to create **families of related objects** without knowing their exact classes.

---

## 🔍 When Should You Use Abstract Factory?

✅ When you need to create **related objects together** (like car + doll from the same brand)  
✅ When your system should be **independent of how those families are created**  
✅ When you want to easily **switch families** (brands) without touching much code

---

## 🧑‍💻 C# Example of Abstract Factory

There are _two different brands_ of toys:

- **Lego Brand**
    
- **SEGA Brand**
    

Each brand can make:

- A **car toy**
    
- A **doll toy**
    

But the toys from each brand look and act differently!

### Step 1: Define Common Interfaces for All Products

```csharp
public interface ICarToy
{
    void Drive();
}

public interface IDollToy
{
    void Speak();
}
```

---

### Step 2: Implement the Products for Each Brand

#### ✅ Lego Toys

```csharp
public class LegoCar : ICarToy
{
    public void Drive()
    {
        Console.WriteLine("Lego car goes click-clack!");
    }
}

public class LegoDoll : IDollToy
{
    public void Speak()
    {
        Console.WriteLine("Lego doll says blocky things!");
    }
}
```

#### ✅ Fisher-Price Toys

```csharp
public class SEGACar : ICarToy
{
    public void Drive()
    {
        Console.WriteLine("SEGA car rolls smoothly!");
    }
}

public class SEGADoll : IDollToy
{
    public void Speak()
    {
        Console.WriteLine("SEGA doll sings a lullaby!");
    }
}
```

---

### Step 3: Create the Abstract Factory Interface

```csharp
public interface IToyFactory
{
    ICarToy CreateCarToy();
    IDollToy CreateDollToy();
}
```

---

### Step 4: Implement Each Brand Factory

#### 🏭 Lego Factory

```csharp
public class LegoFactory : IToyFactory
{
    public ICarToy CreateCarToy()
    {
        return new LegoCar();
    }

    public IDollToy CreateDollToy()
    {
        return new LegoDoll();
    }
}
```

#### 🏭 Fisher-Price Factory

```csharp
public class SEGAFactory : IToyFactory
{
    public ICarToy CreateCarToy()
    {
        return new FisherCar();
    }

    public IDollToy CreateDollToy()
    {
        return new FisherDoll();
    }
}
```

---

### Step 5: Use the Abstract Factory in Your Program

```csharp
class Program
{
    static void Main(string[] args)
    {
        IToyFactory factory;

        // Change this to switch brands!
        factory = new LegoFactory();
        // factory = new FisherPriceFactory();

        ICarToy car = factory.CreateCarToy();
        IDollToy doll = factory.CreateDollToy();

        car.Drive();
        doll.Speak();
    }
}
```

---

## 💡 Summary: Factory vs Abstract Factory

|Concept|Factory Pattern|Abstract Factory Pattern|
|---|---|---|
|Creates|One object at a time|A **family** of related objects|
|Focus|Hiding creation logic|Keeping object **families consistent**|
|Example|A single car toy|A matching car + doll toy set from 1 brand|
|When to use|When type is chosen at runtime|When you need grouped, related objects|

---

## 🎁 Final Thought

The Abstract Factory is like saying:

> “Give me a **Lego set**, and I’ll get a Lego car **and** a Lego doll. I don’t care how they’re made, just give me the matching set.”

It gives you clean, consistent groups of objects — and makes it easy to swap entire groups by just switching the factory.