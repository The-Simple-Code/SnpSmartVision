using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

using WeifenLuo.WinFormsUI.Docking;

using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision.Acquisition.Imaqdx;
using NationalInstruments.Vision.Analysis;

using SnpSystem.Vision.Acquisition;
using SnpSystem.Vision.CameraTrigger;
using SnpSystem.Vision.VisionConfigurationHelper;
using SnpSystem.Vision.Etc;
using SnpSystem.Vision.Viewer;
using SnpSystem.Vision.Parameters;

namespace SnpSmartVision
{
    class MakingTreeList
    {
        public TreeView treeView;
        public MakingTreeList(TreeView treeView)
        {
           this.treeView = treeView;
        }
        public void SetTree(string trees,object ob)
        {
             string[] split = new string[] {"::"};
             string[] tree = trees.Split(split,StringSplitOptions.None);
  
            TreeNode[] matches = treeView.Nodes.Find(tree[0], true);
            TreeNode currentNode = matches.Length == 0 ? treeView.Nodes.Add(tree[0], tree[0]) : matches[0];
            for (int i = 1; i < tree.Length; ++i)
            {
                matches = currentNode.Nodes.Find(tree[i], true);
                {
                    if (matches.Length == 0)
                    {
                        currentNode = currentNode.Nodes.Add(tree[i], tree[i]);
                        if (i == tree.Length - 1)  currentNode.Tag=ob;
                    }
                    else currentNode = matches[0];
                }
            }
        }

        public void RemoveSelectedNode()
        {
            treeView.Nodes.Remove(treeView.SelectedNode);
        }
        public string ExtractPath(string path)
        {
            string[] split = new string[] { "\\" };
            string[] tree = path.Split(split, StringSplitOptions.None);
            if (tree.Length < 3) return "";
            return tree[0] + "::" + tree[1] + "::" + tree[2];
        }

        public string[] ExtractPaths(string path)
        {
            string[] split = new string[] { "\\" };
            return path.Split(split, StringSplitOptions.None);
        }

        public string ReplaceSeperator(string path)
        {
            return path.Replace("\\", "::");
        }

        public void RemoveTree(string trees)
        {
            string[] split = new string[] { "::" };
            string[] tree = trees.Split(split, StringSplitOptions.None);
            int count = tree.Length;
            
            TreeNode[] matches = treeView.Nodes.Find(tree[count-1], true);
            if (matches.Length == 0) return;
            else treeView.Nodes.Remove(matches[0]);
        }


        public void UpdateCamTree(string path, int parameterCount, ImageParameterFile parameterFile)
        {
            if (path == null) return;
            string[] tree = ExtractPaths(path);
            RemoveTree(tree[0] + "::" + tree[1]);
            SetSelectedImageParameterFile(tree[1], parameterCount, parameterFile);

        }
        public void SetImageParameterFile(int count,ImageParameterFile file)
        {
            foreach (string cam in file.CameraList)
            {
                if (cam == "" || cam == null) continue; 
                ImageProcessingParameters[] paras = new ImageProcessingParameters[3];
                paras = file.ProcessingParameter[cam];

                SetRoiData(VisionFilePath.Process + cam + "::Roi", file.roiData.GetContainedData(cam));
                SetExtraProcessingdParameter(VisionFilePath.Process + cam + "::Extra Processing Parameter", file.ExtraData[cam]);
                
                int i = 1;
                foreach (ImageProcessingParameters para in paras)
                {
                    string treeName = VisionFilePath.Process+cam + "::Image Processing Parameter" + i.ToString();
                    SetColorThresholdParameter(treeName, para.colorParameter);
                    SetMedianFilterParameter(treeName, para.medianFilterParameter);
                    SetRemoveParticleParameter(treeName,para.removeParticleParameter);
                    SetParticleFilterOptionParameter(treeName,para.particleFilterParameters.option);
                    SetParticleFilterCriteriaParameters(treeName,para.particleFilterParameters.criteria);
                    SetParticleReportParameters(treeName,para.particleReportParameters);
                    SetDisplayColor(treeName,para.displayColorParameters);
                    i++;
                    if (i > count) break;
                }
            }
        }

        public void RemoveSelectedImageParameterFile(string cameraName)
        {
            string tree = VisionFilePath.Process + cameraName;
            RemoveTree(tree);
        }

