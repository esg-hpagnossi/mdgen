using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarkdownWikiGenerator
{
    class Program
    {
        // 0 = dll src path, 1 = dest root
        static void Main(string[] args)
        {
            // put dll & xml on same diretory.
            var target = "IdleKit.dll";
            string dest = "md";
            string namespaceMatch = "IdleKit";
            if (args.Length == 1)
            {
                target = args[0];
            }
            else if (args.Length == 2)
            {
                target = args[0];
                dest = args[1];
            }
            else if (args.Length == 3) 
            {
                target = args[0];
                dest = args[1];
                namespaceMatch = args[2];
            }

            var types = MarkdownGenerator.Load(target, namespaceMatch);

            // Home Markdown Builder
            var homeBuilder = new MarkdownBuilder();
            homeBuilder.Header(1, "References");
            homeBuilder.AppendLine();

            foreach (var g in types.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
            {
                string path = Path.Combine(dest, g.Key);

                homeBuilder.HeaderWithLink(2, g.Key, g.Key);

                if (!Directory.Exists(path)) 
                    Directory.CreateDirectory(path);
                
                foreach (var item in g.OrderBy(x => x.Name))
                {
                    homeBuilder.AppendLine();

                    var sb = new StringBuilder();
                    homeBuilder.ListLink(MarkdownBuilder.MarkdownCodeQuote(item.BeautifyName), g.Key + "/" + item.BeautyShortenedName);

                    sb.Append(item.ToString());
                    
                    File.WriteAllText(Path.Combine(path, item.BeautyShortenedName + ".md"), sb.ToString());
                    homeBuilder.AppendLine();
                }

                var sb2 = new StringBuilder();
                sb2.Append($"collapse: true");
                File.WriteAllText(Path.Combine(path, ".page"), sb2.ToString());
            }

            // Gen Home
            File.WriteAllText(Path.Combine(dest, "Home.md"), homeBuilder.ToString());
        }
    }
}
