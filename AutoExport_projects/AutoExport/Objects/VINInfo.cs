

namespace AutoExport.Objects
{
    public class VINInfo
    {
        private string mBodystyle = "";
        private bool mError = false;
        private string mErrorDesc;
        private string mMake = "";
        private string mModel = "";
        private string mSizeClass = "";
        private string mVehicleCubicFeet = "";
        private string mVehicleHeight = "";
        private string mVehicleLength = "";
        private string mVehicleWeight = "";
        private string mVehicleWidth = "";
        private string mVehicleYear = "";
        private string mVIN = "";
        private bool mVINDecoded = false;

        public string Bodystyle
        {
            get { return mBodystyle; }
            set { mBodystyle = value; }
        }

        public bool Error
        {
            get { return mError; }
            set { mError = value; }
        }

        public string ErrorDesc
        {
            get { return mErrorDesc; }
            set { mErrorDesc = value; }
        }

        public string Make
        {
            get { return mMake; }
            set { mMake = value; }
        }

        public string Model
        {
            get { return mModel; }
            set { mModel = value; }
        }

        public string SizeClass
        {
            get { return mSizeClass; }
            set { mSizeClass = value; }
        }

        public string VehicleCubicFeet
        {
            get { return mVehicleCubicFeet; }
            set { mVehicleCubicFeet = value; }
        }

        public string VehicleHeight
        {
            get { return mVehicleHeight; }
            set { mVehicleHeight = value; }
        }

        public string VehicleLength
        {
            get { return mVehicleLength; }
            set { mVehicleLength = value; }
        }

        public string VehicleWeight
        {
            get { return mVehicleWeight; }
            set { mVehicleWeight = value; }
        }

        public string VehicleWidth
        {
            get { return mVehicleWidth; }
            set { mVehicleWidth = value; }
        }

        public string VehicleYear
        {
            get { return mVehicleYear; }
            set { mVehicleYear = value; }
        }

        public string VIN
        {
            get { return mVIN; }
            set { mVIN = value; }
        }

        public bool VINDecoded
        {
            get { return mVINDecoded; }
            set { mVINDecoded = value; }
        }

    }
}