        public void SetSelectedImageParameterFile(string cameraName,int count, ImageParameterFile file)
        {
            foreach (string cam in file.CameraList)
            {
                if (cam == "" || cam == null) continue;
                if (cam != cameraName) continue;

                ImageProcessingParameters[] paras = new ImageProcessingParameters[3];
                paras = file.ProcessingParameter[cam];

                if(file.roiData!=null) SetRoiData(VisionFilePath.Process + cam + "::Roi", file.roiData.GetContainedData(cam));
                if(file.ExtraData!=null) SetExtraProcessingdParameter(VisionFilePath.Process + cam + "::Extra Processing Parameter", file.ExtraData[cam]);
                if (file.CalibrationFactor == null)
                {
                    file.CalibrationFactor = new Dictionary<string, double>();
                    file.CalibrationFactor.Add(cam, 1.0);
                }
                SetCalibrationFactor(VisionFilePath.Process + cam + "::Calibration Factor", file.CalibrationFactor[cam]);
                int i = 1;
                foreach (ImageProcessingParameters para in paras)
                {
                    string treeName = VisionFilePath.Process + cam + "::Image Processing Parameter" + i.ToString();
                    SetColorThresholdParameter(treeName, para.colorParameter);
                    SetMedianFilterParameter(treeName, para.medianFilterParameter);
                    SetRemoveParticleParameter(treeName, para.removeParticleParameter);
                    SetParticleFilterOptionParameter(treeName, para.particleFilterParameters.option);
                    SetParticleFilterCriteriaParameters(treeName, para.particleFilterParameters.criteria);
                    SetParticleReportParameters(treeName, para.particleReportParameters);
                    SetDisplayColor(treeName, para.displayColorParameters);
                    i++;
                    if (i > count) break;
                }
            }
        }


        public void SetRootConfigFile(string tree, RootConfigFile file)
        {
            RemoveTree(tree);
            string lane                 = tree+"Lane::"+file.Lane.ToString();
            string count                = tree+"Process Count::"+file.Count.ToString();
            string path                 = tree+"Config File Path::"+file.Root;
            string user                 = tree+"User::"+file.User;
            string inport               = tree + "In Port::" + file.Inport;
            string outport              = tree + "Out Port::" + file.Outport;
            string controlport          = tree + "Control Port::" + file.Controlport;
            string alarm                = tree + "Alarm::" + file.Alarm.ToString();
            string periode              = tree + "Internal Trigger T::" + file.InternalTriggerPeriod;
            string carrier              = tree + "Carrier Count::" + file.Carrier.ToString();
            string LaneNameA            = tree + "Lane A Number::" + file.LaneNumber[0].ToString();
            string StemRemoving         = tree + "Stem Removing::" + file.StemRemoving.ToString();
            string CBinding             = tree + "Data Binding Type::Color:" + file.ColorBindingType.ToString();
            string SBinding             = tree + "Data Binding Type::Size:" + file.SizeBindingType.ToString();
            string ABinding             = tree + "Data Binding Type::Shape:" + file.AspectBindingType.ToString();
            string protocol             = tree + "Tx Protocol::" + file.txProtocol.ToString();
            string AutoStart            = tree + "Auto Start::" + file.AutoStart.ToString();
            string MinSize              = tree + "Minimum Fruit Size::" + file.MinimumSize.ToString();
            string UserMode             = tree + "User Mode::" + file.UserMode;
            string OutlineMethod        = tree + "Outline Method::" + file.outlineMethod.ToString();
            string MinOutlineParticle   = tree + "Min Outline Particle::" + file.minOutlineParticle.ToString();
            string ParticleRemover      = tree + "Color Particle Remover::" + file.particleRemover.ToString();
            string OutlineParticleRemover = tree + "Outline Particle Remover::" + file.outlineParticleRemover.ToString();
            string ParticleFilter       = tree + "Color Particle Filter::" + file.particleFilter.ToString();
            string image1Enc = tree + "Image1 Enhancement::" + file.image1Enhance.ToString();
            string image2Enc = tree + "Image2 Enhancement::" + file.image2Enhance.ToString();
            string ximageEnc = tree + "XImage Enhancement::" + file.ximageEnhance.ToString();
            string StartupCam           = tree + "Default Cam::" + file.DefaultCam;
            string ColorFactor = tree + "Color Factor::" + file.laneBalanceColorFactor;
 
 
            SetTree(user,file);
            SetTree(path, file);
            SetTree(lane, file);
            SetTree(count, file);
            SetTree(periode, file);
            SetTree(inport, file);
            SetTree(outport, file);
            SetTree(controlport, file);
            SetTree(alarm, file);
            SetTree(carrier, file);
            if (file.Lane == 1)
            {
                SetTree(LaneNameA, file);
            }
            else
            {
                SetTree(tree + "Lane B Number::" + file.LaneNumber[1].ToString(),file);
            }
            SetTree(StemRemoving, file);
            SetTree(CBinding, file);
            SetTree(SBinding, file);
            SetTree(ABinding, file);
            SetTree(protocol, file);
            SetTree(AutoStart, file);
            SetTree(MinSize, file);
            SetTree(OutlineMethod, file);
            SetTree(OutlineParticleRemover, file);
            SetTree(MinOutlineParticle, file);
            SetTree(ParticleRemover, file);
            SetTree(ParticleFilter, file);
            SetTree(MinOutlineParticle, file);
            SetTree(ximageEnc, file);
            SetTree(image1Enc, file);
            SetTree(image2Enc, file);
            SetTree(UserMode, file);
            SetTree(StartupCam, file);
            SetTree(ColorFactor, file);
        }

