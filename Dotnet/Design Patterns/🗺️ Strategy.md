
## 🔍 Different Ways to Travel

Imagine you want to go to Grandma's house. You can:

- 🚗 Drive a car
    
- 🚲 Ride a bike
    
- 🚶 Walk
    

All are **different ways** to do the **same task**: _go to Grandma’s house_.

You tell your toy assistant:

> “Use **car strategy** today!”  
> Tomorrow:  
> “Use **bike strategy** instead!”

Each travel method is different, but you use them the same way.

🎉 That’s the **Strategy Pattern**.

---

## 🔍 What is the Strategy Pattern?

The **Strategy Pattern** lets you **change the behavior of an object at runtime** by using **different interchangeable algorithms or strategies**.

You:

- Define a **family of algorithms**
    
- Encapsulate them in separate classes
    
- Make them **interchangeable** inside the object using them
    

---

## 🔍 When Should You Use It?

Use the Strategy Pattern when:

✅ You have **many ways to do something**, and want to choose dynamically
✅You want to **avoid big `if-else` or `switch` statements**
✅ You want to follow the **Open/Closed Principle**: easy to add strategies, no need to touch the rest

---

## 🧑‍💻 C# Example: Choosing Payment Strategy

Let’s say you're building a checkout system where users can pay by:

- **Credit Card**
    
- **PayPal**
    
- **Bitcoin**
    

Each payment method is a **strategy**.

---

### Step 1: Define the Strategy Interface

```csharp
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}
```

---

### Step 2: Create Concrete Strategies

```csharp
public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using Credit Card.");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using PayPal.");
    }
}

public class BitcoinPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using Bitcoin.");
    }
}
```

---

### Step 3: Create the Context Class (e.g., Checkout System)

```csharp
public class ShoppingCart
{
    private IPaymentStrategy paymentStrategy;

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        paymentStrategy = strategy;
    }

    public void Checkout(decimal amount)
    {
        if (paymentStrategy == null)
        {
            Console.WriteLine("Payment strategy not selected.");
        }
        else
        {
            paymentStrategy.Pay(amount);
        }
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
        var cart = new ShoppingCart();

        cart.SetPaymentStrategy(new CreditCardPayment());
        cart.Checkout(49.99m);  // Paid using Credit Card

        cart.SetPaymentStrategy(new PayPalPayment());
        cart.Checkout(19.99m);  // Paid using PayPal

        cart.SetPaymentStrategy(new BitcoinPayment());
        cart.Checkout(99.99m);  // Paid using Bitcoin
    }
}
```

🖨️ Output:

```
Paid $49.99 using Credit Card.
Paid $19.99 using PayPal.
Paid $99.99 using Bitcoin.
```

---

## 🎯 Benefits of Strategy Pattern

|Benefit|Explanation|
|---|---|
|Replaces conditionals|No more `if-else` chains everywhere|
|Easy to add new strategies|Just make a new class implementing the interface|
|Allows runtime flexibility|Change behavior on the fly|
|Promotes clean, modular code|Each strategy has one job and is easy to test|

---

## 🎁 Summary

|Concept|What It Means|
|---|---|
|Strategy|Different **ways to do something** (like paying or moving)|
|Use Case|You want to **swap behaviors at runtime**|
|Real Example|Choose how to **pay for toys**: card, PayPal, or Bitcoin|

---

## 🧠 Real-World Examples

- 🕹️ Game: Different movement strategies (walk, fly, swim)
    
- 📦 Shipping: Choose FedEx, UPS, or DHL shipping
    
- 📊 Sorting: Switch between QuickSort, MergeSort, BubbleSort