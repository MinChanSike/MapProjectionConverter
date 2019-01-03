using ProjNet.CoordinateSystems;
using System;


/// <summary>
/// Map Coordinate Projection Convertion
/// Power by ProjNet, Other usefull library DotSpatial
/// </summary>
namespace MapProjectionConverter {
    class Program {

        ///Ref:: https://epsg.io/3414        
        private static string wkt_SVY21 = "PROJCS[\"SVY21 / Singapore TM\", GEOGCS[\"SVY21\", DATUM[\"SVY21\", SPHEROID[\"WGS 84\", 6378137, 298.257223563, AUTHORITY[\"EPSG\", \"7030\"]], AUTHORITY[\"EPSG\", \"6757\"]], PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.0174532925199433, AUTHORITY[\"EPSG\", \"9122\"]], AUTHORITY[\"EPSG\", \"4757\"]], PROJECTION[\"Transverse_Mercator\"], PARAMETER[\"latitude_of_origin\", 1.366666666666667], PARAMETER[\"central_meridian\", 103.8333333333333], PARAMETER[\"scale_factor\", 1], PARAMETER[\"false_easting\", 28001.642], PARAMETER[\"false_northing\", 38744.572], UNIT[\"metre\", 1, AUTHORITY[\"EPSG\", \"9001\"]], AUTHORITY[\"EPSG\", \"3414\"]]";
        private static string wkt_EPSG4326 = "GEOGCS[\"WGS 84\", DATUM[\"WGS_1984\", SPHEROID[\"WGS 84\", 6378137, 298.257223563, AUTHORITY[\"EPSG\", \"7030\"]], AUTHORITY[\"EPSG\", \"6326\"]], PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.0174532925199433, AUTHORITY[\"EPSG\", \"9122\"]], AUTHORITY[\"EPSG\", \"4326\"]]";

        static void Main(string[] args) {
            var lnglat = new double[2];
            var xy = new double[2];

            ConvertSVY21_to_EPSG3857(22421.58676, 37700.91435, out lnglat);
            var lng = lnglat[0]; var lat = lnglat[1];
            Console.WriteLine($"ConvertSVY21_to_EPSG3857 -> {lat}, {lng}");

            ConvertEPSG3857_to_SVY21(lng, lat, out xy);
            Console.WriteLine($"ConvertEPSG3857_to_SVY21 -> {xy[0]}, {xy[1]}");
            Console.Read();
        }

        public static void ConvertEPSG3857_to_SVY21(double lat, double lng, out double[] xy) {
            var gcs_WGS84 = GeographicCoordinateSystem.WGS84;
            var coordFactory = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            var coordTransform = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            var svy21 = coordFactory.CreateFromWkt(wkt_SVY21);

            var transformTo3857 = coordTransform.CreateFromCoordinateSystems(gcs_WGS84, svy21);
            double[] fromPoint = { lat, lng };
            xy = transformTo3857.MathTransform.Transform(fromPoint);
        }

        public static void ConvertSVY21_to_EPSG3857(double x, double y, out double[] lnglat) {
            var gcs_WGS84 = GeographicCoordinateSystem.WGS84;
            var coordFactory = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            var coordTransform = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            var svy21 = coordFactory.CreateFromWkt(wkt_SVY21);

            var transformTo3857 = coordTransform.CreateFromCoordinateSystems(svy21, gcs_WGS84);
            double[] fromPoint = { x, y };
            lnglat = transformTo3857.MathTransform.Transform(fromPoint);
        }

    }
}
