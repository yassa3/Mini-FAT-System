namespace AKVLNADKLNV
{
	class Virtual_Disk
	{
		static private string path =
			"F:\\OS_Project_final\\OS-PROJECT\\V_disk.txt";
		static public void initializeVdisk()
		{
			FileInfo V_disk_txt = new FileInfo(path);
			if (!V_disk_txt.Exists)
			{
				StreamWriter write = new StreamWriter(path);
				for (int i = 0; i < 1024; i++)
					write.Write('*');
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 1024; j++)
					{
						write.Write('0');
					}
				}
				for (int i = 0; i < 1019; i++)
				{
					for (int j = 0; j < 1024; j++)
						write.Write('#');
				}
				write.Close();
				FAT.initialize_fat_table();
				FAT.set_next(5, -1);
				FAT.write_fat_table_in_file();
				Directory root = new Directory("H:", 1, 5,0, null);
				Program.current_directory = root;
				Program.current_path = root.name;
				root.write_Directory();			
			}
			else
            {
				Directory root = new Directory("H:", 1, 5,0, null);
				FAT.fat_table = FAT.read_fat_table();
				FAT.set_next(5, -1);
				Program.current_directory=root;
				Program.current_path = root.name;
				FAT.write_fat_table_in_file();
				root.Read_Directory();
			}
		}
		public static List<byte[]> splitToBlocksOfBytes(byte[] bytes)
		{
			List<byte[]> ls = new List<byte[]>();

			if (bytes.Length > 0)
			{
				int number_of_arrays = bytes.Length / 1024;
				int rem = bytes.Length % 1024;

				for (int i = 0; i < number_of_arrays; i++)
				{
					byte[] b = new byte[1024];
					for (int j = i * 1024, k = 0; k < 1024; j++, k++)
					{
						b[k] = bytes[j];
					}
					ls.Add(b);
				}
				if (rem > 0)
				{
					byte[] b1 = new byte[1024];
					for (int i = number_of_arrays * 1024, k = 0; k < rem; i++, k++)
					{
						b1[k] = bytes[i];
					}
					ls.Add(b1);
				}
			}
			else
			{
				byte[] b1 = new byte[1024];
				ls.Add(b1);
			}
			return ls;
		}
		public static void write_block(byte[] data, int index)
		{
			using (FileStream write = new FileStream(path, FileMode.Open,FileAccess.ReadWrite))
			{
				write.Seek(1024 * index, SeekOrigin.Begin);
				for (int idx = 0; idx < data.Length; idx++)
				{
					write.WriteByte(data[idx]);
				}
				write.Close();
			}
			
		}
		public static byte[] get_block(int idx)
		{
			byte[] block = new byte[1024];
			using (FileStream read = new FileStream(path, FileMode.Open,FileAccess.ReadWrite))
			{
				read.Seek(idx * 1024, SeekOrigin.Begin);
				read.Read(block, 0, block.Length);
				read.Close();
			}
			return block;
		}
	}
}
