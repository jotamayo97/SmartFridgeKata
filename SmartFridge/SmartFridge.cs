namespace SmartFridge;

public class SmartFridge(DateTime currentDate, EventManager manager)
{
    public void FridgeDoorOpened()
    {
        manager.RegisterEvent(new FridgeDoorOpenedEvent(currentDate));
    }

    public void ItemAdded(string name, DateTime p1, ItemState @sealed)
    {
        throw new NotImplementedException();
    }

    public void FridgeDoorClosed()
    {
        throw new NotImplementedException();
    }

    public void DayOver()
    {
        throw new NotImplementedException();
    }

    public void ItemRemoved(string name)
    {
        throw new NotImplementedException();
    }

    public string Display()
    {
        return string.Empty;
    }
}