
---

## 🔍 A Toy Mediator (a.k.a. “The Teacher”)

Imagine you're in a **classroom** full of people. Everyone wants to talk:

- Timmy wants to talk to Jenny
    
- Max wants to talk to Lucy
    
- Jenny wants to talk to everyone 😄
    

Now, if **everyone talks to everyone directly**, it gets **chaotic**! 🗣️

So, the teacher steps in and says:

> “Stop! If you want to talk to someone, **tell me** — and **I’ll handle it.**”

Now:

- Timmy says: “Hey teacher, tell Jenny I want a crayon.”
    
- Teacher: “Jenny, Timmy wants a crayon.”
    

🎉 That’s the **Mediator Pattern**!

---

## 🔍 What is the Mediator Pattern?

The **Mediator Pattern** is a **behavioral design pattern** that:

- **Centralizes communication** between objects
    
- **Decouples** them from talking directly to each other
    
- Each object talks only to the **mediator**, and the mediator coordinates
    

Think of it as a **traffic controller**, or a **group chat admin**.

---

## 🔍 When Should You Use It?

Use the Mediator Pattern when:

✅You have **many objects** interacting in **complex ways**
✅You want to avoid **tight coupling** between them
✅You want to move **interaction logic** to a single place

---

## 🧑‍💻 C# Example: Chat Room Mediator

Let’s build a **chat room** where users send messages **through a mediator**, not directly to each other.

---

### Step 1: Define the Mediator Interface

```csharp
public interface IChatMediator
{
    void SendMessage(string message, User sender);
    void RegisterUser(User user);
}
```

---

### Step 2: Create the Concrete Mediator

```csharp
public class ChatRoom : IChatMediator
{
    private List<User> users = new List<User>();

    public void RegisterUser(User user)
    {
        users.Add(user);
    }

    public void SendMessage(string message, User sender)
    {
        foreach (var user in users)
        {
            if (user != sender)
            {
                user.Receive(message, sender.Name);
            }
        }
    }
}
```

---

### Step 3: Create the User Class

```csharp
public class User
{
    public string Name { get; private set; }
    private IChatMediator chatMediator;

    public User(string name, IChatMediator mediator)
    {
        Name = name;
        chatMediator = mediator;
    }

    public void Send(string message)
    {
        Console.WriteLine($"{Name} sends: {message}");
        chatMediator.SendMessage(message, this);
    }

    public void Receive(string message, string from)
    {
        Console.WriteLine($"{Name} receives from {from}: {message}");
    }
}
```

---

### Step 4: Use It in Your Main Program

```csharp
class Program
{
    static void Main(string[] args)
    {
        IChatMediator chatRoom = new ChatRoom();

        var alice = new User("Alice", chatRoom);
        var bob = new User("Bob", chatRoom);
        var charlie = new User("Charlie", chatRoom);

        chatRoom.RegisterUser(alice);
        chatRoom.RegisterUser(bob);
        chatRoom.RegisterUser(charlie);

        alice.Send("Hi everyone!");
        bob.Send("Hello Alice!");
    }
}
```

🖨️ **Output:**

```
Alice sends: Hi everyone!
Bob receives from Alice: Hi everyone!
Charlie receives from Alice: Hi everyone!
Bob sends: Hello Alice!
Alice receives from Bob: Hello Alice!
Charlie receives from Bob: Hello Alice!
```

---

## 🧠 Benefits of Mediator Pattern

|Benefit|Explanation|
|---|---|
|Reduces complexity|No tangled mess of direct communication|
|Loose coupling|Components don’t depend on each other|
|Centralized control|Easy to manage interactions in one place|
|Easy to modify|Change interaction rules without changing participants|

---

## 🎁 Summary

|Concept|What It Means|
|---|---|
|Mediator|A **middle-person** who manages communication|
|Use Case|When **many parts talk**, but you want to organize the mess|
|Real World|Air traffic control, group chats, teacher in classroom|

---

## 🧱 Real-World Examples

- 🛫 **Air Traffic Control**: Planes don’t talk to each other — they talk to the tower
    
- 💬 **Chat App**: All messages go through a chat server
    
- 👩‍🏫 **GUI Events**: Button clicks talk to a mediator (like MVC controller)