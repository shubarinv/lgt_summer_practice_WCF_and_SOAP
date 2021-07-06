using System;
using System.IO;
using FileTrainsferFramework.Server.ClientFileTransferServiceReference;

namespace FileTrainsferFramework.Server
{
    /// <summary>
    /// Program File Transfer
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main Program 
        /// </summary>
        /// <param name="args">args of the file</param>
        static void Main(string[] args)
        {
            try
            {
                var fileWatcher =
                    new FileSystemWatcher(System.Configuration.ConfigurationManager.AppSettings["FolderToWatch"]);
                fileWatcher.Created += FileWatcher_Created;
                fileWatcher.EnableRaisingEvents = true;
                Console.WriteLine("Watcher Started...");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                new Logger().Create("Service not started", DateTime.Now, "Error", ex.Message);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// File Watcher when Created
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">e object</param>
        private static void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            FileTransferResponse response = null;
            try
            {
                if (IsFileLocked(e.FullPath) == false)
                {
                    DateTime startAt = DateTime.Now;
                    FileTransferRequest createdFile = new FileTransferRequest()
                    {
                        FileName = e.Name,
                        Content = File.ReadAllBytes(e.FullPath)
                    };

                    response = new FileTransferClient().Put(createdFile);

                    if (response.ResponseStatus != "Successful")
                    {
                        MoveToFailedFolder(e);
                    }
                    else
                    {
                        if (File.Exists(e.FullPath))
                        {
                            File.Delete(e.FullPath);
                        }
                    }

                    Console.WriteLine(response.ResponseStatus + " at: " + DateTime.Now.Subtract(startAt).ToString());
                    new Logger().Create(e.Name, DateTime.Now, response.ResponseStatus, response.Message);
                }
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                MoveToFailedFolder(e);
                Console.WriteLine(ex.Message);
                new Logger().Create(e.Name, DateTime.Now, "Error", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (response != null)
                {
                    new Logger().Create(e.Name, DateTime.Now, response.ResponseStatus, response.Message);
                }
                else
                {
                    new Logger().Create(e.Name, DateTime.Now, "Error", ex.Message);
                }
            }
        }

        /// <summary>
        /// Move File to Failed Folder
        /// </summary>
        /// <param name="e">e file system event args</param>
        private static void MoveToFailedFolder(FileSystemEventArgs e)
        {
            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["FolderToWatch"] +
                            "\\failed\\" + e.Name))
            {
                File.Delete(System.Configuration.ConfigurationManager.AppSettings["FolderToWatch"] +
                            "\\failed\\" + e.Name);
            }

            File.Move(e.FullPath,
                System.Configuration.ConfigurationManager.AppSettings["FolderToWatch"] + "\\failed\\" +
                e.Name);
        }

        /// <summary>
        /// Check if file is locked
        /// </summary>
        /// <param name="filePath">file you want to check</param>
        /// <returns>true if file is locked</returns>
        private static bool IsFileLocked(string filePath)
        {
            try
            {
                var numberOfTying = 1;
                while (numberOfTying <= 10)
                {
                    if (File.Exists(filePath))
                    {
                        FileStream stream = null;
                        try
                        {
                            Console.WriteLine("Try reading the: " + Path.GetFileName(filePath));
                            stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                            Console.WriteLine(" reading File succeeded");
                            return false;
                        }
                        catch
                        {
                            Console.WriteLine(" Wait for 1s ");
                            System.Threading.Thread.Sleep(1000);
                        }
                        finally
                        {
                            if (stream != null)
                            {
                                stream.Close();
                            }
                        }
                    }

                    numberOfTying++;
                }

                return true;
            }
            catch (Exception ex)
            {
                new Logger().Create(filePath, DateTime.Now, "Error", ex.Message);
                return true;
            }
        }
    }
}