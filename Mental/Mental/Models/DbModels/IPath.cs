using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models.DbModels
{
    public interface IPath
    {
        string GetDatabasePath(string filename);
    }
}
