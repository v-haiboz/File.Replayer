namespace File.Replayer.Extractors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class ExtractorBase<T>:HashSet<T>
    {
        protected string filePath = null;
        public HashSet<T> Extract(string filePath)
        {
            this.filePath = filePath;
            Travel(filePath);
            return this;
        }

        protected virtual void Travel(string filePath)
        { 
           
        }

        protected static Encoding GetType(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var r = GetType(fs);
            fs.Close();
            return r;
        }

        protected static Encoding GetType(FileStream fs)
        {
            var reVal = Encoding.Default;
            var r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// check BOM  UTF8 format
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;  //Calculates the number of bytes that should remain in the character currently being analyzed
            byte curByte; //analysis current byte.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //If the first digit of the tag is not 0, it should start with at least two one, such as 110xxxxx..... 1111110x　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //If UTF-8, the first bit must be 1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("Unexpected byte format");
            }
            return true;
        }
    }
}
