using NSubstitute;

namespace SmartFridge.Test;

public class SmartFridgeShould
{
    [Fact]
    public void Acceptance_scenario()
    {
        var fridge = new SmartFridge(new DateTime(2021, 10, 18, 0, 0, 0), new EventManager());

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk",    new DateTime(2021, 10, 21, 0, 0, 0), ItemState.Sealed);
        fridge.ItemAdded("Cheese",  new DateTime(2021, 11, 18, 0, 0, 0), ItemState.Sealed);
        fridge.ItemAdded("Beef",    new DateTime(2021, 10, 20, 0, 0, 0), ItemState.Sealed);
        fridge.ItemAdded("Lettuce", new DateTime(2021, 10, 22, 0, 0, 0), ItemState.Sealed);
        fridge.FridgeDoorClosed();

        fridge.DayOver();

        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.ItemRemoved("Milk");
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk",    new DateTime(2021, 10, 26, 0, 0, 0), ItemState.Opened);
        fridge.ItemAdded("Peppers", new DateTime(2021, 10, 23, 0, 0, 0), ItemState.Opened);
        fridge.FridgeDoorClosed();

        fridge.DayOver();

        fridge.FridgeDoorOpened();
        fridge.ItemRemoved("Beef");
        fridge.ItemRemoved("Lettuce");
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Lettuce", new DateTime(2021, 10, 22, 0, 0, 0), ItemState.Opened);
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();

        fridge.DayOver();

        String display = fridge.Display();

        Assert.Equal(
            @"EXPIRED: Milk
            Lettuce: 0 days remaining
            Peppers: 1 day remaining
            Cheese: 31 days remaining",
            display);
    
    }
    
    [Fact]
    public void Display_shows_empty_when_fridge_has_no_items()
    {
        var fridge = new SmartFridge(new DateTime(2021,10,18), new EventManager());

        Assert.Equal(string.Empty, fridge.Display());
    }
    
    [Fact]
    public void record_a_FridayDoorOpened_event()
    {
        var date = new DateTime(2021,10,18);
        
        var manager = Substitute.For<EventManager>();
        
        var fridge = new SmartFridge(date, manager);

        fridge.FridgeDoorOpened();
        
        manager.Received(1)
               .RegisterEvent(Arg.Any<FridgeDoorOpenedEvent>());
    
    }
    
    [Fact]
    public void display_shows_single_added_item()
    {
        var date = new DateTime(2021, 10, 18);
        var manager = new EventManager();
        var fridge = new SmartFridge(date, manager);

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk", date.AddDays(3), ItemState.Sealed);
        fridge.FridgeDoorClosed();

        var display = fridge.Display();

        Assert.Equal("Milk: 3 days remaining", display);
    }
    
    [Fact]
    public void display_shows_multiple_items_sorted_by_expiry()
    {
        var date = new DateTime(2021, 10, 18);
        var manager = new EventManager();
        var fridge = new SmartFridge(date, manager);

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk", date.AddDays(3), ItemState.Sealed);
        fridge.ItemAdded("Beef", date.AddDays(1), ItemState.Sealed);
        fridge.ItemAdded("Cheese", date.AddDays(10), ItemState.Sealed);
        fridge.FridgeDoorClosed();

        var display = fridge.Display();

        Assert.Equal(
            "Beef: 1 day remaining\n" +
            "Milk: 3 days remaining\n" +
            "Cheese: 10 days remaining",
            display
        );
    }
}