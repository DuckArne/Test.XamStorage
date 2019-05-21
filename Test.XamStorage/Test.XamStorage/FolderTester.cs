using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using XamStorage;

namespace Test.XamStorage
{
    public class FolderTester
    {
        const string TempFileName = "TestFile1";
        const string TempDirectoryName = "TestDirectory1";

        /// <summary>
        /// What the temporary generated file should end with defaults to .txt
        /// </summary>
        public string FileEnding { get; set; } = ".txt";
        /// <summary>
        /// The directory that should be tested
        /// </summary>
        public IFolder Directory { get; set; }
        /// <summary>
        /// Options to test. For now only read and write
        /// </summary>
        public TestOption Option { get; set; }
       /// <summary>
       /// If test returned false check this for errormessage.
       /// </summary>
        public string ErrorMessage { get; set; }

        public FolderTester()
        {          
        }

        public FolderTester(IFolder folder)
        {
            Directory = folder;
        }


        /// <summary>
        /// 1. Requests given permissions.
        /// 2. Checks that Directory exists. 
        /// 3. Creates a file in given directory,
        /// 4. Saves text in that file and gives it the given ending default is .txt closes the file,
        /// 5. Creates another directory inside of given folder,
        /// 6. Gets the temporary file from given directory,
        /// 7. Moves that file to the other created directory,
        /// 8. Deletes temporary file,
        /// 9. Deletes the other directory
        /// </summary>
        /// <returns>true if success, false otherwise. Check ErrorMessage if failure</returns>
        async public Task<bool> RunTest()
        {
            if (Directory == null)
                throw new Exception("Directory Property is Null");

            switch (Option)
            {
                case TestOption.ReadAndWrite:
                    return await ReadAndWriteTest();
                case TestOption.Read:
                    return await ReadTest();
                case TestOption.Write:
                    return await WriteTest();
                default:
                    throw new Exception("This should never happen.");
            }          
        }     

        async private Task<bool> ReadAndWriteTest()
        {

            try
            {
                var file = await Directory.CreateFileAsync(TempFileName + FileEnding, CreationCollisionOption.FailIfExists);
                await file.WriteAllTextAsync("Hello World!!");

                var newFolder = await Directory.CreateFolderAsync(TempDirectoryName, CreationCollisionOption.FailIfExists);

                var oldFile = await Directory.GetFileAsync(TempFileName + FileEnding);

                await oldFile.MoveAsync(Path.Combine(newFolder.Path,TempFileName + FileEnding), newFolder, NameCollisionOption.FailIfExists);

                var movedFile = await newFolder.GetFileAsync(TempFileName + FileEnding);
                await movedFile.DeleteAsync();

                await newFolder.DeleteAsync();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }

            ErrorMessage = null;
            return true;
        }


        private Task<bool> WriteTest()
        {
            throw new NotImplementedException();
        }

        private Task<bool> ReadTest()
        {
            throw new NotImplementedException();
        }
    }
}
