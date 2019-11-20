using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Foundation;
using Mental.iOS;
using Mental.Models.DbModels;
using UIKit;

[assembly: Dependency(typeof(IosDbPath))]
namespace Mental.iOS
{
    public class IosDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", filename);
        }
    }
}