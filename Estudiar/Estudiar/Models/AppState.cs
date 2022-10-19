using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Estudiar.Models
{
    public static class AppState<T>
    {
        
        public static void SerializeAndSave(T CurrentInfo, string DbName)//Convert C# model to json
        {
            
            var json = JsonConvert.SerializeObject(CurrentInfo);
            
            //Xamarin.Forms.Application.Current.Properties[DbName] = json;
            Preferences.Set(DbName, json);
            //await Estudiar.DataMgmt.DB.AddModel(DbName,json);
        }
        public static T DeserializeAndRestore(string DbName)//Convert Json to c# model
        {
            try
            {
                //string json = Xamarin.Forms.Application.Current.Properties[DbName].ToString();
                string json = Preferences.Get(DbName, "");
                if (json == "")
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(json);
                //var obtmodels = await Estudiar.DataMgmt.DB.GetModels();
                /*foreach (var item in obtmodels)
                {
                    if (item.key == DbName)
                    {
                        string json = item.value;
                        var taskModels = JsonConvert.DeserializeObject<T>(json);
                        return taskModels;
                    }
                }*/
                //return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
