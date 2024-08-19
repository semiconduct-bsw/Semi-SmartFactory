namespace _20240819_BinaryReaderWriter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Temp\pic1.png";
            byte[] picture;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                // 사진의 Binary 값 - 직렬화 되어있는 값으로 읽어오기
                picture = br.ReadBytes((int)fs.Length);
                br.Close();
            } // 사진 파일 읽어오기 -> 메모리로 가져왔음 (byte[] picture)

            // pic2.png로 Write 해보기
            string path2 = @"C:\Temp\pic2.png";
            using (FileStream fs = new FileStream(path2, FileMode.Create))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(picture);
                bw.Flush(); // 파일을 비우는 과정
                bw.Close();
            }
        }
    }
}
