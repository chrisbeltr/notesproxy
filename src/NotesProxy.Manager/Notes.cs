using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace NotesProxy.Manager;

internal class Notes : INotes
{
    private string _databaseFile =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NotesProxy", "notes.db");

    public Notes()
    {
        if (!Directory.Exists(Path.GetDirectoryName(_databaseFile)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_databaseFile)!);
        }

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        var tableCreate = connection.CreateCommand();
        tableCreate.CommandText = """
                                  CREATE TABLE IF NOT EXISTS notes (
                                      id INTEGER PRIMARY KEY,
                                      name TEXT NOT NULL UNIQUE,
                                      location TEXT NOT NULL,
                                      editor TEXT NOT NULL,
                                      category TEXT NOT NULL
                                  );
                                  """;
        tableCreate.ExecuteNonQuery();
    }

    private Note? FindNote(string name)
    {
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM notes WHERE name = @name";
        command.Parameters.AddWithValue("@name", name);

        using var reader = command.ExecuteReader();
        Note? note = null;
        while (reader.Read())
        {
            note = new Note(
                Name: reader.GetString(1),
                Location: reader.GetString(2),
                Editor: reader.GetString(3),
                Category: reader.GetString(4)
            );
        }

        return note;
    }

    public Note GetNote(string name)
    {
        return FindNote(name) ?? throw new Exception("Note does not exist.");
    }

    public bool NoteExists(string name) => FindNote(name) != null;

    public List<Note> QueryDatabase(string? queryLocation = null)
    {
        var list = new List<Note>();

        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            queryLocation == null ? "SELECT * FROM notes" : "SELECT * FROM notes WHERE location = @query";
        command.Parameters.AddWithValue("@query", queryLocation);

        using var reader = command.ExecuteReader();
        var schema = reader.GetColumnSchema();
        while (reader.Read())
        {
            var note = new Note
            {
                Name = reader.GetString(1),
                Location = reader.GetString(2),
                Editor = reader.GetString(3),
                Category = reader.GetString(4)
            };

            list.Add(note);
        }

        return list;
    }

    public void InsertNote(Note note)
    {
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var noteAdd = connection.CreateCommand();
        noteAdd.CommandText =
            "INSERT INTO notes (name, location, editor, category) VALUES (@name, @location, @editor, @category)";
        noteAdd.Parameters.AddWithValue("@name", note.Name);
        noteAdd.Parameters.AddWithValue("@location", note.Location);
        noteAdd.Parameters.AddWithValue("@editor", note.Editor);
        noteAdd.Parameters.AddWithValue("@category", note.Category);

        try
        {
            noteAdd.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            if (ex.SqliteExtendedErrorCode == 2067)
                throw new Exception("Note already exists."); // 2067 - SQLITE_CONSTRAINT_UNIQUE
            Console.WriteLine("Unknown error... Please report this!");
            throw;
        }
    }

    public void DeleteNote(string note)
    {
        if (!NoteExists(note)) throw new Exception("Note does not exist.");
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var noteDelete = connection.CreateCommand();
        noteDelete.CommandText = "DELETE FROM notes WHERE name = @name";
        noteDelete.Parameters.AddWithValue("@name", note);

        noteDelete.ExecuteNonQuery();
    }

    public void DeleteNote(int index)
    {
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var noteDelete = connection.CreateCommand();
        noteDelete.CommandText = "DELETE FROM notes WHERE id = (SELECT id FROM notes LIMIT 1 OFFSET @offset)";
        noteDelete.Parameters.AddWithValue("@offset", index - 1);

        noteDelete.ExecuteNonQuery();
    }

    public void UpdateNote(string name, Note newNote)
    {
        if (!NoteExists(name)) throw new Exception("Note does not exist.");

        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var noteUpdate = connection.CreateCommand();
        noteUpdate.CommandText =
            "UPDATE notes SET name = @newname, location = @newlocation, editor = @neweditor, category = @newcategory WHERE name = @name";
        noteUpdate.Parameters.AddWithValue("@newname", newNote.Name);
        noteUpdate.Parameters.AddWithValue("@newlocation", newNote.Location);
        noteUpdate.Parameters.AddWithValue("@neweditor", newNote.Editor);
        noteUpdate.Parameters.AddWithValue("@newcategory", newNote.Category);
        noteUpdate.Parameters.AddWithValue("@name", name);

        try
        {
            noteUpdate.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            if (ex.SqliteExtendedErrorCode == 2067)
                throw new Exception("Note already exists."); // 2067 - SQLITE_CONSTRAINT_UNIQUE
            Console.WriteLine("Unknown error... Please report this!");
            throw;
        }
    }
}