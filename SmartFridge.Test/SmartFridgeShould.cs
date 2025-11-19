using NSubstitute;

namespace SmartFridge.Test;

public class SmartFridgeShould
{
    [Fact]
    public void medium_acceptance_with_multiple_items_and_expired()
    {
        var fridge = new SmartFridge(new DateTime(2021, 10, 18), new EventManager());

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk",    new DateTime(2021,10,19), ItemState.Sealed);
        fridge.ItemAdded("Cheese",  new DateTime(2021,10,25), ItemState.Sealed);
        fridge.ItemAdded("Beef",    new DateTime(2021,10,21), ItemState.Opened);
        fridge.ItemAdded("Lettuce", new DateTime(2021,10,20), ItemState.Sealed);
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();

        fridge.DayOver();

        fridge.FridgeDoorOpened();
        fridge.ItemRemoved("Lettuce");
        fridge.ItemAdded("Lettuce", new DateTime(2021,10,21), ItemState.Opened);
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();

        fridge.DayOver();

        var display = fridge.Display();

        var expected =
            "EXPIRED: Milk\n" +
            "Beef: 0 days remaining\n" +
            "Lettuce: 1 day remaining\n" +
            "Cheese: 4 days remaining";


        Assert.Equal(expected, display);
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
    
    [Fact]
    public void opening_and_closing_the_fridge_degrades_items()
    {
        var current = new DateTime(2021, 10, 18, 0, 0, 0);
        var manager = new EventManager();
        var fridge = new SmartFridge(current, manager);

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Cheese", new DateTime(2021, 10, 19, 0, 0, 0), ItemState.Opened);
        fridge.FridgeDoorClosed();
        
        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();

        var display = fridge.Display();
        
        Assert.Equal("Cheese: 0 days remaining", display);
    }
    
    [Fact]
    public void multiple_fridge_openings_accumulate_degradation_and_can_expire_items()
    {
        var current = new DateTime(2021, 10, 18, 0, 0, 0);
        var manager = new EventManager();
        var fridge = new SmartFridge(current, manager);
        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk", new DateTime(2021, 10, 18, 0, 0, 0), ItemState.Opened);
        fridge.FridgeDoorClosed();
        
        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();
        

        var display = fridge.Display();

        Assert.Equal("EXPIRED: Milk", display);
    }
    
    [Fact]
    public void newly_added_items_after_opening_do_not_receive_past_degradation()
    {
        var current = new DateTime(2021, 10, 18, 0, 0, 0);
        var manager = new EventManager();
        var fridge = new SmartFridge(current, manager);
        
        fridge.FridgeDoorOpened();
        fridge.FridgeDoorClosed();
        
        fridge.ItemAdded("Cheese", 
            new DateTime(2021, 10, 20, 0, 0, 0), 
            ItemState.Sealed
        );

        var display = fridge.Display();
        
        Assert.Equal("Cheese: 2 days remaining", display);
    }
    
    [Fact]
    public void dayover_reduces_one_day_of_remaining_time_for_all_items()
    {
        var date = new DateTime(2021, 10, 18, 0, 0, 0);
        var manager = new EventManager();
        var fridge = new SmartFridge(date, manager);

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk", new DateTime(2021, 10, 21), ItemState.Sealed);
        fridge.FridgeDoorClosed();
        
        Assert.Equal("Milk: 3 days remaining", fridge.Display());

        fridge.DayOver();
        
        Assert.Equal("Milk: 2 days remaining", fridge.Display());
    }
    
    [Fact]
    public void removing_an_item_makes_it_disappear_from_display()
    {
        var date = new DateTime(2021, 10, 18);
        var manager = new EventManager();
        var fridge = new SmartFridge(date, manager);

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk", date.AddDays(3), ItemState.Sealed);
        fridge.ItemAdded("Cheese", date.AddDays(10).AddHours(1), ItemState.Sealed);
        fridge.FridgeDoorClosed();
        
        fridge.FridgeDoorOpened();
        fridge.ItemRemoved("Milk");
        fridge.FridgeDoorClosed();

        var display = fridge.Display();

        Assert.Equal("Cheese: 10 days remaining", display);
    }
}