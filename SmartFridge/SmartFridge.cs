namespace SmartFridge;

public class SmartFridge{

    private DateTime currentDate;

    public void SetCurrentDate(DateTime dateTime)
    {
        currentDate = dateTime;
    }

    public void FridgeDoorOpened()
    {
        throw new NotImplementedException();
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