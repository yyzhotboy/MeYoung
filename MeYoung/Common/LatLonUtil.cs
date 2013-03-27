using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class LatLonUtil
    {
        private static double PI = 3.14159265;
        //private static double EARTH_RADIUS = 6378137;
        //private static double RAD = Math.PI / 180.0;

        //@see http://snipperize.todayclose.com/snippet/php/SQL-Query-to-Find-All-Retailers-Within-a-Given-Radius-of-a-Latitude-and-Longitude--65095/   
        //The circumference of the earth is 24,901 miles.  
        //24,901/360 = 69.17 miles / degree    
        /** 
         * @param raidus 单位米 
         * return minLat,minLng,maxLat,maxLng 
         */
        public static double[] GetAround(double lat, double lon, int raidus)
        {

            Double latitude = lat;
            Double longitude = lon;

            Double degree = (24901 * 1609) / 360.0;
            double raidusMile = raidus;

            Double dpmLat = 1 / degree;
            Double radiusLat = dpmLat * raidusMile;
            Double minLat = latitude - radiusLat;
            Double maxLat = latitude + radiusLat;

            Double mpdLng = degree * Math.Cos(latitude * (PI / 180));
            Double dpmLng = 1 / mpdLng;
            Double radiusLng = dpmLng * raidusMile;
            Double minLng = longitude - radiusLng;
            Double maxLng = longitude + radiusLng;
            //System.out.println("["+minLat+","+minLng+","+maxLat+","+maxLng+"]");  
            return new double[] { minLat, minLng, maxLat, maxLng };
        }


    }
}
