
## 🔍 What is the Factory Pattern?

It’s a design pattern that provides a way to **create objects** without exposing the creation logic. Instead of using `new` directly, you use a **factory method** to get the object you want.

## 🔍 Why and When Should You Use It?

Use the Factory Pattern when:
✅ Am I creating something in two places Identically? If Yes, we may need a factory.
✅ You have **many similar classes**, and you want to decide which one to use at runtime  
✅ You want to **hide complex object creation logic** from the rest of the code  
✅ You want to follow the **Open/Closed Principle** — your code is open to adding new types, but closed to changing old ones
✅ Imagine a case we have a type of buttons lets say Default button that we use one thousand times in our application with no factories, now imagine the default button is going to have a new property, red background color. The latter change will require that we change the buttons manually one by one until the last button. Factory pattern on the other hand, change the production line once, and every created object will change.

---

## 🧸 Let’s Build a Toy Factory in C#

### Step 1: Create a Common Interface for All Toys

```csharp
public interface IToy
{
    void Play();
}
```

### Step 2: Create Different Toy Classes

```csharp
public class CarToy : IToy
{
    public void Play()
    {
        Console.WriteLine("Vroom! I'm a Car Toy.");
    }
}

public class DollToy : IToy
{
    public void Play()
    {
        Console.WriteLine("Hi! I'm a Doll Toy.");
    }
}

public class RobotToy : IToy
{
    public void Play()
    {
        Console.WriteLine("Beep boop! I'm a Robot Toy.");
    }
}
```

### Step 3: Create the Toy Factory

```csharp
public class ToyFactory
{
    public IToy CreateToy(string toyType)
    {
        switch (toyType.ToLower())
        {
            case "car":
                return new CarToy();
            case "doll":
                return new DollToy();
            case "robot":
                return new RobotToy();
            default:
                throw new ArgumentException("Unknown toy type.");
        }
    }
}
```

### Step 4: Use the Factory in Your Program

```csharp
class Program
{
    static void Main(string[] args)
    {
        ToyFactory factory = new ToyFactory();

        IToy toy1 = factory.CreateToy("car");
        toy1.Play();

        IToy toy2 = factory.CreateToy("robot");
        toy2.Play();

        IToy toy3 = factory.CreateToy("doll");
        toy3.Play();
    }
}
```

---

## 💡 How Do You Know When to Use the Factory Pattern?

Ask yourself:

### 🤔 Are you using `new` in many places with similar classes?

If you're doing this:

```csharp
CarToy car = new CarToy();
DollToy doll = new DollToy();
```

And now imagine you want to change **how** these are created. You'd have to change it _everywhere_. Painful, right?

A Factory puts all that in one place.

### 🤔 Is there logic involved in deciding which object to create?

If the answer is “Yes,” a Factory can help centralize and simplify that.

---

## 🎁 Summary: Key Points to Remember

|Concept|What It Means|
|---|---|
|Interface|Common way to interact with all toys (like IToy)|
|Factory Class|Decides which toy to make and gives it to you|
|Benefit|You don't worry about _how_ toys are made. You just play with them!|
|Use Case|Use when object creation is complex or varies at runtime|

---