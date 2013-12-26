using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace WallpaperChange
{
    class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 
            {
                x.Service<WallpaperChanger>(s =>                        
                {
                    s.ConstructUsing(name => new WallpaperChanger());   
                    s.WhenStarted(wc => wc.Start());             
                    s.WhenStopped(wc => wc.Stop());              
                });
                x.RunAsLocalSystem();                            

                x.SetDescription("Changes the 8-bit wallpaper");        
                x.SetDisplayName("8-bit wallpaper changer");                       
                x.SetServiceName("WallpaperChanger");                       
            });                           
        }
    }
}
