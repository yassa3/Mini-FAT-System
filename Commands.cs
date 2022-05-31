namespace AKVLNADKLNV
{
    internal class Commands
    {
        static public void cls()
        {
            Console.Clear();
        }
        static public void help(string par = "")
        {
            if (par == "")
            {
                Console.WriteLine("CLS\t\tCommand used to clear the screen.");
                Console.WriteLine("QUIT\t\tCommand used to quit the shell.");
                Console.WriteLine("HELP\t\tCommand used to provide help information for commands.");
                Console.WriteLine("MD\t\tCommand used to create a new directory.");
                Console.WriteLine("RD\t\tCommand used to remove a directory.");
                Console.WriteLine("CD\t\tCommand used to change the current default directory.");
                Console.WriteLine("DIR\t\tCommand used to list the contents of directory.");
                Console.WriteLine("RENAME\t\tCommand used to rename a file.");
                Console.WriteLine("IMPORT\t\tCommand used to import text file(s) from your computer.");
                Console.WriteLine("EXPORT\t\tCommand used to export text file(s) to your computer.");
                Console.WriteLine("DEL\t\tCommand used to delete one or more files.");
                Console.WriteLine("COPY\t\tCommand used to copy one or more files to another location.");
                Console.WriteLine("TYPE\t\tCommand used to display the contents of a text file.");

            }
            else
            {
                if (par == "cls")
                {
                    Console.WriteLine("CLS\t\tCommand used to clear the screen.");
                }
                else if (par == "quit")
                {
                    Console.WriteLine("QUIT\t\tCommand used to quit the shell.");
                }
                else if (par == "help")
                {
                    Console.WriteLine("HELP\t\tCommand used to provide help information for commands.");
                }
                else if (par == "md")
                {
                    Console.WriteLine("MD\t\tCommand used to create a new directory.");
                }
                else if (par == "rd")
                {
                    Console.WriteLine("RD\t\tCommand used to remove a directory.");
                }
                else if (par == "cd")
                {
                    Console.WriteLine("CD\t\tCommand used to change the current default directory.");
                }
                else if (par == "dir")
                {
                    Console.WriteLine("DIR\t\tCommand used to list the contents of directory.");
                }
                else if (par == "import")
                {
                    Console.WriteLine("IMPORT\t\tCommand used to import text file(s) from your computer.");
                }
                else if (par == "export")
                {
                    Console.WriteLine("EXPORT\t\tCommand used to export text file(s) to your computer.");
                }
                else if (par == "del")
                {
                    Console.WriteLine("DEL\t\tCommand used to delete one or more files.");
                }
                else if (par == "copy")
                {
                    Console.WriteLine("COPY\t\tCommand used to copy one or more files to another location.");
                }
                else if (par == "rename")
                {
                    Console.WriteLine("RENAME\t\tCommand used to rename a file.");
                }
                else if (par == "type")
                {
                    Console.WriteLine("TYPE\t\tCommand used to display the contents of a text file.");
                }
                else
                {
                    Console.WriteLine("This command is not correct.");
                }
            }
        }
        static public void exit()
        {
            Environment.Exit(0);
        }
        static public void md(string argu)
        {
            if (Program.current_directory.Search_Directory(argu) == -1)
            {
                Directory_entry d = new Directory_entry(argu, 1, 0, 0);
                Program.current_directory.dir.Add(d);
                Program.current_directory.write_Directory();
                FAT.set_next(d.F_cluster, -1);
                FAT.write_fat_table_in_file();
                if (Program.current_directory.perant != null)
                {
                    Program.current_directory.perant.Update_content(Program.current_directory.get_directory_entry());

                }
            }
            else
            {
                Console.WriteLine("This directory already exists.");
            }

        }
        static public void rd(string argu)
        {
            int index = Program.current_directory.Search_Directory(argu);
            if (index != -1)
            {
                if (Program.current_directory.dir[index].attr == 1)
                {
                    int first_cluster = Program.current_directory.dir[index].F_cluster;
                    Directory d = new Directory(argu, 1, first_cluster, 0, Program.current_directory);
                    d.delete_directory();
                    d.write_Directory();
                }

            }
            else
            {
                Console.WriteLine("The folder doesn't exist");
            }
        }
        static public void cd(string argu)
        {
            int index = Program.current_directory.Search_Directory(argu);
            if (index != -1)
            {
                int first_cluster = Program.current_directory.dir[index].F_cluster;
                Directory d = new Directory(argu, 1, first_cluster, 0, Program.current_directory);
                d.Read_Directory();
                Program.current_directory = d;
                Program.current_path += '\\'+ Program.current_directory.name;
            }
            else
            {
                if (Program.current_directory.perant != null)
                {
                    
                    string n = new string(Program.current_directory.perant.name);
                    if (n == argu || argu == "..")
                    {
                        Directory d = Program.current_directory.perant;
                        d.Read_Directory();
                        Program.current_directory = d;
                        Program.current_path = Program.current_path.Remove(Program.current_path.Length - 1);
                        while (Program.current_path[Program.current_path.Length - 1] != '\\')
                        {
                            Program.current_path = Program.current_path.Remove(Program.current_path.Length - 1);
                        }
                        Program.current_path = Program.current_path.Remove(Program.current_path.Length - 1);

                    }
                    else
                    {
                        Console.WriteLine("This folder doesn't exist");
                    }
                }
                else
                {
                    Console.WriteLine("This folder doesn't exist");
                }
            }
        }
        static public void dir(string argu = "")
        {
            if (argu == "")
            {
                int num_of_files = 0;
                int num_of_folders = 0;
                int size_of_file = 0;
                Console.WriteLine("Directory of " + Program.current_path);
                Console.WriteLine();
                for (int i = 0; i < Program.current_directory.dir.Count; i++)
                {
                    if (Program.current_directory.dir[i].attr == 0)
                    {
                        int size = Program.current_directory.dir[i].size;
                        string name_of_file = Program.current_directory.dir[i].name;
                        Console.WriteLine("\t" + size + "\t" + name_of_file);
                        num_of_files++;
                        size_of_file += size;
                    }
                    else
                    {
                        string name_of_folder = Program.current_directory.dir[i].name;
                        Console.WriteLine("\t<DIR>   " + name_of_folder);
                        num_of_folders++;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\t" + num_of_files + " File(s)\t" + size_of_file + " bytes");
                Console.WriteLine("\t" + num_of_folders + " Dir(s)\t" + FAT.get_free_space().ToString() + " bytes free");
            }

        }
        static public void import(string argu)
        {


            int idx = argu.LastIndexOf('\\');
            string name = argu.Substring(idx + 1);
            string content = File.ReadAllText(argu);
            int size = content.Length;
            int index = Program.current_directory.Search_Directory(name);
            if (index == -1)
            {
                int fc;
                if (size > 0)
                {
                    fc = FAT.get_Avaliable_Block_ID();
                }
                else
                {
                    fc = 0;
                }
                File_Entry f = new File_Entry(name, 0, fc, size, Program.current_directory, content);
                f.writeFile();
                Directory_entry d = new Directory_entry(name, 0, fc, size);
                Program.current_directory.dir.Add(d);
                Program.current_directory.write_Directory();
            }
            else
            {
                Console.WriteLine("This file already exists.");
            }



        }
        static public void type(string argu)
        {
            int index = Program.current_directory.Search_Directory(argu);
            if (index != -1)
            {
                if (Program.current_directory.dir[index].attr!=1)
                {
                    int fc = Program.current_directory.dir[index].F_cluster;
                    int size = Program.current_directory.dir[index].size;
                    string content = String.Empty;
                    File_Entry f = new File_Entry(argu, 0, fc, size, Program.current_directory, content);
                    f.readFile();
                    Console.WriteLine(f.content);
                }
                else
                {
                    Console.WriteLine("The System can't find the file");
                }
            }
            else
            {
                Console.WriteLine("The System can't find the file");
            }
        }
        static public void export(string src, string des)
        {
            int index = Program.current_directory.Search_Directory(src);
            if (index != -1)
            {
                if (System.IO.Directory.Exists(des))
                {
                    int fc = Program.current_directory.dir[index].F_cluster;
                    int size = Program.current_directory.dir[index].size;
                    string temp = String.Empty;
                    File_Entry f = new File_Entry(src, 0, fc, size, Program.current_directory, temp);
                    f.readFile();
                    if (!File.Exists(des + "\\" + src))
                    {
                        File.WriteAllText(des + "\\" + src, f.content);
                    }
                    else
                    {
                        Console.WriteLine("This file already exists");
                    }
                }
                else
                {
                    Console.WriteLine("The system cannot find the path specified in computer disk");
                }

            }
            else
            {
                Console.WriteLine("This file doesn't exist in Virtual Disk.");
            }
        }
        static public void rename(string old_name, string new_name)
        {
            int index1 = Program.current_directory.Search_Directory(old_name);
            if (index1 != -1)
            {
                int index2 = Program.current_directory.Search_Directory(new_name);
                if (index2 == -1)
                {
                    Directory_entry d = Program.current_directory.dir[index1];
                    d.name = new_name;
                    Program.current_directory.write_Directory();
                }
                else
                {
                    Console.WriteLine("Duplicate file name exist.");
                }
            }
            else
            {
                Console.WriteLine("The system cannot find the file specified.");
            }
        }
        static public void del(string f_name)
        {
            int index = Program.current_directory.Search_Directory(f_name);
            if (index != -1)
            {
                if (Program.current_directory.dir[index].attr == 0)
                {
                    int fc = Program.current_directory.dir[index].F_cluster;
                    int size = Program.current_directory.dir[index].size;
                    File_Entry f = new File_Entry(f_name, 0, fc, size, Program.current_directory, null);
                    f.readFile();
                    f.deleteFile();
                }
                else
                {
                    Console.WriteLine("The system cannot find the file specified.");
                }
            }
            else
            {
                Console.WriteLine("The system cannot find the file specified.");
            }

        }
        static void cd_copy1(string name_of_md)
        {
            int index = Program.current_directory.Search_Directory(name_of_md);
            if (index != -1)
            {
                int first = Program.current_directory.dir[index].F_cluster;
                Directory d = new Directory(name_of_md, 1, first, 0, Program.current_directory);
                Program.current_directory = d;

                Program.current_path = Program.current_path + "\\"+String.Concat(d.name.Where(c => !Char.IsWhiteSpace(c)));
                Program.current_directory.Read_Directory();

            }

        }
        static public void copy(string src, string des)
        {
            
            int index1 = Program.current_directory.Search_Directory(src);
            int fc = Program.current_directory.dir[index1].F_cluster;
            int sz = Program.current_directory.dir[index1].size;
            if (index1 != -1)
            {
                int idx = des.LastIndexOf('\\');
                string name = des.Substring(idx + 1);
                string cur = Program.get_current_direcrory();
                int index2 = Program.current_directory.Search_Directory(des);

                if (index2 != -1)
                {
                    if (Program.current_directory.ToString() != des)
                    {
                        cd_copy1(des);
                        if (Program.current_directory.Search_Directory(src) != -1)
                        {
                            Console.WriteLine("Do you you want to overwrite ?\t Enter y for Yes and n for No");
                            string s = Console.ReadLine();
                            if (s == "y")
                            {
                                Directory_entry d = new Directory_entry(new string(src), 0, fc, sz);
                                int idx1 = Program.current_directory.Search_Directory((string)src);
                                Program.current_directory.dir.RemoveAt(idx1);
                                Program.current_directory.dir.Add(d);

                                Program.current_directory.write_Directory();
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {
                            Directory_entry d = new Directory_entry(src, 0, fc, sz);
                            Program.current_directory.dir.Add(d);
                            Program.current_directory.write_Directory();
                        }
                    }
                    else
                    {
                        Console.WriteLine("The file cannot copied into it self");
                    }
                }
                else
                {
                    Console.WriteLine("The destination is not found");
                }
            }
            else
            {
                Console.WriteLine("The system cannot find the path specified.");
            }
            Directory root = new Directory("H:", 1, 5, 0, null);
            Program.current_directory = root;
            Program.current_path = root.name;
            Program.current_directory.Read_Directory();
        }
    }
}



