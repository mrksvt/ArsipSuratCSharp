using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ArsipSurat
{
    public static class ScannerHelper
    {
        public static string ScanToFile()
        {
            dynamic WiaDialog = null;
            dynamic WiaDevManager = null;
            dynamic device = null;
            dynamic item = null;
            dynamic image = null;
            
            try
            {
                WiaDialog = Activator.CreateInstance(Type.GetTypeFromProgID("WIA.CommonDialog"));
                WiaDevManager = Activator.CreateInstance(Type.GetTypeFromProgID("WIA.DeviceManager"));
                
                device = WiaDialog.ShowSelectDevice(1, false, false);
                if (device == null) return null;
                
                item = device.Items[1];
                image = WiaDialog.ShowAcquireImage(device, item);
                
                if (image == null) return null;
                
                string tempPath = Path.Combine(Path.GetTempPath(), string.Format("scan_{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmss")));
                image.SaveFile(tempPath);
                
                return tempPath;
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2145320928)
                {
                    return null;
                }
                throw new Exception("Scanner error: " + ex.Message);
            }
            finally
            {
                if (image != null) try { Marshal.ReleaseComObject(image); } catch { }
                if (item != null) try { Marshal.ReleaseComObject(item); } catch { }
                if (device != null) try { Marshal.ReleaseComObject(device); } catch { }
                if (WiaDevManager != null) try { Marshal.ReleaseComObject(WiaDevManager); } catch { }
                if (WiaDialog != null) try { Marshal.ReleaseComObject(WiaDialog); } catch { }
            }
        }

        public static bool IsScannerAvailable()
        {
            try
            {
                dynamic devManager = Activator.CreateInstance(Type.GetTypeFromProgID("WIA.DeviceManager"));
                dynamic devices = devManager.DeviceInfos;
                return devices.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
