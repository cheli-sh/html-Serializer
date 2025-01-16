using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class HtmlHelper
{
    private static HtmlHelper instance;
    public string[] HtmlTags { get; private set; }
    public string[] VoidTags { get; private set; }

    private HtmlHelper()
    {
        // טוען את הנתונים מקבצי ה-JSON
        HtmlTags = LoadJson("HtmlTags.json");
        VoidTags = LoadJson("HtmlVoidTags.json");
    }

    public static HtmlHelper Instance
    {
        get
        {
            if (instance == null)
                instance = new HtmlHelper();
            return instance;
        }
    }

    private string[] LoadJson(string filename)
    {
        string json = File.ReadAllText(filename);
        return JsonSerializer.Deserialize<string[]>(json);
    }
}
