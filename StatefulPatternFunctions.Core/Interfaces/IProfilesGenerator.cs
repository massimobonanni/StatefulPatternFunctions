using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Core.Interfaces
{
    public interface IProfilesGenerator
    {
        Task GenerateProfilesAsync(int numberOfProfiles, CancellationToken token);
    }
}
