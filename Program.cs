using System;
using System.IO;

namespace MySalesFindReplace
{
    class Program
    {
        static string[] githubText = { 
            "../common-ux/", 
            "../opportunity-ux/", 
            "../attainment-ux/", 
            "../account-ux/" 
        };
        static string[] localText = {
            "../../../../mysalesmanager-common-app/dist/mysales/common-ux/",
            "../../../../mysalesmanager-opportunity-app/src/mysales/opportunity-ux/",
            "../../../../mysalesmanager-attainment-app/src/mysales/attainment-ux/",
            "../../../../mysalesmanager-accounts-app/src/mysales/account-ux/"
        };
        static string type="";
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Select \n1 to prepare for LOCAL\n2 to prepare for CHECKIN\nYour Input: ");
                type = Console.ReadLine();
                Console.Write("Parent directory (If Empty, Current Dir will be considered): ");
                string dir = Console.ReadLine();
                if (String.IsNullOrEmpty(dir))
                    dir = Directory.GetCurrentDirectory();
                if (dir.Contains("mysales"))
                {
                    Console.WriteLine("\n----------------------------------PROCESS STARTED----------------------------------\nImpacted Files");
                    DirSearch(dir);
                    Console.WriteLine("\n----------------------------------PROCESS END----------------------------------\n\n\n");
                }
                else
                {
                    Console.WriteLine("Not a valid directory to proceed..");
                }
            }
        }
        static void DirSearch(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    if (d.Contains("-app") && !d.Contains("node_modules") && !d.Contains("scss"))
                    {
                        foreach (string f in Directory.GetFiles(d))
                        {
                            if ((f.EndsWith(".css") || f.EndsWith(".html") || f.EndsWith(".js") || f.EndsWith(".json")) && !f.Contains("mdb."))
                            {
                                string text = File.ReadAllText(f);
                                if(ContainsReplacableText(text))
                                {
                                    for(int i = 0; i < githubText.Length; i++)
                                    {
                                        if (type.Contains("1"))
                                            text = text.Replace(githubText[i], localText[i]);
                                        else if (type.Contains("2"))
                                            text = text.Replace(localText[i], githubText[i]);
                                    }
                                    File.WriteAllText(f, text);
                                    Console.WriteLine(f);
                                }
                            }
                        }
                        DirSearch(d);
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        static bool ContainsReplacableText(string text)
        {
            if (type.Contains("1"))
            {
                foreach(var entry in githubText)
                {
                    if (text.Contains(entry))
                        return true;
                }
            }
            else if (type.Contains("2"))
            {
                foreach (var entry in localText)
                {
                    if (text.Contains(entry))
                        return true;
                }
            }
            return false;
        }
    }
}
