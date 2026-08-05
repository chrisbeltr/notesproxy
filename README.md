# notesproxy

## what is this?

it's a quick little terminal note manager! it does not come with an editor, you supply the editor for it to use. all it does is keep track of what notes you have, where they are, and your preferences for each note. i do not plan on making a gui for this project (though an interactive tui is coming soon if you don't like using commands).

## how do i build it?

you need to have the [dotnet sdk installed](https://dotnet.microsoft.com/en-us/download), this project was built using .net 10.  
after that, just run this command, replacing `[platform]` with one of `[win-x64, osx-x64, linux-x64]`:  
```dotnet publish src/NotesProxy.Cli -c Release -r [platform] -o bin/```  
the final program will be in the `bin` folder, you can copy that anywhere in your path for your operating system.

i will also be making install scripts for each operating system (i've only gotten to linux for now sorry)

## how do i use it?

the note commands are:

- `create [name] [-l location] [-e editor] [-c category]`
    - create a note, with the specified settings.
        - if no name is supplied, the default naming format is `note-[M-d-yy-HHmmss]`.
        - if location, editor, or category are not supplied, they will be filled in with the defaults from the configuration file.
- `delete <name>`
    - delete a note with the specified name.
    - if the note no longer exists on the filesystem, the entry for it will still be removed from the database.
- `open <name> [-e editor]`
    - open a note with the specified name.
    - if the editor option is not supplied, it will check the note's settings to find and editor, and then it will default to the editor in the configuration file.
- `edit <name> [-n name] [-l location] [-e editor] [-c category]`
    - edit the settings for a note with the specified name.
    - if an option is not supplied, it will stay the same.
    - if the name or location are changed, the file will be renamed and moved respectively.
- `list`
    - lists all the notes currently stored in the database.

the configuration commands are:

- `location <location>`
    - change the default location for new notes.
- `editor <editor>`
    - change the default editor command for new notes.
- `category <category>`
    - change the default category for new notes.
- `list`
    - list all the current settings in the configuration file.
