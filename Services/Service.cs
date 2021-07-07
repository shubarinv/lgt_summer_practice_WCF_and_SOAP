﻿using System;
using System.IO;
using System.ServiceModel;

namespace Service
{
    [ServiceBehavior]
    public class StreamService : IStream
    {
        public Stream GetLargeObject()
        {
            // Add path to a big file, this one is 2.5 gb
            string filePath = Path.Combine(Environment.CurrentDirectory, "transfer/test_file.txt");

            try
            {
                FileStream imageFile = File.OpenRead(filePath);
                return imageFile;
            }
            catch (IOException ex)
            {
                Console.WriteLine(String.Format("An exception was thrown while trying to open file {0}", filePath));
                Console.WriteLine("Exception is: ");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}