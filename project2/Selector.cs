//public class Selector
//{
//    public string TagName { get; set; }
//    public string Id { get; set; }
//    public List<string> Classes { get; set; } = new List<string>();
//    public Selector ParentSelector { get; set; } // הוספנו מאפיין זה

//    public Selector() { }

//    // פונקציה לניתוח מחרוזת סלקטור
//    public static Selector Parse(string selectorString)
//    {
//        Selector selector = new Selector();
//        string[] parts = selectorString.Split(new char[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);

//        if (selectorString.StartsWith("#"))
//        {
//            selector.Id = parts[0];
//        }
//        else if (selectorString.StartsWith("."))
//        {
//            selector.Classes.AddRange(parts);
//        }
//        else if (selectorString.Contains("."))
//        {
//            selector.TagName = selectorString.Substring(0, selectorString.IndexOf('.'));
//            selector.Classes.AddRange(selectorString.Substring(selectorString.IndexOf('.') + 1).Split('.'));
//        }
//        else
//        {
//            selector.TagName = selectorString;
//        }

//        // בודקים אם יש רווחים (סלקטור עם הורים)
//        if (selectorString.Contains(" "))
//        {
//            string[] partsWithParent = selectorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//            selector.TagName = partsWithParent.Last();
//            if (partsWithParent.Length > 1)
//            {
//                selector.ParentSelector = Parse(partsWithParent[0]); // הפוך את הראשון לאבא
//            }
//        }

//        return selector;
//    }
//}
public class Selector
{
    public string TagName { get; set; }
    public string Id { get; set; }
    public List<string> Classes { get; set; } = new List<string>();

    public Selector() { }

    public static Selector Parse(string selectorString)
    {
        Selector selector = new Selector();

        if (selectorString.Contains(" "))
        {
            string[] parts = selectorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            selectorString = parts.Last();
        }

        string[] parts2 = selectorString.Split(new char[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);

        if (selectorString.StartsWith("#"))
        {
            selector.Id = parts2[0];
        }
        else if (selectorString.StartsWith("."))
        {
            selector.Classes.AddRange(parts2);
        }
        else if (selectorString.Contains("."))
        {
            selector.TagName = selectorString.Substring(0, selectorString.IndexOf('.'));
            selector.Classes.AddRange(selectorString.Substring(selectorString.IndexOf('.') + 1).Split('.'));
        }
        else
        {
            selector.TagName = selectorString;
        }

        return selector;
    }
}