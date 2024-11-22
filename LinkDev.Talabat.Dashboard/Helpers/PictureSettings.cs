namespace LinkDev.Talabat.Dashboard.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. get folder path
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images",folderName);

            // 2. set filename unique
            var fileName = Guid.NewGuid() + file.FileName;

            // 3. get file path
            var filePath = Path.Combine(folderPath, fileName);

            // 4. save file as stream
            var fs = new FileStream(filePath, FileMode.Create);

            // 5. copy my file into streams
            file.CopyTo(fs);

            // 6. return filename
            return Path.Combine( "images\\products", fileName);
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);
            if (File.Exists(filePath)) {
            File.Delete(filePath);
            }
        }
    }
}
