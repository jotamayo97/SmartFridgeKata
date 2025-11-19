namespace SmartFridge.Test;

public class SmartFridgeShould
{
    [Fact]
    public void Acceptance_scenario()
    {
        var fridge = new SmartFridge();
        fridge.SetCurrentDate(new DateTime(2021, 10, 18, 0, 0, 0));

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
}