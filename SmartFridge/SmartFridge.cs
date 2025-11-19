namespace SmartFridge;

public class SmartFridge(DateTime currentDate, EventManager manager)
{
    public void FridgeDoorOpened()
    {
        manager.RegisterEvent(new FridgeDoorOpenedEvent(currentDate));
    }

    public void ItemAdded(string name, DateTime expiry, ItemState state)
    {
        manager.RegisterEvent(new ItemAddedEvent(name, expiry, state, currentDate ));
    }

    public void FridgeDoorClosed()
    {
        manager.RegisterEvent(new FridgeDoorClosedEvent(currentDate));
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
        var state = manager.Rebuild();
        if (state.Count == 0)
            return string.Empty;
        var lines = new List<string>();
        
        foreach (var item in state.Values.OrderBy(i => i.Expiry))
        {
            var remaining = (item.Expiry - currentDate).Days;
            if (remaining < 0)
            {
                lines.Add($"EXPIRED: {item.Name}");
            }
            if (remaining == 1)
            {
                lines.Add($"{item.Name}: {remaining} day remaining");
            }
            else
            {
                lines.Add($"{item.Name}: {remaining} days remaining");
            }
            
        }

        return string.Join("\n", lines);
    }
}