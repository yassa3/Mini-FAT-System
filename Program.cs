using static AKVLNADKLNV.Virtual_Disk;
using static AKVLNADKLNV.Commands;

namespace AKVLNADKLNV
{
    class Program
    {
        static public Directory current_directory;
        static public string current_path;
        static string[]  input(string str)
        {
            string[] s = str.Split(' ');
            return s;
        }
        static public string get_current_direcrory()
        {
            return current_path;
        }
        static void command(string[] str)
        {
            if (str[0] == "quit")
            {
                exit();
            }
            else if (str[0] == "help")
            {
                if (str.Length > 1)
                    help(str[1]);
                else
                    help();
            }
            else if (str[0] == "cls")
            {
                cls();
            }
            else if (str[0]=="cd")
            {
                if (str.Length > 1)
                    cd(str[1]);
                else
                    Console.WriteLine("Please enter the directory");
            }
            else if (str[0] == "rd")
            {
                if (str.Length > 1)
                    rd(str[1]);
                else
                    Console.WriteLine("Please enter the directory");
            }
            else if (str[0] == "md")
            {
                
                if (str.Length > 1)
                    md(str[1]);
                else
                    Console.WriteLine("Please enter the directory");
                
            }
            else if (str[0] == "dir")
            {

                if (str.Length > 1)
                    dir(str[1]);
                else
                    dir();

            }
            else if (str[0] == "import")
            {

                if (str.Length > 1)
                    import(str[1]);
                else
                    Console.WriteLine("Please enter the path of file");

            }
            else if (str[0] == "type")
            {

                if (str.Length > 1)
                    type(str[1]);
                else
                    Console.WriteLine("Please enter the file name");

            }
            else if (str[0] == "export")
            {

                if (str.Length > 2)
                    export(str[1],str[2]);
                else
                    Console.WriteLine("Please enter the file names");

            }
            else if (str[0] == "rename")
            {

                if (str.Length > 2)
                    rename(str[1],str[2]);
                else
                    Console.WriteLine("Please enter the file names");

            }
            else if (str[0] == "del")
            {

                if (str.Length > 1)
                    del(str[1]);
                else
                    Console.WriteLine("Please enter the file name");
            } 
            else if (str[0] == "copy")
            {
                Directory temp_cr = current_directory;
                if (str.Length > 2)
                    copy(str[1],str[2]);
                else
                    Console.WriteLine("Please enter the file name");
                current_directory = temp_cr;
            }
            else
            {
                Console.WriteLine("The command is not found");

            }
        }
        static void Main(string[] args)
		{
			initializeVdisk();
            while(true)
            {
                Console.Write(get_current_direcrory() + "\\>");
                string Input = Console.ReadLine();
                string n = Input.ToLower().TrimStart().TrimEnd();
                string[] str = input(n);
                command(str);
            }
        }
    }
}
