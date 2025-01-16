//public class HtmlElement
//{
//    public string Name { get; set; }
//    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>(); // שומר את המאפיינים
//    public HtmlElement Parent { get; set; }
//    public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

//    private bool MatchesSelector(HtmlElement element, Selector selector)
//    {
//        // בדיקה אם התג תואם
//        if (!string.IsNullOrEmpty(selector.TagName) && !element.Name.Equals(selector.TagName, StringComparison.OrdinalIgnoreCase))
//            return false;

//        // בדיקה אם יש id
//        if (!string.IsNullOrEmpty(selector.Id) && !element.Attributes.ContainsKey("id"))
//            return false;

//        // בדיקה אם יש כיתות
//        if (selector.Classes.Count > 0)
//        {
//            foreach (var className in selector.Classes)
//            {
//                if (!element.Attributes.ContainsKey("class") || !element.Attributes["class"].Contains(className))
//                    return false;
//            }
//        }

//        return true;
//    }

//    public List<HtmlElement> FindElementsBySelector(string selectorString)
//    {
//        string[] selectorParts = selectorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
//        List<HtmlElement> currentResults = new List<HtmlElement> { this };

//        foreach (string part in selectorParts)
//        {
//            Selector selector = Selector.Parse(part);
//            List<HtmlElement> nextResults = new List<HtmlElement>();

//            foreach (HtmlElement currentElement in currentResults)
//            {
//                // חיפוש אלמנטים ילדים בהתחשב גם בהורים
//                FindElementsBySelectorRecursive(currentElement, selector, nextResults);
//            }
//            currentResults = nextResults;
//        }

//        return currentResults;
//    }

//    private void FindElementsBySelectorRecursive(HtmlElement element, Selector selector, List<HtmlElement> results)
//    {
//        // בדוק אם האלמנט הנוכחי תואם לסלקטור
//        if (MatchesSelector(element, selector))
//        {
//            results.Add(element);
//        }

//        // אם יש הורה שמתאים לסלקטור, נוכל לחפש גם את הילדים
//        if (element.Parent != null && MatchesSelector(element.Parent, selector))
//        {
//            results.Add(element);
//        }

//        // חיפוש עבור ילדים
//        foreach (HtmlElement child in element.Children)
//        {
//            FindElementsBySelectorRecursive(child, selector, results);
//        }
//    }
//}
public class HtmlElement
{
    public string Name { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    public HtmlElement Parent { get; set; }
    public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

    private bool MatchesSelector(HtmlElement element, Selector selector)
    {
        // בדיקה אם התג תואם
        if (!string.IsNullOrEmpty(selector.TagName) && !element.Name.Equals(selector.TagName, StringComparison.OrdinalIgnoreCase))
            return false;

        // בדיקה אם יש id
        if (!string.IsNullOrEmpty(selector.Id) && (!element.Attributes.ContainsKey("id") || !element.Attributes["id"].Equals(selector.Id, StringComparison.OrdinalIgnoreCase)))
            return false;

        // בדיקה אם יש כיתות
        if (selector.Classes.Count > 0)
        {
            if (!element.Attributes.ContainsKey("class"))
                return false;

            string elementClasses = element.Attributes["class"];
            foreach (var className in selector.Classes)
            {
                if (!elementClasses.Contains(className))
                    return false;
            }
        }

        return true;
    }

    public List<HtmlElement> FindElementsBySelector(string selectorString)
    {
        string[] selectorParts = selectorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        List<HtmlElement> currentResults = new List<HtmlElement> { this };

        foreach (string part in selectorParts)
        {
            Selector selector = Selector.Parse(part);
            List<HtmlElement> nextResults = new List<HtmlElement>();

            foreach (HtmlElement currentElement in currentResults)
            {
                // חיפוש אלמנטים ילדים בלבד!
                FindElementsBySelectorRecursive(currentElement.Children, selector, nextResults);
            }
            currentResults = nextResults;
        }

        return currentResults;
    }

    private void FindElementsBySelectorRecursive(List<HtmlElement> elements, Selector selector, List<HtmlElement> results)
    {
        foreach (HtmlElement element in elements)
        {
            if (MatchesSelector(element, selector))
            {
                results.Add(element);
            }

            FindElementsBySelectorRecursive(element.Children, selector, results);
        }
    }
}