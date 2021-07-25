using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CommonV5
{
    public class ConfigHelper
    {
        static string configFileName = "CustomizedConfig.config";

        /// <summary>
        /// 查
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            try
            {
                ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = configFileName
                };
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
                string value = configuration.AppSettings.Settings[key].Value;
                Log.Information($"读取{key}:{value}-ok");
                return value;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// 增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool AddOrSetConfig(string key,string value)
        {
            try
            {
                ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = configFileName
                };
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
                // 查看是否已存在该key
                if (configuration.AppSettings.Settings.AllKeys.Contains(key))
                {
                    configuration.AppSettings.Settings[key].Value = value;
                    Log.Information($"修改{key}:{value}-ok");
                }
                else
                {
                    configuration.AppSettings.Settings.Add(key, value);
                    Log.Information($"新增{key}:{value}-ok");
                }
                configuration.Save();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
            
        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetConfig(string key, string value)
        {
            try
            {
                ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = configFileName
                };
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
                configuration.AppSettings.Settings[key].Value = value;
                configuration.Save();
                Log.Information($"修改{key}:{value}-ok");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
            
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DelConfig(string key)
        {
            try
            {
                ExeConfigurationFileMap exeConfigurationFileMap = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = configFileName
                };
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
                configuration.AppSettings.Settings.Remove(key);
                configuration.Save();
                Log.Information($"删除{key}-ok");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
            
        }





    }
}
