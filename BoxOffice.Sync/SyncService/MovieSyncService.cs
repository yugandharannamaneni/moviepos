using BoxOffice.DAL.Interfaces;
using BoxOffice.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BoxOffice.Sync.DAL.Interfaces;

namespace MastiTickets.Win.Sync.SyncService
{
    public class MovieSyncService
    {
        private ILog Log = LogManager.GetLogger(typeof(MovieSyncService));

        private ISeatTemplateRepository _seatTemplateRepository;
        private ISeatLayoutConfig _seatLayoutConfig;
        private IMovieTimingsRepository _movieTimingsRepository;
        private IMovieTimingsSyncRepository _movieTimingsSyncRepository;

        private int TheatreId
        {
            get
            {
                if (ConfigurationManager.AppSettings["TheaterID"] != null)
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["TheaterID"]);
                }
                else
                {
                    return 0;
                }
            }
        }

        public MovieSyncService(ISeatTemplateRepository seatTemplateRepository,
            ISeatLayoutConfig seatLayoutConfig,
            IMovieTimingsRepository movieTimingsRepository,
            IMovieTimingsSyncRepository movieTimingsSyncRepository)
        {
            _seatTemplateRepository = seatTemplateRepository;
            _seatLayoutConfig = seatLayoutConfig;
            _movieTimingsRepository = movieTimingsRepository;
            _movieTimingsSyncRepository = movieTimingsSyncRepository;
        }

        public void SyncTheatreData()
        {

        }

        public void SyncScreensAndScreenClasses()
        {
            try
            {
                var screens = _seatLayoutConfig.GetScreens();

                foreach (var screen in screens)
                {
                    var screenId = InsertScreen(screen);

                    //update web screenId in local database.

                    UpdateScreenClasses(screenId);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting screens list: ", ex);
            }
        }

        private void UpdateScreenClasses(int screenId)
        {
            var screenClasses = _seatLayoutConfig.GetScreeClasses(screenId);

            foreach (var screenClass in screenClasses)
            {
                var screenClassId = InsertScreenClass(screenClass);

                //update web screen classId in local database.
            }
        }

        private int InsertScreen(Screen screen)
        {
            return 1;
        }

        private int InsertScreenClass(Screenclasses screenClass)
        {
            return 1;
        }

        public void SyncMovieTimings()
        {
            try
            {
                //get the list of movie timings to update.
                var movieTimings = _movieTimingsRepository.GetMovieTimingsForSync();

                //insert movie timings into master database.
                _movieTimingsSyncRepository.InsertMovieTiming(movieTimings, TheatreId);

                //update box movie timings with web movie timings.
                _movieTimingsRepository.UpdateWebMovieTimingIds(movieTimings);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while syncing movietimings, {0}", ex);
            }
        }
    }
}
