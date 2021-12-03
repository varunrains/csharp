using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VirusScanApi.Services
{
    public class FileSystemService
    {
        /// <summary>
        /// Checks that the path doesn't exist and if so, creates it.
        /// If the path does exist, no changes are made.
        /// </summary>
        /// <param name="path">The path to the directory to create</param>
        public virtual void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Checks that the path exists and if so, deletes it.
        /// If the path does not exist, no changes are made.
        /// </summary>
        /// <param name="path">The path to the directory to delete</param>
        public virtual void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Saves the data from an <see cref="IFormFile"/> to the specified path.
        /// </summary>
        /// <param name="file">The <see cref="IFormFile"/> to save</param>
        /// <param name="filePath">The path to the file save location</param>
        /// <returns></returns>
        public virtual async Task SaveFile(IFormFile file, string filePath)
        {
            if (file == null)
            {
                return;
            }
            using var stream = new FileStream(filePath, FileMode.CreateNew);
            await file.CopyToAsync(stream).ConfigureAwait(false);
        }

        /// <summary>
        /// Tries to delete a file. Will not fail if the file cannot be deleted. If the delete fails,
        /// may retry in case it was a temporary lock on the file.
        /// </summary>
        /// <param name="filePath"></param>
        public virtual bool TryDeleteFile(string filePath, int retries = 1) { 
            if (retries <= 0) return false;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch (IOException) { 
                retries -= 1;
                System.Threading.Thread.Sleep(50); //Give a slight gap in case something else is using the file
                return TryDeleteFile(filePath, retries);
            }
        }
    }
}
