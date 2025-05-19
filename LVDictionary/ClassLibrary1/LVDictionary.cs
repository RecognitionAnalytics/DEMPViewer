using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClassLibrary1
{

    public class FileList
    {
        private List<string> Files = new List<string>();
        public void AddFile(string file)
        {
            Files.Add(file);
        }

        public FileList()
        {

        }

        public FileList(string[] existingFiles)
        {
            Files.AddRange(existingFiles);
        }

        public string[] ExistingFiles()
        {
            return Files.ToArray();
        }

        public string[] FilterForNew(string[] fileList)
        {
            return fileList.Where(x => Files.Contains(x) == false).ToArray();
        }

        public bool HasMatch(string file)
        {
            return  Files.Contains(file) ;
        }
    }


    public class LVWaveFunction
    {
        public List<double> Data { get; set; }
        public double dt { get; set; }
        public double y0 { get; set; }
    }

    public class KeyboardHelps
    {
        public void Copy(string data)
        {
             Clipboard.SetText(data);
        }
    }

    public class LVDictionary
    {

        public void Clear()
        {
            Dictionary.Clear();
        }

        public Dictionary<string, Dictionary<string, LVWaveFunction>> Dictionary { get; set; } = new Dictionary<string, Dictionary<string, LVWaveFunction>>();
        public void AddReplace(string key, string channel, double[] value, double dt, double y0)
        {
            if (Dictionary.ContainsKey(key))
            {

                var channelDict = Dictionary[key];
                if (channelDict.ContainsKey(channel))
                {
                    channelDict[channel].Data.AddRange(value);
                }
                else
                    channelDict.Add(channel, new LVWaveFunction
                    {
                        Data = value.ToList(),
                        dt = dt,
                        y0 = y0,
                    });
            }
            else
            {
                var channelDict = new Dictionary<string, LVWaveFunction>();
                channelDict.Add(channel, new LVWaveFunction
                {
                    Data = value.ToList(),
                    dt = dt,
                    y0 = y0,
                });
                Dictionary.Add(key, channelDict);
            }
        }




        public double[] GetTimes(string key, string channel)
        {
            var channelInfo = Dictionary[key][channel];
            var times = new double[channelInfo.Data.Count];
            for (int i = 0; i < times.Length; i++)
                times[i] = channelInfo.y0 + i * channelInfo.dt;
            return times;
        }

        public LVWaveFunction GetItem(string key, string channel)
        {
            return Dictionary[key][channel];
        }

        public LVWaveFunction GetSafe(string key, string channel)
        {
            if (Dictionary.ContainsKey(key))
                return Dictionary[key][channel];
            else
                return null;
        }

        public double[] GetData(string key, string channel)
        {
            if (Dictionary.ContainsKey(key))
                return Dictionary[key][channel].Data.ToArray();
            else
                return null;
        }

        public string[] Keys()
        {
            return Dictionary.Keys.ToArray();
        }

        public string[] Channels()
        {
            return Dictionary[Dictionary.Keys.ToArray()[0]].Keys.OrderBy(x=>x).ToArray();
        }


        public bool isNull(object Object)
        {
            return Object == null;
        }

    }

    public class TemplateDictionary<ValueType>
    {
        public Dictionary<string, ValueType> Dictionary { get; set; }
        public void AddReplace(string key, ValueType value)
        {
            if (Dictionary.ContainsKey(key))
                Dictionary.Remove(key);
            Dictionary.Add(key, value);
        }
        public ValueType GetItem(string key)
        {
            return Dictionary[key];
        }
    }
}
