using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using System.Drawing;

using NationalInstruments.Vision;
using NationalInstruments.Vision.WindowsForms;
using NationalInstruments.Vision.Acquisition.Imaqdx;
using NationalInstruments.Vision.Analysis;



namespace SnpSystem.Vision.VisionConfigurationHelper
{
    public enum TxProtocol
    {
        Standard,
        Extended,
    }
    
    public enum DataBindingType
    {
        Minimum,
        Maximum,
        Average,
        Sum,
        LastAverage,
    }
    
    public enum ImageSide
    {
        Top,
        Left,
        Right,
        Front,
        Back,
    }

    public enum ImageLane
    {
        Lane1=0,
        Lane2=1,
    }

    public enum ImageSequence
    {
        Seq0=0,
        Seq1=1,
        Seq2=2,
        Seq3=3,
        Seq4=4,
        Seq5=5,
        Seq6=6,
        Seq7=7,
        Seq8=8,
        Seq9=9,
        SeqA=10,
        SeqB=11,
    }

    public enum OutlineMethodType
    {
        One,
        All,
        MaxBySeperated,
        AllBySeperated
    }
    public enum ImageDirection
    {
        Side1,
        Side2,
        Side3,
    }
    [Flags] public enum RoiDisplayMode
    {
        None=0,
        Defined=1,
        Dynamic=2,
        All=Defined|Dynamic,
    }

    static public class VisionFilePath
    {
        public static string RootPath = @"c:\Program Files\S&P SYSTEM\SNP SMART VISION";
        public static string ConfigPath = RootPath + @"\config";
        public static string DataPath = RootPath + @"\data";
        public static string RootConfigFile = ConfigPath + @"\root_config_file.rcf";

        public static string General = "일반정보::";
        public static string Trigger = "트리거정보::";
        public static string Output  = "출력정보::";
        public static string Process = "화상처리정보::";

    }

    /*
     * 
    [COMMENT] ->설명
    SEH SMART VISION ROOT CONFIGURATION FILE
    [CUSTOMER]->고객사이트명
    S&P SYSTEM
    [CONFIG FILE PATH]->Config File Path
    C:\Program Files\SNP SYSTEM\SEH SMART VISION\config\test.ssvc
    [LANE]->레인수(최대2)
    1
    [IMAGE PROCESSING]->이메지프로세싱에 사용되는 이미지파일(최대 3까지가능)
    2
    [INPORT]->트리거 시리얼포트
    COM3
    [OUTPORT]->출력 시리얼포트
    COM4
    [CONTROLPORT]->알람출력포트
    True/False
    [ALARM]
    [CARRIER]->화상처리범위안에 들어오는 케리어 갯수
    8
    [PERIODE]->내부트리거 발생주기
    124
    [LANE A NUMBER]->첫번째레인번호 
    1
    [LANE B NUMBER]->두번째레인번호
    0
    [STEM REMOVER]->줄기제거 알고리즘을 사용?
    True
    [SIZE BINDING]->크기는 최대값을 적용
    Maximum
    [USER MODE]->프로그램을 수퍼바이저모드로 설정(일반사용자:Customer)
    Supervisor
    [COLOR BINDING]->색상값은 평균값적용(Min,Max,Average,Sum,LastAverage)
    Average
    [ASPECT BINDING]->모양은 최소값을 적용
    Minimum
    [TX PROTOCOL]->출력 통신프로토콜은 표준(색상전용)/Extended
    Standard
    [AUTO START]->자동시작
    True
    [MIN SIZE]->과일감지 최소값(면적이5000픽셀이하면 감지안함)
    5000
    [OUTLINE METHOD]->제일큰 파티클을 사용할것인지,일정이상의 파티클 모두 사용할 것인지
    ONE/ALL
    [MIN OUTLINE PARTICLE] ->OUTLINE METHOD를 ALL로 하였을때 처리할 기준값
    100
    [IMAGE1 ENHANCEMENT] -Image1 Morphology
     True/False
    [IMAGE2 ENHANCEMENT] -Image2 Morphology
    True/False
    [XIMAGE ENHANCEMENT] -XImage Morphology
    True/False
     
    */