        public void SetCalibrationFactor(string tree, double calFactor)
        {
            RemoveTree(tree + "::");
            string factor = tree + "::"+calFactor.ToString();
            SetTree(factor, calFactor);
        }
        public void SetExtraProcessingdParameter(string tree, ExtraProcessingParameters parameter)
        {
 
            RemoveTree(tree + "::");
 
            string range1 = tree + "::Color Threshold::Range1";
            string range2 = tree + "::Color Threshold::Range2";
            string range3 = tree + "::Color Threshold::Range3";
         
            string min1 = range1 + "::min::" + parameter.range["plane1"].Minimum.ToString();
            string min2 = range2 + "::min::" + parameter.range["plane2"].Minimum.ToString();
            string min3 = range3 + "::min::" + parameter.range["plane3"].Minimum.ToString();

            string max1 = range1 + "::max::" + parameter.range["plane1"].Maximum.ToString();
            string max2 = range2 + "::max::" + parameter.range["plane2"].Maximum.ToString();
            string max3 = range3 + "::max::" + parameter.range["plane3"].Maximum.ToString();

            //string enabled = tree + "::Enabled::" + parameter.Enable.ToString();
            string contained = tree + "::Contained Mode::" + parameter.ContainedMode.ToString();
            string combined = tree + "::Combined Type::" + parameter.CombinedImage.ToString();
            string displayColor = tree + "::Display Color::" + parameter.DisplayColor.DisplayColor.ToString();
            string imageType = tree + "::Image Type::" + parameter.colorMode.ToString().ToUpper();
            string composit = tree + "::Composition Method::" + parameter.Composit.ToString();
            
            //SetTree(type, parameter);  //dummy
            SetTree(min1, parameter);
            SetTree(max1, parameter);
            SetTree(min2, parameter);
            SetTree(max2, parameter);
            SetTree(min3, parameter);
            SetTree(max3, parameter);

            //SetTree(enabled, parameter);
            SetTree(contained, parameter);
            SetTree(combined, parameter);
            SetTree(composit, parameter);
            SetTree(displayColor, parameter);
            SetTree(imageType, parameter);

        }
        public void SetColorThresholdParameter(string tree, ColorThresholdParameters parameter)
        {
            RemoveTree(tree+"::");
            string imageType    = tree  +   "::Image Type::"; 
            string range1       = tree  +   "::Color Threshold::Range1";
            string range2       = tree  +   "::Color Threshold::Range2";
            string range3       = tree  +   "::Color Threshold::Range3";

            string type = imageType + parameter.colorMode.ToString().ToUpper();
            string min1 = range1 + "::min::" + parameter.range["plane1"].Minimum.ToString() ;
            string min2 = range2 + "::min::" + parameter.range["plane2"].Minimum.ToString() ;
            string min3 = range3 + "::min::" + parameter.range["plane3"].Minimum.ToString() ;

            string max1 = range1 + "::max::" + parameter.range["plane1"].Maximum.ToString() ;
            string max2 = range2 + "::max::" + parameter.range["plane2"].Maximum.ToString() ;
            string max3 = range3 + "::max::" + parameter.range["plane3"].Maximum.ToString() ;

            SetTree(type, parameter);
            SetTree(min1, parameter);
            SetTree(max1, parameter);
            SetTree(min2, parameter);
            SetTree(max2, parameter);
            SetTree(min3, parameter);
            SetTree(max3, parameter);
        }
   
        
        public void SetMedianFilterParameter(string tree, MedianFilterParameters parameter)
        {
            string ttl = tree + "::Median Filter::";
            RemoveTree(ttl);
            string height = ttl + "height::" + parameter.height.ToString();
            string width = ttl + "width::" + parameter.width.ToString();
            SetTree(width, parameter);
            SetTree(height, parameter);
        }
     
