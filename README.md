# notesproxy

## what is this?

it's a quick little terminal note manager! it does not come with an editor, you supply the editor for it to use. all it does is keep track of what notes you have, where they are, and your preferences for each note. i do not plan on making a gui for this project (though an interactive tui is coming soon if you don't like using commands).

## how do i build it?

you need to have the [dotnet sdk installed](https://dotnet.microsoft.com/en-us/download), this project was built using .net 10.  
after that, just run this command, replacing `[platform]` with one of `[win-x64, osx-x64, linux-x64]`:

```
dotnet publish src/NotesProxy.Cli -c Release -r [platform] -o bin/
```

the final program will be in the `bin` folder, you can copy that anywhere in your path for your operating system.

i will also be making install scripts for each operating system (haven't done macos yet sorry macos users)

## how do i use it?

you can run `notesproxy --help` to see this menu in the terminal, there you can see additional information and command aliases.

the note commands are:

- `create [name] [-l location] [-e editor] [-c category] [-a auto open]`
    - create a note, with the specified settings.
        - if no name is supplied, the default naming format is `note-[M-d-yy-HHmmss]`.
        - if no name is supplied, user will be prompted if they'd like to rename the note after they close it.
        - if location, editor, category, or auto open are not supplied, they will be filled in with the defaults from the configuration file.
- `delete <name>`
    - delete a note with the specified name.
    - if the note no longer exists on the filesystem, the entry for it will still be removed from the database.
- `open <name> [-e editor]`
    - open a note with the specified name.
    - if the editor option is not supplied, it will check the note's settings to find and editor, and then it will default to the editor in the configuration file.
- `edit <name> [-n name] [-l location] [-e editor] [-c category]`
    - edit the settings for a note with the specified name.
    - if an option is not supplied, it will stay the same.
    - if the name or location are changed, the file will be renamed and moved in the filesystem respectively.
- `list [category]`
    - lists all the notes currently stored in the database.
    - if category is supplied, command lists all the notes in that category.
- `nuke`
    - deletes all currently stored notes.
    - this cannot be undone.

the configuration commands are:

- `edit [-l location] [-e editor] [-c category] [-a auto open]`
    - edit the settings in the configuration file.
    - the auto open option works both as a normal option (type either true or false after it) or as a flag (include it in order to set it to true, though it won't be set to false by excluding it).
- `list`
    - list all the current settings in the configuration file.

you can access a textual user interface by using the `notesproxy interactive` command.
