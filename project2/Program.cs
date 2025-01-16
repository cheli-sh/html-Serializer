using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;

public class Program
{
    // Function to parse HTML into an array of HtmlElement
    public static List<HtmlElement> ParseHtmlToTags(string html)
    {
        var elements = new List<HtmlElement>();
        var regex = new Regex(@"<(?<closing>/)?(?<tag>\w+)(?<attributes>(?:\s+\w+\s*=\s*(?:""[^""]*""|'[^']*'|[\w-]+))*)?\s*/?>", RegexOptions.IgnoreCase);
        var matches = regex.Matches(html);

        foreach (Match match in matches)
        {
            var element = new HtmlElement();
            element.Name = match.Groups["tag"].Value;

            if (match.Groups["closing"].Success)
            {
                // Marking as closing tag by prefixing with "/"
                element.Name = "/" + element.Name;
            }

            // Store attributes in a list
            string attributesString = match.Groups["attributes"].Value;
            var attributeRegex = new Regex(@"\s+(?<name>\w+)\s*=\s*(?:""(?<value>[^""]*)""|'(?<value>[^']*)'|(?<value>[\w-]+))", RegexOptions.IgnoreCase);
            foreach (Match attributeMatch in attributeRegex.Matches(attributesString))
            {
                element.Attributes.Add(attributeMatch.Groups["name"].Value.ToLower(), attributeMatch.Groups["value"].Value);
            }

            elements.Add(element);
        }

        return elements;
    }

    // Function to build the DOM tree
    public static void BuildHtmlTree(List<HtmlElement> htmlTags, HtmlElement parent)
    {
        HtmlElement currentElement = parent;
        foreach (var tagData in htmlTags)
        {
            // Check if it's a closing tag (denoted by "/")
            if (tagData.Name.StartsWith("/"))
            {
                if (currentElement.Parent != null)
                {
                    currentElement = currentElement.Parent;
                }
            }
            else
            {
                HtmlElement newElement = new HtmlElement();
                newElement.Name = tagData.Name;
                newElement.Parent = currentElement;

                if (tagData.Attributes.Count > 0)
                {
                    newElement.Attributes = new Dictionary<string, string>(tagData.Attributes);
                }

                currentElement.Children.Add(newElement);
                currentElement = newElement;
            }
        }
    }

    // Function to load HTML from a website
    public static async Task<string> LoadHtml(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throw an exception if not successful
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }
    }

    public static async Task Main(string[] args)
    {
        string url = "https://www.w3schools.com/"; // Or any other website you want to test
        var html = await LoadHtml(url);

        if (html != null)
        {
            var htmlTags = ParseHtmlToTags(html);
            HtmlElement rootElement = new HtmlElement { Name = "root" };
            BuildHtmlTree(htmlTags, rootElement);

            // Usage examples with selectors
            Console.WriteLine("Usage examples with selectors:");

            // Simple selector by tag name (e.g., <h1>)
            List<HtmlElement> elements1 = rootElement.FindElementsBySelector("h1");
            Console.WriteLine($"\nFound {elements1.Count} elements with tag h1:");
            foreach (var element in elements1)
            {
                Console.WriteLine($"\t{element.Name} - Attributes: {string.Join(", ", element.Attributes)}");
            }

            // Complex selector - descendant of div with class (e.g., div.footerlinks_1)
            List<HtmlElement> elements2 = rootElement.FindElementsBySelector(".footerlinks_1");
            Console.WriteLine($"\nFound {elements2.Count} elements that are descendants of .footerlinks_1:");
            foreach (var element in elements2)
            {
                Console.WriteLine($"\t{element.Name} - Attributes: {string.Join(", ", element.Attributes)}");
            }

            // Complex selector with parent-child relationship (e.g., div#parentId h1)
            List<HtmlElement> elements3 = rootElement.FindElementsBySelector("#footerwrapper .footertext");
            Console.WriteLine($"\nFound {elements3.Count} h1 elements inside div#parentId:");
            foreach (var element in elements3)
            {
                Console.WriteLine($"\t{element.Name} - Attributes: {string.Join(", ", element.Attributes)}");
            }

            List<HtmlElement> h1ElementsInParent = rootElement.FindElementsBySelector(".w3-panel");
            Console.WriteLine($"\nFound {h1ElementsInParent.Count} ");
            foreach (var element in h1ElementsInParent)
            {
                Console.WriteLine($"\t{element.Name} - Attributes: {string.Join(", ", element.Attributes)}");
            }

        }



        else
        {
            Console.WriteLine("Failed to load HTML.");
        }
    }
}
