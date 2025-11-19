namespace SmartFridge;

public class EventManager
{
    private readonly List<IFridgeEvent> events = new();
    public void RegisterEvent(IFridgeEvent fridgeEvent)
    {
        events.Add(fridgeEvent);
    }


    public Dictionary<string, Item> Rebuild()
    {
        var state = new Dictionary<string, Item>();

        foreach (var ev in events)
        {
            if (ev is ItemAddedEvent added)
            {
                state[added.productName] =
                    new Item(added.productName, added.expiry, added.state);
            }

            if (ev is FridgeDoorOpenedEvent)
            {
                foreach (var key in state.Keys.ToList())
                {
                    var item = state[key];

                    var degrade = item.state == ItemState.Sealed
                        ? TimeSpan.FromHours(1)
                        : TimeSpan.FromHours(5);

                    state[key] = item with
                    {
                        Expiry = item.Expiry - degrade
                    };
                }
            }
            if (ev is DayOverEvent)
            {
                foreach (var key in state.Keys.ToList())
                {
                    var item = state[key];
                    
                    state[key] = item with
                    {
                        Expiry = item.Expiry - TimeSpan.FromDays(1)
                    };
                }
            }
        }

        return state;
    }
}