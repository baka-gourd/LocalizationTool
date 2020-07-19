using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LocalizationTool
{
    class Process
    {
        public static JObject ParseLangFile(string path)
        {
            var keyReg = new Regex(".+(?==)");
            var nameReg = new Regex("(?<==).+");
            var findEqual = new Regex("=+");
            var findComment1 = new Regex("\n*\r");
            var findComment2 = new Regex("//(.*)");
            var findComment3 = new Regex("#(.*)");
            var findComment4 = new Regex("^( \\*)");
            var findComment5 = new Regex("^(/\\*)");
            var langJObject = new JObject();
            foreach (string str in System.IO.File.ReadAllLines(path, Encoding.UTF8))
            {
                if (!findEqual.IsMatch(str))
                    continue;
                if (findComment1.IsMatch(str))
                    continue;
                if (findComment2.IsMatch(str))
                    continue;
                if (findComment3.IsMatch(str))
                    continue;
                if (findComment4.IsMatch(str))
                    continue;
                if (findComment5.IsMatch(str))
                    continue;
                var key = keyReg.Match(str).ToString();
                var name = nameReg.Match(str).ToString();
                if (key == "" && name == "")
                    continue;
                if (!langJObject.TryGetValue(key, out _))
                {
                    langJObject.Add(key, name);
                }
            }
            return langJObject;
        }

        public static List<string> ParseJsonFile(string path)
        {
            var jFile = System.IO.File.OpenText(path);
            var reader = new JsonTextReader(jFile);
            var jObj = (JObject)JToken.ReadFrom(reader);
            List<string> langList = new List<string>();
            foreach (var jValue in jObj)
            {
                langList.Add(jValue.Key + "=" + jValue.Value);
            }

            return langList;
        }
    }
}
