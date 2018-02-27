namespace BoxOfficeUI.CalenderView
{
    class Scheduling
    {
        public Scheduling()
            : base()
        {
        }

        private int schedulId;
        public int SchedulId
        {
            get { return this.schedulId; }
            set
            {
                if (((this.schedulId == value) == false))
                {
                    this.schedulId = value;
                }
            }
        }

        private string _Subject;
        public string Subject
        {
            get { return this._Subject; }
            set
            {
                if ((string.Equals(this._Subject, value) == false))
                {
                    this._Subject = value;
                }
            }
        }

        private string _Location;
        public string Location
        {
            get { return this._Location; }
            set
            {
                if ((string.Equals(this._Location, value) == false))
                {
                    this._Location = value;
                }
            }
        }

        private string _Details;
        public string Details
        {
            get { return this._Details; }
            set
            {
                if ((string.Equals(this._Details, value) == false))
                {
                    this._Details = value;
                }
            }
        }

        private System.DateTime? _StartTime;
        public System.DateTime? StartTime
        {
            get { return this._StartTime; }
            set
            {
                if ((this._StartTime.Equals(value) == false))
                {
                    this._StartTime = value;
                }
            }
        }

        private System.DateTime? _EndTime;
        public System.DateTime? EndTime
        {
            get { return this._EndTime; }
            set
            {
                if ((this._EndTime.Equals(value) == false))
                {
                    this._EndTime = value;
                }
            }
        }

        private System.DateTime _reccreatedDate;
        public System.DateTime reccreatedDate
        {
            get { return this._reccreatedDate; }
            set
            {
                if (((this._reccreatedDate == value) == false))
                {
                    this._reccreatedDate = value;
                }
            }
        }
    }
}