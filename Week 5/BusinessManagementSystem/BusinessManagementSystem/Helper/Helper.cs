using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace BusinessManagementSystem.Helper
{
    public static class Helpers
    {
        
        public static Guid GenerateGUID()
        {
            Guid obj = Guid.NewGuid();
            return obj;
        }
        public static int GetCurrentAge(this string date)
        {
            DateTime dateTimeOffset=Convert.ToDateTime(date);
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateTimeOffset.Year;
            if (currentDate < dateTimeOffset.AddYears(age))
            {
                age--;
            }
            return age;
        }
        public static HashDto GetHashPassword(string plainPassword)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            //using (var rngCsp = RandomNumberGenerator.Create())
            //{
            //    rngCsp.GetNonZeroBytes(salt);
            //}



            string saltString = Convert.ToBase64String(salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: plainPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            HashDto hashDto = new()
            {
                Hash = hashed,
                Salt = saltString
            };
            return hashDto;
        }
        public static string VerifyHashPassword(string plainPassword,string saltString)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: plainPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }
        public static string DocUpload(IFormFile file, string documentType, string username = "", string nameOfFile = "")
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            string documentRootPath = "";
            string fileName = "";
            string returnString="";
            var extension = Path.GetExtension(file.FileName);

            if (documentType == "ProfilePicture")
            {
                fileName =file.FileName;
                documentRootPath = Path.GetFullPath(Path.Combine(new string[]{Environment.CurrentDirectory, "wwwroot","image", "ProfilePicture"}));
            }
            else
            {
                fileName = Helpers.GenerateGUID() + extension;
                documentRootPath = Path.GetFullPath(Path.Combine(new string[] {Environment.CurrentDirectory, "wwwroot", "image", "EmployeeRelatedDoc", username, documentType }));
                //documentRootPath = Path.Combine(webRootPath, @$"Uploads\EmployeeRelatedDoc\{username}\{documentType}");
            }


            if (!Directory.Exists(documentRootPath))
            {
                Directory.CreateDirectory(documentRootPath);
            }

            

            var fullPath = Path.Combine(documentRootPath, fileName);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            logger.Info("## DocumentPath: " + documentRootPath);
            logger.Info("## FullPath: " + fullPath);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                returnString= fullPath.Split("wwwroot/")[1];
            }
            else
            {
                returnString = fullPath.Split("wwwroot\\")[1];
            }
            return returnString;
        }
        public static bool ValidateDocumentUpload(IFormFile file)
        {
            if (file == null)
                return true;
            else
            {
                string[] validExcel = { ".pdf",".jpeg",".jpg",".png" };
                var extension = Path.GetExtension(file.FileName);
                return validExcel.Contains(extension);
            }
        }
        public static MemoryStream DownloadExcelIntoDifferentSheet(List<string[]> batchForPrint, List<string[]> batchAll)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package=new ExcelPackage())
            {
                LoadDataIntoSheet(package.Workbook.Worksheets.Add("BatchForPrint"), batchForPrint);
                LoadDataIntoSheet(package.Workbook.Worksheets.Add("BatchAll"), batchAll);
                //LoadDataIntoSheet(package.Workbook.Worksheets.Add("Bank"), bankList);

                var stream=new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }
        public static MemoryStream DownloadExcel(string filePath, List<string[]> userList)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                LoadDataIntoSheet(package.Workbook.Worksheets.Add("EmployeeDetail"), userList);

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }
        private static void LoadDataIntoSheet(ExcelWorksheet sheet, List<string[]> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    sheet.Cells[i + 1, j + 1].Value = data[i][j];
                }
            }
        }
    }
}
