namespace NotesProxy.Manager;

public record struct Note(string Name, string Location, string Editor, string Category, bool AutoOpen);