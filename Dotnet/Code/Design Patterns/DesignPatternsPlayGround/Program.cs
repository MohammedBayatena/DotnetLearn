using DesignPatternsPlayGround.Builder;
using DesignPatternsPlayGround.Facade;
using DesignPatternsPlayGround.Factory;
using DesignPatternsPlayGround.Observer.DotNetIObservable;
using DesignPatternsPlayGround.Observer.SimpleObserver;
using DesignPatternsPlayGround.Strategy;
using DesignPatternsPlayGround.Strategy.DependencyInjectedStrategies;
using DesignPatternsPlayGround.Strategy.SimpleStrategy;
using Microsoft.Extensions.DependencyInjection;

#region Factory

//1- Toys Factory

var toysFactory = new ToysFactory();

var car = toysFactory.CreateToy(ToyType.Car);
var miku = toysFactory.CreateToy(ToyType.Miku);

Console.WriteLine("----Toy Factory Output Start----");
car.Play();
miku.Play();
Console.WriteLine("----Toy Factory Output End----");


//2- Abstract Toys Factory

var abstractToysFactory = new AbstractToysFactory(new LegoFactory());

var legoCar = abstractToysFactory.CreateCarToy();
var legoDoll = abstractToysFactory.CreateDollToy();


Console.WriteLine("---- Abstract Factory Output Start----");

legoCar.Drive();
legoDoll.MakeSound();

Console.WriteLine("---- Abstract Factory Output End----");

#endregion

#region Builder

Console.WriteLine("---- Builder Output Start----");

//Builder Using Constructor Pattern
var director = new HouseDirector();
var basicBuilder = new BasicHouseBuilder();
director.Construct(basicBuilder);
var smallHouse = basicBuilder.GetResult();
smallHouse.DescribeTheHouse();


//Builder Using Extension Methods
var fancyHouseBuilder = new FancyHouseBuilder();
var fancyHouse = fancyHouseBuilder.GetResult();


Console.WriteLine("\n");
fancyHouse.DescribeTheHouse();


Console.WriteLine("---- Builder Output End----");

#endregion

#region Facade

Console.WriteLine("---- Facade Output Start----");

// Create the subsystems
var projector = new Projector();
var sound = new SoundSystem();
var lights = new Lights();
var dvd = new DVDPlayer();

// Create the facade
var homeTheater = new HomeTheaterFacade(projector, sound, lights, dvd);

// Use the simple facade method
homeTheater.StartMovie("The Lego Movie");

Console.WriteLine("---- Facade Output End----");

#endregion

#region Strategy

Console.WriteLine("---- Strategy Output Start----");

//1. Dependency Injected Strategies
static ServiceProvider CreateServices()
{
    var serviceProvider = new ServiceCollection()
        .AddTransient<IStrategy, VehicleValidationStrategy>()
        .AddTransient<IStrategy, CarValidationStrategy>()
        .AddTransient<IStrategy, TruckValidationStrategy>()
        .BuildServiceProvider();
    return serviceProvider;
}

var services = CreateServices();
var validationStrategies = services.GetServices<IStrategy>();

//Based on this Type the strategies Will be Executed or Not
// Car and Truck are Both vehicles hence the vehicle should always execute
// Cars and Truck However should not be validated by each other's validation strategy
var vehicle = new Car()
{
    NumberOfWheels = 0,
    LicensePlateNumber = null,
    NumberOfSeats = 5,
    Weight = 7
};

foreach (var strategy in validationStrategies)
{
    var errors = strategy.Validate(vehicle);
    Console.WriteLine(string.Join(", ", errors));
}


// Simple Strategy
var cart = new ShoppingCart();
cart.SetPaymentStrategy(new CreditCardPayment());
cart.Checkout(49.99m); // Paid using Credit Card

cart.SetPaymentStrategy(new PayPalPayment());
cart.Checkout(19.99m); // Paid using PayPal

cart.SetPaymentStrategy(new BitcoinPayment());
cart.Checkout(99.99m); // Paid using Bitcoin

Console.WriteLine("---- Strategy Output End----");

#endregion

#region Observer

Console.WriteLine("---- Observer Output Start----");

//1. Manually Crafted Observer
var samsunPhoneObserver = new PhoneDisplay();
var myHomeObserver = new WindowDisplay();

var weatherStation = new WeatherStation();

weatherStation.RegisterObserver(samsunPhoneObserver);
weatherStation.RegisterObserver(myHomeObserver);

weatherStation.SetTemperature(22.7f);

// Dotnet Observer

var alarmProvider = new Alarm();
var alarmSubscriber1 = new PoliceStation("NYPD");
var alarmSubscriber2 = new PoliceStation("LAPD");

//Subscribe to the Events
alarmSubscriber1.Subscribe(alarmProvider);
alarmSubscriber2.Subscribe(alarmProvider);
//Push an Alarm
alarmProvider.AlarmCriticalValue(555);
//LAPD Station Unsubscribe
alarmSubscriber2.Unsubscribe();
//Push an Alarm again
alarmProvider.AlarmCriticalValue(null);
// Finalize Transmission
alarmProvider.EndTransmission();


Console.WriteLine("---- Observer Output End---");

#endregion