        public void SetRemoveParticleParameter(string tree, RemoveParticleParameters parameter)
        {
            string ttl = tree + "::Particle Removing::";
            RemoveTree(ttl);
            string erosion = ttl + "erosion::"+parameter.erosion.ToString();
            string size = ttl + "Size To Keep::" + parameter.sizeToKeep.ToString();
            string conn = ttl + "Connectivity::" + parameter.connectivity.ToString();
            SetTree(erosion, parameter);
            SetTree(size, parameter);
            SetTree(conn, parameter);
        }
     
        public void SetParticleFilterOptionParameter(string tree, ParticleFilterOptionParameters parameter)
        {
            string ttl = tree + "::Particle Filter::Options::";
            RemoveTree(ttl);
            string reject = ttl + "Reject Matches::" + parameter.rejectMatches.ToString();
            string border = ttl + "Reject Border::" + parameter.rejectBorder.ToString();
            string fill = ttl + "Fill Holes::" + parameter.fillHoles.ToString();
            string conn = ttl + "Connectivity::" + parameter.connectivity.ToString();
            SetTree(reject, parameter);
            SetTree(border, parameter);
            SetTree(fill, parameter);
            SetTree(conn, parameter);
        }
      
        public void SetParticleFilterCriteriaParameters(string tree, ParticleFilterCriteriaParameters parameter)
        {
            string ttl = tree + "::Particle Filter::Critera::";
            RemoveTree(ttl);
            string mttl;
            int i=0;
            //(MeasurementType mType,Range range,bool cali,RangeType rType)
            SetTree(ttl, parameter);
            if (parameter.Criteria.Count <= 0)
            {
                ParticleFilterCriteria criteria = new ParticleFilterCriteria(MeasurementType.AreaByParticleAndHolesArea, new Range(), false); 
                parameter.Criteria.Add(criteria);
            }
            foreach(ParticleFilterCriteria c in parameter.Criteria)
            {
                mttl = ttl+(i+1).ToString()+"::";
                SetTree(mttl + "Measurement Type::" + c.Parameter.ToString(),parameter);
                SetTree(mttl + "Range::min::" + c.Range.Minimum.ToString(), parameter);
                SetTree(mttl + "Range::max::" + c.Range.Maximum.ToString(), parameter);
                SetTree(mttl + "Range Type::" + c.RangeType.ToString(), parameter);
                SetTree(mttl + "Calibration::" + c.Calibrated.ToString(), parameter);
                i++;
            }
        }

        public void SetParticleReportParameters(string tree, ParticleReportParameters parameter)
        {
            string ttl = tree + "::Particle Report::";
            RemoveTree(ttl);
            SetTree(ttl + "Connectivity::" + parameter.connectivity.ToString(),parameter);
        }
      
        public void SetDisplayColor(string tree, DisplayColorParameter parameter)
        {
            string ttl = tree + "::Display Color::";
            RemoveTree(ttl);
            SetTree(ttl + parameter.DisplayColor.ToString(), parameter);
        }
      
        public void SetRoiData(string tree, IEnumerable<RoiData> roi)
        {
            //RemoveTree(tree);
            string tag;
            int i = 0;
            SetTree(tree, null);
            foreach (RoiData r in roi)
            {
                tag = string.Format("::{1}", i++, r.ToString());
                SetTree(tree + tag, roi);
            }
        }
    }
}
