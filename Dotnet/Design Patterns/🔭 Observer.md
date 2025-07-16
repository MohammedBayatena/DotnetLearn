
## 🔍 The Toy That Notifies Friends

Imagine you have a **talking toy** that can yell:

> "It's snack time!"

Now, all your **friends** are sitting nearby. When the toy yells:

- Timmy goes to get cookies 🍪
    
- Jenny grabs a juice box 🧃
    
- Max runs to the kitchen 🚀
    

They all heard the toy and **reacted differently**, but all from the same event.

🎉 This is the **Observer Pattern**.

---

## 🔍 What is the Observer Pattern?

The **Observer Pattern** is a **behavioral design pattern** where:

- One object (called the **subject**) keeps a list of **observers**
    
- When the subject **changes state**, it **notifies all observers**
    
- Observers **react independently**
    

👉 It’s like a **publish/subscribe** system.

---

## 🔍 When Should You Use It?

Use the Observer Pattern when:
✅One object **changes**, and you want **other objects to know and react**
✅ You want to avoid **tight coupling** between objects
✅You want a **dynamic system** where observers can come and go

---

## 🧑‍💻 C# Example: Weather Station Notifies Displays

Let’s say we’re building a **weather station** that notifies **multiple displays** when the temperature changes.

---

### Step 1: Define the Observer Interface

```csharp
public interface IWeatherObserver
{
    void Update(float temperature);
}
```

---

### Step 2: Define the Subject Interface

```csharp
public interface IWeatherStation
{
    void RegisterObserver(IWeatherObserver observer);
    void RemoveObserver(IWeatherObserver observer);
    void NotifyObservers();
}
```

---

### Step 3: Implement the Concrete Subject (Weather Station)

```csharp
public class WeatherStation : IWeatherStation
{
    private List<IWeatherObserver> observers = new List<IWeatherObserver>();
    private float temperature;

    public void RegisterObserver(IWeatherObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IWeatherObserver observer)
    {
        observers.Remove(observer);
    }

    public void SetTemperature(float temp)
    {
        Console.WriteLine($"WeatherStation: Temperature updated to {temp}°C");
        temperature = temp;
        NotifyObservers();
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature);
        }
    }
}
```

---

### Step 4: Create Some Observers (Displays)

```csharp
public class PhoneDisplay : IWeatherObserver
{
    public void Update(float temperature)
    {
        Console.WriteLine($"Phone Display: Current temperature is {temperature}°C");
    }
}

public class WindowDisplay : IWeatherObserver
{
    public void Update(float temperature)
    {
        Console.WriteLine($"Window Display: It's {temperature}°C outside.");
    }
}
```

---

### Step 5: Use It in Your Program

```csharp
class Program
{
    static void Main(string[] args)
    {
        var station = new WeatherStation();

        var phoneDisplay = new PhoneDisplay();
        var windowDisplay = new WindowDisplay();

        station.RegisterObserver(phoneDisplay);
        station.RegisterObserver(windowDisplay);

        station.SetTemperature(25.0f);  // Notify observers
        station.SetTemperature(30.5f);  // Notify again

        station.RemoveObserver(phoneDisplay);
        station.SetTemperature(28.0f);  // Only window display gets notified
    }
}
```

🖨️ **Output:**

```
WeatherStation: Temperature updated to 25°C
Phone Display: Current temperature is 25°C
Window Display: It's 25°C outside.

WeatherStation: Temperature updated to 30.5°C
Phone Display: Current temperature is 30.5°C
Window Display: It's 30.5°C outside.

WeatherStation: Temperature updated to 28°C
Window Display: It's 28°C outside.
```

---

## 🧠 Benefits of Observer Pattern

|Benefit|Explanation|
|---|---|
|Loose coupling|Subject and observers don’t need to know much about each other|
|Easy to add/remove listeners|Observers can subscribe or unsubscribe anytime|
|Great for event-driven systems|GUIs, logging, notifications, etc.|

---

## 🎁 Summary

|Concept|What It Means|
|---|---|
|Subject|The thing that **notifies** (e.g. weather station)|
|Observer|The thing that **listens and reacts** (e.g. displays)|
|Real-World Use|News updates, YouTube subscriptions, event listeners in GUIs|

---

## 📦 Real-World Examples

- 📰 News Feed: Subscribers get updates when news is published
    
- 🖱️ GUI Frameworks: Button click events notify registered handlers
    
- 📧 Email System: User subscribes to receive newsletters
    
- 📊 Stock Ticker: Notifies investors of stock price changes
	
- [DotNet IObserver Interface](https://learn.microsoft.com/en-us/dotnet/api/system.iobserver-1?view=net-6.0)