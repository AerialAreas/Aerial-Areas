using Godot;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

public static class TextFileReader
{
    public static string GetStringFromTextFile(string path)
    {
        using var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
		string data = "";
		while (!file.EofReached())
        {
            string line = file.GetLine();
            data += line + "\n";
		}
		return data;
    }
}