    [Serializable]
    public class RootConfigFile
    {
        private const string Tag="SEH SMART VISION ROOT CONFIGURATION FILE"; 
        public string User="No Configuration";
        public string Root="";
        public int Lane=1;
        public int[] LaneNumber;
        public int Count=2;
        public string Inport;
        public string Outport;
        public string Controlport;
        public bool StemRemoving=true;
        public bool Alarm = false;
        public int InternalTriggerPeriod = 100;
        public int Carrier=1;
        public bool AutoStart = false;
        public int MinimumSize = 5000;
        public TxProtocol txProtocol = TxProtocol.Standard;
        public DataBindingType SizeBindingType=DataBindingType.Maximum;
        public DataBindingType ColorBindingType=DataBindingType.Average;
        public DataBindingType AspectBindingType=DataBindingType.Minimum;
        public string UserMode = "Customer";
        public string DefaultCam = "cam1";
        public OutlineMethodType outlineMethod = OutlineMethodType.One;
        public int minOutlineParticle=100;
        public bool particleRemover=false;
        public bool particleFilter=false;
        public bool outlineParticleRemover = false;
        public bool image1Enhance = true;
        public bool image2Enhance = true;
        public bool ximageEnhance = true;
        public int laneBalanceColorFactor = 1000;
        

        public RootConfigFile()
        {
            LaneNumber = new int[2];
        }
       
        public RootConfigFile Read()
        {
            if (Directory.Exists(VisionFilePath.ConfigPath) == false||File.Exists(VisionFilePath.RootConfigFile)==false) Write();
            using (StreamReader reader = new StreamReader(VisionFilePath.RootConfigFile))
            {
                while (reader.EndOfStream == false)
                {
                    makeParameters(reader.ReadLine(), reader);
                }
                reader.Close();
                return this;
            }
        }
        private void makeParameters(string para, StreamReader reader)
        {
            string p=para.Trim().ToUpper();
            if (p == "[COMMENT]")                       reader.ReadLine();
            else if (p == "[CUSTOMER]")                 User = reader.ReadLine();
            else if (p == "[CONFIG FILE PATH]")         Root = reader.ReadLine();
            else if (p == "[PERIODE]")                  InternalTriggerPeriod = int.Parse(reader.ReadLine());
            else if (p == "[LANE]")                     Lane = int.Parse(reader.ReadLine());
            else if (p == "[IMAGE PROCESSING]")         Count = int.Parse(reader.ReadLine());
            else if (p == "[INPORT]")                   Inport = reader.ReadLine().ToUpper();
            else if (p == "[OUTPORT]")                  Outport = reader.ReadLine().ToUpper();
            else if (p == "[CONTROL PORT]")             Controlport = reader.ReadLine().ToUpper();
            else if (p == "[ALARM]")                    Alarm = bool.Parse(reader.ReadLine());
            else if (p == "[CARRIER]")                  Carrier = int.Parse(reader.ReadLine());
            else if (p == "[LANE A NUMBER]")            LaneNumber[0] = int.Parse(reader.ReadLine());
            else if (p == "[LANE B NUMBER]")            LaneNumber[1] = int.Parse(reader.ReadLine());
            else if (p == "[STEM REMOVER]")             StemRemoving = bool.Parse(reader.ReadLine());
            else if (p == "[SIZE BINDING]")             SizeBindingType = EnumConv.ToDataBindingType(reader.ReadLine());
            else if (p == "[COLOR BINDING]")            ColorBindingType = EnumConv.ToDataBindingType(reader.ReadLine());
            else if (p == "[ASPECT BINDING]")           AspectBindingType = EnumConv.ToDataBindingType(reader.ReadLine());
            else if (p == "[TX PROTOCOL]")              txProtocol = EnumConv.ToTxProtocol(reader.ReadLine());
            else if (p == "[AUTO START]")               AutoStart = bool.Parse(reader.ReadLine());
            else if (p == "[MIN SIZE]")                 MinimumSize = int.Parse(reader.ReadLine());
            else if (p == "[USER MODE]")                UserMode = reader.ReadLine().ToUpper();
            else if (p == "[DEFAULT CAM]")              DefaultCam = reader.ReadLine();
            else if (p == "[OUTLINE METHOD]")           outlineMethod = EnumConv.ToOutlineMethodType(reader.ReadLine());
            else if (p == "[MIN OUTLINE PARTICLE]")     minOutlineParticle = int.Parse(reader.ReadLine());
            else if (p == "[PARTICLE REMOVER]")         particleRemover = bool.Parse(reader.ReadLine());
            else if (p == "[PARTICLE FILTER]")          particleFilter = bool.Parse(reader.ReadLine());
            else if (p == "[OUTLINE PARTICLE REMOVER]") outlineParticleRemover = bool.Parse(reader.ReadLine());
            else if (p == "[IMAGE1 ENHANCE]")           image1Enhance = bool.Parse(reader.ReadLine());
            else if (p == "[IMAGE2 ENHANCE]")           image2Enhance = bool.Parse(reader.ReadLine());
            else if (p == "[XIMAGE ENHANCE]")           ximageEnhance = bool.Parse(reader.ReadLine());
            else if (p == "[LANE BALANCE COLOR FACTOR]") laneBalanceColorFactor = int.Parse(reader.ReadLine());

        }

