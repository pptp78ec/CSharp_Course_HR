using System;
using System.IO;
using System.Linq;

namespace DataClassModel
{
    public static class PhotoWorx
    {
        private static Photos GetPhoto(string _filename, int _emplId)
        {
            byte[] imageData = null;
            FileInfo fInfo = new FileInfo(_filename);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(_filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            imageData = br.ReadBytes((int)numBytes);
            string imageExtension = (Path.GetExtension(_filename)).Replace(".", "").ToLower();
            return new Photos() { EmployeeId = _emplId, EmployeePhoto = imageData, ImageFormat = imageExtension };
        }
        public static void SavePhotoToDB(string _filename, HRWorkEntities _context, int _emplId)
        {

                try
                {
                    var emplPhoto = _context.Photos.FirstOrDefault(p => p.EmployeeId == _emplId);
                    if (emplPhoto == null)
                    {
                        _context.Photos.Add(GetPhoto(_filename, _emplId));
                    }
                    else
                    {
                        var newPhoto = GetPhoto(_filename, _emplId);
                        emplPhoto.ImageFormat = newPhoto.ImageFormat;
                        emplPhoto.EmployeePhoto = newPhoto.EmployeePhoto;
                    }
                }
                catch (Exception) { }
        }
        public static Uri LoadPhotoFromDB(HRWorkEntities _context, int _emplId)
        {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo("systemdata");
                    if (!dir.Exists) { dir.Create(); }
                    Photos photoInDB = _context.Photos.FirstOrDefault(x => x.EmployeeId == _emplId);
                    if (photoInDB != null)
                    {
                        string filename = dir.FullName + "/usrphoto." + photoInDB.ImageFormat;
                        File.WriteAllBytes(filename, photoInDB.EmployeePhoto);
                        return new Uri(filename);
                    }
                    else return null;
                }
                catch (Exception) { return null; }

        }
    }
}
