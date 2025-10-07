using System;
using System.Collections.Generic;

namespace GpsToUnity
{
    [System.Serializable]
    public struct Coords
    {
        public double lat; 
        public double lon; 
        public double alt;

        public Coords( double a, double b, double c)
        {
            this.lat = a;
            this.lon = b;
            this.alt = c;
        }
    }

    public class GetPOS
    {
        public static List<double> GTU(double lat,double lon,double alt)
        {
            double xSize = 1000000d;
            double ySize = 1d;
            double zSize = 1000000d;

            double temp;
            double delta;

            if(lat > 90d)
            {
                temp = lat;
                delta = temp - 90d;
                lat = 90d - delta;
            }

            if(lat < -90d)
            {
                temp = lat;
                delta = temp + 90d;
                lat = -90d - delta;
            }

            if(lon > 180d)
            {
                temp = lon;
                delta = temp - 180d;
                lon = -180d + delta;
            }

            if(lon < -180d)
            {
                temp = lon;
                delta = temp + 180d;
                lon = 180d + delta;
            }

            double x = lon * xSize;
            double y = alt * ySize;
            double z = lat * zSize;

            var retList = new List<double>();

            retList.Add(x);
            retList.Add(y);
            retList.Add(z);

            return retList;
        }
    }
} 
