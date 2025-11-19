namespace SmartFridge.Test;

public class EventManagerShould
{
    [Fact]
    public void rebuilds_state_after_opening_adding_and_closing_door()
    {
        var date = new DateTime(2021, 10, 18);
        var manager = new EventManager();

        manager.RegisterEvent(new FridgeDoorOpenedEvent(date));

        manager.RegisterEvent(new ItemAddedEvent(
            "Milk",
            date.AddDays(3),
            ItemState.Sealed,
            date
        ));

        manager.RegisterEvent(new ItemAddedEvent(
            "Lettuce",
            date.AddDays(1),
            ItemState.Opened,
            date
        ));

        manager.RegisterEvent(new FridgeDoorClosedEvent(date));

        var state = manager.Rebuild();

        Assert.Equal(2, state.Count);

        Assert.Equal(date.AddDays(3), state["Milk"].Expiry);
        Assert.Equal(ItemState.Sealed, state["Milk"].state);

        Assert.Equal(date.AddDays(1), state["Lettuce"].Expiry);
        Assert.Equal(ItemState.Opened, state["Lettuce"].state);
    }
}