        public void Write()
        {
            if (Directory.Exists(VisionFilePath.ConfigPath) == false) Directory.CreateDirectory(VisionFilePath.ConfigPath);
            using (StreamWriter writer = new StreamWriter(VisionFilePath.RootConfigFile, false))
            {
                writer.WriteLine("[COMMENT]");                  writer.WriteLine("SEH SMART VISION ROOT CONFIGURATION FILE");
                writer.WriteLine("[CUSTOMER]");                 writer.WriteLine(User);
                writer.WriteLine("[CONFIG FILE PATH]");         writer.WriteLine(Root);
                writer.WriteLine("[LANE]");                     writer.WriteLine(Lane.ToString());
                writer.WriteLine("[IMAGE PROCESSING]");         writer.WriteLine(Count.ToString());
                writer.WriteLine("[INPORT]");                   writer.WriteLine(Inport);
                writer.WriteLine("[OUTPORT]");                  writer.WriteLine(Outport);
                writer.WriteLine("[CONTROL PORT]");             writer.WriteLine(Controlport);
                writer.WriteLine("[ALARMT]");                   writer.WriteLine(Alarm);
                writer.WriteLine("[CARRIER]");                  writer.WriteLine(Carrier.ToString());
                writer.WriteLine("[PERIODE]");                  writer.WriteLine(InternalTriggerPeriod.ToString());
                writer.WriteLine("[LANE A NUMBER]");            writer.WriteLine(LaneNumber[0].ToString());
                writer.WriteLine("[LANE B NUMBER]");            writer.WriteLine(LaneNumber[1].ToString());
                writer.WriteLine("[STEM REMOVER]");             writer.WriteLine(StemRemoving.ToString());
                writer.WriteLine("[SIZE BINDING]");             writer.WriteLine(SizeBindingType.ToString());
                writer.WriteLine("[COLOR BINDING]");            writer.WriteLine(ColorBindingType.ToString());
                writer.WriteLine("[ASPECT BINDING]");           writer.WriteLine(AspectBindingType.ToString());
                writer.WriteLine("[TX PROTOCOL]");              writer.WriteLine(txProtocol.ToString());
                writer.WriteLine("[AUTO START]");               writer.WriteLine(AutoStart.ToString());
                writer.WriteLine("[MIN SIZE]");                 writer.WriteLine(MinimumSize.ToString());
                writer.WriteLine("[USER MODE]");                writer.WriteLine(UserMode);
                writer.WriteLine("[DEFAULT CAM]");              writer.WriteLine(DefaultCam);
                writer.WriteLine("[OUTLINE METHOD]");           writer.WriteLine(outlineMethod.ToString());
                writer.WriteLine("[MIN OUTLINE PARTICLE]");     writer.WriteLine(minOutlineParticle.ToString());
                writer.WriteLine("[PARTICLE REMOVER]");         writer.WriteLine(particleRemover.ToString());
                writer.WriteLine("[PARTICLE FILTER]");          writer.WriteLine(particleFilter.ToString());
                writer.WriteLine("[OUTLINE PARTICLE REMOVER]"); writer.WriteLine(outlineParticleRemover.ToString());
                writer.WriteLine("[IMAGE1 ENHANCE]");           writer.WriteLine(image1Enhance.ToString());
                writer.WriteLine("[IMAGE2 ENHANCE]");           writer.WriteLine(image2Enhance.ToString());
                writer.WriteLine("[XIMAGE ENHANCE]");           writer.WriteLine(ximageEnhance.ToString());
                writer.WriteLine("[LANE BALANCE COLOR FACTOR]"); writer.WriteLine(laneBalanceColorFactor.ToString());
                writer.Close();
            }
        }
    }


