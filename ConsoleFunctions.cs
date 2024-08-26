using System;
using System.IO;

public class Functions
{
    public static void PfSolara()
    { 
        Console.ForegroundColor = ConsoleColor.DarkMagenta;

        string[] text = new string[]
        {
"███████  ██████  ██       █████  ██████   █████  ",    
"██      ██    ██ ██      ██   ██ ██   ██ ██   ██ ",    
"███████ ██    ██ ██      ███████ ██████  ███████ ",    
"     ██ ██    ██ ██      ██   ██ ██   ██ ██   ██ ",   
"███████  ██████  ███████ ██   ██ ██   ██ ██   ██ ",
"                                                 ",
"-----------------------------------------------------------------------------------------",  
                                                     

        };

        foreach (string line in text)
        {
            Console.WriteLine(line);
        }

        Console.ResetColor();
    }
    public async Task StartPf()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("[ - ] Fetching endpoint . . .");
    }
}
