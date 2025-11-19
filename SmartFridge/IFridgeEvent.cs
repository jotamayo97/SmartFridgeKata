namespace SmartFridge;

public interface IFridgeEvent {DateTime timestamp { get; } }

public record FridgeDoorOpenedEvent(DateTime timestamp) : IFridgeEvent;

public record ItemAddedEvent(string productName, DateTime expiry, ItemState state, DateTime timestamp) : IFridgeEvent;

public record FridgeDoorClosedEvent(DateTime timestamp) : IFridgeEvent;

public record DayOverEvent(DateTime timestamp) : IFridgeEvent;