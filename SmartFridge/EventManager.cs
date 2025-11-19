namespace SmartFridge;

public class EventManager
{
    public readonly List<IFridgeEvent> events = new();
    public void RegisterEvent(IFridgeEvent fridgeEvent)
    {
        events.Add(fridgeEvent);
    }
    
    
}