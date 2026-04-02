using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    public class FileOperation
    {
        public static byte[] SafeReadAllBytes(string inFile)
        {
            try
            {
                if (string.IsNullOrEmpty(inFile))
                {
                    return null;
                }

                if (!File.Exists(inFile))
                {
                    return null;
                }

                File.SetAttributes(inFile, FileAttributes.Normal);
                return Converter.GetBytesByString(File.ReadAllText(inFile));
            }
            catch (GameException ex)
            {
                Log.Error(string.Format("SafeReadAllBytes failed! path = {0} with err = {1}", inFile, ex.Message));
                return null;
            }
        }
    }
}


