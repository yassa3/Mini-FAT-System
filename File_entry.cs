namespace AKVLNADKLNV
{
    public class File_Entry : Directory_entry
    {
        public Directory parent;
        public string content;
        public File_Entry(string Name, byte Attr, int firstClust, int size, Directory p, string con)
            : base(Name, Attr, firstClust, size)
        {
            parent = p;
            content = con;
        }
        public static byte[] ConvertContentToBytes(string Con)
        {
            byte[] contentBytes = new byte[Con.Length];
            for (int i = 0; i < Con.Length; i++)
            {
                contentBytes[i] = (byte)Con[i];
            }
            return contentBytes;
        }
        public static string ConvertBytesToContent(byte[] bytes)
        {
            string con = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((char)bytes[i] != '\0')
                    con += (char)bytes[i];
                else
                    break;
            }
            return con;
        }
        public void writeFile()
        {
            byte[] byteContent = ConvertContentToBytes(content);
            List<byte[]> data = Virtual_Disk.splitToBlocksOfBytes(byteContent);
            int fc = 0, lc = -1;
            if (F_cluster != 0)
            {
                fc = F_cluster;
            }
            else
            {
                fc = FAT.get_Avaliable_Block_ID();
                F_cluster = fc;
            }
            for (int i = 0; i < data.Count; i++)
            {
                Virtual_Disk.write_block(data[i], fc);
                FAT.set_next(fc, -1);
                FAT.write_fat_table_in_file();
                if (lc != -1)
                {
                    FAT.set_next(lc, fc);
                }
                lc = fc;
                fc = FAT.get_Avaliable_Block_ID();
            }
            FAT.write_fat_table_in_file();
        }
        public void readFile()
        {
            if (F_cluster != 0)
            {
                content = string.Empty;
                int cluster = F_cluster;
                int next = FAT.getValue(cluster);
                List<byte> ls = new List<byte>();
                FAT.read_fat_table();
                do
                {
                    ls.AddRange(Virtual_Disk.get_block(cluster));
                    cluster = next;
                    if (cluster != -1)
                        next = FAT.getValue(cluster);
                }
                while (cluster != -1);
                content = ConvertBytesToContent(ls.ToArray());
            }
        }
        public void deleteFile()
        {
            if (F_cluster != 0)
            {
                int clusterIndex = F_cluster;
                int next = FAT.getValue(clusterIndex);
                if (clusterIndex == 5 && next == 0)
                    return;
                do
                {
                    FAT.set_next(clusterIndex, 0);
                    clusterIndex = next;
                    if (clusterIndex != -1)
                        next = FAT.getValue(clusterIndex);
                } while (clusterIndex != -1);
            }
            if (this.parent != null)
            {
                string dirName = new string(name);
                int index = parent.Search_Directory(dirName);
                if (index != -1)
                {
                    parent.dir.RemoveAt(index);
                    parent.write_Directory();
                    FAT.write_fat_table_in_file();
                }
            }
        }
    }
}