    static public class RectOperator
    {

        static public RectangleContour Add(RectangleContour rt1, RectangleContour rt2)
        {
            RectangleContour r = new RectangleContour();
            double r1, b1, r2, b2,right,bottom;

            r1 = rt1.Left + rt1.Width;   //(rt1.Left,rt1.Top)-(r1,b1)
            b1 = rt1.Top + rt1.Height;

            r2 = rt2.Left + rt2.Width;   //(rt2.Left,rt2.Top)-(r2,b2)
            b2 = rt2.Top + rt2.Height;

            r.Left = rt1.Left < rt2.Left ? rt1.Left : rt2.Left;
            r.Top = rt1.Top < rt2.Top ? rt1.Top : rt2.Top;

            right = r1 > r2 ? r1 : r2;
            bottom = b1 > b2 ? b1 : b2;

            r.Width = right - r.Left;
            r.Height = bottom - r.Top;

            return r;
        }

    }

    static public class EnumConv
    {
        static public ImageLane ToLane(string lane)
        {
            if (lane == "Lane1") return ImageLane.Lane1;
            return ImageLane.Lane2;
        }
        static public ImageLane ToLane(int lane)
        {
            ImageLane l = new ImageLane();
            if (lane == 1) l = ImageLane.Lane1;
            else l=ImageLane.Lane2;
            return l;
        }
        static public int ToIntLane(ImageLane lane)
        {
            return (int)lane;
        }

        static public DataBindingType ToDataBindingType(string bindingType)
        {
            DataBindingType bType = new DataBindingType();
            string type = bindingType.ToUpper();
            switch (type)
            {
                case "MINIMUM"      : bType = DataBindingType.Minimum; break;
                case "MAXIMUM"      : bType = DataBindingType.Maximum; break;
                case "AVERAGE"      : bType = DataBindingType.Average; break;
                case "SUM"          : bType = DataBindingType.Sum; break;
                case "LASTAVERAGE"  : bType = DataBindingType.LastAverage; break;
                default: bType = DataBindingType.Average; break;
            }
            return bType;
        }

        static public OutlineMethodType ToOutlineMethodType(string methodType)
        {
            OutlineMethodType oType = new OutlineMethodType();
            string type = methodType.ToUpper();
            switch (type)
            {
                case "ONE":             oType = OutlineMethodType.One; break;
                case "ALL":             oType = OutlineMethodType.All; break;
                case "MAXBYSEPERATED":  oType = OutlineMethodType.MaxBySeperated; break;
                case "ALLBYSEPERATED":  oType = OutlineMethodType.AllBySeperated; break;
                default  :              oType = OutlineMethodType.One; break;
            } 
            return oType;
        }


        static public TxProtocol ToTxProtocol(string txProtocol)
        {
            TxProtocol tx = new TxProtocol();
            string txp = txProtocol.ToUpper();

            if (txp == "STANDARD") tx = TxProtocol.Standard;
            else tx = TxProtocol.Extended;
            return tx;
        }

