

## 🔍 The Only Remote in the House

Imagine you have a **TV remote** 📺 in your house.

There’s **only ONE remote** — no matter how many people want to use it.

- You can pass it around
    
- You can store it on the table
    
- But there's always just **ONE** — not two, not three.
    

🎉 That’s the **Singleton Pattern**.

---

## 🔍 What is the Singleton Pattern?

The **Singleton Pattern** is a **creational design pattern** that:

- Ensures a class **has only one instance**
    
- Provides a **global access point** to that instance
    

Think of it like a **president**, **remote control**, or **printer manager** — only one exists at a time in the system.

---

## 🔍 When Should You Use It?

Use the Singleton Pattern when:

✅Only **one instance** of a class should exist (e.g., configuration manager, logger, cache)
✅You need **global access** to that object
✅You want to **control access and instantiation**

---

## 🧑‍💻 C# Example: Logger Singleton

Let’s create a **logger class** that you can use from anywhere, but **only one instance ever exists**.

---

### Step 1: Basic Singleton Class in C#

```csharp
public class Logger
{
    private static Logger instance;
    private static readonly object lockObj = new object();

    // Private constructor - prevents outside instantiation
    private Logger() {}

    public static Logger Instance
    {
        get
        {
            // Double-checked locking for thread safety
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Logger();
                    }
                }
            }
            return instance;
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"[LOG]: {message}");
    }
}
```

---

### Step 2: Using the Singleton

```csharp
class Program
{
    static void Main(string[] args)
    {
        Logger logger1 = Logger.Instance;
        Logger logger2 = Logger.Instance;

        logger1.Log("First log message.");
        logger2.Log("Second log message.");

        Console.WriteLine(ReferenceEquals(logger1, logger2)); // True
    }
}
```

🖨️ **Output:**

```
[LOG]: First log message.
[LOG]: Second log message.
True
```

🎉 Both `logger1` and `logger2` are the **same instance**!

---

## 🧠 Key Parts of Singleton

|Feature|Why it matters|
|---|---|
|`private` constructor|Prevents other classes from creating new instances|
|`static` instance|Shared by everyone|
|`lock`|Ensures thread safety in multi-threaded apps|

---

## 🚫 Caution: Singleton Anti-Patterns

Singletons can **cause problems** if misused:

- **Global state**: Like global variables — can make code hard to test and debug
    
- **Hidden dependencies**: Classes that rely on Singletons may be harder to reuse
    
- **Concurrency issues**: If not thread-safe, can cause bugs in multithreaded apps
    

✅ Use it **sparingly** for truly one-of-a-kind services (e.g., `Logger`, `ConfigManager`, `GameController`).

---

## 🎁 Summary

|Concept|What It Means|
|---|---|
|Singleton|A class that allows **only one object ever**|
|Use Case|When you need **one shared object** for the app|
|Real World|TV remote, database connection manager, president|

---

## 🧱 Real-World Examples

- 🪵 Logger (one global logger)
    
- 💾 Configuration manager (one set of settings)
    
- 📦 Caching system (one shared memory)
    
- 🎮 GameManager (Unity’s central control system)