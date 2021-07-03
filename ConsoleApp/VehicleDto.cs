using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class VehicleDto
    {
        public class Dangmark
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class PilotSafebelt
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class PilotSunvisor
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class PlateColor
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class PlateNo
        {
            /// <summary>
            /// 浙JP79R0
            /// </summary>
            public string value { get; set; }
        }

        public class PlateRect
        {
            /// <summary>
            /// 
            /// </summary>
            public double height { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double width { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double x { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double y { get; set; }
        }

        public class PlateType
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class Uphone
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class VehicleColor
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class VehicleLogo
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class VehicleType
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class VicePilotSafebelt
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class VicePilotSunvisor
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
        }

        public class Vehicle
        {
            /// <summary>
            /// 
            /// </summary>
            public Dangmark dangmark { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string isMainVehicle { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PilotSafebelt pilotSafebelt { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PilotSunvisor pilotSunvisor { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PlateColor plateColor { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PlateNo plateNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PlateRect plateRect { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public PlateType plateType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Uphone uphone { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VehicleColor vehicleColor { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VehicleLogo vehicleLogo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VehicleType vehicleType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VicePilotSafebelt vicePilotSafebelt { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public VicePilotSunvisor vicePilotSunvisor { get; set; }
        }

        public class TargetItem
        {
            /// <summary>
            /// 
            /// </summary>
            public Vehicle vehicle { get; set; }
        }

        public class TargetAttrs
        {
            /// <summary>
            /// 
            /// </summary>
            public string areaCode { get; set; }
            /// <summary>
            /// G228宏伟路口北电警
            /// </summary>
            public string cameraAddress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cameraIndexCode { get; set; }
            /// <summary>
            /// G228宏伟路口北电警
            /// </summary>
            public string cameraName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cascade { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int crossingId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string crossingIndexCode { get; set; }
            /// <summary>
            /// G228宏伟路口北电警
            /// </summary>
            public string crossingName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string deviceIndexCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string deviceName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string deviceType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string directionIndex { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string imageServerCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int laneNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string latitude { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string longitude { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int multiVehicle { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string passID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string passTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string platePicUrl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int recognitionSign { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string regionIndexCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string vehicleColorDepth { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int vehicleLen { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int vehicleSpeed { get; set; }
        }

        public class VehicleRcogResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<TargetItem> target { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public TargetAttrs targetAttrs { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string targetPicUrl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string taskID { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string sendTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int channelID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string dataType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string dateTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string eventDescription { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string eventType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ipAddress { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int portNo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string recvTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<VehicleRcogResultItem> vehicleRcogResult { get; set; }
        }
    }
}
