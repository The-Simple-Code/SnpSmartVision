using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;

namespace SnpSmartVision
{
    class AlgorithmStopWatchEventArgs : EventArgs
    {
        public string cameraName=null;
        public AlgorithmStopWatchEventArgs()
        {
        }
    }
    //[Synchronization]
    class AlgorithmStopWatch
    {
        //public TimeTableForm timeTable;
       
        public bool enabled = false;
        bool ready = false;
        string targetCamera;
        Stopwatch stopWatch;
        public Dictionary<string, double> Record;

        ListView lv;

        public AlgorithmStopWatch()
        {
            stopWatch = new Stopwatch();
            stopWatch.Reset();
            stopWatch.Stop();
            Record = new Dictionary<string, double>();
            Record.Clear();
        }
     
        public void ReadyRecord(string targetCamera)
        {
            //if (stopWatch.IsRunning == true) return;
            this.targetCamera = targetCamera;
            Record.Clear();
            ready = true;
        }
        public void StartRecord(string targetCamera)
        {
            if (ready == false) return;
            if (this.targetCamera == targetCamera)
            {
                stopWatch.Reset();
                stopWatch.Start();
                ready = false;
                enabled = true;
            }
        }
        public void StopRecord()
        {
            //if (enabled == true)
            //{
                stopWatch.Stop();
                enabled = false;
                ready = false;
            //}
        }

        public void StopRecord(ListView lv)
        {
            if(enabled==false) return;
             this.lv = lv;
             stopWatch.Stop();
             enabled = false;
        }
 
        public void GetTime(string tag)
        {
            //if (enabled == false) return;
            if (Record.ContainsKey(tag) == true) Record[tag] = stopWatch.ElapsedMilliseconds;
            else Record.Add(tag, stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();
        }
    }
}
