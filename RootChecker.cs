using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MS
{
    #if UNITY_ANDROID
    public class RootChecker
    {
        public static bool isRooted()
        {
            return findBinary("su") || findBinary("busybox")|| findBinary("magisk");
        }
        static bool findBinary(string binaryName) 
        {
            bool found = false;
            if (!found) {
                string[] places = { 
                "/data/local/",
                "/data/local/bin/",
                "/data/local/xbin/",
                "/sbin/",
                "/su/bin/",
                "/system/bin/",
                "/system/bin/.ext/",
                "/system/bin/failsafe/",
                "/system/sd/xbin/",
                "/system/usr/we-need-root/",
                "/system/xbin/",
                "/cache/",
                "/data/",
                "/dev/" };
                foreach (string where in places) {
                    if (System.IO.File.Exists(where + binaryName)) {
                        found = true;

                        break;
                    }
                }
            }
            return found;
        }
    }
    #endif
}
