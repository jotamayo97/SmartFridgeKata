namespace SmartFridge.Test;

public class EventManagerShould
{
    [Fact]
    public void record_events_in_order()
    {
        var manager = new EventManager();
        var date = new DateTime(2021, 10, 18);

        manager.RegisterEvent(new FridgeDoorOpenedEvent(date));
        manager.RegisterEvent(new ItemAddedEvent("Milk", date.AddDays(3), ItemState.Sealed, date));
        manager.RegisterEvent(new FridgeDoorClosedEvent(date));

        var events = manager.events;

        Assert.Collection(events,
            e => Assert.IsType<FridgeDoorOpenedEvent>(e),
            e => Assert.IsType<ItemAddedEvent>(e),
            e => Assert.IsType<FridgeDoorClosedEvent>(e)
        );
    }
}