namespace SmartFridge.Test;

public class SmartFridgeShould
{
    [Fact]
    public void Acceptance_scenario()
    {
        var fridge = new SmartFridge();
        fridge.SetCurrentDate("18/10/2021");

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Milk",    "21/10/21", ItemState.Sealed);
        fridge.ItemAdded("Cheese",  "18/11/21", ItemState.Sealed);
        fridge.ItemAdded("Beef",    "20/10/21", ItemState.Sealed);
        fridge.ItemAdded("Lettuce", "22/10/21", ItemState.Sealed);
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
        fridge.ItemAdded("Milk",    "26/10/21", ItemState.Opened);
        fridge.ItemAdded("Peppers", "23/10/21", ItemState.Opened);
        fridge.FridgeDoorClosed();

        fridge.DayOver();

        fridge.FridgeDoorOpened();
        fridge.ItemRemoved("Beef");
        fridge.ItemRemoved("Lettuce");
        fridge.FridgeDoorClosed();

        fridge.FridgeDoorOpened();
        fridge.ItemAdded("Lettuce", "22/10/21", ItemState.Opened);
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