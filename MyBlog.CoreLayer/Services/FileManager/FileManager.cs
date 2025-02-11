using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace MyBlog.CoreLayer.Services.FileManager
{
    public class FileManager : IFileManager
    {
        public void DeleteFile(string fileName, string path)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(path))
                return;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    // در صورت نیاز می‌توانید Exception را log کنید
                    throw new Exception("خطا در حذف فایل", ex);
                }
            }
        }

        public string SaveFileAndReturnName(IFormFile file, string savePath)
        {
            if (file == null)
                throw new Exception("فایل ارسال نشده است");

            // گرفتن پسوند فایل و ساخت نام فایل امن
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), savePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fullPath = Path.Combine(folderPath, fileName);

            try
            {
                using var stream = new FileStream(fullPath, FileMode.Create);
                file.CopyTo(stream); // در صورت نیاز به نسخه Async: await file.CopyToAsync(stream);
            }
            catch (Exception ex)
            {
                // در صورت نیاز Exception را log کنید
                throw new Exception("خطا در ذخیره فایل", ex);
            }
            return fileName;
        }
    }
}