        static public ImageSequence ToSequence(string sequence)
        {
            ImageSequence iSeq = new ImageSequence();
            switch (sequence)
            {
                case "Seq0": iSeq = ImageSequence.Seq0; break;
                case "Seq1": iSeq = ImageSequence.Seq1; break;
                case "Seq2": iSeq = ImageSequence.Seq2; break;
                case "Seq3": iSeq = ImageSequence.Seq3; break;
                case "Seq4": iSeq = ImageSequence.Seq4; break;
                case "Seq5": iSeq = ImageSequence.Seq5; break;
                case "Seq6": iSeq = ImageSequence.Seq6; break;
                case "Seq7": iSeq = ImageSequence.Seq7; break;
                case "Seq8": iSeq = ImageSequence.Seq8; break;
                case "Seq9": iSeq = ImageSequence.Seq9; break;
                case "SeqA": iSeq = ImageSequence.SeqA; break;
                case "SeqB": iSeq = ImageSequence.SeqB; break;
            }
            return iSeq;
        }
        static public ImageSequence ToSequence(int sequence)
        {
            ImageSequence iSeq = new ImageSequence();
            switch (sequence)
            {
                case 0: iSeq = ImageSequence.Seq0; break;
                case 1: iSeq = ImageSequence.Seq1; break;
                case 2: iSeq = ImageSequence.Seq2; break;
                case 3: iSeq = ImageSequence.Seq3; break;
                case 4: iSeq = ImageSequence.Seq4; break;
                case 5: iSeq = ImageSequence.Seq5; break;
                case 6: iSeq = ImageSequence.Seq6; break;
                case 7: iSeq = ImageSequence.Seq7; break;
                case 8: iSeq = ImageSequence.Seq8; break;
                case 9: iSeq = ImageSequence.Seq9; break;
                case 10: iSeq = ImageSequence.SeqA; break;
                case 11: iSeq = ImageSequence.SeqB; break;
            }
            return iSeq;
        }

        static public ImageDirection ToDirection(int direction)
        {
            ImageDirection iDir = new ImageDirection();
            switch (direction)
            {
                case 0: iDir = ImageDirection.Side1; break;
                case 1: iDir = ImageDirection.Side2; break;
                case 2: iDir = ImageDirection.Side3; break;
            }
            return iDir;
        }

        static public ImageDirection ToDirection(string direction)
        {
            ImageDirection iDir = new ImageDirection();
            switch (direction)
            {
                case "Side1": iDir = ImageDirection.Side1; break;
                case "Side2": iDir = ImageDirection.Side2; break;
                case "Side3": iDir = ImageDirection.Side3; break;
            }
            return iDir;
        }

        static public int ToIntDirection(ImageDirection direction)
        {
            return (int)direction;
        }

        static public int ToIntSequence(ImageSequence sequence)
        {
            return (int)sequence;
        }
        static public int ToIntSequence(string sequence)
        {
            return ToIntSequence(ToSequence(sequence));
        }
    }

    public class CameraInformation
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public CameraInformation(string name)
        {
            _name = name;
        }
    }
    public class TriggerInformation
    {
        string _triggerChannel;
        int _manualTriggerPeriode;
        public string TriggerChannel
        {
            get { return _triggerChannel; }
            set { _triggerChannel = value; }
        }

        public int ManualTriggerPeriode
        {
            get { return _manualTriggerPeriode; }
            set { _manualTriggerPeriode = value; }
        }
        public TriggerInformation(string channel, int periode)
        {
            _triggerChannel = channel;
            _manualTriggerPeriode = periode;
        }
    }

    public class ExtractionOutline
    {
        ColorMode _colorMode;
        public ColorMode ColorType  
        {
            set { _colorMode = value; }
            get { return _colorMode; }
        }
        public ExtractionOutline(ColorMode mode)
        {
            ColorType = mode;
        }
    }

    [Serializable]
    public class VisionConfigurationHelper
    {
        List<CameraInformation> _cameras;
        TriggerInformation _triggerInformation;
        ExtractionOutline _extractionOutline;
                 
        public ExtractionOutline OutLine
        {
            set { _extractionOutline = value; }
            get { return _extractionOutline; }
        }
        public List<CameraInformation> Camers
        {
            get { return _cameras; }
            set { _cameras = value; }
        }
        public TriggerInformation TriggerInfo
        {
            get { return _triggerInformation; }
            set { _triggerInformation = value; }
        }

        public int GetCameraCount()
        {
            return _cameras.Count;
        }

        public void AddCamera(CameraInformation camInfo)
        {
            _cameras.Add(camInfo);
        }

        public void DeleteCamera(CameraInformation camInfo)
        {
            _cameras.Remove(camInfo);
        }

        public VisionConfigurationHelper()
        {
            _cameras = new List<CameraInformation>();
            _cameras.Add(new CameraInformation("cam1"));
            _cameras.Add(new CameraInformation("SCAM1"));
            //_cameras.Add(new CameraInformation("SCAM2"));
            _triggerInformation = new TriggerInformation("COM3",50);
            _extractionOutline = new ExtractionOutline(ColorMode.Rgb);
        }
    }
}
