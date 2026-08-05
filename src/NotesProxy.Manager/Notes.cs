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
                                      editor TEXT,
                                      category TEXT
                                  );
                                  """;
        tableCreate.ExecuteNonQuery();
    }
    
    internal List<string?> GetNote(string name)
    {
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM notes WHERE name = @name";
        command.Parameters.AddWithValue("@name", name);

        using var reader = command.ExecuteReader();
        var list = new List<string?>();
        while (reader.Read())
        {
            var schema = reader.GetColumnSchema();
            for (var i = 1; i < schema.Count; i++)
            {
                list.Add(reader.IsDBNull(i) ? null : reader.GetString(i));
            }
        }

        return list;
    }

    internal bool NoteExists(string name)
    {
        var note = GetNote(name);
        return note.Count > 0;
    }

    public List<List<string?>> QueryDatabase(string? queryLocation = null)
    {
        var list = new List<List<string?>>();

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
            var columns = new List<string?>();
            for (var i = 1; i < schema.Count; i++)
            {
                columns.Add(reader.IsDBNull(i) ? null : reader.GetString(i));
            }

            list.Add(columns);
        }

        return list;
    }

    public void InsertNote(List<string?> note)
    {
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var noteAdd = connection.CreateCommand();
        noteAdd.CommandText =
            "INSERT INTO notes (name, location, editor, category) VALUES (@name, @location, @editor, @category)";
        noteAdd.Parameters.AddWithValue("@name", note[0]);
        noteAdd.Parameters.AddWithValue("@location", note[1]);
        noteAdd.Parameters.AddWithValue("@editor", note[2] == null ? DBNull.Value : note[2]);
        noteAdd.Parameters.AddWithValue("@category", note[3] == null ? DBNull.Value : note[3]);

        try
        {
            noteAdd.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            string message = ex.SqliteExtendedErrorCode switch
            {
                2067 => "Note already exists.", // 2067 - SQLITE_CONSTRAINT_UNIQUE
                _ => "Unknown error. Please try again later."
            };
            throw new Exception(message);
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

    public void UpdateNote(string name, List<string?> newNote)
    {
        if (!NoteExists(name)) throw new Exception("Note does not exist.");
        var oldNote = GetNote(name);
        
        using var connection = new SqliteConnection($"Data Source={_databaseFile};");
        connection.Open();

        using var noteUpdate = connection.CreateCommand();
        noteUpdate.CommandText =
            "UPDATE notes SET name = @newname, location = @newlocation, editor = @neweditor, category = @newcategory WHERE name = @name";
        noteUpdate.Parameters.AddWithValue("@newname", newNote[0] == "" ? DBNull.Value : newNote[0] != null ? newNote[0] : oldNote[0] != null ? oldNote[0] : DBNull.Value);
        noteUpdate.Parameters.AddWithValue("@newlocation", newNote[1] == "" ? DBNull.Value : newNote[1] != null ? newNote[1] : oldNote[1] != null ? oldNote[1] : DBNull.Value);
        noteUpdate.Parameters.AddWithValue("@neweditor", newNote[2] == "" ? DBNull.Value : newNote[2] != null ? newNote[2] : oldNote[2] != null ? oldNote[2] : DBNull.Value);
        noteUpdate.Parameters.AddWithValue("@newcategory", newNote[3] == "" ? DBNull.Value : newNote[3] != null ? newNote[3] : oldNote[3] != null ? oldNote[3] : DBNull.Value);
        noteUpdate.Parameters.AddWithValue("@name", name);
        
        try
        {
            noteUpdate.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            string message = ex.SqliteExtendedErrorCode switch
            {
                2067 => "Note already exists.", // 2067 - SQLITE_CONSTRAINT_UNIQUE
                _ => "Unknown error. Please try again later."
            };
            throw new Exception(message);
        }
    }
}